using Simple_Engine.Views.ThreeD.Engine.Core.Interfaces;
using Simple_Engine.Views.ThreeD.Engine.Render;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Effects;

namespace Simple_Engine.Views.ThreeD.Engine.Geometry.ThreeDModels.Cube.Render
{
    public class CubeShader : Shader
    {
        public CubeShader(ShaderMapType shaderType = ShaderMapType.LoadCubeTexture) : base(shaderType, ShaderPath.Cube)
        {
        }

        public override void Live_Update()
        {
            base.Live_Update();
            SetInt(Location_ShaderType, (int)ShaderType);
        }
    }
}