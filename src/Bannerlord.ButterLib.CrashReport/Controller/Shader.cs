// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Bannerlord.ButterLib.CrashReportWindow.UnsafeUtils;

using Silk.NET.OpenGL;

using System;
using System.Runtime.CompilerServices;

namespace Bannerlord.ButterLib.CrashReportWindow.Controller;

internal class Shader
{
    private readonly GL _gl;
    private bool _initialized = false;

    public uint Program { get; private set; }

    public Shader(GL gl, ReadOnlySpan<byte> vertexShaderUtf8, ReadOnlySpan<byte> fragmentShaderUtf8)
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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int GetUniformLocation(ReadOnlySpan<byte> uniformUtf8) => _gl.GetUniformLocation(Program, uniformUtf8);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int GetAttribLocation(ReadOnlySpan<byte> attribUtf8) => _gl.GetAttribLocation(Program, attribUtf8);

    private uint CreateProgram(ReadOnlySpan<byte> vertexShaderUtf8, ReadOnlySpan<byte> fragmentShaderUtf8)
    {
        var program = _gl.CreateProgram();

        var vertexShader = CompileShader(GLEnum.VertexShader, vertexShaderUtf8);
        var fragmentShader = CompileShader(GLEnum.FragmentShader, fragmentShaderUtf8);

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

    private unsafe uint CompileShader(GLEnum type, ReadOnlySpan<byte> sourceUtf8)
    {
        var shader = _gl.CreateShader(type);
        Utf8ZPtr* shaderData = stackalloc Utf8ZPtr[]
        {
            new Utf8ZPtr(in sourceUtf8),
        };
        Span<int> shaderDataLength = stackalloc int[]
        {
            sourceUtf8.Length,
        };
        _gl.ShaderSource(shader, (byte**) shaderData, shaderDataLength);
        _gl.CompileShader(shader);
        CheckShader(shader);

        return shader;
    }

    private void CheckShader(uint handle)
    {
        Span<int> status = stackalloc int[1];
        _gl.GetShader(handle, GLEnum.CompileStatus, status);
        if (status[0] != (int) GLEnum.False) return;

        var info = _gl.GetShaderInfoLog(handle);
        //Debug.WriteLine($"GL.CompileShader for shader [{type}] had info log:\n{info}");
        throw new Exception($"OpenGL Shader: {info}");
    }

    private void CheckProgram(uint handle)
    {
        Span<int> status = stackalloc int[1];
        _gl.GetProgram(handle, GLEnum.LinkStatus, status);
        if (status[0] != (int) GLEnum.False) return;

        var info = _gl.GetProgramInfoLog(handle);
        throw new Exception($"OpenGL Program: {info}");
    }
}