using Simple_Engine.Views.ThreeD.Engine.Core.Interfaces;
using Simple_Engine.Views.ThreeD.Engine.Geometry.TwoD;
using Simple_Engine.Views.ThreeD.Engine.Render;
using Simple_Engine.Views.ThreeD.Engine.Water.Render;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Engine.Views.ThreeD.Engine.Space.Render.PostProcess
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