namespace Bannerlord.ButterLib.CrashUploader
{
    internal record CrashUploaderResult
    {
        public static CrashUploaderResult Success(string url) => new(CrashUploaderStatus.Success) { Url = url };
        public static CrashUploaderResult MetadataNotFound() => new(CrashUploaderStatus.MetadataNotFound);
        public static CrashUploaderResult ResponseIsNotHttpWebResponse() => new(CrashUploaderStatus.ResponseIsNotHttpWebResponse);
        public static CrashUploaderResult WrongStatusCode(string statusCode) => new(CrashUploaderStatus.WrongStatusCode) { StatusCode = statusCode };
        public static CrashUploaderResult ResponseStreamIsNull() => new(CrashUploaderStatus.ResponseStreamIsNull);
        public static CrashUploaderResult FailedWithException(string exception) => new(CrashUploaderStatus.FailedWithException) { Exception = exception };

        public CrashUploaderStatus Status { get; }
        public string? Url { get; private init; }
        public string? StatusCode { get; private init; }
        public string? Exception { get; private init; }

        private CrashUploaderResult(CrashUploaderStatus status)
        {
            Status = status;
        }
    }
}