// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace Bannerlord.ButterLib.CrashReportWindow.OpenGL;

using static Gl;

public enum TextureCoordinate : uint
{
    S = GL_TEXTURE_WRAP_S,
    T = GL_TEXTURE_WRAP_T,
    R = GL_TEXTURE_WRAP_R
}

internal class Texture : IDisposable
{
    public const uint Srgb8Alpha8 = GL_SRGB8_ALPHA8;

    public const uint GL_TEXTURE_MAX_ANISOTROPY_EXT = 34046;
    public const uint GL_MAX_TEXTURE_MAX_ANISOTROPY_EXT = 34047;

    public static float? MaxAniso;

    private static float Clamp(float n, float min, float max)
    {
        if (n < min) return min;
        if (n > max) return max;
        return n;
    }

    private readonly Gl _gl;
    public readonly uint GlTexture;
    public readonly uint Width;
    public readonly uint Height;
    public readonly uint MipmapLevels;
    public readonly int InternalFormat;

    public Texture(Gl gl, uint width, uint height, IntPtr data, bool generateMipmaps = false, bool srgb = false)
    {
        _gl = gl;

        MaxAniso ??= _gl.GetFloat(GL_MAX_TEXTURE_MAX_ANISOTROPY_EXT);
        Width = width;
        Height = height;
        InternalFormat = srgb ? (int) Srgb8Alpha8 : (int) GL_RGBA8;
        MipmapLevels = (uint) (generateMipmaps == false ? 1 : (int) Math.Floor(Math.Log(Math.Max(Width, Height), 2)));

        GlTexture = _gl.GenTexture();
        Bind();

        var pxFormat = GL_BGRA;

        _gl.TexImage2D(GL_TEXTURE_2D, 0, InternalFormat, width, height, 0, pxFormat, GL_UNSIGNED_BYTE, data);
        //glTexStorage2D(GL_TEXTURE_2D, (int) MipmapLevels, (uint) InternalFormat, Width, Height);
        //glTexSubImage2D(GL_TEXTURE_2D, 0, 0, 0, Width, Height, pxFormat, GL_UNSIGNED_BYTE, data);

        //if (generateMipmaps)
        //     _gl.GenerateTextureMipmap(GlTexture);

        SetWrap(TextureCoordinate.S, GL_REPEAT);
        SetWrap(TextureCoordinate.T, GL_REPEAT);

        _gl.TexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAX_LEVEL, (int) (MipmapLevels - 1));
    }

    public void Bind()
    {
        _gl.BindTexture(GL_TEXTURE_2D, GlTexture);
    }

    public void SetMinFilter(uint filter)
    {
        _gl.TexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, (int) filter);
    }

    public void SetMagFilter(uint filter)
    {
        _gl.TexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, (int) filter);
    }

    public void SetAnisotropy(float level)
    {
        _gl.TexParameterf(GL_TEXTURE_2D, GL_TEXTURE_MAX_ANISOTROPY_EXT, Clamp(level, 1, MaxAniso.GetValueOrDefault()));
    }

    public void SetLod(int @base, int min, int max)
    {
        _gl.TexParameteri(GL_TEXTURE_2D, GL_TEXTURE_LOD_BIAS, @base);
        _gl.TexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_LOD, min);
        _gl.TexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAX_LOD, max);
    }

    public void SetWrap(TextureCoordinate coord, uint mode)
    {
        _gl.TexParameteri(GL_TEXTURE_2D, (uint) coord, (int) mode);
    }

    public void Dispose()
    {
        _gl.DeleteTexture(GlTexture);
    }
}