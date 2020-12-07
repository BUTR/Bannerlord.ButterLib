using System.Threading.Tasks;
using Bannerlord.ButterLib.ExceptionHandler;

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