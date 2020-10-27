using InSitU.Views.ThreeD.Engine.Core.Interfaces;
using InSitU.Views.ThreeD.Engine.Render;
using InSitU.Views.ThreeD.ToolBox;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InSitU.Views.ThreeD.Engine.Water.Render
{
    public class Water_FBORefraction : Water_FBOReflection
    {
        public Water_FBORefraction(int _width, int _height) : base(_width, _height)
        {
            Name = FboName.WorldRefraction;
            WrapeTo(TextureDepthId, TextureWrapMode.ClampToBorder);
            WrapeTo(TextureId, TextureWrapMode.ClampToBorder);
        }

        public override void PreRender(Shader ShaderModel)
        {
            GL.Enable(EnableCap.ClipDistance1);
            ShaderModel.SetVector4(ShaderModel.Location_ClipPlanY, ClipPlan);
        }

        public override void PostRender(Shader ShaderModel)
        {
            GL.Disable(EnableCap.ClipDistance1);
        }
    }
}