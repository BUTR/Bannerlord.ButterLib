using Bannerlord.ButterLib.CrashReportWindow.UnsafeUtils;

using System;

namespace Bannerlord.ButterLib.CrashReportWindow.OpenGL;

unsafe partial class Gl
{
    /// <summary>
    /// Returns a function pointer for the OpenGL function with the specified name. 
    /// </summary>
    /// <param name="funcNameUtf8">The name of the function to lookup.</param>
    public delegate IntPtr GetProcAddressHandler(ReadOnlySpan<byte> funcNameUtf8);

    private readonly delegate* unmanaged[Cdecl]<uint, void> _glEnable;
    private readonly delegate* unmanaged[Cdecl]<uint, void> _glDisable;
    private readonly delegate* unmanaged[Cdecl]<uint, int*, void> _glGetIntegerv;
    private readonly delegate* unmanaged[Cdecl]<uint, float*, void> _glGetFloatv;
    private readonly delegate* unmanaged[Cdecl]<uint, void> _glClear;
    private readonly delegate* unmanaged[Cdecl]<int, int, int, int, void> _glViewport;
    private readonly delegate* unmanaged[Cdecl]<int, int, int, int, void> _glScissor;
    private readonly delegate* unmanaged[Cdecl]<uint, uint, void> _glPolygonMode;
    private readonly delegate* unmanaged[Cdecl]<uint, int, void> _glPixelStorei;
    private readonly delegate* unmanaged[Cdecl]<uint, uint*, void> _glGenTextures;
    private readonly delegate* unmanaged[Cdecl]<uint, uint*, void> _glDeleteTextures;
    private readonly delegate* unmanaged[Cdecl]<uint, uint, int, void> _glTexParameteri;
    private readonly delegate* unmanaged[Cdecl]<uint, uint, float, void> _glTexParameterf;
    private readonly delegate* unmanaged[Cdecl]<uint, uint, void> _glBindTexture;
    private readonly delegate* unmanaged[Cdecl]<uint, void> _glActiveTexture;
    private readonly delegate* unmanaged[Cdecl]<uint, int, int, uint, uint, int, uint, uint, void*, void> _glTexImage2D;
    private readonly delegate* unmanaged[Cdecl]<uint, uint> _glCreateShader;
    private readonly delegate* unmanaged[Cdecl]<uint, uint, Utf8ZPtr*, int*, void> _glShaderSource;
    private readonly delegate* unmanaged[Cdecl]<uint, void> _glCompileShader;
    private readonly delegate* unmanaged[Cdecl]<uint, uint, uint*, byte*, void> _glGetShaderInfoLog;
    private readonly delegate* unmanaged[Cdecl]<uint, uint, uint*, byte*, void> _glGetProgramInfoLog;
    private readonly delegate* unmanaged[Cdecl]<uint> _glCreateProgram;
    private readonly delegate* unmanaged[Cdecl]<uint, uint, void> _glAttachShader;
    private readonly delegate* unmanaged[Cdecl]<uint, void> _glLinkProgram;
    private readonly delegate* unmanaged[Cdecl]<uint, void> _glUseProgram;
    private readonly delegate* unmanaged[Cdecl]<uint, int, int*, void> _glGetShaderiv;
    private readonly delegate* unmanaged[Cdecl]<uint, int, int*, void> _glGetProgramiv;
    private readonly delegate* unmanaged[Cdecl]<uint, byte*, int> _glGetUniformLocation;
    private readonly delegate* unmanaged[Cdecl]<int, int, void> _glUniform1i;
    private readonly delegate* unmanaged[Cdecl]<int, uint, byte, float*, void> _glUniformMatrix4fv;
    private readonly delegate* unmanaged[Cdecl]<uint, uint*, void> _glGenBuffers;
    private readonly delegate* unmanaged[Cdecl]<uint, uint*, void> _glDeleteBuffers;
    private readonly delegate* unmanaged[Cdecl]<uint, uint, void> _glBindBuffer;
    private readonly delegate* unmanaged[Cdecl]<uint, uint*, void> _glGenVertexArrays;
    private readonly delegate* unmanaged[Cdecl]<uint, uint*, void> _glDeleteVertexArrays;
    private readonly delegate* unmanaged[Cdecl]<uint, void> _glBindVertexArray;
    private readonly delegate* unmanaged[Cdecl]<uint, void> _glEnableVertexAttribArray;
    private readonly delegate* unmanaged[Cdecl]<uint, int, uint, byte, uint, void*, void> _glVertexAttribPointer;
    private readonly delegate* unmanaged[Cdecl]<uint, byte*, int> _glGetAttribLocation;
    private readonly delegate* unmanaged[Cdecl]<uint, uint, uint, uint, void> _glBlendFuncSeparate;
    private readonly delegate* unmanaged[Cdecl]<uint, uint, void> _glBindSampler;
    private readonly delegate* unmanaged[Cdecl]<uint, void> _glDeleteProgram;
    private readonly delegate* unmanaged[Cdecl]<uint, void> _glDeleteShader;
    private readonly delegate* unmanaged[Cdecl]<uint, uint, void> _glDetachShader;
    private readonly delegate* unmanaged[Cdecl]<uint, uint, void> _glBlendEquationSeparate;
    private readonly delegate* unmanaged[Cdecl]<uint, void> _glBlendEquation;
    private readonly delegate* unmanaged[Cdecl]<uint, byte> _glIsEnabled;
    private readonly delegate* unmanaged[Cdecl]<uint> _glGetError;
    private readonly delegate* unmanaged[Cdecl]<uint, nuint, void*, uint, void> _glBufferData;
    private readonly delegate* unmanaged[Cdecl]<uint, uint, uint, void*, int, void> _glDrawElementsBaseVertex;

    /// <summary>
    /// Imports all OpenGL functions using the specified loader.
    /// </summary>
    /// <param name="getProcAddress">A loader to retrieve a fuction pointer.</param>
    public Gl(GetProcAddressHandler getProcAddress)
    {
        _glEnable = (delegate* unmanaged[Cdecl]<uint, void>) getProcAddress("glEnable\0"u8);
        _glDisable = (delegate* unmanaged[Cdecl]<uint, void>) getProcAddress("glDisable\0"u8);
        _glGetIntegerv = (delegate* unmanaged[Cdecl]<uint, int*, void>) getProcAddress("glGetIntegerv\0"u8);
        _glGetFloatv = (delegate* unmanaged[Cdecl]<uint, float*, void>) getProcAddress("glGetFloatv\0"u8);
        _glClear = (delegate* unmanaged[Cdecl]<uint, void>) getProcAddress("glClear\0"u8); // bitfield
        _glViewport = (delegate* unmanaged[Cdecl]<int, int, int, int, void>) getProcAddress("glViewport\0"u8);
        _glScissor = (delegate* unmanaged[Cdecl]<int, int, int, int, void>) getProcAddress("glScissor\0"u8);
        _glPolygonMode = (delegate* unmanaged[Cdecl]<uint, uint, void>) getProcAddress("glPolygonMode\0"u8);
        _glPixelStorei = (delegate* unmanaged[Cdecl]<uint, int, void>) getProcAddress("glPixelStorei\0"u8);
        _glGenTextures = (delegate* unmanaged[Cdecl]<uint, uint*, void>) getProcAddress("glGenTextures\0"u8);
        _glDeleteTextures = (delegate* unmanaged[Cdecl]<uint, uint*, void>) getProcAddress("glDeleteTextures\0"u8);
        _glTexParameteri = (delegate* unmanaged[Cdecl]<uint, uint, int, void>) getProcAddress("glTexParameteri\0"u8);
        _glTexParameterf = (delegate* unmanaged[Cdecl]<uint, uint, float, void>) getProcAddress("glTexParameterf\0"u8);
        _glBindTexture = (delegate* unmanaged[Cdecl]<uint, uint, void>) getProcAddress("glBindTexture\0"u8);
        _glActiveTexture = (delegate* unmanaged[Cdecl]<uint, void>) getProcAddress("glActiveTexture\0"u8);
        _glTexImage2D = (delegate* unmanaged[Cdecl]<uint, int, int, uint, uint, int, uint, uint, void*, void>) getProcAddress("glTexImage2D\0"u8);
        _glCreateShader = (delegate* unmanaged[Cdecl]<uint, uint>) getProcAddress("glCreateShader\0"u8);
        _glShaderSource = (delegate* unmanaged[Cdecl]<uint, uint, Utf8ZPtr*, int*, void>) getProcAddress("glShaderSource\0"u8);
        _glCompileShader = (delegate* unmanaged[Cdecl]<uint, void>) getProcAddress("glCompileShader\0"u8);
        _glGetShaderInfoLog = (delegate* unmanaged[Cdecl]<uint, uint, uint*, byte*, void>) getProcAddress("glGetShaderInfoLog\0"u8);
        _glGetProgramInfoLog = (delegate* unmanaged[Cdecl]<uint, uint, uint*, byte*, void>) getProcAddress("glGetProgramInfoLog\0"u8);
        _glCreateProgram = (delegate* unmanaged[Cdecl]<uint>) getProcAddress("glCreateProgram\0"u8);
        _glAttachShader = (delegate* unmanaged[Cdecl]<uint, uint, void>) getProcAddress("glAttachShader\0"u8);
        _glLinkProgram = (delegate* unmanaged[Cdecl]<uint, void>) getProcAddress("glLinkProgram\0"u8);
        _glUseProgram = (delegate* unmanaged[Cdecl]<uint, void>) getProcAddress("glUseProgram\0"u8);
        _glGetShaderiv = (delegate* unmanaged[Cdecl]<uint, int, int*, void>) getProcAddress("glGetShaderiv\0"u8);
        _glGetProgramiv = (delegate* unmanaged[Cdecl]<uint, int, int*, void>) getProcAddress("glGetProgramiv\0"u8);
        _glGetUniformLocation = (delegate* unmanaged[Cdecl]<uint, byte*, int>) getProcAddress("glGetUniformLocation\0"u8);
        _glUniform1i = (delegate* unmanaged[Cdecl]<int, int, void>) getProcAddress("glUniform1i\0"u8);
        _glUniformMatrix4fv = (delegate* unmanaged[Cdecl]<int, uint, byte, float*, void>) getProcAddress("glUniformMatrix4fv\0"u8);
        _glGenBuffers = (delegate* unmanaged[Cdecl]<uint, uint*, void>) getProcAddress("glGenBuffers\0"u8);
        _glDeleteBuffers = (delegate* unmanaged[Cdecl]<uint, uint*, void>) getProcAddress("glDeleteBuffers\0"u8);
        _glBindBuffer = (delegate* unmanaged[Cdecl]<uint, uint, void>) getProcAddress("glBindBuffer\0"u8);
        _glGenVertexArrays = (delegate* unmanaged[Cdecl]<uint, uint*, void>) getProcAddress("glGenVertexArrays\0"u8);
        _glDeleteVertexArrays = (delegate* unmanaged[Cdecl]<uint, uint*, void>) getProcAddress("glDeleteVertexArrays\0"u8);
        _glBindVertexArray = (delegate* unmanaged[Cdecl]<uint, void>) getProcAddress("glBindVertexArray\0"u8);
        _glEnableVertexAttribArray = (delegate* unmanaged[Cdecl]<uint, void>) getProcAddress("glEnableVertexAttribArray\0"u8);
        _glVertexAttribPointer = (delegate* unmanaged[Cdecl]<uint, int, uint, byte, uint, void*, void>) getProcAddress("glVertexAttribPointer\0"u8);
        _glGetAttribLocation = (delegate* unmanaged[Cdecl]<uint, byte*, int>) getProcAddress("glGetAttribLocation\0"u8);
        _glBlendFuncSeparate = (delegate* unmanaged[Cdecl]<uint, uint, uint, uint, void>) getProcAddress("glBlendFuncSeparate\0"u8);
        _glBindSampler = (delegate* unmanaged[Cdecl]<uint, uint, void>) getProcAddress("glBindSampler\0"u8);
        _glDeleteProgram = (delegate* unmanaged[Cdecl]<uint, void>) getProcAddress("glDeleteProgram\0"u8);
        _glDeleteShader = (delegate* unmanaged[Cdecl]<uint, void>) getProcAddress("glDeleteShader\0"u8);
        _glDetachShader = (delegate* unmanaged[Cdecl]<uint, uint, void>) getProcAddress("glDetachShader\0"u8);
        _glBlendEquationSeparate = (delegate* unmanaged[Cdecl]<uint, uint, void>) getProcAddress("glBlendEquationSeparate\0"u8);
        _glBlendEquation = (delegate* unmanaged[Cdecl]<uint, void>) getProcAddress("glBlendEquation\0"u8);
        _glIsEnabled = (delegate* unmanaged[Cdecl]<uint, byte>) getProcAddress("glIsEnabled\0"u8);
        _glGetError = (delegate* unmanaged[Cdecl]<uint>) getProcAddress("glGetError\0"u8);
        _glBufferData = (delegate* unmanaged[Cdecl]<uint, nuint, void*, uint, void>) getProcAddress("glBufferData\0"u8);
        _glDrawElementsBaseVertex = (delegate* unmanaged[Cdecl]<uint, uint, uint, void*, int, void>) getProcAddress("glDrawElementsBaseVertex\0"u8);
    }
}