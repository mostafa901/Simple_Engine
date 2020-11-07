using OpenTK.Graphics.OpenGL;
using Simple_Engine.Engine.Core.Interfaces;
using Simple_Engine.Engine.GameSystem;
using Simple_Engine.Engine.Render;
using System;
using System.Collections.Generic;

namespace Simple_Engine.Engine.Water.Render
{
    public class FBO_MTargets : FBO
    {
        public FBO_MTargets(int _width, int _height) : base(_width, _height)
        {
            Name = FboName.MultipleTargets;
        }

        public override int CreateFrameBuffer()
        {
            var fboId = GL.GenFramebuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, fboId);

            var buffs = new DrawBuffersEnum[] {
                DrawBuffersEnum.ColorAttachment0,
                DrawBuffersEnum.ColorAttachment1,
                DrawBuffersEnum.ColorAttachment2,
                };
            GL.DrawBuffers(3, buffs);

            return fboId;
        }

        public override void UpdateSize(int width, int height)
        {
            base.UpdateSize(width, height);

            Setup_Defaults(false);
            BindFrameBuffer();
            SelectionTextureId = createTextureAttachment(FramebufferAttachment.ColorAttachment1);
            VertexSelectionTextureId = createTextureAttachment(FramebufferAttachment.ColorAttachment2);
            UnbindCurrentBuffer();
        }

        public override void BindFrameBuffer()
        {
            base.BindFrameBuffer();
            DisplayManager.CurrentBuffer = DisplayManager.RenderBufferType.Selection;
        }

        public override void Live_Update(Shader ShaderModel)
        {
            throw new NotImplementedException();
        }

        public override void PostRender(Shader ShaderModel)
        {
            throw new NotImplementedException();
        }

        public override void RenderFrame(IDrawable model)
        {
            throw new NotImplementedException();
        }

        public override void RenderFrame(List<IDrawable> models)
        {
            throw new NotImplementedException();
        }

        internal void ResolveResults(int outputFrame, ReadBufferMode readBuffer)
        {
            if (outputFrame != 0)
            {
                GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, outputFrame);
            }

            GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, FBOId);
            GL.ReadBuffer(readBuffer);
            GL.BlitFramebuffer(0, 0, Width, Height, 0, 0, Width, Height, ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit, BlitFramebufferFilter.Nearest);
            UnbindCurrentBuffer();
        }
    }
}