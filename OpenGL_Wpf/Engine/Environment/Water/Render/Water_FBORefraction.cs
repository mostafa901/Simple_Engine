using OpenTK.Graphics.OpenGL;
using Simple_Engine.Engine.Render;

namespace Simple_Engine.Engine.Water.Render
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