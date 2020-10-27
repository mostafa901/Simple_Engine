using Simple_Engine.Engine.Render;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Engine.Engine.GUI.Render
{
    public class GUIShader : Shader
    {
        public GUIShader(ShaderMapType mapType, ShaderPath shaderType) : base(mapType, shaderType)
        {
        }

        public override void BindAttributes()
        {
            BindAttribute(PositionLayoutId, "aPosition2");
            BindAttribute(TextureLayoutId, "aTextureCoor");
            BindAttribute(NormalLayoutId, "aNormals");
        }
    }
}