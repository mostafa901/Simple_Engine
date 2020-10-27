using Simple_Engine.Engine.Render;
using Simple_Engine.Engine.Space;
using OpenTK.Graphics.ES20;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Engine.Engine.Fonts
{
    public class FontShader : Shader
    {
        public FontShader(ShaderMapType mapType, ShaderPath shaderType) : base(mapType, shaderType)
        {
            string path = @"./Engine/Fonts/Render/Source/";
            Setup_Shader($"{path}VertexShader_Font.vert", $"{path}FragmentShader_Font.frag");
        }

    

        public override void BindAttributes()
        {
            BindAttribute(PositionLayoutId, "aPosition2");
            BindAttribute(TextureLayoutId, "aTextureCoor");
        }
    }
}