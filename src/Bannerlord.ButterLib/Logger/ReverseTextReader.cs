﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Bannerlord.ButterLib.Logger
{
    /// <summary>
    /// Reads a text file backwards, line-by-line.
    /// </summary>
    /// <remarks>This class uses file seeking to read a text file of any size in reverse order.  This
    /// is useful for needs such as reading a log file newest-entries first.</remarks>
    internal sealed class ReverseTextReader : IEnumerable<string>
    {
        private const int BufferSize = 16384;   // The number of bytes read from the uderlying stream.
        private readonly Stream _stream;        // Stores the stream feeding data into this reader
        private readonly Encoding _encoding;    // Stores the encoding used to process the file
        private byte[] _leftoverBuffer;         // Stores the leftover partial line after processing a buffer
        private readonly Queue<string> _lines;  // Stores the lines parsed from the buffer

        #region Constructors

        /// <summary>
        /// Creates a reader using the specified stream.
        /// </summary>
        /// <param name="stream"></param>
        public ReverseTextReader(Stream stream) : this(stream, Encoding.Default) { }

        /// <summary>
        /// Creates a reader using the specified stream and encoding.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="encoding"></param>
        public ReverseTextReader(Stream stream, Encoding encoding)
        {
            _stream = stream;
            _encoding = encoding;
            _lines = new Queue<string>(128);
            // The stream needs to support seeking for this to work
            if (!_stream.CanSeek)
                throw new InvalidOperationException("The specified stream needs to support seeking to be read backwards.");
            if (!_stream.CanRead)
                throw new InvalidOperationException("The specified stream needs to support reading to be read backwards.");
            // Set the current position to the end of the file
            _stream.Position = _stream.Length;
            _leftoverBuffer = Array.Empty<byte>();
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Reads the next previous line from the underlying stream.
        /// </summary>
        /// <returns></returns>
        public string? ReadLine()
        {
            // Are there lines left to read? If so, return the next one
            if (_lines.Count != 0) return _lines.Dequeue();
            // Are we at the beginning of the stream? If so, we're done
            if (_stream.Position == 0) return null;

            #region Read and Process the Next Chunk

            // Remember the current position
            var currentPosition = _stream.Position;
            var newPosition = currentPosition - BufferSize;
            // Are we before the beginning of the stream?
            if (newPosition < 0) newPosition = 0;
            // Calculate the buffer size to read
            var count = (int) (currentPosition - newPosition);
            // Set the new position
            _stream.Position = newPosition;
            // Make a new buffer but append the previous leftovers
            var buffer = new byte[count + _leftoverBuffer.Length];
            // Read the next buffer
            _stream.Read(buffer, 0, count);
            // Move the position of the stream back
            _stream.Position = newPosition;
            // And copy in the leftovers from the last buffer
            if (_leftoverBuffer.Length != 0)
                Array.Copy(_leftoverBuffer, 0, buffer, count, _leftoverBuffer.Length);
            // Look for CrLf delimiters
            var end = buffer.Length - 1;
            var start = buffer.Length - 2;
            // Search backwards for a line feed
            while (start >= 0)
            {
                // Is it a line feed?
                if (buffer[start] == 10)
                {
                    // Yes.  Extract a line and queue it (but exclude the \r\n)
                    _lines.Enqueue(_encoding.GetString(buffer, start + 1, end - start - 2));
                    // And reset the end
                    end = start;
                }
                // Move to the previous character
                start--;
            }
            // What's left over is a portion of a line. Save it for later.
            _leftoverBuffer = new byte[end + 1];
            Array.Copy(buffer, 0, _leftoverBuffer, 0, end + 1);
            // Are we at the beginning of the stream?
            if (_stream.Position == 0)
                // Yes.  Add the last line.
                _lines.Enqueue(_encoding.GetString(_leftoverBuffer, 0, end - 1));

            #endregion

            // If we have something in the queue, return it
            return _lines.Count == 0 ? null : _lines.Dequeue();
        }

        #endregion

        #region IEnumerator<string> Interface

        public IEnumerator<string> GetEnumerator()
        {
            // So long as the next line isn't null...
            while (ReadLine() is { } line)
                // Read and return it.
                yield return line;
        }

        IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();

        #endregion
    }
}