using OpenTK;
using OpenTK.Graphics.OpenGL;
using Simple_Engine.Engine.Core.Interfaces;
using Simple_Engine.Engine.Render;
using Simple_Engine.Engine.Render.ShaderSystem;
using Simple_Engine.Engine.Space.Camera;
using Simple_Engine.Engine.Water.Render;
using System.Collections.Generic;

namespace Simple_Engine.Engine.Illumination.Render
{
    public class Shadow_FBO : FBO
    {
        private Shadow_Shader shadow_shader;

        public LightModel SunLight { get; }

        public Shadow_FBO(LightModel sunLight, int _width, int _height) : base(_width, _height)
        {
            SunLight = sunLight;
            Name = FboName.Shadow;

            Setup_Defaults(false);

            sunLight.SetShadowTransform();
            sunLight.ShadowMapId = TextureDepthId;

            shadow_shader = new Shadow_Shader();
            shadow_shader.UploadDefaults(null);
            //change vector4 to 0 to see where the border of the texture map
            BorderColor = new Vector4(1);
            WrapeTo(TextureDepthId, TextureWrapMode.ClampToBorder);
            WrapeTo(TextureId, TextureWrapMode.ClampToBorder);
        }

        private void updatePosition()
        {
            var cam = CameraModel.ActiveCamera;
            Vector3 camTarget = cam.Target;

            float camera_Distance = (cam.Target - cam.Position).Length;
            float camera_FarWidth = (cam.Position - cam.Target).Length;
            float camera_NearWidth = cam.NearDistance;

            //the below is increase the width/heigh of the texture map depending how perpendicular the live camera to the light camera. clamping the result between 50/200 since this is tested to get an acceptable result for a bitmap of size 1024*1024
            var ratio = Vector3.Dot(SunLight.lightCamera.Direction, cam.Direction);
            var width = MathHelper.Clamp((1 - ratio) * 50 * 4, 50, 200);
            SunLight.lightCamera.SetHeight(width);

            SunLight.lightCamera.Activate_Ortho();

            SunLight.lightCamera.Target = camTarget;
            SunLight.lightCamera.Position = camTarget + SunLight.lightCamera.Direction * 50;

            SunLight.lightCamera.UpdateCamera();
        }

        public override void PreRender(Base_Shader ShaderModel)
        {
            updatePosition();
        }

        public override void RenderFrame(IDrawable model)
        {
        }

        public override void Live_Update(Base_Shader ShaderModel)
        {
        }

        public override void PostRender(Base_Shader ShaderModel)
        {
        }

        public override void RenderFrame(List<IDrawable> models)
        {
            if (!SunLight.CastShadow) return;
            PreRender(shadow_shader);

            foreach (var model in models)
            {
                if (!model.CastShadow)
                {
                    continue;
                }

                model.PrepareForRender(shadow_shader);
                model.Renderer.Draw();
                shadow_shader.Stop();
            }
            PostRender(shadow_shader);
        }
    }
}