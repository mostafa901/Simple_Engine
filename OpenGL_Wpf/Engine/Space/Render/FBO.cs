using Simple_Engine.Engine.Core.Interfaces;
using Simple_Engine.Engine.Render;
using Simple_Engine.Extentions;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simple_Engine.Engine.GameSystem;
using Simple_Engine.Engine.Space.Scene;

namespace Simple_Engine.Engine.Water.Render
{
    public abstract class FBO
    {
        public enum FboName
        {
            undefined, //Ignore Rendering this FBO
            WorldReflection,
            WorldRefraction,
            Shadow,
            MultipleTargets,
        }

        public Shader FBO_Shader;

        public FboName Name { get; set; } //just to help while Debugging
        public int FBOId { get; private set; }
        public IDrawable StenciledModel { get; set; } = null;
        protected int Width { get; set; }
        protected int Height { get; set; }
        public Vector3 CameraMirror { get; internal set; }

        public int TextureId;
        public int TextureDepthId;

        private int depthBufferId;
        public int Color00BufferId;

        public int StencilDepthId { get; }
        public Vector4 BorderColor { get; set; } = new Vector4();

        public FBO(int _width, int _height)
        {
            Name = FboName.undefined;
            Width = _width;
            Height = _height;
        }

        public void Setup_Defaults(bool withStencil)
        {
            if (FBOId != 0)
            {
                CleanUp();
            }
            FBOId = createFrameBuffer();
          
            TextureId = createTextureAttachment(FramebufferAttachment.ColorAttachment0);

            ActivateDepthBuffer(withStencil);

            UnbindCurrentBuffer();
        }

        private void ValidateFBO()
        {
            var FramebufferStatus = GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);
            if (FramebufferStatus != FramebufferErrorCode.FramebufferComplete)
            {
                Debugger.Break();
            }
        }

        public virtual void UpdateSize(int width, int height)
        {
            Width = width;
            Height = height;
        }

       

        private void ActivateDepthBuffer(bool withStencil)
        {
            if (withStencil)
            {
                depthBufferId = createDepthStencilBufferAttachment(Width, Height);
                TextureDepthId = createDepthStencilTextureAttachment(Width, Height);
            }
            else
            {
                depthBufferId = createDepthBufferAttachment(Width, Height);
                TextureDepthId = createDepthTextureAttachment(Width, Height);
            }
        }

        private int createFrameBuffer()
        {
           
            var fboId = GL.GenFramebuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, fboId);

            if (Name == FboName.MultipleTargets)
            {
                var buffs = new DrawBuffersEnum[] {
                DrawBuffersEnum.ColorAttachment0,
                DrawBuffersEnum.ColorAttachment1
                };
                GL.DrawBuffers(2, buffs);
            }
            else
            {
                GL.DrawBuffer(DrawBufferMode.ColorAttachment0);
            }

            return fboId;
        }

        public virtual void BindFrameBuffer()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, FBOId);
            GL.Viewport(0, 0, Width, Height);
        }

        public void UnbindCurrentBuffer()
        {
            //check status and store it
            ValidateFBO();

            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            //required if we need to render a different window size from the original window
            GL.Viewport(0, 0, Game.Instance.Width, Game.Instance.Height);
            DisplayManager.CurrentBuffer = DisplayManager.RenderBufferType.Normal;
        }

        public virtual void CleanUp()
        {
            BindFrameBuffer(); //ensure you are connected to this frame buffer prior disposing, to avoid disabling the current connected buffer;
            UnbindCurrentBuffer();
            GL.DeleteFramebuffer(FBOId);
            GL.DeleteRenderbuffer(TextureDepthId);
            GL.DeleteTexture(TextureId);
            GL.DeleteTexture(depthBufferId);
            GL.DeleteTexture(Color00BufferId);
            SceneModel.ActiveScene.FBOs.Remove(this);
        }

        public virtual int createTextureAttachment(FramebufferAttachment attachment)
        {
            //reserve a space for a texture
            var textureId = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, textureId);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, Width, Height, 0, PixelFormat.Rgb, PixelType.UnsignedByte, IntPtr.Zero);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);

            //attach this texture to the frame buffer object
            GL.FramebufferTexture(FramebufferTarget.Framebuffer, attachment, textureId, 0);
            return textureId;
        }

        public void WrapeTo(int textureId, TextureWrapMode mode)
        {
            GL.BindTexture(TextureTarget.Texture2D, textureId); ;
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBorderColor, BorderColor.ToFloat());
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapR, (int)mode);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)mode);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)mode);
            GL.BindTexture(TextureTarget.Texture2D, 0); ;
        }

        public virtual int createDepthTextureAttachment(int width, int height)
        {
            var depthTexture = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, depthTexture);
            GL.TexImage2D(TextureTarget.Texture2D, 0,
            PixelInternalFormat.DepthComponent,
            width, height,
                    0,
                    PixelFormat.DepthComponent,
                    PixelType.Float,
                    IntPtr.Zero);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);

            GL.FramebufferTexture(
            FramebufferTarget.Framebuffer,
            FramebufferAttachment.DepthAttachment,
                    depthTexture, 0);

            return depthTexture;
        }

        public virtual int createDepthStencilTextureAttachment(int width, int height)
        {
            var depthTexture = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, depthTexture);
            GL.TexImage2D(TextureTarget.Texture2D, 0,
            PixelInternalFormat.Depth24Stencil8,
            width, height,
                    0,
                    PixelFormat.DepthStencil,
                    PixelType.UnsignedInt248,
                    IntPtr.Zero);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);

            GL.FramebufferTexture(
            FramebufferTarget.Framebuffer,
            FramebufferAttachment.DepthStencilAttachment,
                    depthTexture, 0);

            return depthTexture;
        }

        private int createDepthBufferAttachment(int width, int height)
        {
            var depthBuffer = GL.GenRenderbuffer();
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, depthBuffer);
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer,
            RenderbufferStorage.DepthComponent, width, height);
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer,
            FramebufferAttachment.DepthAttachment,
            RenderbufferTarget.Renderbuffer, depthBuffer);
            return depthBuffer;
        }

        private int createMultiSampleColorAttachment(int width, int height, FramebufferAttachment colorAttachment)
        {
            var colourBuffer = GL.GenRenderbuffer();
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, colourBuffer);
            GL.NamedRenderbufferStorageMultisample(colourBuffer, 4,
            RenderbufferStorage.Rgba8, width, height);

            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer,
            colorAttachment,
            RenderbufferTarget.Renderbuffer, colourBuffer);
            return colourBuffer;
        }

        private int createDepthStencilBufferAttachment(int width, int height)
        {
            var depthBuffer = GL.GenRenderbuffer();
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, depthBuffer);
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer,
            RenderbufferStorage.Depth24Stencil8,
            width,
                    height);

            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer,
            FramebufferAttachment.DepthStencilAttachment,
            RenderbufferTarget.Renderbuffer, depthBuffer);
            return depthBuffer;
        }

        public virtual void ClearFrame()
        {
            GL.ClearColor(.2f, .2f, .2f, 1);

            GL.Clear(
            ClearBufferMask.ColorBufferBit
            | ClearBufferMask.DepthBufferBit
            | ClearBufferMask.StencilBufferBit
            );
        }

        public virtual void PreRender(Shader ShaderModel)
        {
        }

        public abstract void RenderFrame(IDrawable model);

        public abstract void Live_Update(Shader ShaderModel);

        public abstract void PostRender(Shader ShaderModel);

        public abstract void RenderFrame(List<IDrawable> models);
    }
}