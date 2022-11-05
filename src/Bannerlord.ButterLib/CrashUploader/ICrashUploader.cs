using Bannerlord.ButterLib.ExceptionHandler;

using System.Threading.Tasks;

namespace Bannerlord.ButterLib.CrashUploader
{
    internal interface ICrashUploader
    {
        Task<CrashUploaderResult> UploadAsync(CrashReport crashReport);
    }
}