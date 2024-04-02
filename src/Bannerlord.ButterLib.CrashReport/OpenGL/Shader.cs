// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Bannerlord.ButterLib.CrashReportWindow.UnsafeUtils;

using System;
using System.Runtime.CompilerServices;

namespace Bannerlord.ButterLib.CrashReportWindow.OpenGL;

using static Gl;

/*
struct UniformFieldInfo
{
    public int Location;
    public string Name;
    public int Size;
    public uint Type;
}
*/

internal class Shader
{
    private readonly Gl _gl;
    private bool _initialized = false;

    public uint Program { get; private set; }
    //private readonly Dictionary<string, int> _uniformToLocation = new Dictionary<string, int>();
    //private readonly Dictionary<string, int> _attribLocation = new Dictionary<string, int>();

    public Shader(Gl gl, ReadOnlySpan<byte> vertexShaderUtf8, ReadOnlySpan<byte> fragmentShaderUtf8)
    {
        _gl = gl;
        Program = CreateProgram(vertexShaderUtf8, fragmentShaderUtf8);
    }
    public void UseShader()
    {
        _gl.UseProgram(Program);
    }

    public void Dispose()
    {
        if (_initialized)
        {
            _gl.DeleteProgram(Program);
            _initialized = false;
        }
    }

    /*
    public UniformFieldInfo[] GetUniforms()
    {
        _gl.GetProgramiv(Program, (int) GL_ACTIVE_UNIFORMS, out var uniformCount);

        var uniforms = new UniformFieldInfo[uniformCount];

        for (var i = 0; i < uniformCount; i++)
        {
            var name = _gl.GetActiveUniform(Program, (uint) i, out var size, out var type);

            UniformFieldInfo fieldInfo;
            fieldInfo.Location = GetUniformLocation(name);
            fieldInfo.Name = name;
            fieldInfo.Size = size;
            fieldInfo.Type = type;

            uniforms[i] = fieldInfo;
        }

        return uniforms;
    }
    */

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int GetUniformLocation(ReadOnlySpan<byte> uniformUtf8)
    {
        return _gl.GetUniformLocation(Program, uniformUtf8);

        /*
        if (_uniformToLocation.TryGetValue(uniform, out var location) == false)
        {
            location = _gl.GetUniformLocation(Program, uniformUtf8);
            _uniformToLocation.Add(uniform, location);

            if (location == -1)
            {
                Debug.Print($"The uniform '{uniform}' does not exist in the shader!");
            }
        }

        return location;
        */
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int GetAttribLocation(ReadOnlySpan<byte> attribUtf8)
    {
        return _gl.GetAttribLocation(Program, attribUtf8);

        /*
        if (_attribLocation.TryGetValue(attrib, out var location) == false)
        {
            location = _gl.GetAttribLocation(Program, attribUtf8);
            _attribLocation.Add(attrib, location);

            if (location == -1)
            {
                Debug.Print($"The attrib '{attrib}' does not exist in the shader!");
            }
        }

        return location;
        */
    }

    private uint CreateProgram(ReadOnlySpan<byte> vertexShaderUtf8, ReadOnlySpan<byte> fragmentShaderUtf8)
    {
        var program = _gl.CreateProgram();

        var vertexShader = CompileShader(GL_VERTEX_SHADER, vertexShaderUtf8);
        var fragmentShader = CompileShader(GL_FRAGMENT_SHADER, fragmentShaderUtf8);

        _gl.AttachShader(program, vertexShader);
        _gl.AttachShader(program, fragmentShader);
        _gl.LinkProgram(program);

        CheckProgram(program);

        _gl.DetachShader(program, vertexShader);
        _gl.DetachShader(program, fragmentShader);
        _gl.DeleteShader(vertexShader);
        _gl.DeleteShader(fragmentShader);

        _initialized = true;

        return program;
    }

    private uint CompileShader(uint type, ReadOnlySpan<byte> sourceUtf8)
    {
        var shader = _gl.CreateShader(type);
        Span<Utf8ZPtr> shaderData = stackalloc Utf8ZPtr[]
        {
            new Utf8ZPtr(in sourceUtf8),
        };
        Span<int> shaderDataLength = stackalloc int[]
        {
            sourceUtf8.Length,
        };
        _gl.ShaderSource(shader, shaderData, shaderDataLength);
        _gl.CompileShader(shader);
        CheckShader(shader);

        return shader;
    }

    private void CheckShader(uint handle)
    {
        Span<int> status = stackalloc int[1];
        _gl.GetShaderiv(handle, (int) GL_COMPILE_STATUS, status);
        if (status[0] != GL_FALSE) return;

        var info = _gl.GetShaderInfoLog(handle);
        //Debug.WriteLine($"GL.CompileShader for shader [{type}] had info log:\n{info}");
        throw new Exception($"OpenGL Shader: {info}");
    }

    private void CheckProgram(uint handle)
    {
        Span<int> status = stackalloc int[1];
        _gl.GetProgramiv(handle, (int) GL_LINK_STATUS, status);
        if (status[0] != GL_FALSE) return;

        var info = _gl.GetProgramInfoLog(handle);
        throw new Exception($"OpenGL Program: {info}");
    }
}