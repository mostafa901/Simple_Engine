using com.sun.tools.javac.jvm;
using Simple_Engine.Views.ThreeD.Engine.Core.Interfaces;
using Simple_Engine.Views.ThreeD.Engine.Render;
using Simple_Engine.Views.ThreeD.Engine.Render.Texture;
using Simple_Engine.Views.ThreeD.Engine.Space;
using Simple_Engine.Views.ThreeD.Engine.Water.Render;
using Simple_Engine.Views.ThreeD.ToolBox;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Engine.Views.ThreeD.Engine.Illumination.Render
{
    public class Shadow_FBO : FBO
    {
        private Shadow_Shader shadow_shader;

        public Light SunLight { get; }

        public Shadow_FBO(Light sunLight, int _width, int _height) : base(_width, _height)
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
            SunLight.lightCamera.Width = SunLight.lightCamera.Height = width;

            SunLight.lightCamera.Activate_Ortho();

            SunLight.lightCamera.Target = camTarget;
            SunLight.lightCamera.Position = camTarget + SunLight.lightCamera.Direction * 50;

            SunLight.lightCamera.UpdateCamera();
        }

        public override void PreRender(Shader ShaderModel)
        {
            updatePosition();
        }

        public override void RenderFrame(IDrawable model)
        {

        }

        public override void Live_Update(Shader ShaderModel)
        {
        }

        public override void PostRender(Shader ShaderModel)
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