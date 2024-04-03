using ImGuiNET;

using Silk.NET.OpenGL;

using System;
using System.Numerics;

namespace Bannerlord.ButterLib.CrashReportWindow.Controller;

partial class ImGuiController
{
    /// <summary>
    /// Renders the ImGui draw list data.
    /// </summary>
    public void Render()
    {
        var oldCtx = _imgui.GetCurrentContext();

        if (oldCtx != _context)
        {
            _imgui.SetCurrentContext(_context);
        }

        _imgui.Render();
        RenderImDrawData(_imgui.GetDrawData());

        if (oldCtx != _context)
        {
            _imgui.SetCurrentContext(oldCtx);
        }
    }

    private void RenderImDrawData(ImDrawDataPtr drawData)
    {
        if (drawData.CmdListsCount == 0) return;

        var framebufferWidth = (uint) (drawData.DisplaySize.X * drawData.FramebufferScale.X);
        var framebufferHeight = (uint) (drawData.DisplaySize.Y * drawData.FramebufferScale.Y);
        if (framebufferWidth <= 0 || framebufferHeight <= 0) return;

        // Backup GL state
        var lastActiveTexture = (TextureUnit) _gl.GetInteger(GLEnum.ActiveTexture);
        _gl.ActiveTexture(GLEnum.Texture0);

        var lastProgram = (uint) _gl.GetInteger(GLEnum.CurrentProgram);
        var lastTexture = (uint) _gl.GetInteger(GLEnum.TextureBinding2D);

        var lastSampler = (uint) _gl.GetInteger(GLEnum.SamplerBinding);

        var lastVertexArrayObject = (uint) _gl.GetInteger(GLEnum.VertexArrayBinding);
        var lastArrayBuffer = (uint) _gl.GetInteger(GLEnum.ArrayBufferBinding);

        Span<int> lastPolygonMode = stackalloc int[2];
        _gl.GetInteger(GLEnum.PolygonMode, lastPolygonMode);

        Span<int> lastScissorBox = stackalloc int[4];
        _gl.GetInteger(GLEnum.ScissorBox, lastScissorBox);

        Span<int> lastViewport = stackalloc int[4];
        _gl.GetInteger(GLEnum.Viewport, lastViewport);

        var lastBlendSrcRgb = (BlendingFactor) _gl.GetInteger(GLEnum.BlendSrcRgb);
        var lastBlendDstRgb = (BlendingFactor) _gl.GetInteger(GLEnum.BlendDstRgb);

        var lastBlendSrcAlpha = (BlendingFactor) _gl.GetInteger(GLEnum.BlendSrcAlpha);
        var lastBlendDstAlpha = (BlendingFactor) _gl.GetInteger(GLEnum.BlendDstAlpha);

        var lastBlendEquationRgb = (BlendEquationModeEXT) _gl.GetInteger(GLEnum.BlendEquationRgb);
        var lastBlendEquationAlpha = (BlendEquationModeEXT) _gl.GetInteger(GLEnum.BlendEquationAlpha);

        var lastEnableBlend = _gl.IsEnabled(GLEnum.Blend);
        var lastEnableCullFace = _gl.IsEnabled(GLEnum.CullFace);
        var lastEnableDepthTest = _gl.IsEnabled(GLEnum.DepthTest);
        var lastEnableStencilTest = _gl.IsEnabled(GLEnum.StencilTest);
        var lastEnableScissorTest = _gl.IsEnabled(GLEnum.ScissorTest);

        var lastEnablePrimitiveRestart = _gl.IsEnabled(GLEnum.PrimitiveRestart);

        SetupRenderState(drawData, framebufferWidth, framebufferHeight);

        RenderCommandList(drawData, framebufferWidth, framebufferHeight);

        // Destroy the temporary VAO
        _gl.DeleteVertexArray(_vertexArrayObject);
        _vertexArrayObject = 0;

        // Restore modified GL state
        _gl.UseProgram(lastProgram);
        _gl.BindTexture(GLEnum.Texture2D, lastTexture);

        _gl.BindSampler(0, lastSampler);

        _gl.ActiveTexture(lastActiveTexture);

        _gl.BindVertexArray(lastVertexArrayObject);

        _gl.BindBuffer(GLEnum.ArrayBuffer, lastArrayBuffer);
        _gl.BlendEquationSeparate(lastBlendEquationRgb, lastBlendEquationAlpha);
        _gl.BlendFuncSeparate(lastBlendSrcRgb, lastBlendDstRgb, lastBlendSrcAlpha, lastBlendDstAlpha);

        if (lastEnableBlend) _gl.Enable(GLEnum.Blend);
        else _gl.Disable(GLEnum.Blend);

        if (lastEnableCullFace) _gl.Enable(GLEnum.CullFace);
        else _gl.Disable(GLEnum.CullFace);

        if (lastEnableDepthTest) _gl.Enable(GLEnum.DepthTest);
        else _gl.Disable(GLEnum.DepthTest);
        if (lastEnableStencilTest) _gl.Enable(GLEnum.StencilTest);
        else _gl.Disable(GLEnum.StencilTest);

        if (lastEnableScissorTest) _gl.Enable(GLEnum.ScissorTest);
        else _gl.Disable(GLEnum.ScissorTest);

        if (lastEnablePrimitiveRestart) _gl.Enable(GLEnum.PrimitiveRestart);
        else _gl.Disable(GLEnum.PrimitiveRestart);

        _gl.PolygonMode(GLEnum.Front, (PolygonMode) lastPolygonMode[0]);
        _gl.PolygonMode(GLEnum.Back, (PolygonMode) lastPolygonMode[1]);

        _gl.Viewport(lastViewport[0], lastViewport[1], (uint) lastViewport[2], (uint) lastViewport[3]);

        _gl.Scissor(lastScissorBox[0], lastScissorBox[1], (uint) lastScissorBox[2], (uint) lastScissorBox[3]);
    }

    private void SetupRenderState(ImDrawDataPtr drawData, uint framebufferWidth, uint framebufferHeight)
    {
        _gl.Enable(GLEnum.Blend);
        _gl.BlendEquation(GLEnum.FuncAdd);
        _gl.BlendFuncSeparate(GLEnum.SrcAlpha, GLEnum.OneMinusSrcAlpha, GLEnum.One, GLEnum.OneMinusSrcAlpha);
        _gl.Disable(GLEnum.CullFace);
        _gl.Disable(GLEnum.DepthTest);
        _gl.Disable(GLEnum.StencilTest);
        _gl.Enable(GLEnum.ScissorTest);
        _gl.Disable(GLEnum.PrimitiveRestart);
        _gl.PolygonMode(GLEnum.FrontAndBack, GLEnum.Fill);

        _gl.Viewport(0, 0, framebufferWidth, framebufferHeight);

        var orthographicProjection = Matrix4x4.CreateOrthographicOffCenter(
            left: drawData.DisplayPos.X,
            right: drawData.DisplayPos.X + drawData.DisplaySize.X,
            bottom: drawData.DisplayPos.Y + drawData.DisplaySize.Y,
            top: drawData.DisplayPos.Y,
            zNearPlane: -1,
            zFarPlane: 1);

        _shader.UseShader();
        _gl.Uniform1(_attribLocationTex, 0);
        _gl.UniformMatrix4x4(_attribLocationProjMtx, ref orthographicProjection);
        _gl.CheckGlError("Projection");

        _gl.BindSampler(0, 0);

        // Setup desired GL state
        // Recreate the VAO every time (this is to easily allow multiple GL contexts to be rendered to. VAO are not shared among GL contexts)
        // The renderer would actually work without any VAO bound, but then our VertexAttrib calls would overwrite the default one currently bound.
        _vertexArrayObject = _gl.GenVertexArray();
        _gl.BindVertexArray(_vertexArrayObject);
        _gl.CheckGlError("VAO");

        _gl.BindBuffer(GLEnum.ArrayBuffer, _vboHandle);
        _gl.BindBuffer(GLEnum.ElementArrayBuffer, _elementsHandle);
        _gl.EnableVertexAttribArray(_attribLocationVtxPos);
        _gl.EnableVertexAttribArray(_attribLocationVtxUv);
        _gl.EnableVertexAttribArray(_attribLocationVtxColor);
        _gl.VertexAttribPointer2(_attribLocationVtxPos, 2, GLEnum.Float, false, _sizeOfImDrawVert, _offsetOfImDrawVertPos);
        _gl.VertexAttribPointer2(_attribLocationVtxUv, 2, GLEnum.Float, false, _sizeOfImDrawVert, _offsetOfImDrawVertUV);
        _gl.VertexAttribPointer2(_attribLocationVtxColor, 4, GLEnum.UnsignedByte, true, _sizeOfImDrawVert, _offsetOfImDrawVertCol);
    }

    private void RenderCommandList(ImDrawDataPtr drawData, uint framebufferWidth, uint framebufferHeight)
    {
        // Will project scissor/clipping rectangles into framebuffer space
        var clipOff = drawData.DisplayPos;         // (0,0) unless using multi-viewports
        var clipScale = drawData.FramebufferScale; // (1,1) unless using retina display which are often (2,2)

        // Render command lists
        for (var n = 0; n < drawData.CmdListsCount; n++)
        {
            var cmdList = drawData.CmdLists[n];

            // Upload vertex/index buffers

            _gl.BufferData2(GLEnum.ArrayBuffer, (nuint) (cmdList.VtxBuffer.Size * _sizeOfImDrawVert), cmdList.VtxBuffer.Data, GLEnum.StreamDraw);
            _gl.CheckGlError($"Data Vert {n}");
            _gl.BufferData2(GLEnum.ElementArrayBuffer, (nuint) (cmdList.IdxBuffer.Size * sizeof(ushort)), cmdList.IdxBuffer.Data, GLEnum.StreamDraw);
            _gl.CheckGlError($"Data Idx {n}");

            for (var cmd_i = 0; cmd_i < cmdList.CmdBuffer.Size; cmd_i++)
            {
                var cmd = cmdList.CmdBuffer[cmd_i];

                if (cmd.UserCallback != IntPtr.Zero)
                    throw new NotImplementedException();

                Vector4 clipRect;
                clipRect.X = (cmd.ClipRect.X - clipOff.X) * clipScale.X;
                clipRect.Y = (cmd.ClipRect.Y - clipOff.Y) * clipScale.Y;
                clipRect.Z = (cmd.ClipRect.Z - clipOff.X) * clipScale.X;
                clipRect.W = (cmd.ClipRect.W - clipOff.Y) * clipScale.Y;

                if (clipRect.X < framebufferWidth && clipRect.Y < framebufferHeight && clipRect is { Z: >= 0.0f, W: >= 0.0f })
                {
                    // Apply scissor/clipping rectangle
                    _gl.Scissor((int) clipRect.X, (int) (framebufferHeight - clipRect.W), (uint) (clipRect.Z - clipRect.X), (uint) (clipRect.W - clipRect.Y));
                    _gl.CheckGlError("Scissor");

                    // Bind texture, Draw
                    _gl.BindTexture(GLEnum.Texture2D, (uint) cmd.TextureId);
                    _gl.CheckGlError("Texture");

                    _gl.DrawElementsBaseVertex2(GLEnum.Triangles, cmd.ElemCount, GLEnum.UnsignedShort, (IntPtr) (cmd.IdxOffset * sizeof(ushort)), (int) cmd.VtxOffset);
                    _gl.CheckGlError("Draw");
                }
            }
        }
    }
}