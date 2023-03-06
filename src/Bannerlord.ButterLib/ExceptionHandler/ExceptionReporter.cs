using System;
using System.Diagnostics;

namespace Bannerlord.ButterLib.ExceptionHandler
{
    public static class ExceptionReporter
    {
        public static void Show(Exception exception)
        {
            if (BEWPatch.SuppressedExceptions.Contains(BEWPatch.ExceptionIdentifier.FromException(exception)))
            {
                BEWPatch.SuppressedExceptions.Remove(BEWPatch.ExceptionIdentifier.FromException(exception));
                return;
            }

#if NET472
            HtmlBuilder.BuildAndShow(new CrashReport(exception.Demystify()));
#else
            HtmlBuilder.BuildAndShow(new CrashReport(exception));
#endif
        }
    }
}