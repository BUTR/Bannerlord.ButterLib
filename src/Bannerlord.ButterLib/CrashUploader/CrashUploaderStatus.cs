namespace Bannerlord.ButterLib.CrashUploader;

internal enum CrashUploaderStatus
{
    Success,
    MetadataNotFound,
    ResponseIsNotHttpWebResponse,
    WrongStatusCode,
    ResponseStreamIsNull,
    FailedWithException,
    UrlIsNullOrEmpty,
}