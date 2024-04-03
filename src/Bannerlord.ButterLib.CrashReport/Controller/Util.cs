// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Silk.NET.Core.Attributes;
using Silk.NET.Core.Native;
using Silk.NET.OpenGL;

using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Numerics;

namespace Bannerlord.ButterLib.CrashReportWindow.Controller;

internal static class Util
{
    [Pure]
    public static float Clamp(float value, float min, float max) => value < min ? min : value > max ? max : value;

    [Conditional("DEBUG")]
    public static void CheckGlError(this GL gl, string title)
    {
        var error = gl.GetError();
        if (error != GLEnum.NoError)
        {
            Debug.Print($"{title}: {error}");
        }
    }

    public static unsafe void UniformMatrix4x4(this GL gl, int uniformLocation, ref readonly Matrix4x4 value)
    {
        var data = stackalloc float[16]
        {
            value.M11, value.M12, value.M13, value.M14,
            value.M21, value.M22, value.M23, value.M24,
            value.M31, value.M32, value.M33, value.M34,
            value.M41, value.M42, value.M43, value.M44,
        };
        gl.UniformMatrix4(uniformLocation, 1, false, data);
    }

    public static unsafe void VertexAttribPointer2(this GL gl, [Flow(FlowDirection.In)] uint index, [Flow(FlowDirection.In)] int size, [Flow(FlowDirection.In)] GLEnum type, [Flow(FlowDirection.In)] bool normalized, [Flow(FlowDirection.In)] uint stride, [Flow(FlowDirection.In)] IntPtr pointer)
    {
        gl.VertexAttribPointer(index, size, type, normalized, stride, pointer.ToPointer());
    }

    public static unsafe void BufferData2(this GL gl, [Flow(FlowDirection.In)] GLEnum target, [Flow(FlowDirection.In)] UIntPtr size, [Count(Parameter = "size"), Flow(FlowDirection.In)] IntPtr data, [Flow(FlowDirection.In)] GLEnum usage)
    {
        gl.BufferData(target, size, data.ToPointer(), usage);
    }

    public static unsafe void DrawElementsBaseVertex2(this GL gl, [Flow(FlowDirection.In)] GLEnum mode, [Flow(FlowDirection.In)] uint count, [Flow(FlowDirection.In)] GLEnum type, [Count(Computed = "count, type"), Flow(FlowDirection.In)] IntPtr indices, [Flow(FlowDirection.In)] int basevertex)
    {
        gl.DrawElementsBaseVertex(mode, count, type, indices.ToPointer(), basevertex);
    }
}