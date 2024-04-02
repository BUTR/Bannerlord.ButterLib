using System.Diagnostics;

namespace Bannerlord.ButterLib.CrashReportWindow.OpenGL;

partial class Gl
{
    [Conditional("DEBUG")]
    public void CheckGlError(string title)
    {
        var errorCode = GetError();
        if (errorCode is GL_NO_ERROR) return;

        Debug.Print($"GL {title}: {errorCode}");
    }
}