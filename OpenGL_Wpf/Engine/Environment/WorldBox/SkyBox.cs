using Simple_Engine.Engine.Core.Abstracts;
using Simple_Engine.Engine.Core.Interfaces;
using Simple_Engine.Engine.Geometry;
using Simple_Engine.Engine.Geometry.Core;
using Simple_Engine.Engine.Geometry.Cube;
using Simple_Engine.Engine.Illumination.Render;
using Simple_Engine.Engine.Render;
using Simple_Engine.Engine.Space;
using Simple_Engine.ToolBox;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simple_Engine.Engine.Space.Camera;

namespace Simple_Engine.Engine.Illumination
{
    public class SkyBox : Base_Geo3D
    {
        public float BlendFactor { get; set; } = .2f;

        public SkyBox(CubeModel cube) : base(cube)
        {
            CullMode = CullFaceMode.FrontAndBack;
            IsSystemModel = true;
        }

        private int isDay = 1;

        public override void BuildModel()
        {
            ShaderModel = new Shader(ShaderMapType.LoadCubeTexture, ShaderPath.SkyBox);
            ShaderModel.BrightnessLevels = 10;
            TextureModel = new SkyBoxTexture();
        }

        public override void Setup_Position()
        {
        }

        public override void Setup_TextureCoordinates(float xScale = 1, float yScale = 1)
        {
        }

        public override void Setup_Normals()
        {
        }

        public override void Setup_Indeces()
        {
        }

        public void AnimateBlend()
        {
            Rotate(.01f, new Vector3(0, 1, 0));
            BlendFactor += .001f * isDay;
            BlendFactor = MathHelper.Clamp(BlendFactor, 0, 1);
            if (BlendFactor == 1)
            {
                isDay = -1;
            }
            if (BlendFactor == 0)
            {
                isDay = 1;
            }
        }

        private void AnimateRotation()
        {
            LocalTransform = eMath.MoveTo(LocalTransform, CameraModel.ActiveCamera.Position);
            Rotate(.01f, Vector3.UnitY);
        }

        public override void Live_Update(Shader ShaderModel)
        {
            base.Live_Update(ShaderModel);    
            AnimateBlend();
            AnimateRotation();

            ShaderModel.SetFloat(ShaderModel.BlendFactorLocation, BlendFactor);
            ShaderModel.SetMatrix4(ShaderModel. Location_LocalTransform, LocalTransform);
             
        }

        public override void UploadDefaults(Shader ShaderModel)
        {
           ShaderModel. Location_LocalTransform = ShaderModel.GetLocation(nameof(LocalTransform));
            
            base.UploadDefaults(ShaderModel);
        }
        public override void RenderModel()
        {
            Renderer = new SkyBoxRenderer(this);
            Default_RenderModel();

        }

    }
}