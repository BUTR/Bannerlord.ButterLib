// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Silk.NET.OpenGL;

using System;

namespace Bannerlord.ButterLib.CrashReportWindow.Controller;

internal enum TextureCoordinate
{
    S = TextureParameterName.TextureWrapS,
    T = TextureParameterName.TextureWrapT,
    R = TextureParameterName.TextureWrapR,
}

internal class Texture : IDisposable
{
    public static float? MaxAniso;
    public readonly uint GlTexture;
    public readonly uint Width, Height;
    public readonly uint MipmapLevels;
    public readonly SizedInternalFormat InternalFormat;

    private readonly GL _gl;

    public unsafe Texture(GL gl, uint width, uint height, IntPtr data, bool generateMipmaps = false, bool srgb = false)
    {
        _gl = gl;
        MaxAniso ??= gl.GetFloat(GLEnum.MaxTextureMaxAnisotropy);
        Width = width;
        Height = height;
        InternalFormat = srgb ? SizedInternalFormat.Srgb8Alpha8 : SizedInternalFormat.Rgba8;
        MipmapLevels = generateMipmaps == false ? 1U : (uint) Math.Floor(Math.Log(Math.Max(Width, Height), 2));

        GlTexture = _gl.GenTexture();
        Bind();

        var pxFormat = PixelFormat.Bgra;

        _gl.TexImage2D(GLEnum.Texture2D, 0, (int) InternalFormat, width, height, 0, pxFormat, GLEnum.UnsignedByte, data.ToPointer());
        //_gl.TexStorage2D(GLEnum.Texture2D, MipmapLevels, InternalFormat, Width, Height);
        //_gl.TexSubImage2D(GLEnum.Texture2D, 0, 0, 0, Width, Height, pxFormat, PixelType.UnsignedByte, (void*) data);

        if (generateMipmaps)
            _gl.GenerateTextureMipmap(GlTexture);

        SetWrap(TextureCoordinate.S, TextureWrapMode.Repeat);
        SetWrap(TextureCoordinate.T, TextureWrapMode.Repeat);

        _gl.TexParameterI(GLEnum.Texture2D, TextureParameterName.TextureMaxLevel, MipmapLevels - 1);
    }

    public void Bind()
    {
        _gl.BindTexture(GLEnum.Texture2D, GlTexture);
    }

    public void SetMinFilter(GLEnum filter)
    {
        _gl.TexParameterI(GLEnum.Texture2D, GLEnum.TextureMinFilter, (int) filter);
    }

    public void SetMagFilter(GLEnum filter)
    {
        _gl.TexParameterI(GLEnum.Texture2D, GLEnum.TextureMagFilter, (int) filter);
    }

    public void SetAnisotropy(float level)
    {
        _gl.TexParameter(GLEnum.Texture2D, TextureParameterName.TextureMaxAnisotropy, Util.Clamp(level, 1, MaxAniso.GetValueOrDefault()));
    }

    public void SetLod(int @base, int min, int max)
    {
        _gl.TexParameterI(GLEnum.Texture2D, TextureParameterName.TextureLodBias, @base);
        _gl.TexParameterI(GLEnum.Texture2D, TextureParameterName.TextureMinLod, min);
        _gl.TexParameterI(GLEnum.Texture2D, TextureParameterName.TextureMaxLod, max);
    }

    public void SetWrap(TextureCoordinate coord, TextureWrapMode mode)
    {
        _gl.TexParameterI(GLEnum.Texture2D, (TextureParameterName) coord, (int) mode);
    }

    public void Dispose()
    {
        _gl.DeleteTexture(GlTexture);
    }
}