using System;

namespace Bannerlord.ButterLib.ExceptionHandler
{
    public class ExceptionReporter
    {
        public void Show(Exception exception)
        {
            if (BEWPatch.SuppressedExceptions.Contains((BEWPatch.ExceptionIdentifier.FromException(exception))))
            {
                BEWPatch.SuppressedExceptions.Remove(BEWPatch.ExceptionIdentifier.FromException(exception));
                return;
            }

            HtmlBuilder.BuildAndShow(new CrashReport(exception));
        }
    }
}