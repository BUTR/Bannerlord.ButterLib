using Bannerlord.BLSE;

using System;
using System.Diagnostics;

namespace Bannerlord.ButterLib.ExceptionHandler
{
    [BLSEExceptionHandler]
    public static class ExceptionReporter
    {
        private static void OnException(Exception exception) => Show(exception);
        
        public static void Show(Exception exception)
        {
            if (BEWPatch.SuppressedExceptions.Contains(BEWPatch.ExceptionIdentifier.FromException(exception)))
            {
                BEWPatch.SuppressedExceptions.Remove(BEWPatch.ExceptionIdentifier.FromException(exception));
                return;
            }

            HtmlBuilder.BuildAndShow(new CrashReport(exception.Demystify()));
        }
    }
}