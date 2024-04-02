using Bannerlord.ButterLib.CrashReportWindow.OpenGL;

using ImGuiNET;

using System;
using System.Numerics;

namespace Bannerlord.ButterLib.CrashReportWindow.Controller;

using static Gl;

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

        var framebufferWidth = (int) (drawData.DisplaySize.X * drawData.FramebufferScale.X);
        var framebufferHeight = (int) (drawData.DisplaySize.Y * drawData.FramebufferScale.Y);
        if (framebufferWidth <= 0 || framebufferHeight <= 0) return;

        // Backup GL state
        var lastActiveTexture = (uint) _gl.GetInteger(GL_ACTIVE_TEXTURE);
        _gl.ActiveTexture(GL_TEXTURE0);

        var lastProgram = (uint) _gl.GetInteger(GL_CURRENT_PROGRAM);
        var lastTexture = (uint) _gl.GetInteger(GL_TEXTURE_BINDING_2D);

        var lastSampler = (uint) _gl.GetInteger(GL_SAMPLER_BINDING);

        var lastVertexArrayObject = (uint) _gl.GetInteger(GL_VERTEX_ARRAY_BINDING);
        var lastArrayBuffer = (uint) _gl.GetInteger(GL_ARRAY_BUFFER_BINDING);

        Span<int> lastPolygonMode = stackalloc int[2];
        _gl.GetIntegerv(GL_POLYGON_MODE, lastPolygonMode);

        Span<int> lastScissorBox = stackalloc int[4];
        _gl.GetIntegerv(GL_SCISSOR_BOX, lastScissorBox);

        Span<int> lastViewport = stackalloc int[4];
        _gl.GetIntegerv(GL_VIEWPORT, lastViewport);

        var lastBlendSrcRgb = (uint) _gl.GetInteger(GL_BLEND_SRC_RGB);
        var lastBlendDstRgb = (uint) _gl.GetInteger(GL_BLEND_DST_RGB);

        var lastBlendSrcAlpha = (uint) _gl.GetInteger(GL_BLEND_SRC_ALPHA);
        var lastBlendDstAlpha = (uint) _gl.GetInteger(GL_BLEND_DST_ALPHA);

        var lastBlendEquationRgb = (uint) _gl.GetInteger(GL_BLEND_EQUATION_RGB);
        var lastBlendEquationAlpha = (uint) _gl.GetInteger(GL_BLEND_EQUATION_ALPHA);

        var lastEnableBlend = _gl.IsEnabled(GL_BLEND);
        var lastEnableCullFace = _gl.IsEnabled(GL_CULL_FACE);
        var lastEnableDepthTest = _gl.IsEnabled(GL_DEPTH_TEST);
        var lastEnableStencilTest = _gl.IsEnabled(GL_STENCIL_TEST);
        var lastEnableScissorTest = _gl.IsEnabled(GL_SCISSOR_TEST);

        var lastEnablePrimitiveRestart = _gl.IsEnabled(GL_PRIMITIVE_RESTART);

        SetupRenderState(drawData, framebufferWidth, framebufferHeight);

        RenderCommandList(drawData, framebufferWidth, framebufferHeight);

        // Destroy the temporary VAO
        _gl.DeleteVertexArray(_vertexArrayObject);
        _vertexArrayObject = 0;

        // Restore modified GL state
        _gl.UseProgram(lastProgram);
        _gl.BindTexture(GL_TEXTURE_2D, lastTexture);

        _gl.BindSampler(0, lastSampler);

        _gl.ActiveTexture(lastActiveTexture);

        _gl.BindVertexArray(lastVertexArrayObject);

        _gl.BindBuffer(GL_ARRAY_BUFFER, lastArrayBuffer);
        _gl.BlendEquationSeparate(lastBlendEquationRgb, lastBlendEquationAlpha);
        _gl.BlendFuncSeparate(lastBlendSrcRgb, lastBlendDstRgb, lastBlendSrcAlpha, lastBlendDstAlpha);

        if (lastEnableBlend) _gl.Enable(GL_BLEND);
        else _gl.Disable(GL_BLEND);

        if (lastEnableCullFace) _gl.Enable(GL_CULL_FACE);
        else _gl.Disable(GL_CULL_FACE);

        if (lastEnableDepthTest) _gl.Enable(GL_DEPTH_TEST);
        else _gl.Disable(GL_DEPTH_TEST);
        if (lastEnableStencilTest) _gl.Enable(GL_STENCIL_TEST);
        else _gl.Disable(GL_STENCIL_TEST);

        if (lastEnableScissorTest) _gl.Enable(GL_SCISSOR_TEST);
        else _gl.Disable(GL_SCISSOR_TEST);

        if (lastEnablePrimitiveRestart) _gl.Enable(GL_PRIMITIVE_RESTART);
        else _gl.Disable(GL_PRIMITIVE_RESTART);

        _gl.PolygonMode(GL_FRONT, (uint) lastPolygonMode[0]);
        _gl.PolygonMode(GL_BACK, (uint) lastPolygonMode[1]);

        _gl.Viewport(lastViewport[0], lastViewport[1], lastViewport[2], lastViewport[3]);

        _gl.Scissor(lastScissorBox[0], lastScissorBox[1], lastScissorBox[2], lastScissorBox[3]);
    }

    private void SetupRenderState(ImDrawDataPtr drawData, int framebufferWidth, int framebufferHeight)
    {
        _gl.Enable(GL_BLEND);
        _gl.BlendEquation(GL_FUNC_ADD);
        _gl.BlendFuncSeparate(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA, GL_ONE, GL_ONE_MINUS_SRC_ALPHA);
        _gl.Disable(GL_CULL_FACE);
        _gl.Disable(GL_DEPTH_TEST);
        _gl.Disable(GL_STENCIL_TEST);
        _gl.Enable(GL_SCISSOR_TEST);
        _gl.Disable(GL_PRIMITIVE_RESTART);
        _gl.PolygonMode(GL_FRONT_AND_BACK, GL_FILL);

        _gl.Viewport(0, 0, framebufferWidth, framebufferHeight);

        var orthographicProjection = Matrix4x4.CreateOrthographicOffCenter(
            left: drawData.DisplayPos.X,
            right: drawData.DisplayPos.X + drawData.DisplaySize.X,
            bottom: drawData.DisplayPos.Y + drawData.DisplaySize.Y,
            top: drawData.DisplayPos.Y,
            zNearPlane: -1,
            zFarPlane: 1);

        _shader.UseShader();
        _gl.Uniform1i(_attribLocationTex, 0);
        _gl.UniformMatrix4(_attribLocationProjMtx, ref orthographicProjection);
        _gl.CheckGlError("Projection");

        _gl.BindSampler(0, 0);

        // Setup desired GL state
        // Recreate the VAO every time (this is to easily allow multiple GL contexts to be rendered to. VAO are not shared among GL contexts)
        // The renderer would actually work without any VAO bound, but then our VertexAttrib calls would overwrite the default one currently bound.
        _vertexArrayObject = _gl.GenVertexArray();
        _gl.BindVertexArray(_vertexArrayObject);
        _gl.CheckGlError("VAO");

        _gl.BindBuffer(GL_ARRAY_BUFFER, _vboHandle);
        _gl.BindBuffer(GL_ELEMENT_ARRAY_BUFFER, _elementsHandle);
        _gl.EnableVertexAttribArray(_attribLocationVtxPos);
        _gl.EnableVertexAttribArray(_attribLocationVtxUv);
        _gl.EnableVertexAttribArray(_attribLocationVtxColor);
        _gl.VertexAttribPointer(_attribLocationVtxPos, 2, GL_FLOAT, false, _sizeOfImDrawVert, _offsetOfImDrawVertPos);
        _gl.VertexAttribPointer(_attribLocationVtxUv, 2, GL_FLOAT, false, _sizeOfImDrawVert, _offsetOfImDrawVertUV);
        _gl.VertexAttribPointer(_attribLocationVtxColor, 4, GL_UNSIGNED_BYTE, true, _sizeOfImDrawVert, _offsetOfImDrawVertCol);
    }

    private void RenderCommandList(ImDrawDataPtr drawData, int framebufferWidth, int framebufferHeight)
    {
        // Will project scissor/clipping rectangles into framebuffer space
        var clipOff = drawData.DisplayPos;         // (0,0) unless using multi-viewports
        var clipScale = drawData.FramebufferScale; // (1,1) unless using retina display which are often (2,2)

        // Render command lists
        for (var n = 0; n < drawData.CmdListsCount; n++)
        {
            var cmdList = drawData.CmdLists[n];

            // Upload vertex/index buffers

            _gl.BufferData(GL_ARRAY_BUFFER, (nuint) (cmdList.VtxBuffer.Size * _sizeOfImDrawVert), cmdList.VtxBuffer.Data, GL_STREAM_DRAW);
            _gl.CheckGlError($"Data Vert {n}");
            _gl.BufferData(GL_ELEMENT_ARRAY_BUFFER, (nuint) (cmdList.IdxBuffer.Size * sizeof(ushort)), cmdList.IdxBuffer.Data, GL_STREAM_DRAW);
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
                    _gl.Scissor((int) clipRect.X, (int) (framebufferHeight - clipRect.W), (int) (clipRect.Z - clipRect.X), (int) (clipRect.W - clipRect.Y));
                    _gl.CheckGlError("Scissor");

                    // Bind texture, Draw
                    _gl.BindTexture(GL_TEXTURE_2D, (uint) cmd.TextureId);
                    _gl.CheckGlError("Texture");

                    _gl.DrawElementsBaseVertex(GL_TRIANGLES, cmd.ElemCount, GL_UNSIGNED_SHORT, (IntPtr) (cmd.IdxOffset * sizeof(ushort)), (int) cmd.VtxOffset);
                    _gl.CheckGlError("Draw");
                }
            }
        }
    }
}