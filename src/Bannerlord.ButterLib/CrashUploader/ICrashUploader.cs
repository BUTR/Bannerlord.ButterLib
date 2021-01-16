using Bannerlord.ButterLib.ExceptionHandler;

using System.Threading.Tasks;

namespace Bannerlord.ButterLib.CrashUploader
{
    internal interface ICrashUploader
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="crashReport"></param>
        /// <returns>Url to the report, <see langword="null" /> if it failed.</returns>
        Task<string?> UploadAsync(CrashReport crashReport);
    }
}