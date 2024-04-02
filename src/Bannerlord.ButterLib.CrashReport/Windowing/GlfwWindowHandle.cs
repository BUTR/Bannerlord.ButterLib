namespace Bannerlord.ButterLib.CrashReportWindow.Windowing;

public readonly ref struct GlfwWindowHandle
{
    public static unsafe ref readonly GlfwWindowHandle NullRef() => ref *(GlfwWindowHandle*) null;

    public readonly nuint Handle;
}