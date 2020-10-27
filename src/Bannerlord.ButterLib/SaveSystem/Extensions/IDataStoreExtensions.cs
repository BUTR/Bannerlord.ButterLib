using Newtonsoft.Json;

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Text;

using TaleWorlds.CampaignSystem;

namespace Bannerlord.ButterLib.SaveSystem.Extensions
{
    public sealed class DataStoreChunkArrayEnumerator : IEnumerator<byte[]>
    {
        private readonly IDataStore _dataStore;
        private readonly string _key;
        private readonly int _length;

        private int _index;
        private byte[] _current;

        public byte[] Current => _current;
        object IEnumerator.Current => Current;

        public DataStoreChunkArrayEnumerator(IDataStore dataStore, string key)
        {
            _dataStore = dataStore;
            _key = key;
            _index = 0;
            _current = Array.Empty<byte>();

            _dataStore.SyncData($"{_key}_length", ref _length);
            if (_length > 0)
                _dataStore.SyncData($"{_key}_{_index}", ref _current);
        }

        public bool MoveNext()
        {
            if (_index >= _length)
                return false;

            return _dataStore.SyncData($"{_key}_{_index++}", ref _current);
        }

        public void Reset() => _index = 0;

        public void Dispose() { }
    }

    public sealed class DataStoreChunkArray : IEnumerable<byte[]>
    {
        private readonly IDataStore _dataStore;
        private readonly string _key;

        public DataStoreChunkArray(IDataStore dataStore, string key)
        {
            _dataStore = dataStore;
            _key = key;
        }

        public IEnumerator<byte[]> GetEnumerator() => new DataStoreChunkArrayEnumerator(_dataStore, _key);
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public sealed class DataStoreChunkArrayReaderStream : Stream
    {
        private readonly IEnumerator<byte> _input;
        private bool _disposed;

        public DataStoreChunkArrayReaderStream(IDataStore dataStore, string key)
        {
            _input = new DataStoreChunkArray(dataStore, key).SelectMany(i => i).GetEnumerator();
        }

        public override bool CanRead => true;
        public override bool CanSeek => false;
        public override bool CanWrite => false;
        public override long Length => 0;
        public override long Position { get; set; } = 0;

        public override int Read(byte[] buffer, int offset, int count)
        {
            int i = 0;
            for (; i < count && _input.MoveNext(); i++)
                buffer[i + offset] = _input.Current;
            return i;
        }
        public override void Write(byte[] buffer, int offset, int count) => throw new InvalidOperationException();

        public override long Seek(long offset, SeekOrigin origin) => throw new InvalidOperationException();
        public override void SetLength(long value) => throw new InvalidOperationException();
        public override void Flush() => throw new InvalidOperationException();

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_disposed)
                    return;
                _input.Dispose();
                _disposed=  true;
            }

            base.Dispose(disposing);
        }
    }

    public sealed class DataStoreChunkArrayWriterStream : Stream
    {
        private readonly IDataStore _dataStore;
        private readonly string _key;

        private const int _chunkLength = short.MaxValue - 1024;
        private int _chunkIndex;

        private byte[] _chunk = new byte[_chunkLength];
        private int _chunkPosition;

        private bool _disposed;

        public DataStoreChunkArrayWriterStream(IDataStore dataStore, string key)
        {
            _dataStore = dataStore;
            _key = key;
            var len = 1;
            _dataStore.SyncData($"{_key}_length", ref len);
        }

        public override bool CanRead => false;
        public override bool CanSeek => false;
        public override bool CanWrite => true;
        public override long Length { get; }
        public override long Position { get; set; } = 0;

        public override int Read(byte[] buffer, int offset, int count) => throw new InvalidOperationException();
        public override void Write(byte[] buffer, int offset, int count)
        {
            for (var i = 0; i < count; i++)
            {
                if (_chunkPosition >= _chunkLength)
                {
                    _dataStore.SyncData($"{_key}_{_chunkIndex++}", ref _chunk);
                    Array.Clear(_chunk, 0, _chunk.Length);
                    _chunkPosition = 0;

                    var len = _chunkIndex + 1;
                    _dataStore.SyncData($"{_key}_length", ref len);
                }

                _chunk[_chunkPosition++ + i] = buffer[i];
            }

            _dataStore.SyncData($"{_key}_{_chunkIndex}", ref _chunk);
        }

        public override long Seek(long offset, SeekOrigin origin) => throw new InvalidOperationException();
        public override void SetLength(long value) => throw new InvalidOperationException();
        public override void Flush() { }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_disposed)
                    return;

                _disposed=  true;
            }

            base.Dispose(disposing);
        }
    }


    public static class IDataStoreExtensions
    {
        private static IEnumerable<string> ToChunks(string str, int maxChunkSize)
        {
            for (var i = 0; i < str.Length; i += maxChunkSize)
                yield return str.Substring(i, Math.Min(maxChunkSize, str.Length-i));
        }

        private static string ChunksToString(string[] chunks)
        {
            if (chunks.Length == 0)
                return string.Empty;
            else if (chunks.Length == 1)
                return chunks[0];

            var strBuilder = new StringBuilder(short.MaxValue);

            foreach (var chunk in chunks)
                strBuilder.Append(chunk);

            return strBuilder.ToString();
        }

        public static bool SyncDataAsJson<T>(this IDataStore dataStore, string key, ref T data, JsonSerializerSettings? settings = null)
        {
            // If the type we're synchronizing is a string or string array, then it's ambiguous
            // with our own internal storage types, which imply that the strings contain valid
            // JSON. Standard binary serialization can handle these types just fine, so we avoid
            // the ambiguity by passing this data straight to the game's binary serializer.
            if (typeof(T) == typeof(string) || typeof(T) == typeof(string[]))
                return dataStore.SyncData(key, ref data);

            settings ??= new JsonSerializerSettings
            {
                ContractResolver = new TaleWorldsContractResolver(),
                Converters = { new DictionaryToArrayConverter(), new MBGUIDConverter(), new MBObjectBaseConverter() },
                TypeNameHandling = TypeNameHandling.Auto,
                //ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                //PreserveReferencesHandling = PreserveReferencesHandling.Objects
            };

            if (dataStore.IsSaving)
            {
                using var stream = new DataStoreChunkArrayWriterStream(dataStore, key);
                using var streamWriter = new StreamWriter(stream);
                using var jsonTextWriter = new JsonTextWriter(streamWriter);
                var serializer = JsonSerializer.Create(settings);
                serializer.Serialize(jsonTextWriter, new JsonData(2, JsonConvert.SerializeObject(data)));

                return true;
            }

            if (dataStore.IsLoading)
            {
                try
                {
                    using var stream = new DataStoreChunkArrayReaderStream(dataStore, key);
                    using var streamReader = new StreamReader(stream);
                    using var jsonTextReader = new JsonTextReader(streamReader);
                    var serializer = JsonSerializer.Create(settings);
                    var jsonData = serializer.Deserialize<JsonData>(jsonTextReader);
                    data = jsonData.Format switch
                    {
                        2 => JsonConvert.DeserializeObject<T>(jsonData.Data, settings),
                        _ => data
                    };
                    return true;
                }
                catch (Exception e) when (e is InvalidCastException) { }

                try
                {
                    // The game's save system limits the string to be of size of short.MaxValue
                    // We avoid this limitation by splitting the string into chunks.
                    var jsonDataChunks = Array.Empty<string>();
                    dataStore.SyncData(key, ref jsonDataChunks); // try to get as array of JSON string(s)
                    var jsonData = JsonConvert.DeserializeObject<JsonData>(ChunksToString(jsonDataChunks ?? Array.Empty<string>()), settings);
                    data = jsonData.Format switch
                    {
                        2 => JsonConvert.DeserializeObject<T>(jsonData.Data, settings),
                        _ => data
                    };
                    return true;
                }
                catch (Exception e) when (e is InvalidCastException) { }

                try
                {
                    // The first version of SyncDataAsJson stored the string as a single entity
                    var jsonData = "";
                    dataStore.SyncData(key, ref jsonData); // try to get as JSON string
                    data = JsonConvert.DeserializeObject<T>(jsonData, settings);
                    return true;
                }
                catch (Exception ex) when (ex is InvalidCastException) { }

                try
                {
                    // Most likely the save file stores the data with its default binary serialization
                    // We read it as it is, the next save will convert the data to JSON
                    return dataStore.SyncData(key, ref data);
                }
                catch (Exception ex) when (ex is InvalidCastException) { }
            }

            return false;
        }

        private sealed class JsonData
        {
            public int Format { get; private set; }
            public string Data { get; private set; }

            public JsonData(int format, string data)
            {
                Format = format;
                Data = data;
            }
        }
    }
}