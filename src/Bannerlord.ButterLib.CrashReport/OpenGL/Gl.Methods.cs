using Bannerlord.ButterLib.CrashReportWindow.UnsafeUtils;

using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Security;

namespace Bannerlord.ButterLib.CrashReportWindow.OpenGL;

// Credits to https://gist.github.com/dcronqvist/8e0c594532748e8fc21133ac6e3e8514/
[SuppressUnmanagedCodeSecurity]
unsafe partial class Gl
{
    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void UniformMatrix4(int uniformLocation, ref readonly Matrix4x4 value)
    {
        Span<float> data = stackalloc float[16]
        {
            value.M11, value.M12, value.M13, value.M14,
            value.M21, value.M22, value.M23, value.M24,
            value.M31, value.M32, value.M33, value.M34,
            value.M41, value.M42, value.M43, value.M44,
        };
        UniformMatrix4fv(uniformLocation, 1, false, data);
    }

    /// <summary>
    /// Specify the value of a uniform variable for the current program object.
    /// </summary>
    /// <param name="location">Specifies the location of the uniform variable to be modified.</param>
    /// <param name="count">Specifies the number of matrices that are to be modified.</param>
    /// <param name="transpose">Specifies whether to transpose the matrix as the values are loaded into the uniform variable.</param>
    /// <param name="values">An array of count values that will be used to update the specified uniform variable.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void UniformMatrix4fv(int location, uint count, bool transpose, ReadOnlySpan<float> values)
    {
        fixed (float* value = &values[0])
        {
            _glUniformMatrix4fv(location, count, transpose ? (byte) 1 : (byte) 0, value);
        }
    }

    /// <summary>
    /// Render primitives from array data with a per-element offset.
    /// </summary>
    /// <param name="mode">Specifies what kind of primitives to render. </param>
    /// <param name="count">Specifies the number of elements to be rendered.</param>
    /// <param name="type">Specifies the type of the values in indices.<para>Must be one of GL_UNSIGNED_BYTE, GL_UNSIGNED_SHORT, or GL_UNSIGNED_INT.</para></param>
    /// <param name="indices">Specifies a pointer to the location where the indices are stored.</param>
    /// <param name="baseVertex">Specifies a constant that should be added to each element of indices when choosing elements from the enabled vertex arrays.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void DrawElementsBaseVertex(uint mode, uint count, uint type, IntPtr indices, int baseVertex)
    {
        _glDrawElementsBaseVertex(mode, count, type, indices.ToPointer(), baseVertex);
    }

    /// <summary>
    /// Return a parameter from a program object.
    /// </summary>
    /// <param name="program">Specifies the program object to be queried.</param>
    /// <param name="pname">Specifies the object parameter.</param>
    /// <param name="values">The parameters to return..</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void GetProgramiv(uint program, int pname, ReadOnlySpan<int> values)
    {
        fixed (int* args = &values[0])
        {
            _glGetProgramiv(program, pname, args);
        }
    }

    /// <summary>
    /// Return a parameter from a shader object.
    /// </summary>
    /// <param name="shader">Specifies the shader object to be queried.</param>
    /// <param name="pname">Specifies the object parameter.<para>Must be GL_SHADER_TYPE, GL_DELETE_STATUS, GL_COMPILE_STATUS, GL_INFO_LOG_LENGTH, or GL_SHADER_SOURCE_LENGTH.</para></param>
    /// <param name="values">The parameters to return..</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void GetShaderiv(uint shader, int pname, ReadOnlySpan<int> values)
    {
        fixed (int* args = &values[0])
        {
            _glGetShaderiv(shader, pname, args);
        }
    }

    /// <summary>
    /// Enable a generic vertex attribute array.
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be disabled.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void EnableVertexAttribArray(uint index)
    {
        _glEnableVertexAttribArray(index);
    }

    /// <summary>
    ///     Define an array of generic vertex attribute data
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified.</param>
    /// <param name="size">
    ///     Specifies the number of components per generic vertex attribute.
    ///     <para>Must be 1, 2, 3, 4, or <see cref="GL_BGRA" />.</para>
    /// </param>
    /// <param name="type">Specifies the data type of each component in the array.</param>
    /// <param name="normalized">
    ///     Specifies whether fixed-point data values should be normalized (true) or converted directly as
    ///     fixed-point values (false) when they are accessed.
    /// </param>
    /// <param name="stride">
    ///     Specifies the byte offset between consecutive generic vertex attributes.
    ///     <para>If stride is 0, the generic vertex attributes are understood to be tightly packed in the array.</para>
    /// </param>
    /// <param name="pointer">
    ///     Specifies a offset of the first component of the first generic vertex attribute in the array in
    ///     the data store of the buffer currently bound to the GL_ARRAY_BUFFER target.
    /// </param>
    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void VertexAttribPointer(uint index, int size, uint type, bool normalized, uint stride, IntPtr pointer)
    {
        _glVertexAttribPointer(index, size, type, normalized ? (byte) 1 : (byte) 0, stride, pointer.ToPointer());
    }

    /// <summary>
    /// Returns the location of an attribute variable.
    /// </summary>
    /// <param name="program">Specifies the program object to be queried.</param>
    /// <param name="utf8">A null terminated UTF8 string containing the name of the attribute variable whose location is to be queried.</param>
    /// <returns>The location of the attribute, or <c>-1</c> if an error occured.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public int GetAttribLocation(uint program, ReadOnlySpan<byte> utf8)
    {
        fixed (byte* b = &utf8[0])
        {
            return _glGetAttribLocation(program, b);
        }
    }

    /// <summary>
    /// Generates a single vertex array object name.
    /// </summary>
    /// <returns>A generated vertex array name.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public uint GenVertexArray()
    {
        uint array;
        _glGenVertexArrays(1, &array);
        return array;
    }

    /// <summary>
    /// Bind a vertex array object.
    /// </summary>
    /// <param name="array">Specifies the name of the vertex array to bind.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void BindVertexArray(uint array)
    {
        _glBindVertexArray(array);
    }

    /// <summary>
    /// Deletes a single vertex array object.
    /// </summary>
    /// <param name="array">The array to delete.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void DeleteVertexArray(uint array)
    {
        _glDeleteVertexArrays(1, &array);
    }

    /// <summary>
    /// Specify the value of a uniform variable for the current program object.
    /// </summary>
    /// <param name="location">Specifies the location of the uniform value to be modified.</param>
    /// <param name="v0">The first value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void Uniform1i(int location, int v0)
    {
        _glUniform1i(location, v0);
    }

    /// <summary>
    ///     Specify pixel arithmetic for RGB and alpha components separately.
    /// </summary>
    /// <param name="sFactorRgb">
    ///     Specifies how the red, green, and blue blending factors are computed.
    ///     <para>The initial value is GL_ONE.</para>
    /// </param>
    /// <param name="dFactorRgb">
    ///     Specifies how the red, green, and blue destination blending factors are computed.
    ///     <para>The initial value is GL_ZERO.</para>
    /// </param>
    /// <param name="sFactorAlpha">
    ///     Specified how the alpha source blending factor is computed.
    ///     <para>The initial value is GL_ONE.</para>
    /// </param>
    /// <param name="dFactorAlpha">
    ///     Specified how the alpha destination blending factor is computed.
    ///     <para>The initial value is GL_ZERO.</para>
    /// </param>
    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void BlendFuncSeparate(uint sFactorRgb, uint dFactorRgb, uint sFactorAlpha, uint dFactorAlpha)
    {
        _glBlendFuncSeparate(sFactorRgb, dFactorRgb, sFactorAlpha, dFactorAlpha);
    }

    /// <summary>
    ///     Bind a named buffer object.
    /// </summary>
    /// <param name="target">Specifies the target to which the buffer object is bound.</param>
    /// <param name="buffer">Specifies the name of a buffer object.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void BindBuffer(uint target, uint buffer)
    {
        _glBindBuffer(target, buffer);
    }

    /// <summary>
    ///     Deletes a single buffer object.
    /// </summary>
    /// <param name="buffer">A buffer to be deleted.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void DeleteBuffer(uint buffer)
    {
        _glDeleteBuffers(1, &buffer);
    }

    /// <summary>
    ///     Generate a single buffer object name.
    /// </summary>
    /// <returns>The buffer object name.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public uint GenBuffer()
    {
        uint id;
        _glGenBuffers(1, &id);
        return id;
    }

    /// <summary>
    ///     Bind a named sampler to a texturing target.
    /// </summary>
    /// <param name="unit">Specifies the index of the texture unit to which the sampler is bound.</param>
    /// <param name="sampler">Specifies the name of a sampler.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void BindSampler(uint unit, uint sampler)
    {
        _glBindSampler(unit, sampler);
    }

    /// <summary>
    /// Attaches a shader object to a program object.
    /// </summary>
    /// <param name="program">Specifies the program object to which a shader object will be attached.</param>
    /// <param name="shader">Specifies the shader object that is to be attached.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void AttachShader(uint program, uint shader)
    {
        _glAttachShader(program, shader);
    }

    /// <summary>
    ///     Compiles a shader object.
    /// </summary>
    /// <param name="shader">Specifies the shader object to be compiled.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void CompileShader(uint shader)
    {
        _glCompileShader(shader);
    }

    /// <summary>
    ///     Creates a shader program object.
    /// </summary>
    /// <returns>An empty program object, a non-zero value by which it can be referenced.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public uint CreateProgram()
    {
        return _glCreateProgram();
    }

    /// <summary>
    ///     Creates a shader object.
    /// </summary>
    /// <param name="type">Specifies the type of shader to be created.<para>Must be one of GL_VERTEX_SHADER, GL_GEOMETRY_SHADER, or GL_FRAGMENT_SHADER.</para></param>
    /// <returns>An empty shader object, a non-zero value by which it can be referenced.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public uint CreateShader(uint type)
    {
        return _glCreateShader(type);
    }

    /// <summary>
    ///     Deletes a program object.
    /// </summary>
    /// <param name="program">Specifies the program object to be deleted.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void DeleteProgram(uint program)
    {
        _glDeleteProgram(program);
    }

    /// <summary>
    ///     Deletes a shader object.
    /// </summary>
    /// <param name="shader">Specifies the shader object to be deleted.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void DeleteShader(uint shader)
    {
        _glDeleteShader(shader);
    }

    /// <summary>
    ///     Detaches a shader object from a program object to which it is attached.
    /// </summary>
    /// <param name="program">Specifies the program object from which to detach the shader object.</param>
    /// <param name="shader">Specifies the shader object to be detached.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void DetachShader(uint program, uint shader)
    {
        _glDetachShader(program, shader);
    }

    /// <summary>
    ///     Installs a program object as part of current rendering state.
    /// </summary>
    /// <param name="program">Specifies the handle of the program object whose executables are to be used as part of current rendering state.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void UseProgram(uint program)
    {
        _glUseProgram(program);
    }

    /// <summary>
    ///     Links a program object.
    /// </summary>
    /// <param name="program">Specifies the handle of the program object to be linked.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void LinkProgram(uint program)
    {
        _glLinkProgram(program);
    }

    /// <summary>
    ///      Replaces the source code in a shader object.
    /// </summary>
    /// <param name="shader">Specifies the handle of the shader object whose source code is to be replaced.</param>
    /// <param name="str">Specifies an array of pointers to strings containing the source code to be loaded into the shader.</param>
    /// <param name="length">Specifies an array of string lengths.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void ShaderSource(uint shader, ReadOnlySpan<Utf8ZPtr> str, ReadOnlySpan<int> length)
    {
        fixed (Utf8ZPtr* utf8PtrArrayPtr = str)
        fixed (int* utf8LengthArrayPtr = length)
        {
            _glShaderSource(shader, (uint) str.Length, utf8PtrArrayPtr, utf8LengthArrayPtr);
        }
    }

    /// <summary>
    ///      Returns the location of a uniform variable.
    /// </summary>
    /// <param name="program">Specifies the program object to be queried.</param>
    /// <param name="name">A array of bytes containing the name of the uniform variable whose location is to be queried.</param>
    /// <returns>An integer that represents the location of a specific uniform variable within a program object.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public int GetUniformLocation(uint program, ReadOnlySpan<byte> name)
    {
        fixed (byte* b = &name[0])
        {
            return _glGetUniformLocation(program, b);
        }
    }

    /// <summary>
    ///     Returns the information log for a program object.
    /// </summary>
    /// <param name="program">Specifies the program object whose information log is to be queried.</param>
    /// <param name="bufSize">Specifies the size of the character buffer for storing the returned information log.</param>
    /// <returns>The info log, or <c>null</c> if an error occured.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public string GetProgramInfoLog(uint program, uint bufSize = 1024)
    {
        uint length;
        var buffer = bufSize < 2048 ? stackalloc byte[Unsafe.As<uint, int>(ref bufSize)] : new byte[bufSize];
        fixed (byte* b = buffer)
        {
            _glGetProgramInfoLog(program, bufSize, &length, b);
        }
        return UnsafeHelper.ToString(buffer.Slice(0, Unsafe.As<uint, int>(ref length)));
    }

    /// <summary>
    ///     Returns the information log for a shader object.
    /// </summary>
    /// <param name="shader">Specifies the shader object whose information log is to be queried.</param>
    /// <param name="bufSize">Specifies the size of the character buffer for storing the returned information log.</param>
    /// <returns>The info log, or <c>null</c> if an error occured.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public string GetShaderInfoLog(uint shader, uint bufSize = 1024)
    {
        uint length;
        var buffer = bufSize < 2048 ? stackalloc byte[Unsafe.As<uint, int>(ref bufSize)] : new byte[bufSize];
        fixed (byte* b = buffer)
        {
            _glGetShaderInfoLog(shader, bufSize, &length, b);
        }
        return UnsafeHelper.ToString(buffer.Slice(0, Unsafe.As<uint, int>(ref length)));
    }

    /// <summary>
    ///     Set the RGB blend equation and the alpha blend equation separately.
    /// </summary>
    /// <param name="modeRGB">
    ///     Specifies the RGB blend equation, how the red, green, and blue components of the source and
    ///     destination colors are combined.
    ///     <para>Must be GL_FUNC_ADD, GL_FUNC_SUBTRACT, GL_FUNC_REVERSE_SUBTRACT, GL_MIN, GL_MAX.</para>
    /// </param>
    /// <param name="modeAlpha">
    ///     Specifies the alpha blend equation, how the alpha component of the source and destination
    ///     colors are combined.
    ///     <para>Must be GL_FUNC_ADD, GL_FUNC_SUBTRACT, GL_FUNC_REVERSE_SUBTRACT, GL_MIN, GL_MAX.</para>
    /// </param>
    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void BlendEquationSeparate(uint modeRGB, uint modeAlpha)
    {
        _glBlendEquationSeparate(modeRGB, modeAlpha);
    }

    /// <summary>
    /// Return the value or values of a selected parameter.
    /// </summary>
    /// <param name="paramName">Specifies the parameter value to be returned.</param>
    /// <returns>The request parameter value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public float GetFloat(uint paramName)
    {
        float value;
        _glGetFloatv(paramName, &value);
        return value;
    }

    /// <summary>
    /// Return the value or values of a selected parameter.
    /// </summary>
    /// <param name="paramName">Specifies the parameter value to be returned.</param>
    /// <returns>The request parameter value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public int GetInteger(uint paramName)
    {
        int value;
        _glGetIntegerv(paramName, &value);
        return value;
    }

    /// <summary>
    /// Return the value or values of a selected parameter.
    /// </summary>
    /// <param name="paramName">Specifies the parameter value to be returned.</param>
    /// <param name="values">Returns the value or values of the specified parameter.</param>
    /// <returns>The request parameter value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void GetIntegerv(uint paramName, ReadOnlySpan<int> values)
    {
        fixed (int* valuesPtr = values)
        {
            _glGetIntegerv(paramName, valuesPtr);
        }
    }

    /// <summary>
    ///     Select a polygon rasterization mode
    /// </summary>
    /// <param name="face">
    ///     Specifies the polygons that mode applies to. Must be GL_FRONT_AND_BACK for front- and back-facing
    ///     polygons
    /// </param>
    /// <param name="mode">
    ///     Specifies how polygons will be rasterized.
    ///     <para>Accepted values are GL_POINT, GL_LINE, and GL_FILL.</para>
    ///     The initial value is GL_FILL for both front- and back-facing polygons.
    /// </param>
    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void PolygonMode(uint face, uint mode)
    {
        _glPolygonMode(face, mode);
    }

    /// <summary>
    ///     Define the scissor box.
    /// </summary>
    /// <param name="x">
    ///     Specify the lower left corner of the scissor box on the x-axis
    ///     <para>Initially <c>0</c>.</para>
    /// </param>
    /// <param name="y">
    ///     Specify the lower left corner of the scissor box on the y-axis
    ///     <para>Initially <c>0</c>.</para>
    /// </param>
    /// <param name="width">Specify the width of the scissor box.</param>
    /// <param name="height">Specify the height of the scissor box.</param>
    /// <remarks>When a GL context is first attached to a window, width and height are set to the dimensions of that window.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void Scissor(int x, int y, int width, int height)
    {
        _glScissor(x, y, width, height);
    }

    /// <summary>
    ///     Clear buffers to preset values.
    ///     <para>The value to which each buffer is cleared depends on the setting of the clear value for that buffer.</para>
    /// </summary>
    /// <param name="mask">
    ///     Bitwise OR of masks that indicate the buffers to be cleared.
    ///     <para>The three masks are GL_COLOR_BUFFER_BIT, GL_DEPTH_BUFFER_BIT,, and GL_STENCIL_BUFFER_BIT.</para>
    /// </param>
    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void Clear(uint mask)
    {
        _glClear(mask);
    }

    /// <summary>
    ///     Enable server-side GL capabilities.
    /// </summary>
    /// <param name="cap">Specifies a symbolic constant indicating a GL capability.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void Enable(uint cap)
    {
        _glEnable(cap);
    }

    /// <summary>
    ///     Disable server-side GL capabilities.
    /// </summary>
    /// <param name="cap">Specifies a symbolic constant indicating a GL capability.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void Disable(uint cap)
    {
        _glDisable(cap);
    }

    /// <summary>
    ///     Specify the equation used for both the RGB blend equation and the Alpha blend equation.
    /// </summary>
    /// <param name="mode">Specifies how source and destination colors are combined.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void BlendEquation(uint mode)
    {
        _glBlendEquation(mode);
    }

    /// <summary>
    ///     Set the viewport.
    /// </summary>
    /// <param name="x">The lower left corner of the viewport rectangle on the x-axis, in pixels.</param>
    /// <param name="y">The lower left corner of the viewport rectangle on the y-axis, in pixels.</param>
    /// <param name="width">The width of the viewport, in pixels.</param>
    /// <param name="height">The height of the viewport.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void Viewport(int x, int y, int width, int height)
    {
        _glViewport(x, y, width, height);
    }

    /// <summary>
    ///     Test whether a capability is enabled.
    /// </summary>
    /// <param name="cap">Specifies a symbolic constant indicating a GL capability.</param>
    /// <returns><c>true</c> if capability is enabled, otherwise <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public bool IsEnabled(uint cap)
    {
        return _glIsEnabled(cap) == GL_TRUE;
    }

    /// <summary>
    ///     Set pixel storage modes.
    /// </summary>
    /// <param name="paramName">
    ///     Specifies the symbolic name of the parameter to be set. One value affects the packing of pixel data
    ///     into memory: GL_PACK_ALIGNMENT. The other affects the unpacking of pixel data from memory: GL_UNPACK_ALIGNMENT.
    /// </param>
    /// <param name="param">Specifies the value that <paramref name="paramName" /> is set to. Valid values are 1, 2, 4, or 8.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void PixelStorei(uint paramName, int param)
    {
        _glPixelStorei(paramName, param);
    }

    /// <summary>
    ///     Creates and initializes a buffer object's data store.
    /// </summary>
    /// <param name="target">Specifies the target to which the buffer object is bound.</param>
    /// <param name="size">Specifies the size in bytes of the buffer object's new data store.</param>
    /// <param name="data">
    ///     Specifies a pointer to data that will be copied into the data store for initialization, or NULL if
    ///     no data is to be copied.
    /// </param>
    /// <param name="usage">
    ///     Specifies the expected usage pattern of the data store.
    ///     <para>
    ///         Must be GL_STREAM_DRAW, GL_STREAM_READ, GL_STREAM_COPY, GL_STATIC_DRAW, GL_STATIC_READ, GL_STATIC_COPY,
    ///         GL_DYNAMIC_DRAW, GL_DYNAMIC_READ, or GL_DYNAMIC_COPY.
    ///     </para>
    ///     .
    /// </param>
    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void BufferData(uint target, nuint size, IntPtr data, uint usage)
    {
        _glBufferData(target, size, data.ToPointer(), usage);
    }

    /// <summary>
    ///     Gets the stored error code information.
    /// </summary>
    /// <returns>An OpenGL error code.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public uint GetError()
    {
        return (uint) _glGetError();
    }

    /// <summary>
    ///     Set texture parameters.
    /// </summary>
    /// <param name="target">
    ///     Specifies the target texture of the active texture unit, which must be either GL_TEXTURE_2D or
    ///     GL_TEXTURE_CUBE_MAP.
    /// </param>
    /// <param name="paramName">
    ///     Specifies the symbolic name of a single-valued texture parameter. <paramref name="paramName" /> can be
    ///     one of the following: GL_TEXTURE_MIN_FILTER, GL_TEXTURE_MAG_FILTER, GL_TEXTURE_WRAP_S, or GL_TEXTURE_WRAP_T.
    /// </param>
    /// <param name="param">Specifies the value of <paramref name="paramName" />.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void TexParameterf(uint target, uint paramName, float param)
    {
        _glTexParameterf(target, paramName, param);
    }

    /// <summary>
    ///     Set texture parameters.
    /// </summary>
    /// <param name="target">
    ///     Specifies the target texture of the active texture unit, which must be either GL_TEXTURE_2D or
    ///     GL_TEXTURE_CUBE_MAP.
    /// </param>
    /// <param name="paramName">
    ///     Specifies the symbolic name of a single-valued texture parameter. <paramref name="paramName" /> can be
    ///     one of the following: GL_TEXTURE_MIN_FILTER, GL_TEXTURE_MAG_FILTER, GL_TEXTURE_WRAP_S, or GL_TEXTURE_WRAP_T.
    /// </param>
    /// <param name="param">Specifies the value of <paramref name="paramName" />.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void TexParameteri(uint target, uint paramName, int param)
    {
        _glTexParameteri(target, paramName, param);
    }

    /// <summary>
    /// Specify a two-dimensional texture image.
    /// </summary>
    /// <param name="target">Specifies the target texture.</param>
    /// <param name="level">
    ///     Specifies the level-of-detail number. Level 0 is the base image level. Level n is the nth mipmap
    ///     reduction image.
    /// </param>
    /// <param name="internalFormat">Specifies the number of color components in the texture. </param>
    /// <param name="width">
    ///     Specifies the width of the texture image.
    ///     <para>All implementations support texture images that are at least 1024 texels wide.</para>
    /// </param>
    /// <param name="height">
    ///     Specifies the height of the texture image, or the number of layers in a texture array.
    ///     <para>
    ///         All implementations support 2D texture images that are at least 1024 texels high, and texture arrays that are
    ///         at least 256 layers deep.
    ///     </para>
    /// </param>
    /// <param name="border">This value must be 0.</param>
    /// <param name="format">
    ///     Specifies the format of the pixel data.
    /// </param>
    /// <param name="type">
    ///     Specifies the data type of the pixel data.
    /// </param>
    /// <param name="pixels">Specifies a pointer to the image data in memory.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void TexImage2D(uint target, int level, int internalFormat, uint width, uint height, int border, uint format, uint type, IntPtr pixels)
    {
        _glTexImage2D(target, level, internalFormat, width, height, border, format, type, pixels.ToPointer());
    }

    /// <summary>
    /// Bind a named texture to a texturing target.
    /// </summary>
    /// <param name="target">Specifies the target to which the texture is bound.</param>
    /// <param name="texture">Specifies the name of a texture.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void BindTexture(uint target, uint texture)
    {
        _glBindTexture(target, texture);
    }

    /// <summary>
    /// Select active texture unit.
    /// </summary>
    /// <param name="texture">Specifies which texture unit to make active.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void ActiveTexture(uint texture)
    {
        _glActiveTexture(texture);
    }

    /// <summary>
    /// Deletes a single texture object.
    /// </summary>
    /// <param name="texture">A texture to delete.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public void DeleteTexture(uint texture)
    {
        _glDeleteTextures(1, &texture);
    }

    /// <summary>
    /// Generates a single texture name.
    /// </summary>
    /// <returns>The generated texture name.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | AggressiveOptimization)]
    public uint GenTexture()
    {
        uint texture;
        _glGenTextures(1, &texture);
        return texture;
    }
}