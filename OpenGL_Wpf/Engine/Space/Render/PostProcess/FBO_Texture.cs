using Simple_Engine.Engine.Core.Interfaces;
using Simple_Engine.Engine.Render;
using Simple_Engine.Engine.Water.Render;
using System;
using System.Collections.Generic;

namespace Simple_Engine.Engine.Space.Render.PostProcess
{
    public class FBO_Texture : FBO
    {
        public FBO_Texture(int _width, int _height) : base(_width, _height)
        {
            Setup_Defaults(false);
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
        }
    }
}