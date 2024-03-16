#nullable disable
#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value
using Bannerlord.ButterLib.CrashReportWindow.UnsafeUtils;

using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace Bannerlord.ButterLib.CrashReportWindow.OpenGL;

internal static partial class GL
{
    static GL()
    {
        foreach (var field in typeof(GL).GetFields(BindingFlags.NonPublic | BindingFlags.Static))
        {
            var name = field.Name.Remove(0, 1).Replace("Ptr", string.Empty);
            var ptr = GLFW.glfwGetProcAddress(name);
            if (ptr != IntPtr.Zero)
                field.SetValue(null, Marshal.GetDelegateForFunctionPointer(ptr, field.FieldType));
        }
    }

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate IntPtr GLDEBUGPROC(uint source, uint type, uint id, uint severity, int length, string message, IntPtr userParam);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate IntPtr PFNGLGETSTRINGPROC(uint name);
    private static PFNGLGETSTRINGPROC _glGetString;
    public static string glGetString(uint name) => Marshal.PtrToStringAnsi(_glGetString(name));

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLSHADERSOURCEPROC(uint shader, int count, string[] @string, ref readonly int length);
    private static PFNGLSHADERSOURCEPROC _glShaderSource;
    public static void glShaderSource(uint shader, int count, string[] @string, ref readonly int length) =>
        _glShaderSource(shader, count, @string, in length);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETPROGRAMINFOLOGPROC(uint program, int bufSize, out int length, byte[] infoLog);
    private static PFNGLGETPROGRAMINFOLOGPROC _glGetProgramInfoLog;
    public static void glGetProgramInfoLog(uint program, int bufSize, out int length, out string infoLog)
    {
        var data = new byte[bufSize];
        _glGetProgramInfoLog(program, bufSize, out length, data);
        infoLog = Encoding.ASCII.GetString(data);
    }

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETSHADERINFOLOGPROC(uint shader, int bufSize, out int length, byte[] infoLog);
    private static PFNGLGETSHADERINFOLOGPROC _glGetShaderInfoLog;
    public static void glGetShaderInfoLog(uint shader, int bufSize, out int length, out string infoLog)
    {
        var data = new byte[bufSize];
        _glGetShaderInfoLog(shader, bufSize, out length, data);
        infoLog = Encoding.ASCII.GetString(data);
    }

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCULLFACEPROC(uint mode);
    private static PFNGLCULLFACEPROC _glCullFace;
    public static void glCullFace(uint mode) => _glCullFace(mode);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLFRONTFACEPROC(uint mode);
    private static PFNGLFRONTFACEPROC _glFrontFace;
    public static void glFrontFace(uint mode) => _glFrontFace(mode);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLHINTPROC(uint target, uint mode);
    private static PFNGLHINTPROC _glHint;
    public static void glHint(uint target, uint mode) => _glHint(target, mode);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLLINEWIDTHPROC(float width);
    private static PFNGLLINEWIDTHPROC _glLineWidth;
    public static void glLineWidth(float width) => _glLineWidth(width);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPOINTSIZEPROC(float size);
    private static PFNGLPOINTSIZEPROC _glPointSize;
    public static void glPointSize(float size) => _glPointSize(size);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPOLYGONMODEPROC(uint face, uint mode);
    private static PFNGLPOLYGONMODEPROC _glPolygonMode;
    public static void glPolygonMode(uint face, uint mode) => _glPolygonMode(face, mode);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLSCISSORPROC(int x, int y, int width, int height);
    private static PFNGLSCISSORPROC _glScissor;
    public static void glScissor(int x, int y, int width, int height) => _glScissor(x, y, width, height);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXPARAMETERFPROC(uint target, uint pname, float param);
    private static PFNGLTEXPARAMETERFPROC _glTexParameterf;
    public static void glTexParameterf(uint target, uint pname, float param) => _glTexParameterf(target, pname, param);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXPARAMETERFVPROC(uint target, uint pname, ref readonly float @params);
    private static PFNGLTEXPARAMETERFVPROC _glTexParameterfv;
    public static void glTexParameterfv(uint target, uint pname, ref readonly float @params) => _glTexParameterfv(target, pname, in @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXPARAMETERIPROC(uint target, uint pname, int param);
    private static PFNGLTEXPARAMETERIPROC _glTexParameteri;
    public static void glTexParameteri(uint target, uint pname, int param) => _glTexParameteri(target, pname, param);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXPARAMETERIVPROC(uint target, uint pname, ref readonly int @params);
    private static PFNGLTEXPARAMETERIVPROC _glTexParameteriv;
    public static void glTexParameteriv(uint target, uint pname, ref readonly int @params) => _glTexParameteriv(target, pname, in @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXIMAGE1DPROC(uint target, int level, int internalformat, int width, int border, uint format, uint type, IntPtr pixels);
    private static PFNGLTEXIMAGE1DPROC _glTexImage1D;
    public static void glTexImage1D(uint target, int level, int internalformat, int width, int border, uint format, uint type, IntPtr pixels) => _glTexImage1D(target, level, internalformat, width, border, format, type, pixels);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXIMAGE2DPROC(uint target, int level, int internalformat, int width, int height, int border, uint format, uint type, IntPtr pixels);
    private static PFNGLTEXIMAGE2DPROC _glTexImage2D;
    public static void glTexImage2D(uint target, int level, int internalformat, int width, int height, int border, uint format, uint type, IntPtr pixels) => _glTexImage2D(target, level, internalformat, width, height, border, format, type, pixels);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDRAWBUFFERPROC(uint buf);
    private static PFNGLDRAWBUFFERPROC _glDrawBuffer;
    public static void glDrawBuffer(uint buf) => _glDrawBuffer(buf);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCLEARPROC(uint mask);
    private static PFNGLCLEARPROC _glClear;
    public static void glClear(uint mask) => _glClear(mask);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCLEARCOLORPROC(float red, float green, float blue, float alpha);
    private static PFNGLCLEARCOLORPROC _glClearColor;
    public static void glClearColor(float red, float green, float blue, float alpha) => _glClearColor(red, green, blue, alpha);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCLEARSTENCILPROC(int s);
    private static PFNGLCLEARSTENCILPROC _glClearStencil;
    public static void glClearStencil(int s) => _glClearStencil(s);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCLEARDEPTHPROC(double depth);
    private static PFNGLCLEARDEPTHPROC _glClearDepth;
    public static void glClearDepth(double depth) => _glClearDepth(depth);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLSTENCILMASKPROC(uint mask);
    private static PFNGLSTENCILMASKPROC _glStencilMask;
    public static void glStencilMask(uint mask) => _glStencilMask(mask);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCOLORMASKPROC(bool red, bool green, bool blue, bool alpha);
    private static PFNGLCOLORMASKPROC _glColorMask;
    public static void glColorMask(bool red, bool green, bool blue, bool alpha) => _glColorMask(red, green, blue, alpha);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDEPTHMASKPROC(bool flag);
    private static PFNGLDEPTHMASKPROC _glDepthMask;
    public static void glDepthMask(bool flag) => _glDepthMask(flag);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDISABLEPROC(uint cap);
    private static PFNGLDISABLEPROC _glDisable;
    public static void glDisable(uint cap) => _glDisable(cap);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLENABLEPROC(uint cap);
    private static PFNGLENABLEPROC _glEnable;
    public static void glEnable(uint cap) => _glEnable(cap);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLFINISHPROC();
    private static PFNGLFINISHPROC _glFinish;
    public static void glFinish() => _glFinish();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLFLUSHPROC();
    private static PFNGLFLUSHPROC _glFlush;
    public static void glFlush() => _glFlush();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBLENDFUNCPROC(uint sfactor, uint dfactor);
    private static PFNGLBLENDFUNCPROC _glBlendFunc;
    public static void glBlendFunc(uint sfactor, uint dfactor) => _glBlendFunc(sfactor, dfactor);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLLOGICOPPROC(uint opcode);
    private static PFNGLLOGICOPPROC _glLogicOp;
    public static void glLogicOp(uint opcode) => _glLogicOp(opcode);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLSTENCILFUNCPROC(uint func, int @ref, uint mask);
    private static PFNGLSTENCILFUNCPROC _glStencilFunc;
    public static void glStencilFunc(uint func, int @ref, uint mask) => _glStencilFunc(func, @ref, mask);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLSTENCILOPPROC(uint fail, uint zfail, uint zpass);
    private static PFNGLSTENCILOPPROC _glStencilOp;
    public static void glStencilOp(uint fail, uint zfail, uint zpass) => _glStencilOp(fail, zfail, zpass);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDEPTHFUNCPROC(uint func);
    private static PFNGLDEPTHFUNCPROC _glDepthFunc;
    public static void glDepthFunc(uint func) => _glDepthFunc(func);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPIXELSTOREFPROC(uint pname, float param);
    private static PFNGLPIXELSTOREFPROC _glPixelStoref;
    public static void glPixelStoref(uint pname, float param) => _glPixelStoref(pname, param);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPIXELSTOREIPROC(uint pname, int param);
    private static PFNGLPIXELSTOREIPROC _glPixelStorei;
    public static void glPixelStorei(uint pname, int param) => _glPixelStorei(pname, param);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLREADBUFFERPROC(uint src);
    private static PFNGLREADBUFFERPROC _glReadBuffer;
    public static void glReadBuffer(uint src) => _glReadBuffer(src);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLREADPIXELSPROC(int x, int y, int width, int height, uint format, uint type, IntPtr pixels);
    private static PFNGLREADPIXELSPROC _glReadPixels;
    public static void glReadPixels(int x, int y, int width, int height, uint format, uint type, IntPtr pixels) => _glReadPixels(x, y, width, height, format, type, pixels);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETBOOLEANVPROC(uint pname, out bool data);
    private static PFNGLGETBOOLEANVPROC _glGetBooleanv;
    public static void glGetBooleanv(uint pname, out bool data) => _glGetBooleanv(pname, out data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETDOUBLEVPROC(uint pname, out double data);
    private static PFNGLGETDOUBLEVPROC _glGetDoublev;
    public static void glGetDoublev(uint pname, out double data) => _glGetDoublev(pname, out data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate uint PFNGLGETERRORPROC();
    private static PFNGLGETERRORPROC _glGetError;
    public static uint glGetError() => _glGetError();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETFLOATVPROC(uint pname, out float data);
    private static PFNGLGETFLOATVPROC _glGetFloatv;
    public static void glGetFloatv(uint pname, out float data) => _glGetFloatv(pname, out data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETINTEGERVPROC(uint pname, out int data);
    private static PFNGLGETINTEGERVPROC _glGetIntegerv;
    public static void glGetIntegerv(uint pname, out int data) => _glGetIntegerv(pname, out data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETTEXIMAGEPROC(uint target, int level, uint format, uint type, IntPtr pixels);
    private static PFNGLGETTEXIMAGEPROC _glGetTexImage;
    public static void glGetTexImage(uint target, int level, uint format, uint type, IntPtr pixels) => _glGetTexImage(target, level, format, type, pixels);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETTEXPARAMETERFVPROC(uint target, uint pname, out float @params);
    private static PFNGLGETTEXPARAMETERFVPROC _glGetTexParameterfv;
    public static void glGetTexParameterfv(uint target, uint pname, out float @params) => _glGetTexParameterfv(target, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETTEXPARAMETERIVPROC(uint target, uint pname, out int @params);
    private static PFNGLGETTEXPARAMETERIVPROC _glGetTexParameteriv;
    public static void glGetTexParameteriv(uint target, uint pname, out int @params) => _glGetTexParameteriv(target, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETTEXLEVELPARAMETERFVPROC(uint target, int level, uint pname, out float @params);
    private static PFNGLGETTEXLEVELPARAMETERFVPROC _glGetTexLevelParameterfv;
    public static void glGetTexLevelParameterfv(uint target, int level, uint pname, out float @params) => _glGetTexLevelParameterfv(target, level, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETTEXLEVELPARAMETERIVPROC(uint target, int level, uint pname, out int @params);
    private static PFNGLGETTEXLEVELPARAMETERIVPROC _glGetTexLevelParameteriv;
    public static void glGetTexLevelParameteriv(uint target, int level, uint pname, out int @params) => _glGetTexLevelParameteriv(target, level, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate bool PFNGLISENABLEDPROC(uint cap);
    private static PFNGLISENABLEDPROC _glIsEnabled;
    public static bool glIsEnabled(uint cap) => _glIsEnabled(cap);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDEPTHRANGEPROC(double n, double f);
    private static PFNGLDEPTHRANGEPROC _glDepthRange;
    public static void glDepthRange(double n, double f) => _glDepthRange(n, f);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVIEWPORTPROC(int x, int y, int width, int height);
    private static PFNGLVIEWPORTPROC _glViewport;
    public static void glViewport(int x, int y, int width, int height) => _glViewport(x, y, width, height);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDRAWARRAYSPROC(uint mode, int first, int count);
    private static PFNGLDRAWARRAYSPROC _glDrawArrays;
    public static void glDrawArrays(uint mode, int first, int count) => _glDrawArrays(mode, first, count);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDRAWELEMENTSPROC(uint mode, int count, uint type, IntPtr indices);
    private static PFNGLDRAWELEMENTSPROC _glDrawElements;
    public static void glDrawElements(uint mode, int count, uint type, IntPtr indices) => _glDrawElements(mode, count, type, indices);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPOLYGONOFFSETPROC(float factor, float units);
    private static PFNGLPOLYGONOFFSETPROC _glPolygonOffset;
    public static void glPolygonOffset(float factor, float units) => _glPolygonOffset(factor, units);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCOPYTEXIMAGE1DPROC(uint target, int level, uint internalformat, int x, int y, int width, int border);
    private static PFNGLCOPYTEXIMAGE1DPROC _glCopyTexImage1D;
    public static void glCopyTexImage1D(uint target, int level, uint internalformat, int x, int y, int width, int border) => _glCopyTexImage1D(target, level, internalformat, x, y, width, border);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCOPYTEXIMAGE2DPROC(uint target, int level, uint internalformat, int x, int y, int width, int height, int border);
    private static PFNGLCOPYTEXIMAGE2DPROC _glCopyTexImage2D;
    public static void glCopyTexImage2D(uint target, int level, uint internalformat, int x, int y, int width, int height, int border) => _glCopyTexImage2D(target, level, internalformat, x, y, width, height, border);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCOPYTEXSUBIMAGE1DPROC(uint target, int level, int xoffset, int x, int y, int width);
    private static PFNGLCOPYTEXSUBIMAGE1DPROC _glCopyTexSubImage1D;
    public static void glCopyTexSubImage1D(uint target, int level, int xoffset, int x, int y, int width) => _glCopyTexSubImage1D(target, level, xoffset, x, y, width);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCOPYTEXSUBIMAGE2DPROC(uint target, int level, int xoffset, int yoffset, int x, int y, int width, int height);
    private static PFNGLCOPYTEXSUBIMAGE2DPROC _glCopyTexSubImage2D;
    public static void glCopyTexSubImage2D(uint target, int level, int xoffset, int yoffset, int x, int y, int width, int height) => _glCopyTexSubImage2D(target, level, xoffset, yoffset, x, y, width, height);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXSUBIMAGE1DPROC(uint target, int level, int xoffset, int width, uint format, uint type, IntPtr pixels);
    private static PFNGLTEXSUBIMAGE1DPROC _glTexSubImage1D;
    public static void glTexSubImage1D(uint target, int level, int xoffset, int width, uint format, uint type, IntPtr pixels) => _glTexSubImage1D(target, level, xoffset, width, format, type, pixels);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXSUBIMAGE2DPROC(uint target, int level, int xoffset, int yoffset, int width, int height, uint format, uint type, IntPtr pixels);
    private static PFNGLTEXSUBIMAGE2DPROC _glTexSubImage2D;
    public static void glTexSubImage2D(uint target, int level, int xoffset, int yoffset, int width, int height, uint format, uint type, IntPtr pixels) => _glTexSubImage2D(target, level, xoffset, yoffset, width, height, format, type, pixels);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBINDTEXTUREPROC(uint target, uint texture);
    private static PFNGLBINDTEXTUREPROC _glBindTexture;
    public static void glBindTexture(uint target, uint texture) => _glBindTexture(target, texture);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDELETETEXTURESPROC(int n, ref readonly uint textures);
    private static PFNGLDELETETEXTURESPROC _glDeleteTextures;
    public static void glDeleteTextures(int n, ref readonly uint textures) => _glDeleteTextures(n, in textures);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGENTEXTURESPROC(int n, out uint textures);
    private static PFNGLGENTEXTURESPROC _glGenTextures;
    public static void glGenTextures(int n, out uint textures) => _glGenTextures(n, out textures);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate bool PFNGLISTEXTUREPROC(uint texture);
    private static PFNGLISTEXTUREPROC _glIsTexture;
    public static bool glIsTexture(uint texture) => _glIsTexture(texture);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDRAWRANGEELEMENTSPROC(uint mode, uint start, uint end, int count, uint type, IntPtr indices);
    private static PFNGLDRAWRANGEELEMENTSPROC _glDrawRangeElements;
    public static void glDrawRangeElements(uint mode, uint start, uint end, int count, uint type, IntPtr indices) => _glDrawRangeElements(mode, start, end, count, type, indices);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXIMAGE3DPROC(uint target, int level, int internalformat, int width, int height, int depth, int border, uint format, uint type, IntPtr pixels);
    private static PFNGLTEXIMAGE3DPROC _glTexImage3D;
    public static void glTexImage3D(uint target, int level, int internalformat, int width, int height, int depth, int border, uint format, uint type, IntPtr pixels) => _glTexImage3D(target, level, internalformat, width, height, depth, border, format, type, pixels);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXSUBIMAGE3DPROC(uint target, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, uint type, IntPtr pixels);
    private static PFNGLTEXSUBIMAGE3DPROC _glTexSubImage3D;
    public static void glTexSubImage3D(uint target, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, uint type, IntPtr pixels) => _glTexSubImage3D(target, level, xoffset, yoffset, zoffset, width, height, depth, format, type, pixels);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCOPYTEXSUBIMAGE3DPROC(uint target, int level, int xoffset, int yoffset, int zoffset, int x, int y, int width, int height);
    private static PFNGLCOPYTEXSUBIMAGE3DPROC _glCopyTexSubImage3D;
    public static void glCopyTexSubImage3D(uint target, int level, int xoffset, int yoffset, int zoffset, int x, int y, int width, int height) => _glCopyTexSubImage3D(target, level, xoffset, yoffset, zoffset, x, y, width, height);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLACTIVETEXTUREPROC(uint texture);
    private static PFNGLACTIVETEXTUREPROC _glActiveTexture;
    public static void glActiveTexture(uint texture) => _glActiveTexture(texture);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLSAMPLECOVERAGEPROC(float value, bool invert);
    private static PFNGLSAMPLECOVERAGEPROC _glSampleCoverage;
    public static void glSampleCoverage(float value, bool invert) => _glSampleCoverage(value, invert);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCOMPRESSEDTEXIMAGE3DPROC(uint target, int level, uint internalformat, int width, int height, int depth, int border, int imageSize, IntPtr data);
    private static PFNGLCOMPRESSEDTEXIMAGE3DPROC _glCompressedTexImage3D;
    public static void glCompressedTexImage3D(uint target, int level, uint internalformat, int width, int height, int depth, int border, int imageSize, IntPtr data) => _glCompressedTexImage3D(target, level, internalformat, width, height, depth, border, imageSize, data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCOMPRESSEDTEXIMAGE2DPROC(uint target, int level, uint internalformat, int width, int height, int border, int imageSize, IntPtr data);
    private static PFNGLCOMPRESSEDTEXIMAGE2DPROC _glCompressedTexImage2D;
    public static void glCompressedTexImage2D(uint target, int level, uint internalformat, int width, int height, int border, int imageSize, IntPtr data) => _glCompressedTexImage2D(target, level, internalformat, width, height, border, imageSize, data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCOMPRESSEDTEXIMAGE1DPROC(uint target, int level, uint internalformat, int width, int border, int imageSize, IntPtr data);
    private static PFNGLCOMPRESSEDTEXIMAGE1DPROC _glCompressedTexImage1D;
    public static void glCompressedTexImage1D(uint target, int level, uint internalformat, int width, int border, int imageSize, IntPtr data) => _glCompressedTexImage1D(target, level, internalformat, width, border, imageSize, data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCOMPRESSEDTEXSUBIMAGE3DPROC(uint target, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, int imageSize, IntPtr data);
    private static PFNGLCOMPRESSEDTEXSUBIMAGE3DPROC _glCompressedTexSubImage3D;
    public static void glCompressedTexSubImage3D(uint target, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, int imageSize, IntPtr data) => _glCompressedTexSubImage3D(target, level, xoffset, yoffset, zoffset, width, height, depth, format, imageSize, data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCOMPRESSEDTEXSUBIMAGE2DPROC(uint target, int level, int xoffset, int yoffset, int width, int height, uint format, int imageSize, IntPtr data);
    private static PFNGLCOMPRESSEDTEXSUBIMAGE2DPROC _glCompressedTexSubImage2D;
    public static void glCompressedTexSubImage2D(uint target, int level, int xoffset, int yoffset, int width, int height, uint format, int imageSize, IntPtr data) => _glCompressedTexSubImage2D(target, level, xoffset, yoffset, width, height, format, imageSize, data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCOMPRESSEDTEXSUBIMAGE1DPROC(uint target, int level, int xoffset, int width, uint format, int imageSize, IntPtr data);
    private static PFNGLCOMPRESSEDTEXSUBIMAGE1DPROC _glCompressedTexSubImage1D;
    public static void glCompressedTexSubImage1D(uint target, int level, int xoffset, int width, uint format, int imageSize, IntPtr data) => _glCompressedTexSubImage1D(target, level, xoffset, width, format, imageSize, data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETCOMPRESSEDTEXIMAGEPROC(uint target, int level, IntPtr img);
    private static PFNGLGETCOMPRESSEDTEXIMAGEPROC _glGetCompressedTexImage;
    public static void glGetCompressedTexImage(uint target, int level, IntPtr img) => _glGetCompressedTexImage(target, level, img);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBLENDFUNCSEPARATEPROC(uint sfactorRGB, uint dfactorRGB, uint sfactorAlpha, uint dfactorAlpha);
    private static PFNGLBLENDFUNCSEPARATEPROC _glBlendFuncSeparate;
    public static void glBlendFuncSeparate(uint sfactorRGB, uint dfactorRGB, uint sfactorAlpha, uint dfactorAlpha) => _glBlendFuncSeparate(sfactorRGB, dfactorRGB, sfactorAlpha, dfactorAlpha);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLMULTIDRAWARRAYSPROC(uint mode, ref readonly int first, ref readonly int count, int drawcount);
    private static PFNGLMULTIDRAWARRAYSPROC _glMultiDrawArrays;
    public static void glMultiDrawArrays(uint mode, ref readonly int first, ref readonly int count, int drawcount) => _glMultiDrawArrays(mode, in first, in count, drawcount);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLMULTIDRAWELEMENTSPROC(uint mode, ref readonly int count, uint type, IntPtr indices, int drawcount);
    private static PFNGLMULTIDRAWELEMENTSPROC _glMultiDrawElements;
    public static void glMultiDrawElements(uint mode, ref readonly int count, uint type, IntPtr indices, int drawcount) => _glMultiDrawElements(mode, in count, type, indices, drawcount);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPOINTPARAMETERFPROC(uint pname, float param);
    private static PFNGLPOINTPARAMETERFPROC _glPointParameterf;
    public static void glPointParameterf(uint pname, float param) => _glPointParameterf(pname, param);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPOINTPARAMETERFVPROC(uint pname, ref readonly float @params);
    private static PFNGLPOINTPARAMETERFVPROC _glPointParameterfv;
    public static void glPointParameterfv(uint pname, ref readonly float @params) => _glPointParameterfv(pname, in @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPOINTPARAMETERIPROC(uint pname, int param);
    private static PFNGLPOINTPARAMETERIPROC _glPointParameteri;
    public static void glPointParameteri(uint pname, int param) => _glPointParameteri(pname, param);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPOINTPARAMETERIVPROC(uint pname, ref readonly int @params);
    private static PFNGLPOINTPARAMETERIVPROC _glPointParameteriv;
    public static void glPointParameteriv(uint pname, ref readonly int @params) => _glPointParameteriv(pname, in @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBLENDCOLORPROC(float red, float green, float blue, float alpha);
    private static PFNGLBLENDCOLORPROC _glBlendColor;
    public static void glBlendColor(float red, float green, float blue, float alpha) => _glBlendColor(red, green, blue, alpha);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBLENDEQUATIONPROC(uint mode);
    private static PFNGLBLENDEQUATIONPROC _glBlendEquation;
    public static void glBlendEquation(uint mode) => _glBlendEquation(mode);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGENQUERIESPROC(int n, out uint ids);
    private static PFNGLGENQUERIESPROC _glGenQueries;
    public static void glGenQueries(int n, out uint ids) => _glGenQueries(n, out ids);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDELETEQUERIESPROC(int n, ref readonly uint ids);
    private static PFNGLDELETEQUERIESPROC _glDeleteQueries;
    public static void glDeleteQueries(int n, ref readonly uint ids) => _glDeleteQueries(n, in ids);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate bool PFNGLISQUERYPROC(uint id);
    private static PFNGLISQUERYPROC _glIsQuery;
    public static bool glIsQuery(uint id) => _glIsQuery(id);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBEGINQUERYPROC(uint target, uint id);
    private static PFNGLBEGINQUERYPROC _glBeginQuery;
    public static void glBeginQuery(uint target, uint id) => _glBeginQuery(target, id);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLENDQUERYPROC(uint target);
    private static PFNGLENDQUERYPROC _glEndQuery;
    public static void glEndQuery(uint target) => _glEndQuery(target);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETQUERYIVPROC(uint target, uint pname, out int @params);
    private static PFNGLGETQUERYIVPROC _glGetQueryiv;
    public static void glGetQueryiv(uint target, uint pname, out int @params) => _glGetQueryiv(target, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETQUERYOBJECTIVPROC(uint id, uint pname, out int @params);
    private static PFNGLGETQUERYOBJECTIVPROC _glGetQueryObjectiv;
    public static void glGetQueryObjectiv(uint id, uint pname, out int @params) => _glGetQueryObjectiv(id, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETQUERYOBJECTUIVPROC(uint id, uint pname, out uint @params);
    private static PFNGLGETQUERYOBJECTUIVPROC _glGetQueryObjectuiv;
    public static void glGetQueryObjectuiv(uint id, uint pname, out uint @params) => _glGetQueryObjectuiv(id, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBINDBUFFERPROC(uint target, uint buffer);
    private static PFNGLBINDBUFFERPROC _glBindBuffer;
    public static void glBindBuffer(uint target, uint buffer) => _glBindBuffer(target, buffer);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDELETEBUFFERSPROC(int n, ref readonly uint buffers);
    private static PFNGLDELETEBUFFERSPROC _glDeleteBuffers;
    public static void glDeleteBuffers(int n, ref readonly uint buffers) => _glDeleteBuffers(n, in buffers);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGENBUFFERSPROC(int n, out uint buffers);
    private static PFNGLGENBUFFERSPROC _glGenBuffers;
    public static void glGenBuffers(int n, out uint buffers) => _glGenBuffers(n, out buffers);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate bool PFNGLISBUFFERPROC(uint buffer);
    private static PFNGLISBUFFERPROC _glIsBuffer;
    public static bool glIsBuffer(uint buffer) => _glIsBuffer(buffer);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBUFFERDATAPROC(uint target, ulong size, IntPtr data, uint usage);
    private static PFNGLBUFFERDATAPROC _glBufferData;
    public static void glBufferData(uint target, ulong size, IntPtr data, uint usage) => _glBufferData(target, size, data, usage);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBUFFERSUBDATAPROC(uint target, ulong offset, ulong size, IntPtr data);
    private static PFNGLBUFFERSUBDATAPROC _glBufferSubData;
    public static void glBufferSubData(uint target, ulong offset, ulong size, IntPtr data) => _glBufferSubData(target, offset, size, data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETBUFFERSUBDATAPROC(uint target, ulong offset, ulong size, IntPtr data);
    private static PFNGLGETBUFFERSUBDATAPROC _glGetBufferSubData;
    public static void glGetBufferSubData(uint target, ulong offset, ulong size, IntPtr data) => _glGetBufferSubData(target, offset, size, data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate IntPtr PFNGLMAPBUFFERPROC(uint target, uint access);
    private static PFNGLMAPBUFFERPROC _glMapBuffer;
    public static IntPtr glMapBuffer(uint target, uint access) => _glMapBuffer(target, access);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate bool PFNGLUNMAPBUFFERPROC(uint target);
    private static PFNGLUNMAPBUFFERPROC _glUnmapBuffer;
    public static bool glUnmapBuffer(uint target) => _glUnmapBuffer(target);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETBUFFERPARAMETERIVPROC(uint target, uint pname, out int @params);
    private static PFNGLGETBUFFERPARAMETERIVPROC _glGetBufferParameteriv;
    public static void glGetBufferParameteriv(uint target, uint pname, out int @params) => _glGetBufferParameteriv(target, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETBUFFERPOINTERVPROC(uint target, uint pname, IntPtr @params);
    private static PFNGLGETBUFFERPOINTERVPROC _glGetBufferPointerv;
    public static void glGetBufferPointerv(uint target, uint pname, IntPtr @params) => _glGetBufferPointerv(target, pname, @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBLENDEQUATIONSEPARATEPROC(uint modeRGB, uint modeAlpha);
    private static PFNGLBLENDEQUATIONSEPARATEPROC _glBlendEquationSeparate;
    public static void glBlendEquationSeparate(uint modeRGB, uint modeAlpha) => _glBlendEquationSeparate(modeRGB, modeAlpha);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDRAWBUFFERSPROC(int n, ref readonly uint bufs);
    private static PFNGLDRAWBUFFERSPROC _glDrawBuffers;
    public static void glDrawBuffers(int n, ref readonly uint bufs) => _glDrawBuffers(n, in bufs);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLSTENCILOPSEPARATEPROC(uint face, uint sfail, uint dpfail, uint dppass);
    private static PFNGLSTENCILOPSEPARATEPROC _glStencilOpSeparate;
    public static void glStencilOpSeparate(uint face, uint sfail, uint dpfail, uint dppass) => _glStencilOpSeparate(face, sfail, dpfail, dppass);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLSTENCILFUNCSEPARATEPROC(uint face, uint func, int @ref, uint mask);
    private static PFNGLSTENCILFUNCSEPARATEPROC _glStencilFuncSeparate;
    public static void glStencilFuncSeparate(uint face, uint func, int @ref, uint mask) => _glStencilFuncSeparate(face, func, @ref, mask);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLSTENCILMASKSEPARATEPROC(uint face, uint mask);
    private static PFNGLSTENCILMASKSEPARATEPROC _glStencilMaskSeparate;
    public static void glStencilMaskSeparate(uint face, uint mask) => _glStencilMaskSeparate(face, mask);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLATTACHSHADERPROC(uint program, uint shader);
    private static PFNGLATTACHSHADERPROC _glAttachShader;
    public static void glAttachShader(uint program, uint shader) => _glAttachShader(program, shader);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBINDATTRIBLOCATIONPROC(uint program, uint index, string name);
    private static PFNGLBINDATTRIBLOCATIONPROC _glBindAttribLocation;
    public static void glBindAttribLocation(uint program, uint index, string name) => _glBindAttribLocation(program, index, name);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCOMPILESHADERPROC(uint shader);
    private static PFNGLCOMPILESHADERPROC _glCompileShader;
    public static void glCompileShader(uint shader) => _glCompileShader(shader);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate uint PFNGLCREATEPROGRAMPROC();
    private static PFNGLCREATEPROGRAMPROC _glCreateProgram;
    public static uint glCreateProgram() => _glCreateProgram();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate uint PFNGLCREATESHADERPROC(uint type);
    private static PFNGLCREATESHADERPROC _glCreateShader;
    public static uint glCreateShader(uint type) => _glCreateShader(type);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDELETEPROGRAMPROC(uint program);
    private static PFNGLDELETEPROGRAMPROC _glDeleteProgram;
    public static void glDeleteProgram(uint program) => _glDeleteProgram(program);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDELETESHADERPROC(uint shader);
    private static PFNGLDELETESHADERPROC _glDeleteShader;
    public static void glDeleteShader(uint shader) => _glDeleteShader(shader);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDETACHSHADERPROC(uint program, uint shader);
    private static PFNGLDETACHSHADERPROC _glDetachShader;
    public static void glDetachShader(uint program, uint shader) => _glDetachShader(program, shader);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDISABLEVERTEXATTRIBARRAYPROC(uint index);
    private static PFNGLDISABLEVERTEXATTRIBARRAYPROC _glDisableVertexAttribArray;
    public static void glDisableVertexAttribArray(uint index) => _glDisableVertexAttribArray(index);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLENABLEVERTEXATTRIBARRAYPROC(uint index);
    private static PFNGLENABLEVERTEXATTRIBARRAYPROC _glEnableVertexAttribArray;
    public static void glEnableVertexAttribArray(uint index) => _glEnableVertexAttribArray(index);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETACTIVEATTRIBPROC(uint program, uint index, int bufSize, out int length, out int size, out uint type, string name);
    private static PFNGLGETACTIVEATTRIBPROC _glGetActiveAttrib;
    public static void glGetActiveAttrib(uint program, uint index, int bufSize, out int length, out int size, out uint type, string name) => _glGetActiveAttrib(program, index, bufSize, out length, out size, out type, name);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETACTIVEUNIFORMPROC(uint program, uint index, int bufSize, out int length, out int size, out uint type, string name);
    private static PFNGLGETACTIVEUNIFORMPROC _glGetActiveUniform;
    public static void glGetActiveUniform(uint program, uint index, int bufSize, out int length, out int size, out uint type, string name) => _glGetActiveUniform(program, index, bufSize, out length, out size, out type, name);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETATTACHEDSHADERSPROC(uint program, int maxCount, out int count, out uint shaders);
    private static PFNGLGETATTACHEDSHADERSPROC _glGetAttachedShaders;
    public static void glGetAttachedShaders(uint program, int maxCount, out int count, out uint shaders) => _glGetAttachedShaders(program, maxCount, out count, out shaders);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int PFNGLGETATTRIBLOCATIONPROC(uint program, string name);
    private static PFNGLGETATTRIBLOCATIONPROC _glGetAttribLocation;
    public static int glGetAttribLocation(uint program, string name) => _glGetAttribLocation(program, name);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETPROGRAMIVPROC(uint program, uint pname, out int @params);
    private static PFNGLGETPROGRAMIVPROC _glGetProgramiv;
    public static void glGetProgramiv(uint program, uint pname, out int @params) => _glGetProgramiv(program, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETSHADERIVPROC(uint shader, uint pname, out int @params);
    private static PFNGLGETSHADERIVPROC _glGetShaderiv;
    public static void glGetShaderiv(uint shader, uint pname, out int @params) => _glGetShaderiv(shader, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETSHADERSOURCEPROC(uint shader, int bufSize, out int length, string source);
    private static PFNGLGETSHADERSOURCEPROC _glGetShaderSource;
    public static void glGetShaderSource(uint shader, int bufSize, out int length, string source) => _glGetShaderSource(shader, bufSize, out length, source);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int PFNGLGETUNIFORMLOCATIONPROC(uint program, string name);
    private static PFNGLGETUNIFORMLOCATIONPROC _glGetUniformLocation;
    public static int glGetUniformLocation(uint program, string name) => _glGetUniformLocation(program, name);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETUNIFORMFVPROC(uint program, int location, out float @params);
    private static PFNGLGETUNIFORMFVPROC _glGetUniformfv;
    public static void glGetUniformfv(uint program, int location, out float @params) => _glGetUniformfv(program, location, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETUNIFORMIVPROC(uint program, int location, out int @params);
    private static PFNGLGETUNIFORMIVPROC _glGetUniformiv;
    public static void glGetUniformiv(uint program, int location, out int @params) => _glGetUniformiv(program, location, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETVERTEXATTRIBDVPROC(uint index, uint pname, out double @params);
    private static PFNGLGETVERTEXATTRIBDVPROC _glGetVertexAttribdv;
    public static void glGetVertexAttribdv(uint index, uint pname, out double @params) => _glGetVertexAttribdv(index, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETVERTEXATTRIBFVPROC(uint index, uint pname, out float @params);
    private static PFNGLGETVERTEXATTRIBFVPROC _glGetVertexAttribfv;
    public static void glGetVertexAttribfv(uint index, uint pname, out float @params) => _glGetVertexAttribfv(index, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETVERTEXATTRIBIVPROC(uint index, uint pname, out int @params);
    private static PFNGLGETVERTEXATTRIBIVPROC _glGetVertexAttribiv;
    public static void glGetVertexAttribiv(uint index, uint pname, out int @params) => _glGetVertexAttribiv(index, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETVERTEXATTRIBPOINTERVPROC(uint index, uint pname, IntPtr pointer);
    private static PFNGLGETVERTEXATTRIBPOINTERVPROC _glGetVertexAttribPointerv;
    public static void glGetVertexAttribPointerv(uint index, uint pname, IntPtr pointer) => _glGetVertexAttribPointerv(index, pname, pointer);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate bool PFNGLISPROGRAMPROC(uint program);
    private static PFNGLISPROGRAMPROC _glIsProgram;
    public static bool glIsProgram(uint program) => _glIsProgram(program);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate bool PFNGLISSHADERPROC(uint shader);
    private static PFNGLISSHADERPROC _glIsShader;
    public static bool glIsShader(uint shader) => _glIsShader(shader);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLLINKPROGRAMPROC(uint program);
    private static PFNGLLINKPROGRAMPROC _glLinkProgram;
    public static void glLinkProgram(uint program) => _glLinkProgram(program);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUSEPROGRAMPROC(uint program);
    private static PFNGLUSEPROGRAMPROC _glUseProgram;
    public static void glUseProgram(uint program) => _glUseProgram(program);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORM1FPROC(int location, float v0);
    private static PFNGLUNIFORM1FPROC _glUniform1f;
    public static void glUniform1f(int location, float v0) => _glUniform1f(location, v0);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORM2FPROC(int location, float v0, float v1);
    private static PFNGLUNIFORM2FPROC _glUniform2f;
    public static void glUniform2f(int location, float v0, float v1) => _glUniform2f(location, v0, v1);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORM3FPROC(int location, float v0, float v1, float v2);
    private static PFNGLUNIFORM3FPROC _glUniform3f;
    public static void glUniform3f(int location, float v0, float v1, float v2) => _glUniform3f(location, v0, v1, v2);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORM4FPROC(int location, float v0, float v1, float v2, float v3);
    private static PFNGLUNIFORM4FPROC _glUniform4f;
    public static void glUniform4f(int location, float v0, float v1, float v2, float v3) => _glUniform4f(location, v0, v1, v2, v3);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORM1IPROC(int location, int v0);
    private static PFNGLUNIFORM1IPROC _glUniform1i;
    public static void glUniform1i(int location, int v0) => _glUniform1i(location, v0);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORM2IPROC(int location, int v0, int v1);
    private static PFNGLUNIFORM2IPROC _glUniform2i;
    public static void glUniform2i(int location, int v0, int v1) => _glUniform2i(location, v0, v1);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORM3IPROC(int location, int v0, int v1, int v2);
    private static PFNGLUNIFORM3IPROC _glUniform3i;
    public static void glUniform3i(int location, int v0, int v1, int v2) => _glUniform3i(location, v0, v1, v2);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORM4IPROC(int location, int v0, int v1, int v2, int v3);
    private static PFNGLUNIFORM4IPROC _glUniform4i;
    public static void glUniform4i(int location, int v0, int v1, int v2, int v3) => _glUniform4i(location, v0, v1, v2, v3);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORM1FVPROC(int location, int count, ref readonly float value);
    private static PFNGLUNIFORM1FVPROC _glUniform1fv;
    public static void glUniform1fv(int location, int count, ref readonly float value) => _glUniform1fv(location, count, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORM2FVPROC(int location, int count, ref readonly float value);
    private static PFNGLUNIFORM2FVPROC _glUniform2fv;
    public static void glUniform2fv(int location, int count, ref readonly float value) => _glUniform2fv(location, count, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORM3FVPROC(int location, int count, ref readonly float value);
    private static PFNGLUNIFORM3FVPROC _glUniform3fv;
    public static void glUniform3fv(int location, int count, ref readonly float value) => _glUniform3fv(location, count, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORM4FVPROC(int location, int count, ref readonly float value);
    private static PFNGLUNIFORM4FVPROC _glUniform4fv;
    public static void glUniform4fv(int location, int count, ref readonly float value) => _glUniform4fv(location, count, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORM1IVPROC(int location, int count, ref readonly int value);
    private static PFNGLUNIFORM1IVPROC _glUniform1iv;
    public static void glUniform1iv(int location, int count, ref readonly int value) => _glUniform1iv(location, count, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORM2IVPROC(int location, int count, ref readonly int value);
    private static PFNGLUNIFORM2IVPROC _glUniform2iv;
    public static void glUniform2iv(int location, int count, ref readonly int value) => _glUniform2iv(location, count, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORM3IVPROC(int location, int count, ref readonly int value);
    private static PFNGLUNIFORM3IVPROC _glUniform3iv;
    public static void glUniform3iv(int location, int count, ref readonly int value) => _glUniform3iv(location, count, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORM4IVPROC(int location, int count, ref readonly int value);
    private static PFNGLUNIFORM4IVPROC _glUniform4iv;
    public static void glUniform4iv(int location, int count, ref readonly int value) => _glUniform4iv(location, count, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORMMATRIX2FVPROC(int location, int count, bool transpose, ref readonly float value);
    private static PFNGLUNIFORMMATRIX2FVPROC _glUniformMatrix2fv;
    public static void glUniformMatrix2fv(int location, int count, bool transpose, ref readonly float value) => _glUniformMatrix2fv(location, count, transpose, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORMMATRIX3FVPROC(int location, int count, bool transpose, ref readonly float value);
    private static PFNGLUNIFORMMATRIX3FVPROC _glUniformMatrix3fv;
    public static void glUniformMatrix3fv(int location, int count, bool transpose, ref readonly float value) => _glUniformMatrix3fv(location, count, transpose, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORMMATRIX4FVPROC(int location, int count, bool transpose, ref readonly float value);
    private static PFNGLUNIFORMMATRIX4FVPROC _glUniformMatrix4fv;
    public static void glUniformMatrix4fv(int location, int count, bool transpose, ref readonly float value) => _glUniformMatrix4fv(location, count, transpose, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVALIDATEPROGRAMPROC(uint program);
    private static PFNGLVALIDATEPROGRAMPROC _glValidateProgram;
    public static void glValidateProgram(uint program) => _glValidateProgram(program);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIB1DPROC(uint index, double x);
    private static PFNGLVERTEXATTRIB1DPROC _glVertexAttrib1d;
    public static void glVertexAttrib1d(uint index, double x) => _glVertexAttrib1d(index, x);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIB1DVPROC(uint index, ref readonly double v);
    private static PFNGLVERTEXATTRIB1DVPROC _glVertexAttrib1dv;
    public static void glVertexAttrib1dv(uint index, ref readonly double v) => _glVertexAttrib1dv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIB1FPROC(uint index, float x);
    private static PFNGLVERTEXATTRIB1FPROC _glVertexAttrib1f;
    public static void glVertexAttrib1f(uint index, float x) => _glVertexAttrib1f(index, x);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIB1FVPROC(uint index, ref readonly float v);
    private static PFNGLVERTEXATTRIB1FVPROC _glVertexAttrib1fv;
    public static void glVertexAttrib1fv(uint index, ref readonly float v) => _glVertexAttrib1fv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIB1SPROC(uint index, short x);
    private static PFNGLVERTEXATTRIB1SPROC _glVertexAttrib1s;
    public static void glVertexAttrib1s(uint index, short x) => _glVertexAttrib1s(index, x);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIB1SVPROC(uint index, in short v);
    private static PFNGLVERTEXATTRIB1SVPROC _glVertexAttrib1sv;
    public static void glVertexAttrib1sv(uint index, in short v) => _glVertexAttrib1sv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIB2DPROC(uint index, double x, double y);
    private static PFNGLVERTEXATTRIB2DPROC _glVertexAttrib2d;
    public static void glVertexAttrib2d(uint index, double x, double y) => _glVertexAttrib2d(index, x, y);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIB2DVPROC(uint index, ref readonly double v);
    private static PFNGLVERTEXATTRIB2DVPROC _glVertexAttrib2dv;
    public static void glVertexAttrib2dv(uint index, ref readonly double v) => _glVertexAttrib2dv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIB2FPROC(uint index, float x, float y);
    private static PFNGLVERTEXATTRIB2FPROC _glVertexAttrib2f;
    public static void glVertexAttrib2f(uint index, float x, float y) => _glVertexAttrib2f(index, x, y);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIB2FVPROC(uint index, ref readonly float v);
    private static PFNGLVERTEXATTRIB2FVPROC _glVertexAttrib2fv;
    public static void glVertexAttrib2fv(uint index, ref readonly float v) => _glVertexAttrib2fv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIB2SPROC(uint index, short x, short y);
    private static PFNGLVERTEXATTRIB2SPROC _glVertexAttrib2s;
    public static void glVertexAttrib2s(uint index, short x, short y) => _glVertexAttrib2s(index, x, y);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIB2SVPROC(uint index, in short v);
    private static PFNGLVERTEXATTRIB2SVPROC _glVertexAttrib2sv;
    public static void glVertexAttrib2sv(uint index, in short v) => _glVertexAttrib2sv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIB3DPROC(uint index, double x, double y, double z);
    private static PFNGLVERTEXATTRIB3DPROC _glVertexAttrib3d;
    public static void glVertexAttrib3d(uint index, double x, double y, double z) => _glVertexAttrib3d(index, x, y, z);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIB3DVPROC(uint index, ref readonly double v);
    private static PFNGLVERTEXATTRIB3DVPROC _glVertexAttrib3dv;
    public static void glVertexAttrib3dv(uint index, ref readonly double v) => _glVertexAttrib3dv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIB3FPROC(uint index, float x, float y, float z);
    private static PFNGLVERTEXATTRIB3FPROC _glVertexAttrib3f;
    public static void glVertexAttrib3f(uint index, float x, float y, float z) => _glVertexAttrib3f(index, x, y, z);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIB3FVPROC(uint index, ref readonly float v);
    private static PFNGLVERTEXATTRIB3FVPROC _glVertexAttrib3fv;
    public static void glVertexAttrib3fv(uint index, ref readonly float v) => _glVertexAttrib3fv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIB3SPROC(uint index, short x, short y, short z);
    private static PFNGLVERTEXATTRIB3SPROC _glVertexAttrib3s;
    public static void glVertexAttrib3s(uint index, short x, short y, short z) => _glVertexAttrib3s(index, x, y, z);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIB3SVPROC(uint index, in short v);
    private static PFNGLVERTEXATTRIB3SVPROC _glVertexAttrib3sv;
    public static void glVertexAttrib3sv(uint index, in short v) => _glVertexAttrib3sv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIB4NBVPROC(uint index, in sbyte v);
    private static PFNGLVERTEXATTRIB4NBVPROC _glVertexAttrib4Nbv;
    public static void glVertexAttrib4Nbv(uint index, in sbyte v) => _glVertexAttrib4Nbv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIB4NIVPROC(uint index, ref readonly int v);
    private static PFNGLVERTEXATTRIB4NIVPROC _glVertexAttrib4Niv;
    public static void glVertexAttrib4Niv(uint index, ref readonly int v) => _glVertexAttrib4Niv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIB4NSVPROC(uint index, in short v);
    private static PFNGLVERTEXATTRIB4NSVPROC _glVertexAttrib4Nsv;
    public static void glVertexAttrib4Nsv(uint index, in short v) => _glVertexAttrib4Nsv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIB4NUBPROC(uint index, byte x, byte y, byte z, byte w);
    private static PFNGLVERTEXATTRIB4NUBPROC _glVertexAttrib4Nub;
    public static void glVertexAttrib4Nub(uint index, byte x, byte y, byte z, byte w) => _glVertexAttrib4Nub(index, x, y, z, w);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIB4NUBVPROC(uint index, IntPtr v);
    private static PFNGLVERTEXATTRIB4NUBVPROC _glVertexAttrib4Nubv;
    public static void glVertexAttrib4Nubv(uint index, IntPtr v) => _glVertexAttrib4Nubv(index, v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIB4NUIVPROC(uint index, ref readonly uint v);
    private static PFNGLVERTEXATTRIB4NUIVPROC _glVertexAttrib4Nuiv;
    public static void glVertexAttrib4Nuiv(uint index, ref readonly uint v) => _glVertexAttrib4Nuiv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIB4NUSVPROC(uint index, in ushort v);
    private static PFNGLVERTEXATTRIB4NUSVPROC _glVertexAttrib4Nusv;
    public static void glVertexAttrib4Nusv(uint index, in ushort v) => _glVertexAttrib4Nusv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIB4BVPROC(uint index, in sbyte v);
    private static PFNGLVERTEXATTRIB4BVPROC _glVertexAttrib4bv;
    public static void glVertexAttrib4bv(uint index, in sbyte v) => _glVertexAttrib4bv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIB4DPROC(uint index, double x, double y, double z, double w);
    private static PFNGLVERTEXATTRIB4DPROC _glVertexAttrib4d;
    public static void glVertexAttrib4d(uint index, double x, double y, double z, double w) => _glVertexAttrib4d(index, x, y, z, w);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIB4DVPROC(uint index, ref readonly double v);
    private static PFNGLVERTEXATTRIB4DVPROC _glVertexAttrib4dv;
    public static void glVertexAttrib4dv(uint index, ref readonly double v) => _glVertexAttrib4dv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIB4FPROC(uint index, float x, float y, float z, float w);
    private static PFNGLVERTEXATTRIB4FPROC _glVertexAttrib4f;
    public static void glVertexAttrib4f(uint index, float x, float y, float z, float w) => _glVertexAttrib4f(index, x, y, z, w);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIB4FVPROC(uint index, ref readonly float v);
    private static PFNGLVERTEXATTRIB4FVPROC _glVertexAttrib4fv;
    public static void glVertexAttrib4fv(uint index, ref readonly float v) => _glVertexAttrib4fv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIB4IVPROC(uint index, ref readonly int v);
    private static PFNGLVERTEXATTRIB4IVPROC _glVertexAttrib4iv;
    public static void glVertexAttrib4iv(uint index, ref readonly int v) => _glVertexAttrib4iv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIB4SPROC(uint index, short x, short y, short z, short w);
    private static PFNGLVERTEXATTRIB4SPROC _glVertexAttrib4s;
    public static void glVertexAttrib4s(uint index, short x, short y, short z, short w) => _glVertexAttrib4s(index, x, y, z, w);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIB4SVPROC(uint index, in short v);
    private static PFNGLVERTEXATTRIB4SVPROC _glVertexAttrib4sv;
    public static void glVertexAttrib4sv(uint index, in short v) => _glVertexAttrib4sv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIB4UBVPROC(uint index, IntPtr v);
    private static PFNGLVERTEXATTRIB4UBVPROC _glVertexAttrib4ubv;
    public static void glVertexAttrib4ubv(uint index, IntPtr v) => _glVertexAttrib4ubv(index, v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIB4UIVPROC(uint index, ref readonly uint v);
    private static PFNGLVERTEXATTRIB4UIVPROC _glVertexAttrib4uiv;
    public static void glVertexAttrib4uiv(uint index, ref readonly uint v) => _glVertexAttrib4uiv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIB4USVPROC(uint index, in ushort v);
    private static PFNGLVERTEXATTRIB4USVPROC _glVertexAttrib4usv;
    public static void glVertexAttrib4usv(uint index, in ushort v) => _glVertexAttrib4usv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBPOINTERPROC(uint index, int size, uint type, bool normalized, int stride, IntPtr pointer);
    private static PFNGLVERTEXATTRIBPOINTERPROC _glVertexAttribPointer;
    public static void glVertexAttribPointer(uint index, int size, uint type, bool normalized, int stride, IntPtr pointer) => _glVertexAttribPointer(index, size, type, normalized, stride, pointer);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORMMATRIX2X3FVPROC(int location, int count, bool transpose, ref readonly float value);
    private static PFNGLUNIFORMMATRIX2X3FVPROC _glUniformMatrix2x3fv;
    public static void glUniformMatrix2x3fv(int location, int count, bool transpose, ref readonly float value) => _glUniformMatrix2x3fv(location, count, transpose, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORMMATRIX3X2FVPROC(int location, int count, bool transpose, ref readonly float value);
    private static PFNGLUNIFORMMATRIX3X2FVPROC _glUniformMatrix3x2fv;
    public static void glUniformMatrix3x2fv(int location, int count, bool transpose, ref readonly float value) => _glUniformMatrix3x2fv(location, count, transpose, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORMMATRIX2X4FVPROC(int location, int count, bool transpose, ref readonly float value);
    private static PFNGLUNIFORMMATRIX2X4FVPROC _glUniformMatrix2x4fv;
    public static void glUniformMatrix2x4fv(int location, int count, bool transpose, ref readonly float value) => _glUniformMatrix2x4fv(location, count, transpose, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORMMATRIX4X2FVPROC(int location, int count, bool transpose, ref readonly float value);
    private static PFNGLUNIFORMMATRIX4X2FVPROC _glUniformMatrix4x2fv;
    public static void glUniformMatrix4x2fv(int location, int count, bool transpose, ref readonly float value) => _glUniformMatrix4x2fv(location, count, transpose, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORMMATRIX3X4FVPROC(int location, int count, bool transpose, ref readonly float value);
    private static PFNGLUNIFORMMATRIX3X4FVPROC _glUniformMatrix3x4fv;
    public static void glUniformMatrix3x4fv(int location, int count, bool transpose, ref readonly float value) => _glUniformMatrix3x4fv(location, count, transpose, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORMMATRIX4X3FVPROC(int location, int count, bool transpose, ref readonly float value);
    private static PFNGLUNIFORMMATRIX4X3FVPROC _glUniformMatrix4x3fv;
    public static void glUniformMatrix4x3fv(int location, int count, bool transpose, ref readonly float value) => _glUniformMatrix4x3fv(location, count, transpose, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCOLORMASKIPROC(uint index, bool r, bool g, bool b, bool a);
    private static PFNGLCOLORMASKIPROC _glColorMaski;
    public static void glColorMaski(uint index, bool r, bool g, bool b, bool a) => _glColorMaski(index, r, g, b, a);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETBOOLEANI_VPROC(uint target, uint index, out bool data);
    private static PFNGLGETBOOLEANI_VPROC _glGetBooleani_v;
    public static void glGetBooleani_v(uint target, uint index, out bool data) => _glGetBooleani_v(target, index, out data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETINTEGERI_VPROC(uint target, uint index, out int data);
    private static PFNGLGETINTEGERI_VPROC _glGetIntegeri_v;
    public static void glGetIntegeri_v(uint target, uint index, out int data) => _glGetIntegeri_v(target, index, out data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLENABLEIPROC(uint target, uint index);
    private static PFNGLENABLEIPROC _glEnablei;
    public static void glEnablei(uint target, uint index) => _glEnablei(target, index);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDISABLEIPROC(uint target, uint index);
    private static PFNGLDISABLEIPROC _glDisablei;
    public static void glDisablei(uint target, uint index) => _glDisablei(target, index);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate bool PFNGLISENABLEDIPROC(uint target, uint index);
    private static PFNGLISENABLEDIPROC _glIsEnabledi;
    public static bool glIsEnabledi(uint target, uint index) => _glIsEnabledi(target, index);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBEGINTRANSFORMFEEDBACKPROC(uint primitiveMode);
    private static PFNGLBEGINTRANSFORMFEEDBACKPROC _glBeginTransformFeedback;
    public static void glBeginTransformFeedback(uint primitiveMode) => _glBeginTransformFeedback(primitiveMode);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLENDTRANSFORMFEEDBACKPROC();
    private static PFNGLENDTRANSFORMFEEDBACKPROC _glEndTransformFeedback;
    public static void glEndTransformFeedback() => _glEndTransformFeedback();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBINDBUFFERRANGEPROC(uint target, uint index, uint buffer, ulong offset, ulong size);
    private static PFNGLBINDBUFFERRANGEPROC _glBindBufferRange;
    public static void glBindBufferRange(uint target, uint index, uint buffer, ulong offset, ulong size) => _glBindBufferRange(target, index, buffer, offset, size);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBINDBUFFERBASEPROC(uint target, uint index, uint buffer);
    private static PFNGLBINDBUFFERBASEPROC _glBindBufferBase;
    public static void glBindBufferBase(uint target, uint index, uint buffer) => _glBindBufferBase(target, index, buffer);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTRANSFORMFEEDBACKVARYINGSPROC(uint program, int count, string varyings, uint bufferMode);
    private static PFNGLTRANSFORMFEEDBACKVARYINGSPROC _glTransformFeedbackVaryings;
    public static void glTransformFeedbackVaryings(uint program, int count, string varyings, uint bufferMode) => _glTransformFeedbackVaryings(program, count, varyings, bufferMode);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETTRANSFORMFEEDBACKVARYINGPROC(uint program, uint index, int bufSize, out int length, out int size, out uint type, string name);
    private static PFNGLGETTRANSFORMFEEDBACKVARYINGPROC _glGetTransformFeedbackVarying;
    public static void glGetTransformFeedbackVarying(uint program, uint index, int bufSize, out int length, out int size, out uint type, string name) => _glGetTransformFeedbackVarying(program, index, bufSize, out length, out size, out type, name);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCLAMPCOLORPROC(uint target, uint clamp);
    private static PFNGLCLAMPCOLORPROC _glClampColor;
    public static void glClampColor(uint target, uint clamp) => _glClampColor(target, clamp);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBEGINCONDITIONALRENDERPROC(uint id, uint mode);
    private static PFNGLBEGINCONDITIONALRENDERPROC _glBeginConditionalRender;
    public static void glBeginConditionalRender(uint id, uint mode) => _glBeginConditionalRender(id, mode);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLENDCONDITIONALRENDERPROC();
    private static PFNGLENDCONDITIONALRENDERPROC _glEndConditionalRender;
    public static void glEndConditionalRender() => _glEndConditionalRender();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBIPOINTERPROC(uint index, int size, uint type, int stride, IntPtr pointer);
    private static PFNGLVERTEXATTRIBIPOINTERPROC _glVertexAttribIPointer;
    public static void glVertexAttribIPointer(uint index, int size, uint type, int stride, IntPtr pointer) => _glVertexAttribIPointer(index, size, type, stride, pointer);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETVERTEXATTRIBIIVPROC(uint index, uint pname, out int @params);
    private static PFNGLGETVERTEXATTRIBIIVPROC _glGetVertexAttribIiv;
    public static void glGetVertexAttribIiv(uint index, uint pname, out int @params) => _glGetVertexAttribIiv(index, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETVERTEXATTRIBIUIVPROC(uint index, uint pname, out uint @params);
    private static PFNGLGETVERTEXATTRIBIUIVPROC _glGetVertexAttribIuiv;
    public static void glGetVertexAttribIuiv(uint index, uint pname, out uint @params) => _glGetVertexAttribIuiv(index, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBI1IPROC(uint index, int x);
    private static PFNGLVERTEXATTRIBI1IPROC _glVertexAttribI1i;
    public static void glVertexAttribI1i(uint index, int x) => _glVertexAttribI1i(index, x);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBI2IPROC(uint index, int x, int y);
    private static PFNGLVERTEXATTRIBI2IPROC _glVertexAttribI2i;
    public static void glVertexAttribI2i(uint index, int x, int y) => _glVertexAttribI2i(index, x, y);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBI3IPROC(uint index, int x, int y, int z);
    private static PFNGLVERTEXATTRIBI3IPROC _glVertexAttribI3i;
    public static void glVertexAttribI3i(uint index, int x, int y, int z) => _glVertexAttribI3i(index, x, y, z);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBI4IPROC(uint index, int x, int y, int z, int w);
    private static PFNGLVERTEXATTRIBI4IPROC _glVertexAttribI4i;
    public static void glVertexAttribI4i(uint index, int x, int y, int z, int w) => _glVertexAttribI4i(index, x, y, z, w);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBI1UIPROC(uint index, uint x);
    private static PFNGLVERTEXATTRIBI1UIPROC _glVertexAttribI1ui;
    public static void glVertexAttribI1ui(uint index, uint x) => _glVertexAttribI1ui(index, x);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBI2UIPROC(uint index, uint x, uint y);
    private static PFNGLVERTEXATTRIBI2UIPROC _glVertexAttribI2ui;
    public static void glVertexAttribI2ui(uint index, uint x, uint y) => _glVertexAttribI2ui(index, x, y);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBI3UIPROC(uint index, uint x, uint y, uint z);
    private static PFNGLVERTEXATTRIBI3UIPROC _glVertexAttribI3ui;
    public static void glVertexAttribI3ui(uint index, uint x, uint y, uint z) => _glVertexAttribI3ui(index, x, y, z);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBI4UIPROC(uint index, uint x, uint y, uint z, uint w);
    private static PFNGLVERTEXATTRIBI4UIPROC _glVertexAttribI4ui;
    public static void glVertexAttribI4ui(uint index, uint x, uint y, uint z, uint w) => _glVertexAttribI4ui(index, x, y, z, w);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBI1IVPROC(uint index, ref readonly int v);
    private static PFNGLVERTEXATTRIBI1IVPROC _glVertexAttribI1iv;
    public static void glVertexAttribI1iv(uint index, ref readonly int v) => _glVertexAttribI1iv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBI2IVPROC(uint index, ref readonly int v);
    private static PFNGLVERTEXATTRIBI2IVPROC _glVertexAttribI2iv;
    public static void glVertexAttribI2iv(uint index, ref readonly int v) => _glVertexAttribI2iv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBI3IVPROC(uint index, ref readonly int v);
    private static PFNGLVERTEXATTRIBI3IVPROC _glVertexAttribI3iv;
    public static void glVertexAttribI3iv(uint index, ref readonly int v) => _glVertexAttribI3iv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBI4IVPROC(uint index, ref readonly int v);
    private static PFNGLVERTEXATTRIBI4IVPROC _glVertexAttribI4iv;
    public static void glVertexAttribI4iv(uint index, ref readonly int v) => _glVertexAttribI4iv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBI1UIVPROC(uint index, ref readonly uint v);
    private static PFNGLVERTEXATTRIBI1UIVPROC _glVertexAttribI1uiv;
    public static void glVertexAttribI1uiv(uint index, ref readonly uint v) => _glVertexAttribI1uiv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBI2UIVPROC(uint index, ref readonly uint v);
    private static PFNGLVERTEXATTRIBI2UIVPROC _glVertexAttribI2uiv;
    public static void glVertexAttribI2uiv(uint index, ref readonly uint v) => _glVertexAttribI2uiv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBI3UIVPROC(uint index, ref readonly uint v);
    private static PFNGLVERTEXATTRIBI3UIVPROC _glVertexAttribI3uiv;
    public static void glVertexAttribI3uiv(uint index, ref readonly uint v) => _glVertexAttribI3uiv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBI4UIVPROC(uint index, ref readonly uint v);
    private static PFNGLVERTEXATTRIBI4UIVPROC _glVertexAttribI4uiv;
    public static void glVertexAttribI4uiv(uint index, ref readonly uint v) => _glVertexAttribI4uiv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBI4BVPROC(uint index, in sbyte v);
    private static PFNGLVERTEXATTRIBI4BVPROC _glVertexAttribI4bv;
    public static void glVertexAttribI4bv(uint index, in sbyte v) => _glVertexAttribI4bv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBI4SVPROC(uint index, in short v);
    private static PFNGLVERTEXATTRIBI4SVPROC _glVertexAttribI4sv;
    public static void glVertexAttribI4sv(uint index, in short v) => _glVertexAttribI4sv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBI4UBVPROC(uint index, IntPtr v);
    private static PFNGLVERTEXATTRIBI4UBVPROC _glVertexAttribI4ubv;
    public static void glVertexAttribI4ubv(uint index, IntPtr v) => _glVertexAttribI4ubv(index, v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBI4USVPROC(uint index, in ushort v);
    private static PFNGLVERTEXATTRIBI4USVPROC _glVertexAttribI4usv;
    public static void glVertexAttribI4usv(uint index, in ushort v) => _glVertexAttribI4usv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETUNIFORMUIVPROC(uint program, int location, out uint @params);
    private static PFNGLGETUNIFORMUIVPROC _glGetUniformuiv;
    public static void glGetUniformuiv(uint program, int location, out uint @params) => _glGetUniformuiv(program, location, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBINDFRAGDATALOCATIONPROC(uint program, uint color, string name);
    private static PFNGLBINDFRAGDATALOCATIONPROC _glBindFragDataLocation;
    public static void glBindFragDataLocation(uint program, uint color, string name) => _glBindFragDataLocation(program, color, name);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int PFNGLGETFRAGDATALOCATIONPROC(uint program, string name);
    private static PFNGLGETFRAGDATALOCATIONPROC _glGetFragDataLocation;
    public static int glGetFragDataLocation(uint program, string name) => _glGetFragDataLocation(program, name);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORM1UIPROC(int location, uint v0);
    private static PFNGLUNIFORM1UIPROC _glUniform1ui;
    public static void glUniform1ui(int location, uint v0) => _glUniform1ui(location, v0);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORM2UIPROC(int location, uint v0, uint v1);
    private static PFNGLUNIFORM2UIPROC _glUniform2ui;
    public static void glUniform2ui(int location, uint v0, uint v1) => _glUniform2ui(location, v0, v1);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORM3UIPROC(int location, uint v0, uint v1, uint v2);
    private static PFNGLUNIFORM3UIPROC _glUniform3ui;
    public static void glUniform3ui(int location, uint v0, uint v1, uint v2) => _glUniform3ui(location, v0, v1, v2);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORM4UIPROC(int location, uint v0, uint v1, uint v2, uint v3);
    private static PFNGLUNIFORM4UIPROC _glUniform4ui;
    public static void glUniform4ui(int location, uint v0, uint v1, uint v2, uint v3) => _glUniform4ui(location, v0, v1, v2, v3);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORM1UIVPROC(int location, int count, ref readonly uint value);
    private static PFNGLUNIFORM1UIVPROC _glUniform1uiv;
    public static void glUniform1uiv(int location, int count, ref readonly uint value) => _glUniform1uiv(location, count, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORM2UIVPROC(int location, int count, ref readonly uint value);
    private static PFNGLUNIFORM2UIVPROC _glUniform2uiv;
    public static void glUniform2uiv(int location, int count, ref readonly uint value) => _glUniform2uiv(location, count, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORM3UIVPROC(int location, int count, ref readonly uint value);
    private static PFNGLUNIFORM3UIVPROC _glUniform3uiv;
    public static void glUniform3uiv(int location, int count, ref readonly uint value) => _glUniform3uiv(location, count, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORM4UIVPROC(int location, int count, ref readonly uint value);
    private static PFNGLUNIFORM4UIVPROC _glUniform4uiv;
    public static void glUniform4uiv(int location, int count, ref readonly uint value) => _glUniform4uiv(location, count, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXPARAMETERIIVPROC(uint target, uint pname, ref readonly int @params);
    private static PFNGLTEXPARAMETERIIVPROC _glTexParameterIiv;
    public static void glTexParameterIiv(uint target, uint pname, ref readonly int @params) => _glTexParameterIiv(target, pname, in @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXPARAMETERIUIVPROC(uint target, uint pname, ref readonly uint @params);
    private static PFNGLTEXPARAMETERIUIVPROC _glTexParameterIuiv;
    public static void glTexParameterIuiv(uint target, uint pname, ref readonly uint @params) => _glTexParameterIuiv(target, pname, in @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETTEXPARAMETERIIVPROC(uint target, uint pname, out int @params);
    private static PFNGLGETTEXPARAMETERIIVPROC _glGetTexParameterIiv;
    public static void glGetTexParameterIiv(uint target, uint pname, out int @params) => _glGetTexParameterIiv(target, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETTEXPARAMETERIUIVPROC(uint target, uint pname, out uint @params);
    private static PFNGLGETTEXPARAMETERIUIVPROC _glGetTexParameterIuiv;
    public static void glGetTexParameterIuiv(uint target, uint pname, out uint @params) => _glGetTexParameterIuiv(target, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCLEARBUFFERIVPROC(uint buffer, int drawbuffer, ref readonly int value);
    private static PFNGLCLEARBUFFERIVPROC _glClearBufferiv;
    public static void glClearBufferiv(uint buffer, int drawbuffer, ref readonly int value) => _glClearBufferiv(buffer, drawbuffer, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCLEARBUFFERUIVPROC(uint buffer, int drawbuffer, ref readonly uint value);
    private static PFNGLCLEARBUFFERUIVPROC _glClearBufferuiv;
    public static void glClearBufferuiv(uint buffer, int drawbuffer, ref readonly uint value) => _glClearBufferuiv(buffer, drawbuffer, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCLEARBUFFERFVPROC(uint buffer, int drawbuffer, ref readonly float value);
    private static PFNGLCLEARBUFFERFVPROC _glClearBufferfv;
    public static void glClearBufferfv(uint buffer, int drawbuffer, ref readonly float value) => _glClearBufferfv(buffer, drawbuffer, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCLEARBUFFERFIPROC(uint buffer, int drawbuffer, float depth, int stencil);
    private static PFNGLCLEARBUFFERFIPROC _glClearBufferfi;
    public static void glClearBufferfi(uint buffer, int drawbuffer, float depth, int stencil) => _glClearBufferfi(buffer, drawbuffer, depth, stencil);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate IntPtr PFNGLGETSTRINGIPROC(uint name, uint index);
    private static PFNGLGETSTRINGIPROC _glGetStringi;
    public static IntPtr glGetStringi(uint name, uint index) => _glGetStringi(name, index);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate bool PFNGLISRENDERBUFFERPROC(uint renderbuffer);
    private static PFNGLISRENDERBUFFERPROC _glIsRenderbuffer;
    public static bool glIsRenderbuffer(uint renderbuffer) => _glIsRenderbuffer(renderbuffer);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBINDRENDERBUFFERPROC(uint target, uint renderbuffer);
    private static PFNGLBINDRENDERBUFFERPROC _glBindRenderbuffer;
    public static void glBindRenderbuffer(uint target, uint renderbuffer) => _glBindRenderbuffer(target, renderbuffer);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDELETERENDERBUFFERSPROC(int n, ref readonly uint renderbuffers);
    private static PFNGLDELETERENDERBUFFERSPROC _glDeleteRenderbuffers;
    public static void glDeleteRenderbuffers(int n, ref readonly uint renderbuffers) => _glDeleteRenderbuffers(n, in renderbuffers);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGENRENDERBUFFERSPROC(int n, out uint renderbuffers);
    private static PFNGLGENRENDERBUFFERSPROC _glGenRenderbuffers;
    public static void glGenRenderbuffers(int n, out uint renderbuffers) => _glGenRenderbuffers(n, out renderbuffers);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLRENDERBUFFERSTORAGEPROC(uint target, uint internalformat, int width, int height);
    private static PFNGLRENDERBUFFERSTORAGEPROC _glRenderbufferStorage;
    public static void glRenderbufferStorage(uint target, uint internalformat, int width, int height) => _glRenderbufferStorage(target, internalformat, width, height);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETRENDERBUFFERPARAMETERIVPROC(uint target, uint pname, out int @params);
    private static PFNGLGETRENDERBUFFERPARAMETERIVPROC _glGetRenderbufferParameteriv;
    public static void glGetRenderbufferParameteriv(uint target, uint pname, out int @params) => _glGetRenderbufferParameteriv(target, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate bool PFNGLISFRAMEBUFFERPROC(uint framebuffer);
    private static PFNGLISFRAMEBUFFERPROC _glIsFramebuffer;
    public static bool glIsFramebuffer(uint framebuffer) => _glIsFramebuffer(framebuffer);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBINDFRAMEBUFFERPROC(uint target, uint framebuffer);
    private static PFNGLBINDFRAMEBUFFERPROC _glBindFramebuffer;
    public static void glBindFramebuffer(uint target, uint framebuffer) => _glBindFramebuffer(target, framebuffer);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDELETEFRAMEBUFFERSPROC(int n, ref readonly uint framebuffers);
    private static PFNGLDELETEFRAMEBUFFERSPROC _glDeleteFramebuffers;
    public static void glDeleteFramebuffers(int n, ref readonly uint framebuffers) => _glDeleteFramebuffers(n, in framebuffers);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGENFRAMEBUFFERSPROC(int n, out uint framebuffers);
    private static PFNGLGENFRAMEBUFFERSPROC _glGenFramebuffers;
    public static void glGenFramebuffers(int n, out uint framebuffers) => _glGenFramebuffers(n, out framebuffers);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate uint PFNGLCHECKFRAMEBUFFERSTATUSPROC(uint target);
    private static PFNGLCHECKFRAMEBUFFERSTATUSPROC _glCheckFramebufferStatus;
    public static uint glCheckFramebufferStatus(uint target) => _glCheckFramebufferStatus(target);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLFRAMEBUFFERTEXTURE1DPROC(uint target, uint attachment, uint textarget, uint texture, int level);
    private static PFNGLFRAMEBUFFERTEXTURE1DPROC _glFramebufferTexture1D;
    public static void glFramebufferTexture1D(uint target, uint attachment, uint textarget, uint texture, int level) => _glFramebufferTexture1D(target, attachment, textarget, texture, level);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLFRAMEBUFFERTEXTURE2DPROC(uint target, uint attachment, uint textarget, uint texture, int level);
    private static PFNGLFRAMEBUFFERTEXTURE2DPROC _glFramebufferTexture2D;
    public static void glFramebufferTexture2D(uint target, uint attachment, uint textarget, uint texture, int level) => _glFramebufferTexture2D(target, attachment, textarget, texture, level);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLFRAMEBUFFERTEXTURE3DPROC(uint target, uint attachment, uint textarget, uint texture, int level, int zoffset);
    private static PFNGLFRAMEBUFFERTEXTURE3DPROC _glFramebufferTexture3D;
    public static void glFramebufferTexture3D(uint target, uint attachment, uint textarget, uint texture, int level, int zoffset) => _glFramebufferTexture3D(target, attachment, textarget, texture, level, zoffset);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLFRAMEBUFFERRENDERBUFFERPROC(uint target, uint attachment, uint renderbuffertarget, uint renderbuffer);
    private static PFNGLFRAMEBUFFERRENDERBUFFERPROC _glFramebufferRenderbuffer;
    public static void glFramebufferRenderbuffer(uint target, uint attachment, uint renderbuffertarget, uint renderbuffer) => _glFramebufferRenderbuffer(target, attachment, renderbuffertarget, renderbuffer);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETFRAMEBUFFERATTACHMENTPARAMETERIVPROC(uint target, uint attachment, uint pname, out int @params);
    private static PFNGLGETFRAMEBUFFERATTACHMENTPARAMETERIVPROC _glGetFramebufferAttachmentParameteriv;
    public static void glGetFramebufferAttachmentParameteriv(uint target, uint attachment, uint pname, out int @params) => _glGetFramebufferAttachmentParameteriv(target, attachment, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGENERATEMIPMAPPROC(uint target);
    private static PFNGLGENERATEMIPMAPPROC _glGenerateMipmap;
    public static void glGenerateMipmap(uint target) => _glGenerateMipmap(target);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBLITFRAMEBUFFERPROC(int srcX0, int srcY0, int srcX1, int srcY1, int dstX0, int dstY0, int dstX1, int dstY1, uint mask, uint filter);
    private static PFNGLBLITFRAMEBUFFERPROC _glBlitFramebuffer;
    public static void glBlitFramebuffer(int srcX0, int srcY0, int srcX1, int srcY1, int dstX0, int dstY0, int dstX1, int dstY1, uint mask, uint filter) => _glBlitFramebuffer(srcX0, srcY0, srcX1, srcY1, dstX0, dstY0, dstX1, dstY1, mask, filter);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLRENDERBUFFERSTORAGEMULTISAMPLEPROC(uint target, int samples, uint internalformat, int width, int height);
    private static PFNGLRENDERBUFFERSTORAGEMULTISAMPLEPROC _glRenderbufferStorageMultisample;
    public static void glRenderbufferStorageMultisample(uint target, int samples, uint internalformat, int width, int height) => _glRenderbufferStorageMultisample(target, samples, internalformat, width, height);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLFRAMEBUFFERTEXTURELAYERPROC(uint target, uint attachment, uint texture, int level, int layer);
    private static PFNGLFRAMEBUFFERTEXTURELAYERPROC _glFramebufferTextureLayer;
    public static void glFramebufferTextureLayer(uint target, uint attachment, uint texture, int level, int layer) => _glFramebufferTextureLayer(target, attachment, texture, level, layer);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate IntPtr PFNGLMAPBUFFERRANGEPROC(uint target, ulong offset, ulong length, uint access);
    private static PFNGLMAPBUFFERRANGEPROC _glMapBufferRange;
    public static IntPtr glMapBufferRange(uint target, ulong offset, ulong length, uint access) => _glMapBufferRange(target, offset, length, access);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLFLUSHMAPPEDBUFFERRANGEPROC(uint target, ulong offset, ulong length);
    private static PFNGLFLUSHMAPPEDBUFFERRANGEPROC _glFlushMappedBufferRange;
    public static void glFlushMappedBufferRange(uint target, ulong offset, ulong length) => _glFlushMappedBufferRange(target, offset, length);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBINDVERTEXARRAYPROC(uint array);
    private static PFNGLBINDVERTEXARRAYPROC _glBindVertexArray;
    public static void glBindVertexArray(uint array) => _glBindVertexArray(array);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDELETEVERTEXARRAYSPROC(int n, ref readonly uint arrays);
    private static PFNGLDELETEVERTEXARRAYSPROC _glDeleteVertexArrays;
    public static void glDeleteVertexArrays(int n, ref readonly uint arrays) => _glDeleteVertexArrays(n, in arrays);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGENVERTEXARRAYSPROC(int n, out uint arrays);
    private static PFNGLGENVERTEXARRAYSPROC _glGenVertexArrays;
    public static void glGenVertexArrays(int n, out uint arrays) => _glGenVertexArrays(n, out arrays);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate bool PFNGLISVERTEXARRAYPROC(uint array);
    private static PFNGLISVERTEXARRAYPROC _glIsVertexArray;
    public static bool glIsVertexArray(uint array) => _glIsVertexArray(array);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDRAWARRAYSINSTANCEDPROC(uint mode, int first, int count, int instancecount);
    private static PFNGLDRAWARRAYSINSTANCEDPROC _glDrawArraysInstanced;
    public static void glDrawArraysInstanced(uint mode, int first, int count, int instancecount) => _glDrawArraysInstanced(mode, first, count, instancecount);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDRAWELEMENTSINSTANCEDPROC(uint mode, int count, uint type, IntPtr indices, int instancecount);
    private static PFNGLDRAWELEMENTSINSTANCEDPROC _glDrawElementsInstanced;
    public static void glDrawElementsInstanced(uint mode, int count, uint type, IntPtr indices, int instancecount) => _glDrawElementsInstanced(mode, count, type, indices, instancecount);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXBUFFERPROC(uint target, uint internalformat, uint buffer);
    private static PFNGLTEXBUFFERPROC _glTexBuffer;
    public static void glTexBuffer(uint target, uint internalformat, uint buffer) => _glTexBuffer(target, internalformat, buffer);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPRIMITIVERESTARTINDEXPROC(uint index);
    private static PFNGLPRIMITIVERESTARTINDEXPROC _glPrimitiveRestartIndex;
    public static void glPrimitiveRestartIndex(uint index) => _glPrimitiveRestartIndex(index);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCOPYBUFFERSUBDATAPROC(uint readTarget, uint writeTarget, ulong readOffset, ulong writeOffset, ulong size);
    private static PFNGLCOPYBUFFERSUBDATAPROC _glCopyBufferSubData;
    public static void glCopyBufferSubData(uint readTarget, uint writeTarget, ulong readOffset, ulong writeOffset, ulong size) => _glCopyBufferSubData(readTarget, writeTarget, readOffset, writeOffset, size);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETUNIFORMINDICESPROC(uint program, int uniformCount, string uniformNames, out uint uniformIndices);
    private static PFNGLGETUNIFORMINDICESPROC _glGetUniformIndices;
    public static void glGetUniformIndices(uint program, int uniformCount, string uniformNames, out uint uniformIndices) => _glGetUniformIndices(program, uniformCount, uniformNames, out uniformIndices);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETACTIVEUNIFORMSIVPROC(uint program, int uniformCount, ref readonly uint uniformIndices, uint pname, out int @params);
    private static PFNGLGETACTIVEUNIFORMSIVPROC _glGetActiveUniformsiv;
    public static void glGetActiveUniformsiv(uint program, int uniformCount, ref readonly uint uniformIndices, uint pname, out int @params) => _glGetActiveUniformsiv(program, uniformCount, in uniformIndices, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETACTIVEUNIFORMNAMEPROC(uint program, uint uniformIndex, int bufSize, out int length, string uniformName);
    private static PFNGLGETACTIVEUNIFORMNAMEPROC _glGetActiveUniformName;
    public static void glGetActiveUniformName(uint program, uint uniformIndex, int bufSize, out int length, string uniformName) => _glGetActiveUniformName(program, uniformIndex, bufSize, out length, uniformName);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate uint PFNGLGETUNIFORMBLOCKINDEXPROC(uint program, string uniformBlockName);
    private static PFNGLGETUNIFORMBLOCKINDEXPROC _glGetUniformBlockIndex;
    public static uint glGetUniformBlockIndex(uint program, string uniformBlockName) => _glGetUniformBlockIndex(program, uniformBlockName);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETACTIVEUNIFORMBLOCKIVPROC(uint program, uint uniformBlockIndex, uint pname, out int @params);
    private static PFNGLGETACTIVEUNIFORMBLOCKIVPROC _glGetActiveUniformBlockiv;
    public static void glGetActiveUniformBlockiv(uint program, uint uniformBlockIndex, uint pname, out int @params) => _glGetActiveUniformBlockiv(program, uniformBlockIndex, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETACTIVEUNIFORMBLOCKNAMEPROC(uint program, uint uniformBlockIndex, int bufSize, out int length, string uniformBlockName);
    private static PFNGLGETACTIVEUNIFORMBLOCKNAMEPROC _glGetActiveUniformBlockName;
    public static void glGetActiveUniformBlockName(uint program, uint uniformBlockIndex, int bufSize, out int length, string uniformBlockName) => _glGetActiveUniformBlockName(program, uniformBlockIndex, bufSize, out length, uniformBlockName);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORMBLOCKBINDINGPROC(uint program, uint uniformBlockIndex, uint uniformBlockBinding);
    private static PFNGLUNIFORMBLOCKBINDINGPROC _glUniformBlockBinding;
    public static void glUniformBlockBinding(uint program, uint uniformBlockIndex, uint uniformBlockBinding) => _glUniformBlockBinding(program, uniformBlockIndex, uniformBlockBinding);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDRAWELEMENTSBASEVERTEXPROC(uint mode, int count, uint type, IntPtr indices, int basevertex);
    private static PFNGLDRAWELEMENTSBASEVERTEXPROC _glDrawElementsBaseVertex;
    public static void glDrawElementsBaseVertex(uint mode, int count, uint type, IntPtr indices, int basevertex) => _glDrawElementsBaseVertex(mode, count, type, indices, basevertex);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDRAWRANGEELEMENTSBASEVERTEXPROC(uint mode, uint start, uint end, int count, uint type, IntPtr indices, int basevertex);
    private static PFNGLDRAWRANGEELEMENTSBASEVERTEXPROC _glDrawRangeElementsBaseVertex;
    public static void glDrawRangeElementsBaseVertex(uint mode, uint start, uint end, int count, uint type, IntPtr indices, int basevertex) => _glDrawRangeElementsBaseVertex(mode, start, end, count, type, indices, basevertex);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDRAWELEMENTSINSTANCEDBASEVERTEXPROC(uint mode, int count, uint type, IntPtr indices, int instancecount, int basevertex);
    private static PFNGLDRAWELEMENTSINSTANCEDBASEVERTEXPROC _glDrawElementsInstancedBaseVertex;
    public static void glDrawElementsInstancedBaseVertex(uint mode, int count, uint type, IntPtr indices, int instancecount, int basevertex) => _glDrawElementsInstancedBaseVertex(mode, count, type, indices, instancecount, basevertex);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLMULTIDRAWELEMENTSBASEVERTEXPROC(uint mode, ref readonly int count, uint type, IntPtr indices, int drawcount, ref readonly int basevertex);
    private static PFNGLMULTIDRAWELEMENTSBASEVERTEXPROC _glMultiDrawElementsBaseVertex;
    public static void glMultiDrawElementsBaseVertex(uint mode, ref readonly int count, uint type, IntPtr indices, int drawcount, ref readonly int basevertex) => _glMultiDrawElementsBaseVertex(mode, in count, type, indices, drawcount, in basevertex);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROVOKINGVERTEXPROC(uint mode);
    private static PFNGLPROVOKINGVERTEXPROC _glProvokingVertex;
    public static void glProvokingVertex(uint mode) => _glProvokingVertex(mode);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate IntPtr PFNGLFENCESYNCPROC(uint condition, uint flags);
    private static PFNGLFENCESYNCPROC _glFenceSync;
    public static IntPtr glFenceSync(uint condition, uint flags) => _glFenceSync(condition, flags);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate bool PFNGLISSYNCPROC(IntPtr sync);
    private static PFNGLISSYNCPROC _glIsSync;
    public static bool glIsSync(IntPtr sync) => _glIsSync(sync);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDELETESYNCPROC(IntPtr sync);
    private static PFNGLDELETESYNCPROC _glDeleteSync;
    public static void glDeleteSync(IntPtr sync) => _glDeleteSync(sync);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate uint PFNGLCLIENTWAITSYNCPROC(IntPtr sync, uint flags, ulong timeout);
    private static PFNGLCLIENTWAITSYNCPROC _glClientWaitSync;
    public static uint glClientWaitSync(IntPtr sync, uint flags, ulong timeout) => _glClientWaitSync(sync, flags, timeout);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLWAITSYNCPROC(IntPtr sync, uint flags, ulong timeout);
    private static PFNGLWAITSYNCPROC _glWaitSync;
    public static void glWaitSync(IntPtr sync, uint flags, ulong timeout) => _glWaitSync(sync, flags, timeout);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETINTEGER64VPROC(uint pname, out long data);
    private static PFNGLGETINTEGER64VPROC _glGetInteger64v;
    public static void glGetInteger64v(uint pname, out long data) => _glGetInteger64v(pname, out data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETSYNCIVPROC(IntPtr sync, uint pname, int count, out int length, out int values);
    private static PFNGLGETSYNCIVPROC _glGetSynciv;
    public static void glGetSynciv(IntPtr sync, uint pname, int count, out int length, out int values) => _glGetSynciv(sync, pname, count, out length, out values);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETINTEGER64I_VPROC(uint target, uint index, out long data);
    private static PFNGLGETINTEGER64I_VPROC _glGetInteger64i_v;
    public static void glGetInteger64i_v(uint target, uint index, out long data) => _glGetInteger64i_v(target, index, out data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETBUFFERPARAMETERI64VPROC(uint target, uint pname, out long @params);
    private static PFNGLGETBUFFERPARAMETERI64VPROC _glGetBufferParameteri64v;
    public static void glGetBufferParameteri64v(uint target, uint pname, out long @params) => _glGetBufferParameteri64v(target, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLFRAMEBUFFERTEXTUREPROC(uint target, uint attachment, uint texture, int level);
    private static PFNGLFRAMEBUFFERTEXTUREPROC _glFramebufferTexture;
    public static void glFramebufferTexture(uint target, uint attachment, uint texture, int level) => _glFramebufferTexture(target, attachment, texture, level);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXIMAGE2DMULTISAMPLEPROC(uint target, int samples, uint internalformat, int width, int height, bool fixedsamplelocations);
    private static PFNGLTEXIMAGE2DMULTISAMPLEPROC _glTexImage2DMultisample;
    public static void glTexImage2DMultisample(uint target, int samples, uint internalformat, int width, int height, bool fixedsamplelocations) => _glTexImage2DMultisample(target, samples, internalformat, width, height, fixedsamplelocations);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXIMAGE3DMULTISAMPLEPROC(uint target, int samples, uint internalformat, int width, int height, int depth, bool fixedsamplelocations);
    private static PFNGLTEXIMAGE3DMULTISAMPLEPROC _glTexImage3DMultisample;
    public static void glTexImage3DMultisample(uint target, int samples, uint internalformat, int width, int height, int depth, bool fixedsamplelocations) => _glTexImage3DMultisample(target, samples, internalformat, width, height, depth, fixedsamplelocations);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETMULTISAMPLEFVPROC(uint pname, uint index, out float val);
    private static PFNGLGETMULTISAMPLEFVPROC _glGetMultisamplefv;
    public static void glGetMultisamplefv(uint pname, uint index, out float val) => _glGetMultisamplefv(pname, index, out val);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLSAMPLEMASKIPROC(uint maskNumber, uint mask);
    private static PFNGLSAMPLEMASKIPROC _glSampleMaski;
    public static void glSampleMaski(uint maskNumber, uint mask) => _glSampleMaski(maskNumber, mask);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBINDFRAGDATALOCATIONINDEXEDPROC(uint program, uint colorNumber, uint index, string name);
    private static PFNGLBINDFRAGDATALOCATIONINDEXEDPROC _glBindFragDataLocationIndexed;
    public static void glBindFragDataLocationIndexed(uint program, uint colorNumber, uint index, string name) => _glBindFragDataLocationIndexed(program, colorNumber, index, name);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int PFNGLGETFRAGDATAINDEXPROC(uint program, string name);
    private static PFNGLGETFRAGDATAINDEXPROC _glGetFragDataIndex;
    public static int glGetFragDataIndex(uint program, string name) => _glGetFragDataIndex(program, name);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGENSAMPLERSPROC(int count, out uint samplers);
    private static PFNGLGENSAMPLERSPROC _glGenSamplers;
    public static void glGenSamplers(int count, out uint samplers) => _glGenSamplers(count, out samplers);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDELETESAMPLERSPROC(int count, ref readonly uint samplers);
    private static PFNGLDELETESAMPLERSPROC _glDeleteSamplers;
    public static void glDeleteSamplers(int count, ref readonly uint samplers) => _glDeleteSamplers(count, in samplers);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate bool PFNGLISSAMPLERPROC(uint sampler);
    private static PFNGLISSAMPLERPROC _glIsSampler;
    public static bool glIsSampler(uint sampler) => _glIsSampler(sampler);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBINDSAMPLERPROC(uint unit, uint sampler);
    private static PFNGLBINDSAMPLERPROC _glBindSampler;
    public static void glBindSampler(uint unit, uint sampler) => _glBindSampler(unit, sampler);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLSAMPLERPARAMETERIPROC(uint sampler, uint pname, int param);
    private static PFNGLSAMPLERPARAMETERIPROC _glSamplerParameteri;
    public static void glSamplerParameteri(uint sampler, uint pname, int param) => _glSamplerParameteri(sampler, pname, param);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLSAMPLERPARAMETERIVPROC(uint sampler, uint pname, ref readonly int param);
    private static PFNGLSAMPLERPARAMETERIVPROC _glSamplerParameteriv;
    public static void glSamplerParameteriv(uint sampler, uint pname, ref readonly int param) => _glSamplerParameteriv(sampler, pname, in param);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLSAMPLERPARAMETERFPROC(uint sampler, uint pname, float param);
    private static PFNGLSAMPLERPARAMETERFPROC _glSamplerParameterf;
    public static void glSamplerParameterf(uint sampler, uint pname, float param) => _glSamplerParameterf(sampler, pname, param);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLSAMPLERPARAMETERFVPROC(uint sampler, uint pname, ref readonly float param);
    private static PFNGLSAMPLERPARAMETERFVPROC _glSamplerParameterfv;
    public static void glSamplerParameterfv(uint sampler, uint pname, ref readonly float param) => _glSamplerParameterfv(sampler, pname, in param);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLSAMPLERPARAMETERIIVPROC(uint sampler, uint pname, ref readonly int param);
    private static PFNGLSAMPLERPARAMETERIIVPROC _glSamplerParameterIiv;
    public static void glSamplerParameterIiv(uint sampler, uint pname, ref readonly int param) => _glSamplerParameterIiv(sampler, pname, in param);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLSAMPLERPARAMETERIUIVPROC(uint sampler, uint pname, ref readonly uint param);
    private static PFNGLSAMPLERPARAMETERIUIVPROC _glSamplerParameterIuiv;
    public static void glSamplerParameterIuiv(uint sampler, uint pname, ref readonly uint param) => _glSamplerParameterIuiv(sampler, pname, in param);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETSAMPLERPARAMETERIVPROC(uint sampler, uint pname, out int @params);
    private static PFNGLGETSAMPLERPARAMETERIVPROC _glGetSamplerParameteriv;
    public static void glGetSamplerParameteriv(uint sampler, uint pname, out int @params) => _glGetSamplerParameteriv(sampler, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETSAMPLERPARAMETERIIVPROC(uint sampler, uint pname, out int @params);
    private static PFNGLGETSAMPLERPARAMETERIIVPROC _glGetSamplerParameterIiv;
    public static void glGetSamplerParameterIiv(uint sampler, uint pname, out int @params) => _glGetSamplerParameterIiv(sampler, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETSAMPLERPARAMETERFVPROC(uint sampler, uint pname, out float @params);
    private static PFNGLGETSAMPLERPARAMETERFVPROC _glGetSamplerParameterfv;
    public static void glGetSamplerParameterfv(uint sampler, uint pname, out float @params) => _glGetSamplerParameterfv(sampler, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETSAMPLERPARAMETERIUIVPROC(uint sampler, uint pname, out uint @params);
    private static PFNGLGETSAMPLERPARAMETERIUIVPROC _glGetSamplerParameterIuiv;
    public static void glGetSamplerParameterIuiv(uint sampler, uint pname, out uint @params) => _glGetSamplerParameterIuiv(sampler, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLQUERYCOUNTERPROC(uint id, uint target);
    private static PFNGLQUERYCOUNTERPROC _glQueryCounter;
    public static void glQueryCounter(uint id, uint target) => _glQueryCounter(id, target);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETQUERYOBJECTI64VPROC(uint id, uint pname, out long @params);
    private static PFNGLGETQUERYOBJECTI64VPROC _glGetQueryObjecti64v;
    public static void glGetQueryObjecti64v(uint id, uint pname, out long @params) => _glGetQueryObjecti64v(id, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETQUERYOBJECTUI64VPROC(uint id, uint pname, out ulong @params);
    private static PFNGLGETQUERYOBJECTUI64VPROC _glGetQueryObjectui64v;
    public static void glGetQueryObjectui64v(uint id, uint pname, out ulong @params) => _glGetQueryObjectui64v(id, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBDIVISORPROC(uint index, uint divisor);
    private static PFNGLVERTEXATTRIBDIVISORPROC _glVertexAttribDivisor;
    public static void glVertexAttribDivisor(uint index, uint divisor) => _glVertexAttribDivisor(index, divisor);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBP1UIPROC(uint index, uint type, bool normalized, uint value);
    private static PFNGLVERTEXATTRIBP1UIPROC _glVertexAttribP1ui;
    public static void glVertexAttribP1ui(uint index, uint type, bool normalized, uint value) => _glVertexAttribP1ui(index, type, normalized, value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBP1UIVPROC(uint index, uint type, bool normalized, ref readonly uint value);
    private static PFNGLVERTEXATTRIBP1UIVPROC _glVertexAttribP1uiv;
    public static void glVertexAttribP1uiv(uint index, uint type, bool normalized, ref readonly uint value) => _glVertexAttribP1uiv(index, type, normalized, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBP2UIPROC(uint index, uint type, bool normalized, uint value);
    private static PFNGLVERTEXATTRIBP2UIPROC _glVertexAttribP2ui;
    public static void glVertexAttribP2ui(uint index, uint type, bool normalized, uint value) => _glVertexAttribP2ui(index, type, normalized, value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBP2UIVPROC(uint index, uint type, bool normalized, ref readonly uint value);
    private static PFNGLVERTEXATTRIBP2UIVPROC _glVertexAttribP2uiv;
    public static void glVertexAttribP2uiv(uint index, uint type, bool normalized, ref readonly uint value) => _glVertexAttribP2uiv(index, type, normalized, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBP3UIPROC(uint index, uint type, bool normalized, uint value);
    private static PFNGLVERTEXATTRIBP3UIPROC _glVertexAttribP3ui;
    public static void glVertexAttribP3ui(uint index, uint type, bool normalized, uint value) => _glVertexAttribP3ui(index, type, normalized, value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBP3UIVPROC(uint index, uint type, bool normalized, ref readonly uint value);
    private static PFNGLVERTEXATTRIBP3UIVPROC _glVertexAttribP3uiv;
    public static void glVertexAttribP3uiv(uint index, uint type, bool normalized, ref readonly uint value) => _glVertexAttribP3uiv(index, type, normalized, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBP4UIPROC(uint index, uint type, bool normalized, uint value);
    private static PFNGLVERTEXATTRIBP4UIPROC _glVertexAttribP4ui;
    public static void glVertexAttribP4ui(uint index, uint type, bool normalized, uint value) => _glVertexAttribP4ui(index, type, normalized, value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBP4UIVPROC(uint index, uint type, bool normalized, ref readonly uint value);
    private static PFNGLVERTEXATTRIBP4UIVPROC _glVertexAttribP4uiv;
    public static void glVertexAttribP4uiv(uint index, uint type, bool normalized, ref readonly uint value) => _glVertexAttribP4uiv(index, type, normalized, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXP2UIPROC(uint type, uint value);
    private static PFNGLVERTEXP2UIPROC _glVertexP2ui;
    public static void glVertexP2ui(uint type, uint value) => _glVertexP2ui(type, value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXP2UIVPROC(uint type, ref readonly uint value);
    private static PFNGLVERTEXP2UIVPROC _glVertexP2uiv;
    public static void glVertexP2uiv(uint type, ref readonly uint value) => _glVertexP2uiv(type, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXP3UIPROC(uint type, uint value);
    private static PFNGLVERTEXP3UIPROC _glVertexP3ui;
    public static void glVertexP3ui(uint type, uint value) => _glVertexP3ui(type, value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXP3UIVPROC(uint type, ref readonly uint value);
    private static PFNGLVERTEXP3UIVPROC _glVertexP3uiv;
    public static void glVertexP3uiv(uint type, ref readonly uint value) => _glVertexP3uiv(type, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXP4UIPROC(uint type, uint value);
    private static PFNGLVERTEXP4UIPROC _glVertexP4ui;
    public static void glVertexP4ui(uint type, uint value) => _glVertexP4ui(type, value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXP4UIVPROC(uint type, ref readonly uint value);
    private static PFNGLVERTEXP4UIVPROC _glVertexP4uiv;
    public static void glVertexP4uiv(uint type, ref readonly uint value) => _glVertexP4uiv(type, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXCOORDP1UIPROC(uint type, uint coords);
    private static PFNGLTEXCOORDP1UIPROC _glTexCoordP1ui;
    public static void glTexCoordP1ui(uint type, uint coords) => _glTexCoordP1ui(type, coords);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXCOORDP1UIVPROC(uint type, ref readonly uint coords);
    private static PFNGLTEXCOORDP1UIVPROC _glTexCoordP1uiv;
    public static void glTexCoordP1uiv(uint type, ref readonly uint coords) => _glTexCoordP1uiv(type, in coords);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXCOORDP2UIPROC(uint type, uint coords);
    private static PFNGLTEXCOORDP2UIPROC _glTexCoordP2ui;
    public static void glTexCoordP2ui(uint type, uint coords) => _glTexCoordP2ui(type, coords);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXCOORDP2UIVPROC(uint type, ref readonly uint coords);
    private static PFNGLTEXCOORDP2UIVPROC _glTexCoordP2uiv;
    public static void glTexCoordP2uiv(uint type, ref readonly uint coords) => _glTexCoordP2uiv(type, in coords);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXCOORDP3UIPROC(uint type, uint coords);
    private static PFNGLTEXCOORDP3UIPROC _glTexCoordP3ui;
    public static void glTexCoordP3ui(uint type, uint coords) => _glTexCoordP3ui(type, coords);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXCOORDP3UIVPROC(uint type, ref readonly uint coords);
    private static PFNGLTEXCOORDP3UIVPROC _glTexCoordP3uiv;
    public static void glTexCoordP3uiv(uint type, ref readonly uint coords) => _glTexCoordP3uiv(type, in coords);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXCOORDP4UIPROC(uint type, uint coords);
    private static PFNGLTEXCOORDP4UIPROC _glTexCoordP4ui;
    public static void glTexCoordP4ui(uint type, uint coords) => _glTexCoordP4ui(type, coords);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXCOORDP4UIVPROC(uint type, ref readonly uint coords);
    private static PFNGLTEXCOORDP4UIVPROC _glTexCoordP4uiv;
    public static void glTexCoordP4uiv(uint type, ref readonly uint coords) => _glTexCoordP4uiv(type, in coords);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLMULTITEXCOORDP1UIPROC(uint texture, uint type, uint coords);
    private static PFNGLMULTITEXCOORDP1UIPROC _glMultiTexCoordP1ui;
    public static void glMultiTexCoordP1ui(uint texture, uint type, uint coords) => _glMultiTexCoordP1ui(texture, type, coords);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLMULTITEXCOORDP1UIVPROC(uint texture, uint type, ref readonly uint coords);
    private static PFNGLMULTITEXCOORDP1UIVPROC _glMultiTexCoordP1uiv;
    public static void glMultiTexCoordP1uiv(uint texture, uint type, ref readonly uint coords) => _glMultiTexCoordP1uiv(texture, type, in coords);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLMULTITEXCOORDP2UIPROC(uint texture, uint type, uint coords);
    private static PFNGLMULTITEXCOORDP2UIPROC _glMultiTexCoordP2ui;
    public static void glMultiTexCoordP2ui(uint texture, uint type, uint coords) => _glMultiTexCoordP2ui(texture, type, coords);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLMULTITEXCOORDP2UIVPROC(uint texture, uint type, ref readonly uint coords);
    private static PFNGLMULTITEXCOORDP2UIVPROC _glMultiTexCoordP2uiv;
    public static void glMultiTexCoordP2uiv(uint texture, uint type, ref readonly uint coords) => _glMultiTexCoordP2uiv(texture, type, in coords);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLMULTITEXCOORDP3UIPROC(uint texture, uint type, uint coords);
    private static PFNGLMULTITEXCOORDP3UIPROC _glMultiTexCoordP3ui;
    public static void glMultiTexCoordP3ui(uint texture, uint type, uint coords) => _glMultiTexCoordP3ui(texture, type, coords);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLMULTITEXCOORDP3UIVPROC(uint texture, uint type, ref readonly uint coords);
    private static PFNGLMULTITEXCOORDP3UIVPROC _glMultiTexCoordP3uiv;
    public static void glMultiTexCoordP3uiv(uint texture, uint type, ref readonly uint coords) => _glMultiTexCoordP3uiv(texture, type, in coords);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLMULTITEXCOORDP4UIPROC(uint texture, uint type, uint coords);
    private static PFNGLMULTITEXCOORDP4UIPROC _glMultiTexCoordP4ui;
    public static void glMultiTexCoordP4ui(uint texture, uint type, uint coords) => _glMultiTexCoordP4ui(texture, type, coords);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLMULTITEXCOORDP4UIVPROC(uint texture, uint type, ref readonly uint coords);
    private static PFNGLMULTITEXCOORDP4UIVPROC _glMultiTexCoordP4uiv;
    public static void glMultiTexCoordP4uiv(uint texture, uint type, ref readonly uint coords) => _glMultiTexCoordP4uiv(texture, type, in coords);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLNORMALP3UIPROC(uint type, uint coords);
    private static PFNGLNORMALP3UIPROC _glNormalP3ui;
    public static void glNormalP3ui(uint type, uint coords) => _glNormalP3ui(type, coords);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLNORMALP3UIVPROC(uint type, ref readonly uint coords);
    private static PFNGLNORMALP3UIVPROC _glNormalP3uiv;
    public static void glNormalP3uiv(uint type, ref readonly uint coords) => _glNormalP3uiv(type, in coords);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCOLORP3UIPROC(uint type, uint color);
    private static PFNGLCOLORP3UIPROC _glColorP3ui;
    public static void glColorP3ui(uint type, uint color) => _glColorP3ui(type, color);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCOLORP3UIVPROC(uint type, ref readonly uint color);
    private static PFNGLCOLORP3UIVPROC _glColorP3uiv;
    public static void glColorP3uiv(uint type, ref readonly uint color) => _glColorP3uiv(type, in color);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCOLORP4UIPROC(uint type, uint color);
    private static PFNGLCOLORP4UIPROC _glColorP4ui;
    public static void glColorP4ui(uint type, uint color) => _glColorP4ui(type, color);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCOLORP4UIVPROC(uint type, ref readonly uint color);
    private static PFNGLCOLORP4UIVPROC _glColorP4uiv;
    public static void glColorP4uiv(uint type, ref readonly uint color) => _glColorP4uiv(type, in color);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLSECONDARYCOLORP3UIPROC(uint type, uint color);
    private static PFNGLSECONDARYCOLORP3UIPROC _glSecondaryColorP3ui;
    public static void glSecondaryColorP3ui(uint type, uint color) => _glSecondaryColorP3ui(type, color);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLSECONDARYCOLORP3UIVPROC(uint type, ref readonly uint color);
    private static PFNGLSECONDARYCOLORP3UIVPROC _glSecondaryColorP3uiv;
    public static void glSecondaryColorP3uiv(uint type, ref readonly uint color) => _glSecondaryColorP3uiv(type, in color);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLMINSAMPLESHADINGPROC(float value);
    private static PFNGLMINSAMPLESHADINGPROC _glMinSampleShading;
    public static void glMinSampleShading(float value) => _glMinSampleShading(value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBLENDEQUATIONIPROC(uint buf, uint mode);
    private static PFNGLBLENDEQUATIONIPROC _glBlendEquationi;
    public static void glBlendEquationi(uint buf, uint mode) => _glBlendEquationi(buf, mode);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBLENDEQUATIONSEPARATEIPROC(uint buf, uint modeRGB, uint modeAlpha);
    private static PFNGLBLENDEQUATIONSEPARATEIPROC _glBlendEquationSeparatei;
    public static void glBlendEquationSeparatei(uint buf, uint modeRGB, uint modeAlpha) => _glBlendEquationSeparatei(buf, modeRGB, modeAlpha);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBLENDFUNCIPROC(uint buf, uint src, uint dst);
    private static PFNGLBLENDFUNCIPROC _glBlendFunci;
    public static void glBlendFunci(uint buf, uint src, uint dst) => _glBlendFunci(buf, src, dst);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBLENDFUNCSEPARATEIPROC(uint buf, uint srcRGB, uint dstRGB, uint srcAlpha, uint dstAlpha);
    private static PFNGLBLENDFUNCSEPARATEIPROC _glBlendFuncSeparatei;
    public static void glBlendFuncSeparatei(uint buf, uint srcRGB, uint dstRGB, uint srcAlpha, uint dstAlpha) => _glBlendFuncSeparatei(buf, srcRGB, dstRGB, srcAlpha, dstAlpha);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDRAWARRAYSINDIRECTPROC(uint mode, IntPtr indirect);
    private static PFNGLDRAWARRAYSINDIRECTPROC _glDrawArraysIndirect;
    public static void glDrawArraysIndirect(uint mode, IntPtr indirect) => _glDrawArraysIndirect(mode, indirect);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDRAWELEMENTSINDIRECTPROC(uint mode, uint type, IntPtr indirect);
    private static PFNGLDRAWELEMENTSINDIRECTPROC _glDrawElementsIndirect;
    public static void glDrawElementsIndirect(uint mode, uint type, IntPtr indirect) => _glDrawElementsIndirect(mode, type, indirect);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORM1DPROC(int location, double x);
    private static PFNGLUNIFORM1DPROC _glUniform1d;
    public static void glUniform1d(int location, double x) => _glUniform1d(location, x);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORM2DPROC(int location, double x, double y);
    private static PFNGLUNIFORM2DPROC _glUniform2d;
    public static void glUniform2d(int location, double x, double y) => _glUniform2d(location, x, y);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORM3DPROC(int location, double x, double y, double z);
    private static PFNGLUNIFORM3DPROC _glUniform3d;
    public static void glUniform3d(int location, double x, double y, double z) => _glUniform3d(location, x, y, z);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORM4DPROC(int location, double x, double y, double z, double w);
    private static PFNGLUNIFORM4DPROC _glUniform4d;
    public static void glUniform4d(int location, double x, double y, double z, double w) => _glUniform4d(location, x, y, z, w);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORM1DVPROC(int location, int count, ref readonly double value);
    private static PFNGLUNIFORM1DVPROC _glUniform1dv;
    public static void glUniform1dv(int location, int count, ref readonly double value) => _glUniform1dv(location, count, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORM2DVPROC(int location, int count, ref readonly double value);
    private static PFNGLUNIFORM2DVPROC _glUniform2dv;
    public static void glUniform2dv(int location, int count, ref readonly double value) => _glUniform2dv(location, count, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORM3DVPROC(int location, int count, ref readonly double value);
    private static PFNGLUNIFORM3DVPROC _glUniform3dv;
    public static void glUniform3dv(int location, int count, ref readonly double value) => _glUniform3dv(location, count, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORM4DVPROC(int location, int count, ref readonly double value);
    private static PFNGLUNIFORM4DVPROC _glUniform4dv;
    public static void glUniform4dv(int location, int count, ref readonly double value) => _glUniform4dv(location, count, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORMMATRIX2DVPROC(int location, int count, bool transpose, ref readonly double value);
    private static PFNGLUNIFORMMATRIX2DVPROC _glUniformMatrix2dv;
    public static void glUniformMatrix2dv(int location, int count, bool transpose, ref readonly double value) => _glUniformMatrix2dv(location, count, transpose, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORMMATRIX3DVPROC(int location, int count, bool transpose, ref readonly double value);
    private static PFNGLUNIFORMMATRIX3DVPROC _glUniformMatrix3dv;
    public static void glUniformMatrix3dv(int location, int count, bool transpose, ref readonly double value) => _glUniformMatrix3dv(location, count, transpose, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORMMATRIX4DVPROC(int location, int count, bool transpose, ref readonly double value);
    private static PFNGLUNIFORMMATRIX4DVPROC _glUniformMatrix4dv;
    public static void glUniformMatrix4dv(int location, int count, bool transpose, ref readonly double value) => _glUniformMatrix4dv(location, count, transpose, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORMMATRIX2X3DVPROC(int location, int count, bool transpose, ref readonly double value);
    private static PFNGLUNIFORMMATRIX2X3DVPROC _glUniformMatrix2x3dv;
    public static void glUniformMatrix2x3dv(int location, int count, bool transpose, ref readonly double value) => _glUniformMatrix2x3dv(location, count, transpose, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORMMATRIX2X4DVPROC(int location, int count, bool transpose, ref readonly double value);
    private static PFNGLUNIFORMMATRIX2X4DVPROC _glUniformMatrix2x4dv;
    public static void glUniformMatrix2x4dv(int location, int count, bool transpose, ref readonly double value) => _glUniformMatrix2x4dv(location, count, transpose, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORMMATRIX3X2DVPROC(int location, int count, bool transpose, ref readonly double value);
    private static PFNGLUNIFORMMATRIX3X2DVPROC _glUniformMatrix3x2dv;
    public static void glUniformMatrix3x2dv(int location, int count, bool transpose, ref readonly double value) => _glUniformMatrix3x2dv(location, count, transpose, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORMMATRIX3X4DVPROC(int location, int count, bool transpose, ref readonly double value);
    private static PFNGLUNIFORMMATRIX3X4DVPROC _glUniformMatrix3x4dv;
    public static void glUniformMatrix3x4dv(int location, int count, bool transpose, ref readonly double value) => _glUniformMatrix3x4dv(location, count, transpose, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORMMATRIX4X2DVPROC(int location, int count, bool transpose, ref readonly double value);
    private static PFNGLUNIFORMMATRIX4X2DVPROC _glUniformMatrix4x2dv;
    public static void glUniformMatrix4x2dv(int location, int count, bool transpose, ref readonly double value) => _glUniformMatrix4x2dv(location, count, transpose, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORMMATRIX4X3DVPROC(int location, int count, bool transpose, ref readonly double value);
    private static PFNGLUNIFORMMATRIX4X3DVPROC _glUniformMatrix4x3dv;
    public static void glUniformMatrix4x3dv(int location, int count, bool transpose, ref readonly double value) => _glUniformMatrix4x3dv(location, count, transpose, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETUNIFORMDVPROC(uint program, int location, out double @params);
    private static PFNGLGETUNIFORMDVPROC _glGetUniformdv;
    public static void glGetUniformdv(uint program, int location, out double @params) => _glGetUniformdv(program, location, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int PFNGLGETSUBROUTINEUNIFORMLOCATIONPROC(uint program, uint shadertype, string name);
    private static PFNGLGETSUBROUTINEUNIFORMLOCATIONPROC _glGetSubroutineUniformLocation;
    public static int glGetSubroutineUniformLocation(uint program, uint shadertype, string name) => _glGetSubroutineUniformLocation(program, shadertype, name);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate uint PFNGLGETSUBROUTINEINDEXPROC(uint program, uint shadertype, string name);
    private static PFNGLGETSUBROUTINEINDEXPROC _glGetSubroutineIndex;
    public static uint glGetSubroutineIndex(uint program, uint shadertype, string name) => _glGetSubroutineIndex(program, shadertype, name);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETACTIVESUBROUTINEUNIFORMIVPROC(uint program, uint shadertype, uint index, uint pname, out int values);
    private static PFNGLGETACTIVESUBROUTINEUNIFORMIVPROC _glGetActiveSubroutineUniformiv;
    public static void glGetActiveSubroutineUniformiv(uint program, uint shadertype, uint index, uint pname, out int values) => _glGetActiveSubroutineUniformiv(program, shadertype, index, pname, out values);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETACTIVESUBROUTINEUNIFORMNAMEPROC(uint program, uint shadertype, uint index, int bufSize, out int length, string name);
    private static PFNGLGETACTIVESUBROUTINEUNIFORMNAMEPROC _glGetActiveSubroutineUniformName;
    public static void glGetActiveSubroutineUniformName(uint program, uint shadertype, uint index, int bufSize, out int length, string name) => _glGetActiveSubroutineUniformName(program, shadertype, index, bufSize, out length, name);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETACTIVESUBROUTINENAMEPROC(uint program, uint shadertype, uint index, int bufSize, out int length, string name);
    private static PFNGLGETACTIVESUBROUTINENAMEPROC _glGetActiveSubroutineName;
    public static void glGetActiveSubroutineName(uint program, uint shadertype, uint index, int bufSize, out int length, string name) => _glGetActiveSubroutineName(program, shadertype, index, bufSize, out length, name);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORMSUBROUTINESUIVPROC(uint shadertype, int count, ref readonly uint indices);
    private static PFNGLUNIFORMSUBROUTINESUIVPROC _glUniformSubroutinesuiv;
    public static void glUniformSubroutinesuiv(uint shadertype, int count, ref readonly uint indices) => _glUniformSubroutinesuiv(shadertype, count, in indices);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETUNIFORMSUBROUTINEUIVPROC(uint shadertype, int location, out uint @params);
    private static PFNGLGETUNIFORMSUBROUTINEUIVPROC _glGetUniformSubroutineuiv;
    public static void glGetUniformSubroutineuiv(uint shadertype, int location, out uint @params) => _glGetUniformSubroutineuiv(shadertype, location, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETPROGRAMSTAGEIVPROC(uint program, uint shadertype, uint pname, out int values);
    private static PFNGLGETPROGRAMSTAGEIVPROC _glGetProgramStageiv;
    public static void glGetProgramStageiv(uint program, uint shadertype, uint pname, out int values) => _glGetProgramStageiv(program, shadertype, pname, out values);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPATCHPARAMETERIPROC(uint pname, int value);
    private static PFNGLPATCHPARAMETERIPROC _glPatchParameteri;
    public static void glPatchParameteri(uint pname, int value) => _glPatchParameteri(pname, value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPATCHPARAMETERFVPROC(uint pname, ref readonly float values);
    private static PFNGLPATCHPARAMETERFVPROC _glPatchParameterfv;
    public static void glPatchParameterfv(uint pname, ref readonly float values) => _glPatchParameterfv(pname, in values);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBINDTRANSFORMFEEDBACKPROC(uint target, uint id);
    private static PFNGLBINDTRANSFORMFEEDBACKPROC _glBindTransformFeedback;
    public static void glBindTransformFeedback(uint target, uint id) => _glBindTransformFeedback(target, id);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDELETETRANSFORMFEEDBACKSPROC(int n, ref readonly uint ids);
    private static PFNGLDELETETRANSFORMFEEDBACKSPROC _glDeleteTransformFeedbacks;
    public static void glDeleteTransformFeedbacks(int n, ref readonly uint ids) => _glDeleteTransformFeedbacks(n, in ids);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGENTRANSFORMFEEDBACKSPROC(int n, out uint ids);
    private static PFNGLGENTRANSFORMFEEDBACKSPROC _glGenTransformFeedbacks;
    public static void glGenTransformFeedbacks(int n, out uint ids) => _glGenTransformFeedbacks(n, out ids);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate bool PFNGLISTRANSFORMFEEDBACKPROC(uint id);
    private static PFNGLISTRANSFORMFEEDBACKPROC _glIsTransformFeedback;
    public static bool glIsTransformFeedback(uint id) => _glIsTransformFeedback(id);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPAUSETRANSFORMFEEDBACKPROC();
    private static PFNGLPAUSETRANSFORMFEEDBACKPROC _glPauseTransformFeedback;
    public static void glPauseTransformFeedback() => _glPauseTransformFeedback();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLRESUMETRANSFORMFEEDBACKPROC();
    private static PFNGLRESUMETRANSFORMFEEDBACKPROC _glResumeTransformFeedback;
    public static void glResumeTransformFeedback() => _glResumeTransformFeedback();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDRAWTRANSFORMFEEDBACKPROC(uint mode, uint id);
    private static PFNGLDRAWTRANSFORMFEEDBACKPROC _glDrawTransformFeedback;
    public static void glDrawTransformFeedback(uint mode, uint id) => _glDrawTransformFeedback(mode, id);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDRAWTRANSFORMFEEDBACKSTREAMPROC(uint mode, uint id, uint stream);
    private static PFNGLDRAWTRANSFORMFEEDBACKSTREAMPROC _glDrawTransformFeedbackStream;
    public static void glDrawTransformFeedbackStream(uint mode, uint id, uint stream) => _glDrawTransformFeedbackStream(mode, id, stream);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBEGINQUERYINDEXEDPROC(uint target, uint index, uint id);
    private static PFNGLBEGINQUERYINDEXEDPROC _glBeginQueryIndexed;
    public static void glBeginQueryIndexed(uint target, uint index, uint id) => _glBeginQueryIndexed(target, index, id);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLENDQUERYINDEXEDPROC(uint target, uint index);
    private static PFNGLENDQUERYINDEXEDPROC _glEndQueryIndexed;
    public static void glEndQueryIndexed(uint target, uint index) => _glEndQueryIndexed(target, index);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETQUERYINDEXEDIVPROC(uint target, uint index, uint pname, out int @params);
    private static PFNGLGETQUERYINDEXEDIVPROC _glGetQueryIndexediv;
    public static void glGetQueryIndexediv(uint target, uint index, uint pname, out int @params) => _glGetQueryIndexediv(target, index, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLRELEASESHADERCOMPILERPROC();
    private static PFNGLRELEASESHADERCOMPILERPROC _glReleaseShaderCompiler;
    public static void glReleaseShaderCompiler() => _glReleaseShaderCompiler();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLSHADERBINARYPROC(int count, ref readonly uint shaders, uint binaryFormat, IntPtr binary, int length);
    private static PFNGLSHADERBINARYPROC _glShaderBinary;
    public static void glShaderBinary(int count, ref readonly uint shaders, uint binaryFormat, IntPtr binary, int length) => _glShaderBinary(count, in shaders, binaryFormat, binary, length);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETSHADERPRECISIONFORMATPROC(uint shadertype, uint precisiontype, out int range, out int precision);
    private static PFNGLGETSHADERPRECISIONFORMATPROC _glGetShaderPrecisionFormat;
    public static void glGetShaderPrecisionFormat(uint shadertype, uint precisiontype, out int range, out int precision) => _glGetShaderPrecisionFormat(shadertype, precisiontype, out range, out precision);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDEPTHRANGEFPROC(float n, float f);
    private static PFNGLDEPTHRANGEFPROC _glDepthRangef;
    public static void glDepthRangef(float n, float f) => _glDepthRangef(n, f);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCLEARDEPTHFPROC(float d);
    private static PFNGLCLEARDEPTHFPROC _glClearDepthf;
    public static void glClearDepthf(float d) => _glClearDepthf(d);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETPROGRAMBINARYPROC(uint program, int bufSize, out int length, out uint binaryFormat, IntPtr binary);
    private static PFNGLGETPROGRAMBINARYPROC _glGetProgramBinary;
    public static void glGetProgramBinary(uint program, int bufSize, out int length, out uint binaryFormat, IntPtr binary) => _glGetProgramBinary(program, bufSize, out length, out binaryFormat, binary);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMBINARYPROC(uint program, uint binaryFormat, IntPtr binary, int length);
    private static PFNGLPROGRAMBINARYPROC _glProgramBinary;
    public static void glProgramBinary(uint program, uint binaryFormat, IntPtr binary, int length) => _glProgramBinary(program, binaryFormat, binary, length);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMPARAMETERIPROC(uint program, uint pname, int value);
    private static PFNGLPROGRAMPARAMETERIPROC _glProgramParameteri;
    public static void glProgramParameteri(uint program, uint pname, int value) => _glProgramParameteri(program, pname, value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUSEPROGRAMSTAGESPROC(uint pipeline, uint stages, uint program);
    private static PFNGLUSEPROGRAMSTAGESPROC _glUseProgramStages;
    public static void glUseProgramStages(uint pipeline, uint stages, uint program) => _glUseProgramStages(pipeline, stages, program);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLACTIVESHADERPROGRAMPROC(uint pipeline, uint program);
    private static PFNGLACTIVESHADERPROGRAMPROC _glActiveShaderProgram;
    public static void glActiveShaderProgram(uint pipeline, uint program) => _glActiveShaderProgram(pipeline, program);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate uint PFNGLCREATESHADERPROGRAMVPROC(uint type, int count, string strings);
    private static PFNGLCREATESHADERPROGRAMVPROC _glCreateShaderProgramv;
    public static uint glCreateShaderProgramv(uint type, int count, string strings) => _glCreateShaderProgramv(type, count, strings);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBINDPROGRAMPIPELINEPROC(uint pipeline);
    private static PFNGLBINDPROGRAMPIPELINEPROC _glBindProgramPipeline;
    public static void glBindProgramPipeline(uint pipeline) => _glBindProgramPipeline(pipeline);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDELETEPROGRAMPIPELINESPROC(int n, ref readonly uint pipelines);
    private static PFNGLDELETEPROGRAMPIPELINESPROC _glDeleteProgramPipelines;
    public static void glDeleteProgramPipelines(int n, ref readonly uint pipelines) => _glDeleteProgramPipelines(n, in pipelines);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGENPROGRAMPIPELINESPROC(int n, out uint pipelines);
    private static PFNGLGENPROGRAMPIPELINESPROC _glGenProgramPipelines;
    public static void glGenProgramPipelines(int n, out uint pipelines) => _glGenProgramPipelines(n, out pipelines);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate bool PFNGLISPROGRAMPIPELINEPROC(uint pipeline);
    private static PFNGLISPROGRAMPIPELINEPROC _glIsProgramPipeline;
    public static bool glIsProgramPipeline(uint pipeline) => _glIsProgramPipeline(pipeline);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETPROGRAMPIPELINEIVPROC(uint pipeline, uint pname, out int @params);
    private static PFNGLGETPROGRAMPIPELINEIVPROC _glGetProgramPipelineiv;
    public static void glGetProgramPipelineiv(uint pipeline, uint pname, out int @params) => _glGetProgramPipelineiv(pipeline, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM1IPROC(uint program, int location, int v0);
    private static PFNGLPROGRAMUNIFORM1IPROC _glProgramUniform1i;
    public static void glProgramUniform1i(uint program, int location, int v0) => _glProgramUniform1i(program, location, v0);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM1IVPROC(uint program, int location, int count, ref readonly int value);
    private static PFNGLPROGRAMUNIFORM1IVPROC _glProgramUniform1iv;
    public static void glProgramUniform1iv(uint program, int location, int count, ref readonly int value) => _glProgramUniform1iv(program, location, count, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM1FPROC(uint program, int location, float v0);
    private static PFNGLPROGRAMUNIFORM1FPROC _glProgramUniform1f;
    public static void glProgramUniform1f(uint program, int location, float v0) => _glProgramUniform1f(program, location, v0);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM1FVPROC(uint program, int location, int count, ref readonly float value);
    private static PFNGLPROGRAMUNIFORM1FVPROC _glProgramUniform1fv;
    public static void glProgramUniform1fv(uint program, int location, int count, ref readonly float value) => _glProgramUniform1fv(program, location, count, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM1DPROC(uint program, int location, double v0);
    private static PFNGLPROGRAMUNIFORM1DPROC _glProgramUniform1d;
    public static void glProgramUniform1d(uint program, int location, double v0) => _glProgramUniform1d(program, location, v0);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM1DVPROC(uint program, int location, int count, ref readonly double value);
    private static PFNGLPROGRAMUNIFORM1DVPROC _glProgramUniform1dv;
    public static void glProgramUniform1dv(uint program, int location, int count, ref readonly double value) => _glProgramUniform1dv(program, location, count, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM1UIPROC(uint program, int location, uint v0);
    private static PFNGLPROGRAMUNIFORM1UIPROC _glProgramUniform1ui;
    public static void glProgramUniform1ui(uint program, int location, uint v0) => _glProgramUniform1ui(program, location, v0);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM1UIVPROC(uint program, int location, int count, ref readonly uint value);
    private static PFNGLPROGRAMUNIFORM1UIVPROC _glProgramUniform1uiv;
    public static void glProgramUniform1uiv(uint program, int location, int count, ref readonly uint value) => _glProgramUniform1uiv(program, location, count, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM2IPROC(uint program, int location, int v0, int v1);
    private static PFNGLPROGRAMUNIFORM2IPROC _glProgramUniform2i;
    public static void glProgramUniform2i(uint program, int location, int v0, int v1) => _glProgramUniform2i(program, location, v0, v1);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM2IVPROC(uint program, int location, int count, ref readonly int value);
    private static PFNGLPROGRAMUNIFORM2IVPROC _glProgramUniform2iv;
    public static void glProgramUniform2iv(uint program, int location, int count, ref readonly int value) => _glProgramUniform2iv(program, location, count, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM2FPROC(uint program, int location, float v0, float v1);
    private static PFNGLPROGRAMUNIFORM2FPROC _glProgramUniform2f;
    public static void glProgramUniform2f(uint program, int location, float v0, float v1) => _glProgramUniform2f(program, location, v0, v1);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM2FVPROC(uint program, int location, int count, ref readonly float value);
    private static PFNGLPROGRAMUNIFORM2FVPROC _glProgramUniform2fv;
    public static void glProgramUniform2fv(uint program, int location, int count, ref readonly float value) => _glProgramUniform2fv(program, location, count, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM2DPROC(uint program, int location, double v0, double v1);
    private static PFNGLPROGRAMUNIFORM2DPROC _glProgramUniform2d;
    public static void glProgramUniform2d(uint program, int location, double v0, double v1) => _glProgramUniform2d(program, location, v0, v1);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM2DVPROC(uint program, int location, int count, ref readonly double value);
    private static PFNGLPROGRAMUNIFORM2DVPROC _glProgramUniform2dv;
    public static void glProgramUniform2dv(uint program, int location, int count, ref readonly double value) => _glProgramUniform2dv(program, location, count, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM2UIPROC(uint program, int location, uint v0, uint v1);
    private static PFNGLPROGRAMUNIFORM2UIPROC _glProgramUniform2ui;
    public static void glProgramUniform2ui(uint program, int location, uint v0, uint v1) => _glProgramUniform2ui(program, location, v0, v1);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM2UIVPROC(uint program, int location, int count, ref readonly uint value);
    private static PFNGLPROGRAMUNIFORM2UIVPROC _glProgramUniform2uiv;
    public static void glProgramUniform2uiv(uint program, int location, int count, ref readonly uint value) => _glProgramUniform2uiv(program, location, count, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM3IPROC(uint program, int location, int v0, int v1, int v2);
    private static PFNGLPROGRAMUNIFORM3IPROC _glProgramUniform3i;
    public static void glProgramUniform3i(uint program, int location, int v0, int v1, int v2) => _glProgramUniform3i(program, location, v0, v1, v2);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM3IVPROC(uint program, int location, int count, ref readonly int value);
    private static PFNGLPROGRAMUNIFORM3IVPROC _glProgramUniform3iv;
    public static void glProgramUniform3iv(uint program, int location, int count, ref readonly int value) => _glProgramUniform3iv(program, location, count, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM3FPROC(uint program, int location, float v0, float v1, float v2);
    private static PFNGLPROGRAMUNIFORM3FPROC _glProgramUniform3f;
    public static void glProgramUniform3f(uint program, int location, float v0, float v1, float v2) => _glProgramUniform3f(program, location, v0, v1, v2);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM3FVPROC(uint program, int location, int count, ref readonly float value);
    private static PFNGLPROGRAMUNIFORM3FVPROC _glProgramUniform3fv;
    public static void glProgramUniform3fv(uint program, int location, int count, ref readonly float value) => _glProgramUniform3fv(program, location, count, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM3DPROC(uint program, int location, double v0, double v1, double v2);
    private static PFNGLPROGRAMUNIFORM3DPROC _glProgramUniform3d;
    public static void glProgramUniform3d(uint program, int location, double v0, double v1, double v2) => _glProgramUniform3d(program, location, v0, v1, v2);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM3DVPROC(uint program, int location, int count, ref readonly double value);
    private static PFNGLPROGRAMUNIFORM3DVPROC _glProgramUniform3dv;
    public static void glProgramUniform3dv(uint program, int location, int count, ref readonly double value) => _glProgramUniform3dv(program, location, count, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM3UIPROC(uint program, int location, uint v0, uint v1, uint v2);
    private static PFNGLPROGRAMUNIFORM3UIPROC _glProgramUniform3ui;
    public static void glProgramUniform3ui(uint program, int location, uint v0, uint v1, uint v2) => _glProgramUniform3ui(program, location, v0, v1, v2);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM3UIVPROC(uint program, int location, int count, ref readonly uint value);
    private static PFNGLPROGRAMUNIFORM3UIVPROC _glProgramUniform3uiv;
    public static void glProgramUniform3uiv(uint program, int location, int count, ref readonly uint value) => _glProgramUniform3uiv(program, location, count, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM4IPROC(uint program, int location, int v0, int v1, int v2, int v3);
    private static PFNGLPROGRAMUNIFORM4IPROC _glProgramUniform4i;
    public static void glProgramUniform4i(uint program, int location, int v0, int v1, int v2, int v3) => _glProgramUniform4i(program, location, v0, v1, v2, v3);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM4IVPROC(uint program, int location, int count, ref readonly int value);
    private static PFNGLPROGRAMUNIFORM4IVPROC _glProgramUniform4iv;
    public static void glProgramUniform4iv(uint program, int location, int count, ref readonly int value) => _glProgramUniform4iv(program, location, count, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM4FPROC(uint program, int location, float v0, float v1, float v2, float v3);
    private static PFNGLPROGRAMUNIFORM4FPROC _glProgramUniform4f;
    public static void glProgramUniform4f(uint program, int location, float v0, float v1, float v2, float v3) => _glProgramUniform4f(program, location, v0, v1, v2, v3);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM4FVPROC(uint program, int location, int count, ref readonly float value);
    private static PFNGLPROGRAMUNIFORM4FVPROC _glProgramUniform4fv;
    public static void glProgramUniform4fv(uint program, int location, int count, ref readonly float value) => _glProgramUniform4fv(program, location, count, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM4DPROC(uint program, int location, double v0, double v1, double v2, double v3);
    private static PFNGLPROGRAMUNIFORM4DPROC _glProgramUniform4d;
    public static void glProgramUniform4d(uint program, int location, double v0, double v1, double v2, double v3) => _glProgramUniform4d(program, location, v0, v1, v2, v3);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM4DVPROC(uint program, int location, int count, ref readonly double value);
    private static PFNGLPROGRAMUNIFORM4DVPROC _glProgramUniform4dv;
    public static void glProgramUniform4dv(uint program, int location, int count, ref readonly double value) => _glProgramUniform4dv(program, location, count, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM4UIPROC(uint program, int location, uint v0, uint v1, uint v2, uint v3);
    private static PFNGLPROGRAMUNIFORM4UIPROC _glProgramUniform4ui;
    public static void glProgramUniform4ui(uint program, int location, uint v0, uint v1, uint v2, uint v3) => _glProgramUniform4ui(program, location, v0, v1, v2, v3);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM4UIVPROC(uint program, int location, int count, ref readonly uint value);
    private static PFNGLPROGRAMUNIFORM4UIVPROC _glProgramUniform4uiv;
    public static void glProgramUniform4uiv(uint program, int location, int count, ref readonly uint value) => _glProgramUniform4uiv(program, location, count, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORMMATRIX2FVPROC(uint program, int location, int count, bool transpose, ref readonly float value);
    private static PFNGLPROGRAMUNIFORMMATRIX2FVPROC _glProgramUniformMatrix2fv;
    public static void glProgramUniformMatrix2fv(uint program, int location, int count, bool transpose, ref readonly float value) => _glProgramUniformMatrix2fv(program, location, count, transpose, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORMMATRIX3FVPROC(uint program, int location, int count, bool transpose, ref readonly float value);
    private static PFNGLPROGRAMUNIFORMMATRIX3FVPROC _glProgramUniformMatrix3fv;
    public static void glProgramUniformMatrix3fv(uint program, int location, int count, bool transpose, ref readonly float value) => _glProgramUniformMatrix3fv(program, location, count, transpose, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORMMATRIX4FVPROC(uint program, int location, int count, bool transpose, ref readonly float value);
    private static PFNGLPROGRAMUNIFORMMATRIX4FVPROC _glProgramUniformMatrix4fv;
    public static void glProgramUniformMatrix4fv(uint program, int location, int count, bool transpose, ref readonly float value) => _glProgramUniformMatrix4fv(program, location, count, transpose, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORMMATRIX2DVPROC(uint program, int location, int count, bool transpose, ref readonly double value);
    private static PFNGLPROGRAMUNIFORMMATRIX2DVPROC _glProgramUniformMatrix2dv;
    public static void glProgramUniformMatrix2dv(uint program, int location, int count, bool transpose, ref readonly double value) => _glProgramUniformMatrix2dv(program, location, count, transpose, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORMMATRIX3DVPROC(uint program, int location, int count, bool transpose, ref readonly double value);
    private static PFNGLPROGRAMUNIFORMMATRIX3DVPROC _glProgramUniformMatrix3dv;
    public static void glProgramUniformMatrix3dv(uint program, int location, int count, bool transpose, ref readonly double value) => _glProgramUniformMatrix3dv(program, location, count, transpose, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORMMATRIX4DVPROC(uint program, int location, int count, bool transpose, ref readonly double value);
    private static PFNGLPROGRAMUNIFORMMATRIX4DVPROC _glProgramUniformMatrix4dv;
    public static void glProgramUniformMatrix4dv(uint program, int location, int count, bool transpose, ref readonly double value) => _glProgramUniformMatrix4dv(program, location, count, transpose, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORMMATRIX2X3FVPROC(uint program, int location, int count, bool transpose, ref readonly float value);
    private static PFNGLPROGRAMUNIFORMMATRIX2X3FVPROC _glProgramUniformMatrix2x3fv;
    public static void glProgramUniformMatrix2x3fv(uint program, int location, int count, bool transpose, ref readonly float value) => _glProgramUniformMatrix2x3fv(program, location, count, transpose, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORMMATRIX3X2FVPROC(uint program, int location, int count, bool transpose, ref readonly float value);
    private static PFNGLPROGRAMUNIFORMMATRIX3X2FVPROC _glProgramUniformMatrix3x2fv;
    public static void glProgramUniformMatrix3x2fv(uint program, int location, int count, bool transpose, ref readonly float value) => _glProgramUniformMatrix3x2fv(program, location, count, transpose, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORMMATRIX2X4FVPROC(uint program, int location, int count, bool transpose, ref readonly float value);
    private static PFNGLPROGRAMUNIFORMMATRIX2X4FVPROC _glProgramUniformMatrix2x4fv;
    public static void glProgramUniformMatrix2x4fv(uint program, int location, int count, bool transpose, ref readonly float value) => _glProgramUniformMatrix2x4fv(program, location, count, transpose, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORMMATRIX4X2FVPROC(uint program, int location, int count, bool transpose, ref readonly float value);
    private static PFNGLPROGRAMUNIFORMMATRIX4X2FVPROC _glProgramUniformMatrix4x2fv;
    public static void glProgramUniformMatrix4x2fv(uint program, int location, int count, bool transpose, ref readonly float value) => _glProgramUniformMatrix4x2fv(program, location, count, transpose, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORMMATRIX3X4FVPROC(uint program, int location, int count, bool transpose, ref readonly float value);
    private static PFNGLPROGRAMUNIFORMMATRIX3X4FVPROC _glProgramUniformMatrix3x4fv;
    public static void glProgramUniformMatrix3x4fv(uint program, int location, int count, bool transpose, ref readonly float value) => _glProgramUniformMatrix3x4fv(program, location, count, transpose, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORMMATRIX4X3FVPROC(uint program, int location, int count, bool transpose, ref readonly float value);
    private static PFNGLPROGRAMUNIFORMMATRIX4X3FVPROC _glProgramUniformMatrix4x3fv;
    public static void glProgramUniformMatrix4x3fv(uint program, int location, int count, bool transpose, ref readonly float value) => _glProgramUniformMatrix4x3fv(program, location, count, transpose, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORMMATRIX2X3DVPROC(uint program, int location, int count, bool transpose, ref readonly double value);
    private static PFNGLPROGRAMUNIFORMMATRIX2X3DVPROC _glProgramUniformMatrix2x3dv;
    public static void glProgramUniformMatrix2x3dv(uint program, int location, int count, bool transpose, ref readonly double value) => _glProgramUniformMatrix2x3dv(program, location, count, transpose, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORMMATRIX3X2DVPROC(uint program, int location, int count, bool transpose, ref readonly double value);
    private static PFNGLPROGRAMUNIFORMMATRIX3X2DVPROC _glProgramUniformMatrix3x2dv;
    public static void glProgramUniformMatrix3x2dv(uint program, int location, int count, bool transpose, ref readonly double value) => _glProgramUniformMatrix3x2dv(program, location, count, transpose, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORMMATRIX2X4DVPROC(uint program, int location, int count, bool transpose, ref readonly double value);
    private static PFNGLPROGRAMUNIFORMMATRIX2X4DVPROC _glProgramUniformMatrix2x4dv;
    public static void glProgramUniformMatrix2x4dv(uint program, int location, int count, bool transpose, ref readonly double value) => _glProgramUniformMatrix2x4dv(program, location, count, transpose, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORMMATRIX4X2DVPROC(uint program, int location, int count, bool transpose, ref readonly double value);
    private static PFNGLPROGRAMUNIFORMMATRIX4X2DVPROC _glProgramUniformMatrix4x2dv;
    public static void glProgramUniformMatrix4x2dv(uint program, int location, int count, bool transpose, ref readonly double value) => _glProgramUniformMatrix4x2dv(program, location, count, transpose, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORMMATRIX3X4DVPROC(uint program, int location, int count, bool transpose, ref readonly double value);
    private static PFNGLPROGRAMUNIFORMMATRIX3X4DVPROC _glProgramUniformMatrix3x4dv;
    public static void glProgramUniformMatrix3x4dv(uint program, int location, int count, bool transpose, ref readonly double value) => _glProgramUniformMatrix3x4dv(program, location, count, transpose, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORMMATRIX4X3DVPROC(uint program, int location, int count, bool transpose, ref readonly double value);
    private static PFNGLPROGRAMUNIFORMMATRIX4X3DVPROC _glProgramUniformMatrix4x3dv;
    public static void glProgramUniformMatrix4x3dv(uint program, int location, int count, bool transpose, ref readonly double value) => _glProgramUniformMatrix4x3dv(program, location, count, transpose, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVALIDATEPROGRAMPIPELINEPROC(uint pipeline);
    private static PFNGLVALIDATEPROGRAMPIPELINEPROC _glValidateProgramPipeline;
    public static void glValidateProgramPipeline(uint pipeline) => _glValidateProgramPipeline(pipeline);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETPROGRAMPIPELINEINFOLOGPROC(uint pipeline, int bufSize, out int length, string infoLog);
    private static PFNGLGETPROGRAMPIPELINEINFOLOGPROC _glGetProgramPipelineInfoLog;
    public static void glGetProgramPipelineInfoLog(uint pipeline, int bufSize, out int length, string infoLog) => _glGetProgramPipelineInfoLog(pipeline, bufSize, out length, infoLog);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBL1DPROC(uint index, double x);
    private static PFNGLVERTEXATTRIBL1DPROC _glVertexAttribL1d;
    public static void glVertexAttribL1d(uint index, double x) => _glVertexAttribL1d(index, x);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBL2DPROC(uint index, double x, double y);
    private static PFNGLVERTEXATTRIBL2DPROC _glVertexAttribL2d;
    public static void glVertexAttribL2d(uint index, double x, double y) => _glVertexAttribL2d(index, x, y);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBL3DPROC(uint index, double x, double y, double z);
    private static PFNGLVERTEXATTRIBL3DPROC _glVertexAttribL3d;
    public static void glVertexAttribL3d(uint index, double x, double y, double z) => _glVertexAttribL3d(index, x, y, z);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBL4DPROC(uint index, double x, double y, double z, double w);
    private static PFNGLVERTEXATTRIBL4DPROC _glVertexAttribL4d;
    public static void glVertexAttribL4d(uint index, double x, double y, double z, double w) => _glVertexAttribL4d(index, x, y, z, w);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBL1DVPROC(uint index, ref readonly double v);
    private static PFNGLVERTEXATTRIBL1DVPROC _glVertexAttribL1dv;
    public static void glVertexAttribL1dv(uint index, ref readonly double v) => _glVertexAttribL1dv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBL2DVPROC(uint index, ref readonly double v);
    private static PFNGLVERTEXATTRIBL2DVPROC _glVertexAttribL2dv;
    public static void glVertexAttribL2dv(uint index, ref readonly double v) => _glVertexAttribL2dv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBL3DVPROC(uint index, ref readonly double v);
    private static PFNGLVERTEXATTRIBL3DVPROC _glVertexAttribL3dv;
    public static void glVertexAttribL3dv(uint index, ref readonly double v) => _glVertexAttribL3dv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBL4DVPROC(uint index, ref readonly double v);
    private static PFNGLVERTEXATTRIBL4DVPROC _glVertexAttribL4dv;
    public static void glVertexAttribL4dv(uint index, ref readonly double v) => _glVertexAttribL4dv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBLPOINTERPROC(uint index, int size, uint type, int stride, IntPtr pointer);
    private static PFNGLVERTEXATTRIBLPOINTERPROC _glVertexAttribLPointer;
    public static void glVertexAttribLPointer(uint index, int size, uint type, int stride, IntPtr pointer) => _glVertexAttribLPointer(index, size, type, stride, pointer);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETVERTEXATTRIBLDVPROC(uint index, uint pname, out double @params);
    private static PFNGLGETVERTEXATTRIBLDVPROC _glGetVertexAttribLdv;
    public static void glGetVertexAttribLdv(uint index, uint pname, out double @params) => _glGetVertexAttribLdv(index, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVIEWPORTARRAYVPROC(uint first, int count, ref readonly float v);
    private static PFNGLVIEWPORTARRAYVPROC _glViewportArrayv;
    public static void glViewportArrayv(uint first, int count, ref readonly float v) => _glViewportArrayv(first, count, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVIEWPORTINDEXEDFPROC(uint index, float x, float y, float w, float h);
    private static PFNGLVIEWPORTINDEXEDFPROC _glViewportIndexedf;
    public static void glViewportIndexedf(uint index, float x, float y, float w, float h) => _glViewportIndexedf(index, x, y, w, h);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVIEWPORTINDEXEDFVPROC(uint index, ref readonly float v);
    private static PFNGLVIEWPORTINDEXEDFVPROC _glViewportIndexedfv;
    public static void glViewportIndexedfv(uint index, ref readonly float v) => _glViewportIndexedfv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLSCISSORARRAYVPROC(uint first, int count, ref readonly int v);
    private static PFNGLSCISSORARRAYVPROC _glScissorArrayv;
    public static void glScissorArrayv(uint first, int count, ref readonly int v) => _glScissorArrayv(first, count, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLSCISSORINDEXEDPROC(uint index, int left, int bottom, int width, int height);
    private static PFNGLSCISSORINDEXEDPROC _glScissorIndexed;
    public static void glScissorIndexed(uint index, int left, int bottom, int width, int height) => _glScissorIndexed(index, left, bottom, width, height);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLSCISSORINDEXEDVPROC(uint index, ref readonly int v);
    private static PFNGLSCISSORINDEXEDVPROC _glScissorIndexedv;
    public static void glScissorIndexedv(uint index, ref readonly int v) => _glScissorIndexedv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDEPTHRANGEARRAYVPROC(uint first, int count, ref readonly double v);
    private static PFNGLDEPTHRANGEARRAYVPROC _glDepthRangeArrayv;
    public static void glDepthRangeArrayv(uint first, int count, ref readonly double v) => _glDepthRangeArrayv(first, count, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDEPTHRANGEINDEXEDPROC(uint index, double n, double f);
    private static PFNGLDEPTHRANGEINDEXEDPROC _glDepthRangeIndexed;
    public static void glDepthRangeIndexed(uint index, double n, double f) => _glDepthRangeIndexed(index, n, f);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETFLOATI_VPROC(uint target, uint index, out float data);
    private static PFNGLGETFLOATI_VPROC _glGetFloati_v;
    public static void glGetFloati_v(uint target, uint index, out float data) => _glGetFloati_v(target, index, out data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETDOUBLEI_VPROC(uint target, uint index, out double data);
    private static PFNGLGETDOUBLEI_VPROC _glGetDoublei_v;
    public static void glGetDoublei_v(uint target, uint index, out double data) => _glGetDoublei_v(target, index, out data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDRAWARRAYSINSTANCEDBASEINSTANCEPROC(uint mode, int first, int count, int instancecount, uint baseinstance);
    private static PFNGLDRAWARRAYSINSTANCEDBASEINSTANCEPROC _glDrawArraysInstancedBaseInstance;
    public static void glDrawArraysInstancedBaseInstance(uint mode, int first, int count, int instancecount, uint baseinstance) => _glDrawArraysInstancedBaseInstance(mode, first, count, instancecount, baseinstance);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDRAWELEMENTSINSTANCEDBASEINSTANCEPROC(uint mode, int count, uint type, IntPtr indices, int instancecount, uint baseinstance);
    private static PFNGLDRAWELEMENTSINSTANCEDBASEINSTANCEPROC _glDrawElementsInstancedBaseInstance;
    public static void glDrawElementsInstancedBaseInstance(uint mode, int count, uint type, IntPtr indices, int instancecount, uint baseinstance) => _glDrawElementsInstancedBaseInstance(mode, count, type, indices, instancecount, baseinstance);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDRAWELEMENTSINSTANCEDBASEVERTEXBASEINSTANCEPROC(uint mode, int count, uint type, IntPtr indices, int instancecount, int basevertex, uint baseinstance);
    private static PFNGLDRAWELEMENTSINSTANCEDBASEVERTEXBASEINSTANCEPROC _glDrawElementsInstancedBaseVertexBaseInstance;
    public static void glDrawElementsInstancedBaseVertexBaseInstance(uint mode, int count, uint type, IntPtr indices, int instancecount, int basevertex, uint baseinstance) => _glDrawElementsInstancedBaseVertexBaseInstance(mode, count, type, indices, instancecount, basevertex, baseinstance);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETINTERNALFORMATIVPROC(uint target, uint internalformat, uint pname, int count, out int @params);
    private static PFNGLGETINTERNALFORMATIVPROC _glGetInternalformativ;
    public static void glGetInternalformativ(uint target, uint internalformat, uint pname, int count, out int @params) => _glGetInternalformativ(target, internalformat, pname, count, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETACTIVEATOMICCOUNTERBUFFERIVPROC(uint program, uint bufferIndex, uint pname, out int @params);
    private static PFNGLGETACTIVEATOMICCOUNTERBUFFERIVPROC _glGetActiveAtomicCounterBufferiv;
    public static void glGetActiveAtomicCounterBufferiv(uint program, uint bufferIndex, uint pname, out int @params) => _glGetActiveAtomicCounterBufferiv(program, bufferIndex, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBINDIMAGETEXTUREPROC(uint unit, uint texture, int level, bool layered, int layer, uint access, uint format);
    private static PFNGLBINDIMAGETEXTUREPROC _glBindImageTexture;
    public static void glBindImageTexture(uint unit, uint texture, int level, bool layered, int layer, uint access, uint format) => _glBindImageTexture(unit, texture, level, layered, layer, access, format);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLMEMORYBARRIERPROC(uint barriers);
    private static PFNGLMEMORYBARRIERPROC _glMemoryBarrier;
    public static void glMemoryBarrier(uint barriers) => _glMemoryBarrier(barriers);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXSTORAGE1DPROC(uint target, int levels, uint internalformat, int width);
    private static PFNGLTEXSTORAGE1DPROC _glTexStorage1D;
    public static void glTexStorage1D(uint target, int levels, uint internalformat, int width) => _glTexStorage1D(target, levels, internalformat, width);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXSTORAGE2DPROC(uint target, int levels, uint internalformat, int width, int height);
    private static PFNGLTEXSTORAGE2DPROC _glTexStorage2D;
    public static void glTexStorage2D(uint target, int levels, uint internalformat, int width, int height) => _glTexStorage2D(target, levels, internalformat, width, height);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXSTORAGE3DPROC(uint target, int levels, uint internalformat, int width, int height, int depth);
    private static PFNGLTEXSTORAGE3DPROC _glTexStorage3D;
    public static void glTexStorage3D(uint target, int levels, uint internalformat, int width, int height, int depth) => _glTexStorage3D(target, levels, internalformat, width, height, depth);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDRAWTRANSFORMFEEDBACKINSTANCEDPROC(uint mode, uint id, int instancecount);
    private static PFNGLDRAWTRANSFORMFEEDBACKINSTANCEDPROC _glDrawTransformFeedbackInstanced;
    public static void glDrawTransformFeedbackInstanced(uint mode, uint id, int instancecount) => _glDrawTransformFeedbackInstanced(mode, id, instancecount);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDRAWTRANSFORMFEEDBACKSTREAMINSTANCEDPROC(uint mode, uint id, uint stream, int instancecount);
    private static PFNGLDRAWTRANSFORMFEEDBACKSTREAMINSTANCEDPROC _glDrawTransformFeedbackStreamInstanced;
    public static void glDrawTransformFeedbackStreamInstanced(uint mode, uint id, uint stream, int instancecount) => _glDrawTransformFeedbackStreamInstanced(mode, id, stream, instancecount);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCLEARBUFFERDATAPROC(uint target, uint internalformat, uint format, uint type, IntPtr data);
    private static PFNGLCLEARBUFFERDATAPROC _glClearBufferData;
    public static void glClearBufferData(uint target, uint internalformat, uint format, uint type, IntPtr data) => _glClearBufferData(target, internalformat, format, type, data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCLEARBUFFERSUBDATAPROC(uint target, uint internalformat, ulong offset, ulong size, uint format, uint type, IntPtr data);
    private static PFNGLCLEARBUFFERSUBDATAPROC _glClearBufferSubData;
    public static void glClearBufferSubData(uint target, uint internalformat, ulong offset, ulong size, uint format, uint type, IntPtr data) => _glClearBufferSubData(target, internalformat, offset, size, format, type, data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDISPATCHCOMPUTEPROC(uint num_groups_x, uint num_groups_y, uint num_groups_z);
    private static PFNGLDISPATCHCOMPUTEPROC _glDispatchCompute;
    public static void glDispatchCompute(uint num_groups_x, uint num_groups_y, uint num_groups_z) => _glDispatchCompute(num_groups_x, num_groups_y, num_groups_z);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDISPATCHCOMPUTEINDIRECTPROC(ulong indirect);
    private static PFNGLDISPATCHCOMPUTEINDIRECTPROC _glDispatchComputeIndirect;
    public static void glDispatchComputeIndirect(ulong indirect) => _glDispatchComputeIndirect(indirect);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCOPYIMAGESUBDATAPROC(uint srcName, uint srcTarget, int srcLevel, int srcX, int srcY, int srcZ, uint dstName, uint dstTarget, int dstLevel, int dstX, int dstY, int dstZ, int srcWidth, int srcHeight, int srcDepth);
    private static PFNGLCOPYIMAGESUBDATAPROC _glCopyImageSubData;
    public static void glCopyImageSubData(uint srcName, uint srcTarget, int srcLevel, int srcX, int srcY, int srcZ, uint dstName, uint dstTarget, int dstLevel, int dstX, int dstY, int dstZ, int srcWidth, int srcHeight, int srcDepth) => _glCopyImageSubData(srcName, srcTarget, srcLevel, srcX, srcY, srcZ, dstName, dstTarget, dstLevel, dstX, dstY, dstZ, srcWidth, srcHeight, srcDepth);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLFRAMEBUFFERPARAMETERIPROC(uint target, uint pname, int param);
    private static PFNGLFRAMEBUFFERPARAMETERIPROC _glFramebufferParameteri;
    public static void glFramebufferParameteri(uint target, uint pname, int param) => _glFramebufferParameteri(target, pname, param);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETFRAMEBUFFERPARAMETERIVPROC(uint target, uint pname, out int @params);
    private static PFNGLGETFRAMEBUFFERPARAMETERIVPROC _glGetFramebufferParameteriv;
    public static void glGetFramebufferParameteriv(uint target, uint pname, out int @params) => _glGetFramebufferParameteriv(target, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETINTERNALFORMATI64VPROC(uint target, uint internalformat, uint pname, int count, out long @params);
    private static PFNGLGETINTERNALFORMATI64VPROC _glGetInternalformati64v;
    public static void glGetInternalformati64v(uint target, uint internalformat, uint pname, int count, out long @params) => _glGetInternalformati64v(target, internalformat, pname, count, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLINVALIDATETEXSUBIMAGEPROC(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth);
    private static PFNGLINVALIDATETEXSUBIMAGEPROC _glInvalidateTexSubImage;
    public static void glInvalidateTexSubImage(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth) => _glInvalidateTexSubImage(texture, level, xoffset, yoffset, zoffset, width, height, depth);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLINVALIDATETEXIMAGEPROC(uint texture, int level);
    private static PFNGLINVALIDATETEXIMAGEPROC _glInvalidateTexImage;
    public static void glInvalidateTexImage(uint texture, int level) => _glInvalidateTexImage(texture, level);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLINVALIDATEBUFFERSUBDATAPROC(uint buffer, ulong offset, ulong length);
    private static PFNGLINVALIDATEBUFFERSUBDATAPROC _glInvalidateBufferSubData;
    public static void glInvalidateBufferSubData(uint buffer, ulong offset, ulong length) => _glInvalidateBufferSubData(buffer, offset, length);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLINVALIDATEBUFFERDATAPROC(uint buffer);
    private static PFNGLINVALIDATEBUFFERDATAPROC _glInvalidateBufferData;
    public static void glInvalidateBufferData(uint buffer) => _glInvalidateBufferData(buffer);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLINVALIDATEFRAMEBUFFERPROC(uint target, int numAttachments, ref readonly uint attachments);
    private static PFNGLINVALIDATEFRAMEBUFFERPROC _glInvalidateFramebuffer;
    public static void glInvalidateFramebuffer(uint target, int numAttachments, ref readonly uint attachments) => _glInvalidateFramebuffer(target, numAttachments, in attachments);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLINVALIDATESUBFRAMEBUFFERPROC(uint target, int numAttachments, ref readonly uint attachments, int x, int y, int width, int height);
    private static PFNGLINVALIDATESUBFRAMEBUFFERPROC _glInvalidateSubFramebuffer;
    public static void glInvalidateSubFramebuffer(uint target, int numAttachments, ref readonly uint attachments, int x, int y, int width, int height) => _glInvalidateSubFramebuffer(target, numAttachments, in attachments, x, y, width, height);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLMULTIDRAWARRAYSINDIRECTPROC(uint mode, IntPtr indirect, int drawcount, int stride);
    private static PFNGLMULTIDRAWARRAYSINDIRECTPROC _glMultiDrawArraysIndirect;
    public static void glMultiDrawArraysIndirect(uint mode, IntPtr indirect, int drawcount, int stride) => _glMultiDrawArraysIndirect(mode, indirect, drawcount, stride);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLMULTIDRAWELEMENTSINDIRECTPROC(uint mode, uint type, IntPtr indirect, int drawcount, int stride);
    private static PFNGLMULTIDRAWELEMENTSINDIRECTPROC _glMultiDrawElementsIndirect;
    public static void glMultiDrawElementsIndirect(uint mode, uint type, IntPtr indirect, int drawcount, int stride) => _glMultiDrawElementsIndirect(mode, type, indirect, drawcount, stride);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETPROGRAMINTERFACEIVPROC(uint program, uint programInterface, uint pname, out int @params);
    private static PFNGLGETPROGRAMINTERFACEIVPROC _glGetProgramInterfaceiv;
    public static void glGetProgramInterfaceiv(uint program, uint programInterface, uint pname, out int @params) => _glGetProgramInterfaceiv(program, programInterface, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate uint PFNGLGETPROGRAMRESOURCEINDEXPROC(uint program, uint programInterface, string name);
    private static PFNGLGETPROGRAMRESOURCEINDEXPROC _glGetProgramResourceIndex;
    public static uint glGetProgramResourceIndex(uint program, uint programInterface, string name) => _glGetProgramResourceIndex(program, programInterface, name);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETPROGRAMRESOURCENAMEPROC(uint program, uint programInterface, uint index, int bufSize, out int length, string name);
    private static PFNGLGETPROGRAMRESOURCENAMEPROC _glGetProgramResourceName;
    public static void glGetProgramResourceName(uint program, uint programInterface, uint index, int bufSize, out int length, string name) => _glGetProgramResourceName(program, programInterface, index, bufSize, out length, name);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETPROGRAMRESOURCEIVPROC(uint program, uint programInterface, uint index, int propCount, ref readonly uint props, int count, out int length, out int @params);
    private static PFNGLGETPROGRAMRESOURCEIVPROC _glGetProgramResourceiv;
    public static void glGetProgramResourceiv(uint program, uint programInterface, uint index, int propCount, ref readonly uint props, int count, out int length, out int @params) => _glGetProgramResourceiv(program, programInterface, index, propCount, in props, count, out length, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int PFNGLGETPROGRAMRESOURCELOCATIONPROC(uint program, uint programInterface, string name);
    private static PFNGLGETPROGRAMRESOURCELOCATIONPROC _glGetProgramResourceLocation;
    public static int glGetProgramResourceLocation(uint program, uint programInterface, string name) => _glGetProgramResourceLocation(program, programInterface, name);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int PFNGLGETPROGRAMRESOURCELOCATIONINDEXPROC(uint program, uint programInterface, string name);
    private static PFNGLGETPROGRAMRESOURCELOCATIONINDEXPROC _glGetProgramResourceLocationIndex;
    public static int glGetProgramResourceLocationIndex(uint program, uint programInterface, string name) => _glGetProgramResourceLocationIndex(program, programInterface, name);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLSHADERSTORAGEBLOCKBINDINGPROC(uint program, uint storageBlockIndex, uint storageBlockBinding);
    private static PFNGLSHADERSTORAGEBLOCKBINDINGPROC _glShaderStorageBlockBinding;
    public static void glShaderStorageBlockBinding(uint program, uint storageBlockIndex, uint storageBlockBinding) => _glShaderStorageBlockBinding(program, storageBlockIndex, storageBlockBinding);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXBUFFERRANGEPROC(uint target, uint internalformat, uint buffer, ulong offset, ulong size);
    private static PFNGLTEXBUFFERRANGEPROC _glTexBufferRange;
    public static void glTexBufferRange(uint target, uint internalformat, uint buffer, ulong offset, ulong size) => _glTexBufferRange(target, internalformat, buffer, offset, size);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXSTORAGE2DMULTISAMPLEPROC(uint target, int samples, uint internalformat, int width, int height, bool fixedsamplelocations);
    private static PFNGLTEXSTORAGE2DMULTISAMPLEPROC _glTexStorage2DMultisample;
    public static void glTexStorage2DMultisample(uint target, int samples, uint internalformat, int width, int height, bool fixedsamplelocations) => _glTexStorage2DMultisample(target, samples, internalformat, width, height, fixedsamplelocations);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXSTORAGE3DMULTISAMPLEPROC(uint target, int samples, uint internalformat, int width, int height, int depth, bool fixedsamplelocations);
    private static PFNGLTEXSTORAGE3DMULTISAMPLEPROC _glTexStorage3DMultisample;
    public static void glTexStorage3DMultisample(uint target, int samples, uint internalformat, int width, int height, int depth, bool fixedsamplelocations) => _glTexStorage3DMultisample(target, samples, internalformat, width, height, depth, fixedsamplelocations);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXTUREVIEWPROC(uint texture, uint target, uint origtexture, uint internalformat, uint minlevel, uint numlevels, uint minlayer, uint numlayers);
    private static PFNGLTEXTUREVIEWPROC _glTextureView;
    public static void glTextureView(uint texture, uint target, uint origtexture, uint internalformat, uint minlevel, uint numlevels, uint minlayer, uint numlayers) => _glTextureView(texture, target, origtexture, internalformat, minlevel, numlevels, minlayer, numlayers);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBINDVERTEXBUFFERPROC(uint bindingindex, uint buffer, ulong offset, int stride);
    private static PFNGLBINDVERTEXBUFFERPROC _glBindVertexBuffer;
    public static void glBindVertexBuffer(uint bindingindex, uint buffer, ulong offset, int stride) => _glBindVertexBuffer(bindingindex, buffer, offset, stride);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBFORMATPROC(uint attribindex, int size, uint type, bool normalized, uint relativeoffset);
    private static PFNGLVERTEXATTRIBFORMATPROC _glVertexAttribFormat;
    public static void glVertexAttribFormat(uint attribindex, int size, uint type, bool normalized, uint relativeoffset) => _glVertexAttribFormat(attribindex, size, type, normalized, relativeoffset);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBIFORMATPROC(uint attribindex, int size, uint type, uint relativeoffset);
    private static PFNGLVERTEXATTRIBIFORMATPROC _glVertexAttribIFormat;
    public static void glVertexAttribIFormat(uint attribindex, int size, uint type, uint relativeoffset) => _glVertexAttribIFormat(attribindex, size, type, relativeoffset);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBLFORMATPROC(uint attribindex, int size, uint type, uint relativeoffset);
    private static PFNGLVERTEXATTRIBLFORMATPROC _glVertexAttribLFormat;
    public static void glVertexAttribLFormat(uint attribindex, int size, uint type, uint relativeoffset) => _glVertexAttribLFormat(attribindex, size, type, relativeoffset);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBBINDINGPROC(uint attribindex, uint bindingindex);
    private static PFNGLVERTEXATTRIBBINDINGPROC _glVertexAttribBinding;
    public static void glVertexAttribBinding(uint attribindex, uint bindingindex) => _glVertexAttribBinding(attribindex, bindingindex);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXBINDINGDIVISORPROC(uint bindingindex, uint divisor);
    private static PFNGLVERTEXBINDINGDIVISORPROC _glVertexBindingDivisor;
    public static void glVertexBindingDivisor(uint bindingindex, uint divisor) => _glVertexBindingDivisor(bindingindex, divisor);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDEBUGMESSAGECONTROLPROC(uint source, uint type, uint severity, int count, ref readonly uint ids, bool enabled);
    private static PFNGLDEBUGMESSAGECONTROLPROC _glDebugMessageControl;
    public static void glDebugMessageControl(uint source, uint type, uint severity, int count, ref readonly uint ids, bool enabled) => _glDebugMessageControl(source, type, severity, count, in ids, enabled);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDEBUGMESSAGEINSERTPROC(uint source, uint type, uint id, uint severity, int length, string buf);
    private static PFNGLDEBUGMESSAGEINSERTPROC _glDebugMessageInsert;
    public static void glDebugMessageInsert(uint source, uint type, uint id, uint severity, int length, string buf) => _glDebugMessageInsert(source, type, id, severity, length, buf);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDEBUGMESSAGECALLBACKPROC(GLDEBUGPROC callback, IntPtr userParam);
    private static PFNGLDEBUGMESSAGECALLBACKPROC _glDebugMessageCallback;
    public static void glDebugMessageCallback(GLDEBUGPROC callback, IntPtr userParam) => _glDebugMessageCallback(callback, userParam);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate uint PFNGLGETDEBUGMESSAGELOGPROC(uint count, int bufSize, out uint sources, out uint types, out uint ids, out uint severities, out int lengths, string messageLog);
    private static PFNGLGETDEBUGMESSAGELOGPROC _glGetDebugMessageLog;
    public static uint glGetDebugMessageLog(uint count, int bufSize, out uint sources, out uint types, out uint ids, out uint severities, out int lengths, string messageLog) => _glGetDebugMessageLog(count, bufSize, out sources, out types, out ids, out severities, out lengths, messageLog);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPUSHDEBUGGROUPPROC(uint source, uint id, int length, string message);
    private static PFNGLPUSHDEBUGGROUPPROC _glPushDebugGroup;
    public static void glPushDebugGroup(uint source, uint id, int length, string message) => _glPushDebugGroup(source, id, length, message);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPOPDEBUGGROUPPROC();
    private static PFNGLPOPDEBUGGROUPPROC _glPopDebugGroup;
    public static void glPopDebugGroup() => _glPopDebugGroup();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLOBJECTLABELPROC(uint identifier, uint name, int length, string label);
    private static PFNGLOBJECTLABELPROC _glObjectLabel;
    public static void glObjectLabel(uint identifier, uint name, int length, string label) => _glObjectLabel(identifier, name, length, label);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETOBJECTLABELPROC(uint identifier, uint name, int bufSize, out int length, string label);
    private static PFNGLGETOBJECTLABELPROC _glGetObjectLabel;
    public static void glGetObjectLabel(uint identifier, uint name, int bufSize, out int length, string label) => _glGetObjectLabel(identifier, name, bufSize, out length, label);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLOBJECTPTRLABELPROC(IntPtr ptr, int length, string label);
    private static PFNGLOBJECTPTRLABELPROC _glObjectPtrLabel;
    public static void glObjectPtrLabel(IntPtr ptr, int length, string label) => _glObjectPtrLabel(ptr, length, label);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETOBJECTPTRLABELPROC(IntPtr ptr, int bufSize, out int length, string label);
    private static PFNGLGETOBJECTPTRLABELPROC _glGetObjectPtrLabel;
    public static void glGetObjectPtrLabel(IntPtr ptr, int bufSize, out int length, string label) => _glGetObjectPtrLabel(ptr, bufSize, out length, label);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETPOINTERVPROC(uint pname, IntPtr @params);
    private static PFNGLGETPOINTERVPROC _glGetPointerv;
    public static void glGetPointerv(uint pname, IntPtr @params) => _glGetPointerv(pname, @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBUFFERSTORAGEPROC(uint target, ulong size, IntPtr data, uint flags);
    private static PFNGLBUFFERSTORAGEPROC _glBufferStorage;
    public static void glBufferStorage(uint target, ulong size, IntPtr data, uint flags) => _glBufferStorage(target, size, data, flags);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCLEARTEXIMAGEPROC(uint texture, int level, uint format, uint type, IntPtr data);
    private static PFNGLCLEARTEXIMAGEPROC _glClearTexImage;
    public static void glClearTexImage(uint texture, int level, uint format, uint type, IntPtr data) => _glClearTexImage(texture, level, format, type, data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCLEARTEXSUBIMAGEPROC(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, uint type, IntPtr data);
    private static PFNGLCLEARTEXSUBIMAGEPROC _glClearTexSubImage;
    public static void glClearTexSubImage(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, uint type, IntPtr data) => _glClearTexSubImage(texture, level, xoffset, yoffset, zoffset, width, height, depth, format, type, data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBINDBUFFERSBASEPROC(uint target, uint first, int count, ref readonly uint buffers);
    private static PFNGLBINDBUFFERSBASEPROC _glBindBuffersBase;
    public static void glBindBuffersBase(uint target, uint first, int count, ref readonly uint buffers) => _glBindBuffersBase(target, first, count, in buffers);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBINDBUFFERSRANGEPROC(uint target, uint first, int count, ref readonly uint buffers, ref readonly ulong offsets, ref readonly ulong sizes);
    private static PFNGLBINDBUFFERSRANGEPROC _glBindBuffersRange;
    public static void glBindBuffersRange(uint target, uint first, int count, ref readonly uint buffers, ref readonly ulong offsets, ref readonly ulong sizes) => _glBindBuffersRange(target, first, count, in buffers, in offsets, in sizes);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBINDTEXTURESPROC(uint first, int count, ref readonly uint textures);
    private static PFNGLBINDTEXTURESPROC _glBindTextures;
    public static void glBindTextures(uint first, int count, ref readonly uint textures) => _glBindTextures(first, count, in textures);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBINDSAMPLERSPROC(uint first, int count, ref readonly uint samplers);
    private static PFNGLBINDSAMPLERSPROC _glBindSamplers;
    public static void glBindSamplers(uint first, int count, ref readonly uint samplers) => _glBindSamplers(first, count, in samplers);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBINDIMAGETEXTURESPROC(uint first, int count, ref readonly uint textures);
    private static PFNGLBINDIMAGETEXTURESPROC _glBindImageTextures;
    public static void glBindImageTextures(uint first, int count, ref readonly uint textures) => _glBindImageTextures(first, count, in textures);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBINDVERTEXBUFFERSPROC(uint first, int count, ref readonly uint buffers, ref readonly ulong offsets, ref readonly int strides);
    private static PFNGLBINDVERTEXBUFFERSPROC _glBindVertexBuffers;
    public static void glBindVertexBuffers(uint first, int count, ref readonly uint buffers, ref readonly ulong offsets, ref readonly int strides) => _glBindVertexBuffers(first, count, in buffers, in offsets, in strides);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCLIPCONTROLPROC(uint origin, uint depth);
    private static PFNGLCLIPCONTROLPROC _glClipControl;
    public static void glClipControl(uint origin, uint depth) => _glClipControl(origin, depth);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCREATETRANSFORMFEEDBACKSPROC(int n, out uint ids);
    private static PFNGLCREATETRANSFORMFEEDBACKSPROC _glCreateTransformFeedbacks;
    public static void glCreateTransformFeedbacks(int n, out uint ids) => _glCreateTransformFeedbacks(n, out ids);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTRANSFORMFEEDBACKBUFFERBASEPROC(uint xfb, uint index, uint buffer);
    private static PFNGLTRANSFORMFEEDBACKBUFFERBASEPROC _glTransformFeedbackBufferBase;
    public static void glTransformFeedbackBufferBase(uint xfb, uint index, uint buffer) => _glTransformFeedbackBufferBase(xfb, index, buffer);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTRANSFORMFEEDBACKBUFFERRANGEPROC(uint xfb, uint index, uint buffer, ulong offset, ulong size);
    private static PFNGLTRANSFORMFEEDBACKBUFFERRANGEPROC _glTransformFeedbackBufferRange;
    public static void glTransformFeedbackBufferRange(uint xfb, uint index, uint buffer, ulong offset, ulong size) => _glTransformFeedbackBufferRange(xfb, index, buffer, offset, size);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETTRANSFORMFEEDBACKIVPROC(uint xfb, uint pname, out int param);
    private static PFNGLGETTRANSFORMFEEDBACKIVPROC _glGetTransformFeedbackiv;
    public static void glGetTransformFeedbackiv(uint xfb, uint pname, out int param) => _glGetTransformFeedbackiv(xfb, pname, out param);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETTRANSFORMFEEDBACKI_VPROC(uint xfb, uint pname, uint index, out int param);
    private static PFNGLGETTRANSFORMFEEDBACKI_VPROC _glGetTransformFeedbacki_v;
    public static void glGetTransformFeedbacki_v(uint xfb, uint pname, uint index, out int param) => _glGetTransformFeedbacki_v(xfb, pname, index, out param);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETTRANSFORMFEEDBACKI64_VPROC(uint xfb, uint pname, uint index, out long param);
    private static PFNGLGETTRANSFORMFEEDBACKI64_VPROC _glGetTransformFeedbacki64_v;
    public static void glGetTransformFeedbacki64_v(uint xfb, uint pname, uint index, out long param) => _glGetTransformFeedbacki64_v(xfb, pname, index, out param);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCREATEBUFFERSPROC(int n, out uint buffers);
    private static PFNGLCREATEBUFFERSPROC _glCreateBuffers;
    public static void glCreateBuffers(int n, out uint buffers) => _glCreateBuffers(n, out buffers);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLNAMEDBUFFERSTORAGEPROC(uint buffer, ulong size, IntPtr data, uint flags);
    private static PFNGLNAMEDBUFFERSTORAGEPROC _glNamedBufferStorage;
    public static void glNamedBufferStorage(uint buffer, ulong size, IntPtr data, uint flags) => _glNamedBufferStorage(buffer, size, data, flags);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLNAMEDBUFFERDATAPROC(uint buffer, ulong size, IntPtr data, uint usage);
    private static PFNGLNAMEDBUFFERDATAPROC _glNamedBufferData;
    public static void glNamedBufferData(uint buffer, ulong size, IntPtr data, uint usage) => _glNamedBufferData(buffer, size, data, usage);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLNAMEDBUFFERSUBDATAPROC(uint buffer, ulong offset, ulong size, IntPtr data);
    private static PFNGLNAMEDBUFFERSUBDATAPROC _glNamedBufferSubData;
    public static void glNamedBufferSubData(uint buffer, ulong offset, ulong size, IntPtr data) => _glNamedBufferSubData(buffer, offset, size, data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCOPYNAMEDBUFFERSUBDATAPROC(uint readBuffer, uint writeBuffer, ulong readOffset, ulong writeOffset, ulong size);
    private static PFNGLCOPYNAMEDBUFFERSUBDATAPROC _glCopyNamedBufferSubData;
    public static void glCopyNamedBufferSubData(uint readBuffer, uint writeBuffer, ulong readOffset, ulong writeOffset, ulong size) => _glCopyNamedBufferSubData(readBuffer, writeBuffer, readOffset, writeOffset, size);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCLEARNAMEDBUFFERDATAPROC(uint buffer, uint internalformat, uint format, uint type, IntPtr data);
    private static PFNGLCLEARNAMEDBUFFERDATAPROC _glClearNamedBufferData;
    public static void glClearNamedBufferData(uint buffer, uint internalformat, uint format, uint type, IntPtr data) => _glClearNamedBufferData(buffer, internalformat, format, type, data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCLEARNAMEDBUFFERSUBDATAPROC(uint buffer, uint internalformat, ulong offset, ulong size, uint format, uint type, IntPtr data);
    private static PFNGLCLEARNAMEDBUFFERSUBDATAPROC _glClearNamedBufferSubData;
    public static void glClearNamedBufferSubData(uint buffer, uint internalformat, ulong offset, ulong size, uint format, uint type, IntPtr data) => _glClearNamedBufferSubData(buffer, internalformat, offset, size, format, type, data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate IntPtr PFNGLMAPNAMEDBUFFERPROC(uint buffer, uint access);
    private static PFNGLMAPNAMEDBUFFERPROC _glMapNamedBuffer;
    public static IntPtr glMapNamedBuffer(uint buffer, uint access) => _glMapNamedBuffer(buffer, access);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate IntPtr PFNGLMAPNAMEDBUFFERRANGEPROC(uint buffer, ulong offset, ulong length, uint access);
    private static PFNGLMAPNAMEDBUFFERRANGEPROC _glMapNamedBufferRange;
    public static IntPtr glMapNamedBufferRange(uint buffer, ulong offset, ulong length, uint access) => _glMapNamedBufferRange(buffer, offset, length, access);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate bool PFNGLUNMAPNAMEDBUFFERPROC(uint buffer);
    private static PFNGLUNMAPNAMEDBUFFERPROC _glUnmapNamedBuffer;
    public static bool glUnmapNamedBuffer(uint buffer) => _glUnmapNamedBuffer(buffer);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLFLUSHMAPPEDNAMEDBUFFERRANGEPROC(uint buffer, ulong offset, ulong length);
    private static PFNGLFLUSHMAPPEDNAMEDBUFFERRANGEPROC _glFlushMappedNamedBufferRange;
    public static void glFlushMappedNamedBufferRange(uint buffer, ulong offset, ulong length) => _glFlushMappedNamedBufferRange(buffer, offset, length);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETNAMEDBUFFERPARAMETERIVPROC(uint buffer, uint pname, out int @params);
    private static PFNGLGETNAMEDBUFFERPARAMETERIVPROC _glGetNamedBufferParameteriv;
    public static void glGetNamedBufferParameteriv(uint buffer, uint pname, out int @params) => _glGetNamedBufferParameteriv(buffer, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETNAMEDBUFFERPARAMETERI64VPROC(uint buffer, uint pname, out long @params);
    private static PFNGLGETNAMEDBUFFERPARAMETERI64VPROC _glGetNamedBufferParameteri64v;
    public static void glGetNamedBufferParameteri64v(uint buffer, uint pname, out long @params) => _glGetNamedBufferParameteri64v(buffer, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETNAMEDBUFFERPOINTERVPROC(uint buffer, uint pname, IntPtr @params);
    private static PFNGLGETNAMEDBUFFERPOINTERVPROC _glGetNamedBufferPointerv;
    public static void glGetNamedBufferPointerv(uint buffer, uint pname, IntPtr @params) => _glGetNamedBufferPointerv(buffer, pname, @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETNAMEDBUFFERSUBDATAPROC(uint buffer, ulong offset, ulong size, IntPtr data);
    private static PFNGLGETNAMEDBUFFERSUBDATAPROC _glGetNamedBufferSubData;
    public static void glGetNamedBufferSubData(uint buffer, ulong offset, ulong size, IntPtr data) => _glGetNamedBufferSubData(buffer, offset, size, data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCREATEFRAMEBUFFERSPROC(int n, out uint framebuffers);
    private static PFNGLCREATEFRAMEBUFFERSPROC _glCreateFramebuffers;
    public static void glCreateFramebuffers(int n, out uint framebuffers) => _glCreateFramebuffers(n, out framebuffers);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLNAMEDFRAMEBUFFERRENDERBUFFERPROC(uint framebuffer, uint attachment, uint renderbuffertarget, uint renderbuffer);
    private static PFNGLNAMEDFRAMEBUFFERRENDERBUFFERPROC _glNamedFramebufferRenderbuffer;
    public static void glNamedFramebufferRenderbuffer(uint framebuffer, uint attachment, uint renderbuffertarget, uint renderbuffer) => _glNamedFramebufferRenderbuffer(framebuffer, attachment, renderbuffertarget, renderbuffer);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLNAMEDFRAMEBUFFERPARAMETERIPROC(uint framebuffer, uint pname, int param);
    private static PFNGLNAMEDFRAMEBUFFERPARAMETERIPROC _glNamedFramebufferParameteri;
    public static void glNamedFramebufferParameteri(uint framebuffer, uint pname, int param) => _glNamedFramebufferParameteri(framebuffer, pname, param);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLNAMEDFRAMEBUFFERTEXTUREPROC(uint framebuffer, uint attachment, uint texture, int level);
    private static PFNGLNAMEDFRAMEBUFFERTEXTUREPROC _glNamedFramebufferTexture;
    public static void glNamedFramebufferTexture(uint framebuffer, uint attachment, uint texture, int level) => _glNamedFramebufferTexture(framebuffer, attachment, texture, level);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLNAMEDFRAMEBUFFERTEXTURELAYERPROC(uint framebuffer, uint attachment, uint texture, int level, int layer);
    private static PFNGLNAMEDFRAMEBUFFERTEXTURELAYERPROC _glNamedFramebufferTextureLayer;
    public static void glNamedFramebufferTextureLayer(uint framebuffer, uint attachment, uint texture, int level, int layer) => _glNamedFramebufferTextureLayer(framebuffer, attachment, texture, level, layer);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLNAMEDFRAMEBUFFERDRAWBUFFERPROC(uint framebuffer, uint buf);
    private static PFNGLNAMEDFRAMEBUFFERDRAWBUFFERPROC _glNamedFramebufferDrawBuffer;
    public static void glNamedFramebufferDrawBuffer(uint framebuffer, uint buf) => _glNamedFramebufferDrawBuffer(framebuffer, buf);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLNAMEDFRAMEBUFFERDRAWBUFFERSPROC(uint framebuffer, int n, ref readonly uint bufs);
    private static PFNGLNAMEDFRAMEBUFFERDRAWBUFFERSPROC _glNamedFramebufferDrawBuffers;
    public static void glNamedFramebufferDrawBuffers(uint framebuffer, int n, ref readonly uint bufs) => _glNamedFramebufferDrawBuffers(framebuffer, n, in bufs);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLNAMEDFRAMEBUFFERREADBUFFERPROC(uint framebuffer, uint src);
    private static PFNGLNAMEDFRAMEBUFFERREADBUFFERPROC _glNamedFramebufferReadBuffer;
    public static void glNamedFramebufferReadBuffer(uint framebuffer, uint src) => _glNamedFramebufferReadBuffer(framebuffer, src);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLINVALIDATENAMEDFRAMEBUFFERDATAPROC(uint framebuffer, int numAttachments, ref readonly uint attachments);
    private static PFNGLINVALIDATENAMEDFRAMEBUFFERDATAPROC _glInvalidateNamedFramebufferData;
    public static void glInvalidateNamedFramebufferData(uint framebuffer, int numAttachments, ref readonly uint attachments) => _glInvalidateNamedFramebufferData(framebuffer, numAttachments, in attachments);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLINVALIDATENAMEDFRAMEBUFFERSUBDATAPROC(uint framebuffer, int numAttachments, ref readonly uint attachments, int x, int y, int width, int height);
    private static PFNGLINVALIDATENAMEDFRAMEBUFFERSUBDATAPROC _glInvalidateNamedFramebufferSubData;
    public static void glInvalidateNamedFramebufferSubData(uint framebuffer, int numAttachments, ref readonly uint attachments, int x, int y, int width, int height) => _glInvalidateNamedFramebufferSubData(framebuffer, numAttachments, in attachments, x, y, width, height);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCLEARNAMEDFRAMEBUFFERIVPROC(uint framebuffer, uint buffer, int drawbuffer, ref readonly int value);
    private static PFNGLCLEARNAMEDFRAMEBUFFERIVPROC _glClearNamedFramebufferiv;
    public static void glClearNamedFramebufferiv(uint framebuffer, uint buffer, int drawbuffer, ref readonly int value) => _glClearNamedFramebufferiv(framebuffer, buffer, drawbuffer, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCLEARNAMEDFRAMEBUFFERUIVPROC(uint framebuffer, uint buffer, int drawbuffer, ref readonly uint value);
    private static PFNGLCLEARNAMEDFRAMEBUFFERUIVPROC _glClearNamedFramebufferuiv;
    public static void glClearNamedFramebufferuiv(uint framebuffer, uint buffer, int drawbuffer, ref readonly uint value) => _glClearNamedFramebufferuiv(framebuffer, buffer, drawbuffer, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCLEARNAMEDFRAMEBUFFERFVPROC(uint framebuffer, uint buffer, int drawbuffer, ref readonly float value);
    private static PFNGLCLEARNAMEDFRAMEBUFFERFVPROC _glClearNamedFramebufferfv;
    public static void glClearNamedFramebufferfv(uint framebuffer, uint buffer, int drawbuffer, ref readonly float value) => _glClearNamedFramebufferfv(framebuffer, buffer, drawbuffer, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCLEARNAMEDFRAMEBUFFERFIPROC(uint framebuffer, uint buffer, int drawbuffer, float depth, int stencil);
    private static PFNGLCLEARNAMEDFRAMEBUFFERFIPROC _glClearNamedFramebufferfi;
    public static void glClearNamedFramebufferfi(uint framebuffer, uint buffer, int drawbuffer, float depth, int stencil) => _glClearNamedFramebufferfi(framebuffer, buffer, drawbuffer, depth, stencil);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBLITNAMEDFRAMEBUFFERPROC(uint readFramebuffer, uint drawFramebuffer, int srcX0, int srcY0, int srcX1, int srcY1, int dstX0, int dstY0, int dstX1, int dstY1, uint mask, uint filter);
    private static PFNGLBLITNAMEDFRAMEBUFFERPROC _glBlitNamedFramebuffer;
    public static void glBlitNamedFramebuffer(uint readFramebuffer, uint drawFramebuffer, int srcX0, int srcY0, int srcX1, int srcY1, int dstX0, int dstY0, int dstX1, int dstY1, uint mask, uint filter) => _glBlitNamedFramebuffer(readFramebuffer, drawFramebuffer, srcX0, srcY0, srcX1, srcY1, dstX0, dstY0, dstX1, dstY1, mask, filter);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate uint PFNGLCHECKNAMEDFRAMEBUFFERSTATUSPROC(uint framebuffer, uint target);
    private static PFNGLCHECKNAMEDFRAMEBUFFERSTATUSPROC _glCheckNamedFramebufferStatus;
    public static uint glCheckNamedFramebufferStatus(uint framebuffer, uint target) => _glCheckNamedFramebufferStatus(framebuffer, target);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETNAMEDFRAMEBUFFERPARAMETERIVPROC(uint framebuffer, uint pname, out int param);
    private static PFNGLGETNAMEDFRAMEBUFFERPARAMETERIVPROC _glGetNamedFramebufferParameteriv;
    public static void glGetNamedFramebufferParameteriv(uint framebuffer, uint pname, out int param) => _glGetNamedFramebufferParameteriv(framebuffer, pname, out param);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETNAMEDFRAMEBUFFERATTACHMENTPARAMETERIVPROC(uint framebuffer, uint attachment, uint pname, out int @params);
    private static PFNGLGETNAMEDFRAMEBUFFERATTACHMENTPARAMETERIVPROC _glGetNamedFramebufferAttachmentParameteriv;
    public static void glGetNamedFramebufferAttachmentParameteriv(uint framebuffer, uint attachment, uint pname, out int @params) => _glGetNamedFramebufferAttachmentParameteriv(framebuffer, attachment, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCREATERENDERBUFFERSPROC(int n, out uint renderbuffers);
    private static PFNGLCREATERENDERBUFFERSPROC _glCreateRenderbuffers;
    public static void glCreateRenderbuffers(int n, out uint renderbuffers) => _glCreateRenderbuffers(n, out renderbuffers);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLNAMEDRENDERBUFFERSTORAGEPROC(uint renderbuffer, uint internalformat, int width, int height);
    private static PFNGLNAMEDRENDERBUFFERSTORAGEPROC _glNamedRenderbufferStorage;
    public static void glNamedRenderbufferStorage(uint renderbuffer, uint internalformat, int width, int height) => _glNamedRenderbufferStorage(renderbuffer, internalformat, width, height);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLNAMEDRENDERBUFFERSTORAGEMULTISAMPLEPROC(uint renderbuffer, int samples, uint internalformat, int width, int height);
    private static PFNGLNAMEDRENDERBUFFERSTORAGEMULTISAMPLEPROC _glNamedRenderbufferStorageMultisample;
    public static void glNamedRenderbufferStorageMultisample(uint renderbuffer, int samples, uint internalformat, int width, int height) => _glNamedRenderbufferStorageMultisample(renderbuffer, samples, internalformat, width, height);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETNAMEDRENDERBUFFERPARAMETERIVPROC(uint renderbuffer, uint pname, out int @params);
    private static PFNGLGETNAMEDRENDERBUFFERPARAMETERIVPROC _glGetNamedRenderbufferParameteriv;
    public static void glGetNamedRenderbufferParameteriv(uint renderbuffer, uint pname, out int @params) => _glGetNamedRenderbufferParameteriv(renderbuffer, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCREATETEXTURESPROC(uint target, int n, out uint textures);
    private static PFNGLCREATETEXTURESPROC _glCreateTextures;
    public static void glCreateTextures(uint target, int n, out uint textures) => _glCreateTextures(target, n, out textures);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXTUREBUFFERPROC(uint texture, uint internalformat, uint buffer);
    private static PFNGLTEXTUREBUFFERPROC _glTextureBuffer;
    public static void glTextureBuffer(uint texture, uint internalformat, uint buffer) => _glTextureBuffer(texture, internalformat, buffer);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXTUREBUFFERRANGEPROC(uint texture, uint internalformat, uint buffer, ulong offset, ulong size);
    private static PFNGLTEXTUREBUFFERRANGEPROC _glTextureBufferRange;
    public static void glTextureBufferRange(uint texture, uint internalformat, uint buffer, ulong offset, ulong size) => _glTextureBufferRange(texture, internalformat, buffer, offset, size);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXTURESTORAGE1DPROC(uint texture, int levels, uint internalformat, int width);
    private static PFNGLTEXTURESTORAGE1DPROC _glTextureStorage1D;
    public static void glTextureStorage1D(uint texture, int levels, uint internalformat, int width) => _glTextureStorage1D(texture, levels, internalformat, width);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXTURESTORAGE2DPROC(uint texture, int levels, uint internalformat, int width, int height);
    private static PFNGLTEXTURESTORAGE2DPROC _glTextureStorage2D;
    public static void glTextureStorage2D(uint texture, int levels, uint internalformat, int width, int height) => _glTextureStorage2D(texture, levels, internalformat, width, height);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXTURESTORAGE3DPROC(uint texture, int levels, uint internalformat, int width, int height, int depth);
    private static PFNGLTEXTURESTORAGE3DPROC _glTextureStorage3D;
    public static void glTextureStorage3D(uint texture, int levels, uint internalformat, int width, int height, int depth) => _glTextureStorage3D(texture, levels, internalformat, width, height, depth);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXTURESTORAGE2DMULTISAMPLEPROC(uint texture, int samples, uint internalformat, int width, int height, bool fixedsamplelocations);
    private static PFNGLTEXTURESTORAGE2DMULTISAMPLEPROC _glTextureStorage2DMultisample;
    public static void glTextureStorage2DMultisample(uint texture, int samples, uint internalformat, int width, int height, bool fixedsamplelocations) => _glTextureStorage2DMultisample(texture, samples, internalformat, width, height, fixedsamplelocations);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXTURESTORAGE3DMULTISAMPLEPROC(uint texture, int samples, uint internalformat, int width, int height, int depth, bool fixedsamplelocations);
    private static PFNGLTEXTURESTORAGE3DMULTISAMPLEPROC _glTextureStorage3DMultisample;
    public static void glTextureStorage3DMultisample(uint texture, int samples, uint internalformat, int width, int height, int depth, bool fixedsamplelocations) => _glTextureStorage3DMultisample(texture, samples, internalformat, width, height, depth, fixedsamplelocations);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXTURESUBIMAGE1DPROC(uint texture, int level, int xoffset, int width, uint format, uint type, IntPtr pixels);
    private static PFNGLTEXTURESUBIMAGE1DPROC _glTextureSubImage1D;
    public static void glTextureSubImage1D(uint texture, int level, int xoffset, int width, uint format, uint type, IntPtr pixels) => _glTextureSubImage1D(texture, level, xoffset, width, format, type, pixels);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXTURESUBIMAGE2DPROC(uint texture, int level, int xoffset, int yoffset, int width, int height, uint format, uint type, IntPtr pixels);
    private static PFNGLTEXTURESUBIMAGE2DPROC _glTextureSubImage2D;
    public static void glTextureSubImage2D(uint texture, int level, int xoffset, int yoffset, int width, int height, uint format, uint type, IntPtr pixels) => _glTextureSubImage2D(texture, level, xoffset, yoffset, width, height, format, type, pixels);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXTURESUBIMAGE3DPROC(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, uint type, IntPtr pixels);
    private static PFNGLTEXTURESUBIMAGE3DPROC _glTextureSubImage3D;
    public static void glTextureSubImage3D(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, uint type, IntPtr pixels) => _glTextureSubImage3D(texture, level, xoffset, yoffset, zoffset, width, height, depth, format, type, pixels);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCOMPRESSEDTEXTURESUBIMAGE1DPROC(uint texture, int level, int xoffset, int width, uint format, int imageSize, IntPtr data);
    private static PFNGLCOMPRESSEDTEXTURESUBIMAGE1DPROC _glCompressedTextureSubImage1D;
    public static void glCompressedTextureSubImage1D(uint texture, int level, int xoffset, int width, uint format, int imageSize, IntPtr data) => _glCompressedTextureSubImage1D(texture, level, xoffset, width, format, imageSize, data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCOMPRESSEDTEXTURESUBIMAGE2DPROC(uint texture, int level, int xoffset, int yoffset, int width, int height, uint format, int imageSize, IntPtr data);
    private static PFNGLCOMPRESSEDTEXTURESUBIMAGE2DPROC _glCompressedTextureSubImage2D;
    public static void glCompressedTextureSubImage2D(uint texture, int level, int xoffset, int yoffset, int width, int height, uint format, int imageSize, IntPtr data) => _glCompressedTextureSubImage2D(texture, level, xoffset, yoffset, width, height, format, imageSize, data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCOMPRESSEDTEXTURESUBIMAGE3DPROC(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, int imageSize, IntPtr data);
    private static PFNGLCOMPRESSEDTEXTURESUBIMAGE3DPROC _glCompressedTextureSubImage3D;
    public static void glCompressedTextureSubImage3D(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, int imageSize, IntPtr data) => _glCompressedTextureSubImage3D(texture, level, xoffset, yoffset, zoffset, width, height, depth, format, imageSize, data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCOPYTEXTURESUBIMAGE1DPROC(uint texture, int level, int xoffset, int x, int y, int width);
    private static PFNGLCOPYTEXTURESUBIMAGE1DPROC _glCopyTextureSubImage1D;
    public static void glCopyTextureSubImage1D(uint texture, int level, int xoffset, int x, int y, int width) => _glCopyTextureSubImage1D(texture, level, xoffset, x, y, width);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCOPYTEXTURESUBIMAGE2DPROC(uint texture, int level, int xoffset, int yoffset, int x, int y, int width, int height);
    private static PFNGLCOPYTEXTURESUBIMAGE2DPROC _glCopyTextureSubImage2D;
    public static void glCopyTextureSubImage2D(uint texture, int level, int xoffset, int yoffset, int x, int y, int width, int height) => _glCopyTextureSubImage2D(texture, level, xoffset, yoffset, x, y, width, height);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCOPYTEXTURESUBIMAGE3DPROC(uint texture, int level, int xoffset, int yoffset, int zoffset, int x, int y, int width, int height);
    private static PFNGLCOPYTEXTURESUBIMAGE3DPROC _glCopyTextureSubImage3D;
    public static void glCopyTextureSubImage3D(uint texture, int level, int xoffset, int yoffset, int zoffset, int x, int y, int width, int height) => _glCopyTextureSubImage3D(texture, level, xoffset, yoffset, zoffset, x, y, width, height);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXTUREPARAMETERFPROC(uint texture, uint pname, float param);
    private static PFNGLTEXTUREPARAMETERFPROC _glTextureParameterf;
    public static void glTextureParameterf(uint texture, uint pname, float param) => _glTextureParameterf(texture, pname, param);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXTUREPARAMETERFVPROC(uint texture, uint pname, ref readonly float param);
    private static PFNGLTEXTUREPARAMETERFVPROC _glTextureParameterfv;
    public static void glTextureParameterfv(uint texture, uint pname, ref readonly float param) => _glTextureParameterfv(texture, pname, in param);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXTUREPARAMETERIPROC(uint texture, uint pname, int param);
    private static PFNGLTEXTUREPARAMETERIPROC _glTextureParameteri;
    public static void glTextureParameteri(uint texture, uint pname, int param) => _glTextureParameteri(texture, pname, param);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXTUREPARAMETERIIVPROC(uint texture, uint pname, ref readonly int @params);
    private static PFNGLTEXTUREPARAMETERIIVPROC _glTextureParameterIiv;
    public static void glTextureParameterIiv(uint texture, uint pname, ref readonly int @params) => _glTextureParameterIiv(texture, pname, in @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXTUREPARAMETERIUIVPROC(uint texture, uint pname, ref readonly uint @params);
    private static PFNGLTEXTUREPARAMETERIUIVPROC _glTextureParameterIuiv;
    public static void glTextureParameterIuiv(uint texture, uint pname, ref readonly uint @params) => _glTextureParameterIuiv(texture, pname, in @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXTUREPARAMETERIVPROC(uint texture, uint pname, ref readonly int param);
    private static PFNGLTEXTUREPARAMETERIVPROC _glTextureParameteriv;
    public static void glTextureParameteriv(uint texture, uint pname, ref readonly int param) => _glTextureParameteriv(texture, pname, in param);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGENERATETEXTUREMIPMAPPROC(uint texture);
    private static PFNGLGENERATETEXTUREMIPMAPPROC _glGenerateTextureMipmap;
    public static void glGenerateTextureMipmap(uint texture) => _glGenerateTextureMipmap(texture);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBINDTEXTUREUNITPROC(uint unit, uint texture);
    private static PFNGLBINDTEXTUREUNITPROC _glBindTextureUnit;
    public static void glBindTextureUnit(uint unit, uint texture) => _glBindTextureUnit(unit, texture);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETTEXTUREIMAGEPROC(uint texture, int level, uint format, uint type, int bufSize, IntPtr pixels);
    private static PFNGLGETTEXTUREIMAGEPROC _glGetTextureImage;
    public static void glGetTextureImage(uint texture, int level, uint format, uint type, int bufSize, IntPtr pixels) => _glGetTextureImage(texture, level, format, type, bufSize, pixels);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETCOMPRESSEDTEXTUREIMAGEPROC(uint texture, int level, int bufSize, IntPtr pixels);
    private static PFNGLGETCOMPRESSEDTEXTUREIMAGEPROC _glGetCompressedTextureImage;
    public static void glGetCompressedTextureImage(uint texture, int level, int bufSize, IntPtr pixels) => _glGetCompressedTextureImage(texture, level, bufSize, pixels);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETTEXTURELEVELPARAMETERFVPROC(uint texture, int level, uint pname, out float @params);
    private static PFNGLGETTEXTURELEVELPARAMETERFVPROC _glGetTextureLevelParameterfv;
    public static void glGetTextureLevelParameterfv(uint texture, int level, uint pname, out float @params) => _glGetTextureLevelParameterfv(texture, level, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETTEXTURELEVELPARAMETERIVPROC(uint texture, int level, uint pname, out int @params);
    private static PFNGLGETTEXTURELEVELPARAMETERIVPROC _glGetTextureLevelParameteriv;
    public static void glGetTextureLevelParameteriv(uint texture, int level, uint pname, out int @params) => _glGetTextureLevelParameteriv(texture, level, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETTEXTUREPARAMETERFVPROC(uint texture, uint pname, out float @params);
    private static PFNGLGETTEXTUREPARAMETERFVPROC _glGetTextureParameterfv;
    public static void glGetTextureParameterfv(uint texture, uint pname, out float @params) => _glGetTextureParameterfv(texture, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETTEXTUREPARAMETERIIVPROC(uint texture, uint pname, out int @params);
    private static PFNGLGETTEXTUREPARAMETERIIVPROC _glGetTextureParameterIiv;
    public static void glGetTextureParameterIiv(uint texture, uint pname, out int @params) => _glGetTextureParameterIiv(texture, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETTEXTUREPARAMETERIUIVPROC(uint texture, uint pname, out uint @params);
    private static PFNGLGETTEXTUREPARAMETERIUIVPROC _glGetTextureParameterIuiv;
    public static void glGetTextureParameterIuiv(uint texture, uint pname, out uint @params) => _glGetTextureParameterIuiv(texture, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETTEXTUREPARAMETERIVPROC(uint texture, uint pname, out int @params);
    private static PFNGLGETTEXTUREPARAMETERIVPROC _glGetTextureParameteriv;
    public static void glGetTextureParameteriv(uint texture, uint pname, out int @params) => _glGetTextureParameteriv(texture, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCREATEVERTEXARRAYSPROC(int n, out uint arrays);
    private static PFNGLCREATEVERTEXARRAYSPROC _glCreateVertexArrays;
    public static void glCreateVertexArrays(int n, out uint arrays) => _glCreateVertexArrays(n, out arrays);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDISABLEVERTEXARRAYATTRIBPROC(uint vaobj, uint index);
    private static PFNGLDISABLEVERTEXARRAYATTRIBPROC _glDisableVertexArrayAttrib;
    public static void glDisableVertexArrayAttrib(uint vaobj, uint index) => _glDisableVertexArrayAttrib(vaobj, index);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLENABLEVERTEXARRAYATTRIBPROC(uint vaobj, uint index);
    private static PFNGLENABLEVERTEXARRAYATTRIBPROC _glEnableVertexArrayAttrib;
    public static void glEnableVertexArrayAttrib(uint vaobj, uint index) => _glEnableVertexArrayAttrib(vaobj, index);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXARRAYELEMENTBUFFERPROC(uint vaobj, uint buffer);
    private static PFNGLVERTEXARRAYELEMENTBUFFERPROC _glVertexArrayElementBuffer;
    public static void glVertexArrayElementBuffer(uint vaobj, uint buffer) => _glVertexArrayElementBuffer(vaobj, buffer);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXARRAYVERTEXBUFFERPROC(uint vaobj, uint bindingindex, uint buffer, ulong offset, int stride);
    private static PFNGLVERTEXARRAYVERTEXBUFFERPROC _glVertexArrayVertexBuffer;
    public static void glVertexArrayVertexBuffer(uint vaobj, uint bindingindex, uint buffer, ulong offset, int stride) => _glVertexArrayVertexBuffer(vaobj, bindingindex, buffer, offset, stride);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXARRAYVERTEXBUFFERSPROC(uint vaobj, uint first, int count, ref readonly uint buffers, ref readonly ulong offsets, ref readonly int strides);
    private static PFNGLVERTEXARRAYVERTEXBUFFERSPROC _glVertexArrayVertexBuffers;
    public static void glVertexArrayVertexBuffers(uint vaobj, uint first, int count, ref readonly uint buffers, ref readonly ulong offsets, ref readonly int strides) => _glVertexArrayVertexBuffers(vaobj, first, count, in buffers, in offsets, in strides);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXARRAYATTRIBBINDINGPROC(uint vaobj, uint attribindex, uint bindingindex);
    private static PFNGLVERTEXARRAYATTRIBBINDINGPROC _glVertexArrayAttribBinding;
    public static void glVertexArrayAttribBinding(uint vaobj, uint attribindex, uint bindingindex) => _glVertexArrayAttribBinding(vaobj, attribindex, bindingindex);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXARRAYATTRIBFORMATPROC(uint vaobj, uint attribindex, int size, uint type, bool normalized, uint relativeoffset);
    private static PFNGLVERTEXARRAYATTRIBFORMATPROC _glVertexArrayAttribFormat;
    public static void glVertexArrayAttribFormat(uint vaobj, uint attribindex, int size, uint type, bool normalized, uint relativeoffset) => _glVertexArrayAttribFormat(vaobj, attribindex, size, type, normalized, relativeoffset);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXARRAYATTRIBIFORMATPROC(uint vaobj, uint attribindex, int size, uint type, uint relativeoffset);
    private static PFNGLVERTEXARRAYATTRIBIFORMATPROC _glVertexArrayAttribIFormat;
    public static void glVertexArrayAttribIFormat(uint vaobj, uint attribindex, int size, uint type, uint relativeoffset) => _glVertexArrayAttribIFormat(vaobj, attribindex, size, type, relativeoffset);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXARRAYATTRIBLFORMATPROC(uint vaobj, uint attribindex, int size, uint type, uint relativeoffset);
    private static PFNGLVERTEXARRAYATTRIBLFORMATPROC _glVertexArrayAttribLFormat;
    public static void glVertexArrayAttribLFormat(uint vaobj, uint attribindex, int size, uint type, uint relativeoffset) => _glVertexArrayAttribLFormat(vaobj, attribindex, size, type, relativeoffset);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXARRAYBINDINGDIVISORPROC(uint vaobj, uint bindingindex, uint divisor);
    private static PFNGLVERTEXARRAYBINDINGDIVISORPROC _glVertexArrayBindingDivisor;
    public static void glVertexArrayBindingDivisor(uint vaobj, uint bindingindex, uint divisor) => _glVertexArrayBindingDivisor(vaobj, bindingindex, divisor);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETVERTEXARRAYIVPROC(uint vaobj, uint pname, out int param);
    private static PFNGLGETVERTEXARRAYIVPROC _glGetVertexArrayiv;
    public static void glGetVertexArrayiv(uint vaobj, uint pname, out int param) => _glGetVertexArrayiv(vaobj, pname, out param);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETVERTEXARRAYINDEXEDIVPROC(uint vaobj, uint index, uint pname, out int param);
    private static PFNGLGETVERTEXARRAYINDEXEDIVPROC _glGetVertexArrayIndexediv;
    public static void glGetVertexArrayIndexediv(uint vaobj, uint index, uint pname, out int param) => _glGetVertexArrayIndexediv(vaobj, index, pname, out param);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETVERTEXARRAYINDEXED64IVPROC(uint vaobj, uint index, uint pname, out long param);
    private static PFNGLGETVERTEXARRAYINDEXED64IVPROC _glGetVertexArrayIndexed64iv;
    public static void glGetVertexArrayIndexed64iv(uint vaobj, uint index, uint pname, out long param) => _glGetVertexArrayIndexed64iv(vaobj, index, pname, out param);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCREATESAMPLERSPROC(int n, out uint samplers);
    private static PFNGLCREATESAMPLERSPROC _glCreateSamplers;
    public static void glCreateSamplers(int n, out uint samplers) => _glCreateSamplers(n, out samplers);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCREATEPROGRAMPIPELINESPROC(int n, out uint pipelines);
    private static PFNGLCREATEPROGRAMPIPELINESPROC _glCreateProgramPipelines;
    public static void glCreateProgramPipelines(int n, out uint pipelines) => _glCreateProgramPipelines(n, out pipelines);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCREATEQUERIESPROC(uint target, int n, out uint ids);
    private static PFNGLCREATEQUERIESPROC _glCreateQueries;
    public static void glCreateQueries(uint target, int n, out uint ids) => _glCreateQueries(target, n, out ids);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETQUERYBUFFEROBJECTI64VPROC(uint id, uint buffer, uint pname, ulong offset);
    private static PFNGLGETQUERYBUFFEROBJECTI64VPROC _glGetQueryBufferObjecti64v;
    public static void glGetQueryBufferObjecti64v(uint id, uint buffer, uint pname, ulong offset) => _glGetQueryBufferObjecti64v(id, buffer, pname, offset);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETQUERYBUFFEROBJECTIVPROC(uint id, uint buffer, uint pname, ulong offset);
    private static PFNGLGETQUERYBUFFEROBJECTIVPROC _glGetQueryBufferObjectiv;
    public static void glGetQueryBufferObjectiv(uint id, uint buffer, uint pname, ulong offset) => _glGetQueryBufferObjectiv(id, buffer, pname, offset);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETQUERYBUFFEROBJECTUI64VPROC(uint id, uint buffer, uint pname, ulong offset);
    private static PFNGLGETQUERYBUFFEROBJECTUI64VPROC _glGetQueryBufferObjectui64v;
    public static void glGetQueryBufferObjectui64v(uint id, uint buffer, uint pname, ulong offset) => _glGetQueryBufferObjectui64v(id, buffer, pname, offset);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETQUERYBUFFEROBJECTUIVPROC(uint id, uint buffer, uint pname, ulong offset);
    private static PFNGLGETQUERYBUFFEROBJECTUIVPROC _glGetQueryBufferObjectuiv;
    public static void glGetQueryBufferObjectuiv(uint id, uint buffer, uint pname, ulong offset) => _glGetQueryBufferObjectuiv(id, buffer, pname, offset);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLMEMORYBARRIERBYREGIONPROC(uint barriers);
    private static PFNGLMEMORYBARRIERBYREGIONPROC _glMemoryBarrierByRegion;
    public static void glMemoryBarrierByRegion(uint barriers) => _glMemoryBarrierByRegion(barriers);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETTEXTURESUBIMAGEPROC(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, uint type, int bufSize, IntPtr pixels);
    private static PFNGLGETTEXTURESUBIMAGEPROC _glGetTextureSubImage;
    public static void glGetTextureSubImage(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, uint type, int bufSize, IntPtr pixels) => _glGetTextureSubImage(texture, level, xoffset, yoffset, zoffset, width, height, depth, format, type, bufSize, pixels);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETCOMPRESSEDTEXTURESUBIMAGEPROC(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, int bufSize, IntPtr pixels);
    private static PFNGLGETCOMPRESSEDTEXTURESUBIMAGEPROC _glGetCompressedTextureSubImage;
    public static void glGetCompressedTextureSubImage(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, int bufSize, IntPtr pixels) => _glGetCompressedTextureSubImage(texture, level, xoffset, yoffset, zoffset, width, height, depth, bufSize, pixels);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate uint PFNGLGETGRAPHICSRESETSTATUSPROC();
    private static PFNGLGETGRAPHICSRESETSTATUSPROC _glGetGraphicsResetStatus;
    public static uint glGetGraphicsResetStatus() => _glGetGraphicsResetStatus();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETNCOMPRESSEDTEXIMAGEPROC(uint target, int lod, int bufSize, IntPtr pixels);
    private static PFNGLGETNCOMPRESSEDTEXIMAGEPROC _glGetnCompressedTexImage;
    public static void glGetnCompressedTexImage(uint target, int lod, int bufSize, IntPtr pixels) => _glGetnCompressedTexImage(target, lod, bufSize, pixels);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETNTEXIMAGEPROC(uint target, int level, uint format, uint type, int bufSize, IntPtr pixels);
    private static PFNGLGETNTEXIMAGEPROC _glGetnTexImage;
    public static void glGetnTexImage(uint target, int level, uint format, uint type, int bufSize, IntPtr pixels) => _glGetnTexImage(target, level, format, type, bufSize, pixels);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETNUNIFORMDVPROC(uint program, int location, int bufSize, out double @params);
    private static PFNGLGETNUNIFORMDVPROC _glGetnUniformdv;
    public static void glGetnUniformdv(uint program, int location, int bufSize, out double @params) => _glGetnUniformdv(program, location, bufSize, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETNUNIFORMFVPROC(uint program, int location, int bufSize, out float @params);
    private static PFNGLGETNUNIFORMFVPROC _glGetnUniformfv;
    public static void glGetnUniformfv(uint program, int location, int bufSize, out float @params) => _glGetnUniformfv(program, location, bufSize, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETNUNIFORMIVPROC(uint program, int location, int bufSize, out int @params);
    private static PFNGLGETNUNIFORMIVPROC _glGetnUniformiv;
    public static void glGetnUniformiv(uint program, int location, int bufSize, out int @params) => _glGetnUniformiv(program, location, bufSize, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETNUNIFORMUIVPROC(uint program, int location, int bufSize, out uint @params);
    private static PFNGLGETNUNIFORMUIVPROC _glGetnUniformuiv;
    public static void glGetnUniformuiv(uint program, int location, int bufSize, out uint @params) => _glGetnUniformuiv(program, location, bufSize, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLREADNPIXELSPROC(int x, int y, int width, int height, uint format, uint type, int bufSize, IntPtr data);
    private static PFNGLREADNPIXELSPROC _glReadnPixels;
    public static void glReadnPixels(int x, int y, int width, int height, uint format, uint type, int bufSize, IntPtr data) => _glReadnPixels(x, y, width, height, format, type, bufSize, data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETNMAPDVPROC(uint target, uint query, int bufSize, out double v);
    private static PFNGLGETNMAPDVPROC _glGetnMapdv;
    public static void glGetnMapdv(uint target, uint query, int bufSize, out double v) => _glGetnMapdv(target, query, bufSize, out v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETNMAPFVPROC(uint target, uint query, int bufSize, out float v);
    private static PFNGLGETNMAPFVPROC _glGetnMapfv;
    public static void glGetnMapfv(uint target, uint query, int bufSize, out float v) => _glGetnMapfv(target, query, bufSize, out v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETNMAPIVPROC(uint target, uint query, int bufSize, out int v);
    private static PFNGLGETNMAPIVPROC _glGetnMapiv;
    public static void glGetnMapiv(uint target, uint query, int bufSize, out int v) => _glGetnMapiv(target, query, bufSize, out v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETNPIXELMAPFVPROC(uint map, int bufSize, out float values);
    private static PFNGLGETNPIXELMAPFVPROC _glGetnPixelMapfv;
    public static void glGetnPixelMapfv(uint map, int bufSize, out float values) => _glGetnPixelMapfv(map, bufSize, out values);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETNPIXELMAPUIVPROC(uint map, int bufSize, out uint values);
    private static PFNGLGETNPIXELMAPUIVPROC _glGetnPixelMapuiv;
    public static void glGetnPixelMapuiv(uint map, int bufSize, out uint values) => _glGetnPixelMapuiv(map, bufSize, out values);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETNPIXELMAPUSVPROC(uint map, int bufSize, out ushort values);
    private static PFNGLGETNPIXELMAPUSVPROC _glGetnPixelMapusv;
    public static void glGetnPixelMapusv(uint map, int bufSize, out ushort values) => _glGetnPixelMapusv(map, bufSize, out values);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETNPOLYGONSTIPPLEPROC(int bufSize, IntPtr pattern);
    private static PFNGLGETNPOLYGONSTIPPLEPROC _glGetnPolygonStipple;
    public static void glGetnPolygonStipple(int bufSize, IntPtr pattern) => _glGetnPolygonStipple(bufSize, pattern);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETNCOLORTABLEPROC(uint target, uint format, uint type, int bufSize, IntPtr table);
    private static PFNGLGETNCOLORTABLEPROC _glGetnColorTable;
    public static void glGetnColorTable(uint target, uint format, uint type, int bufSize, IntPtr table) => _glGetnColorTable(target, format, type, bufSize, table);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETNCONVOLUTIONFILTERPROC(uint target, uint format, uint type, int bufSize, IntPtr image);
    private static PFNGLGETNCONVOLUTIONFILTERPROC _glGetnConvolutionFilter;
    public static void glGetnConvolutionFilter(uint target, uint format, uint type, int bufSize, IntPtr image) => _glGetnConvolutionFilter(target, format, type, bufSize, image);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETNSEPARABLEFILTERPROC(uint target, uint format, uint type, int rowBufSize, IntPtr row, int columnBufSize, IntPtr column, IntPtr span);
    private static PFNGLGETNSEPARABLEFILTERPROC _glGetnSeparableFilter;
    public static void glGetnSeparableFilter(uint target, uint format, uint type, int rowBufSize, IntPtr row, int columnBufSize, IntPtr column, IntPtr span) => _glGetnSeparableFilter(target, format, type, rowBufSize, row, columnBufSize, column, span);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETNHISTOGRAMPROC(uint target, bool reset, uint format, uint type, int bufSize, IntPtr values);
    private static PFNGLGETNHISTOGRAMPROC _glGetnHistogram;
    public static void glGetnHistogram(uint target, bool reset, uint format, uint type, int bufSize, IntPtr values) => _glGetnHistogram(target, reset, format, type, bufSize, values);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETNMINMAXPROC(uint target, bool reset, uint format, uint type, int bufSize, IntPtr values);
    private static PFNGLGETNMINMAXPROC _glGetnMinmax;
    public static void glGetnMinmax(uint target, bool reset, uint format, uint type, int bufSize, IntPtr values) => _glGetnMinmax(target, reset, format, type, bufSize, values);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXTUREBARRIERPROC();
    private static PFNGLTEXTUREBARRIERPROC _glTextureBarrier;
    public static void glTextureBarrier() => _glTextureBarrier();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLSPECIALIZESHADERPROC(uint shader, string pEntryPoint, uint numSpecializationConstants, ref readonly uint pConstantIndex, ref readonly uint pConstantValue);
    private static PFNGLSPECIALIZESHADERPROC _glSpecializeShader;
    public static void glSpecializeShader(uint shader, string pEntryPoint, uint numSpecializationConstants, ref readonly uint pConstantIndex, ref readonly uint pConstantValue) => _glSpecializeShader(shader, pEntryPoint, numSpecializationConstants, in pConstantIndex, in pConstantValue);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLMULTIDRAWARRAYSINDIRECTCOUNTPROC(uint mode, IntPtr indirect, ulong drawcount, int maxdrawcount, int stride);
    private static PFNGLMULTIDRAWARRAYSINDIRECTCOUNTPROC _glMultiDrawArraysIndirectCount;
    public static void glMultiDrawArraysIndirectCount(uint mode, IntPtr indirect, ulong drawcount, int maxdrawcount, int stride) => _glMultiDrawArraysIndirectCount(mode, indirect, drawcount, maxdrawcount, stride);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLMULTIDRAWELEMENTSINDIRECTCOUNTPROC(uint mode, uint type, IntPtr indirect, ulong drawcount, int maxdrawcount, int stride);
    private static PFNGLMULTIDRAWELEMENTSINDIRECTCOUNTPROC _glMultiDrawElementsIndirectCount;
    public static void glMultiDrawElementsIndirectCount(uint mode, uint type, IntPtr indirect, ulong drawcount, int maxdrawcount, int stride) => _glMultiDrawElementsIndirectCount(mode, type, indirect, drawcount, maxdrawcount, stride);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPOLYGONOFFSETCLAMPPROC(float factor, float units, float clamp);
    private static PFNGLPOLYGONOFFSETCLAMPPROC _glPolygonOffsetClamp;
    public static void glPolygonOffsetClamp(float factor, float units, float clamp) => _glPolygonOffsetClamp(factor, units, clamp);



    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLSHADERSOURCEPROC_PTR(uint shader, uint count, ref readonly Utf8ZPtr utf8PtrArray, ref readonly int utf8LengthArray);
    private static PFNGLSHADERSOURCEPROC_PTR _glShaderSourcePtr;
    public static void glShaderSource(uint shader, uint count, ref readonly Utf8ZPtr utf8PtrArray, ref readonly int utf8LengthArray) =>
        _glShaderSourcePtr(shader, count, in utf8PtrArray, in utf8LengthArray);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int PFNGLGETUNIFORMLOCATIONPROC_PTR(uint program, ref readonly byte name);
    private static PFNGLGETUNIFORMLOCATIONPROC_PTR _glGetUniformLocationPtr;
    public static int glGetUniformLocation(uint program, ReadOnlySpan<byte> name) => _glGetUniformLocationPtr(program, ref MemoryMarshal.GetReference(name));

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int PFNGLGETATTRIBLOCATIONPROC_PTR(uint program, ref readonly byte name);
    private static PFNGLGETATTRIBLOCATIONPROC_PTR _glGetAttribLocationPtr;
    public static int glGetAttribLocation(uint program, ReadOnlySpan<byte> name) => _glGetAttribLocationPtr(program, ref MemoryMarshal.GetReference(name));
}