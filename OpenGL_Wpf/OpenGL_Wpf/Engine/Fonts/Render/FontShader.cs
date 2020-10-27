using InSitU.Views.ThreeD.Engine.Render;
using InSitU.Views.ThreeD.Engine.Space;
using OpenTK.Graphics.ES20;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InSitU.Views.ThreeD.Engine.Fonts
{
    public class FontShader : Shader
    {
        public FontShader(ShaderMapType mapType, ShaderPath shaderType) : base(mapType, shaderType)
        {
            string path = @"./Views/ThreeD/Engine/Fonts/Render/Source/";
            Setup_Shader($"{path}VertexShader_Font.vert", $"{path}FragmentShader_Font.frag");
        }

    

        public override void BindAttributes()
        {
            BindAttribute(PositionLayoutId, "aPosition2");
            BindAttribute(TextureLayoutId, "aTextureCoor");
        }
    }
}