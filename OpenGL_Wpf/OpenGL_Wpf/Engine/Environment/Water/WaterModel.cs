using InSitU.Views.ThreeD.Engine.Core.Abstracts;
using InSitU.Views.ThreeD.Engine.Core.Interfaces;
using InSitU.Views.ThreeD.Engine.Geometry.Core;
using InSitU.Views.ThreeD.Engine.Core.Events;
using InSitU.Views.ThreeD.Engine.Particles;
using InSitU.Views.ThreeD.Engine.Render;
using InSitU.Views.ThreeD.Engine.Water.Render;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InSitU.Views.ThreeD.Engine.Water
{
    public class WaterModel : Base_Geo2D
    {
        //todo: try this method
        //https://blog.bonzaisoftware.com/tnp/gl-water-tutorial/
        public override void BuildModel()
        {
            SetHeight(50);
            SetWidth(50);
            Build_DefaultModel();
            CullMode = CullFaceMode.Back;

            MoveTo(new Vector3(GetWidth(), 0, GetHeight()));
            Rotate(90, new Vector3(1, 0, 0));
            DefaultColor = new Vector4(.1f, .1f, 1, 1);
            ShaderModel = new WaterShader(ShaderMapType.LoadColor, ShaderPath.Water);
            TextureModel = new WaterTexture(TextureMode.Texture2D);
            Game.Context.ActiveScene.FBOs.ForEach(o => o.StenciledModel = this);
            Material = new WaterMaterial();
            Material.Glossiness = new Opticals.Gloss(.6f, 20f);
            AllowReflect = true;
            IsBlended = true;
        }

        public override void RenderModel()
        {
            Renderer = new WaterRenderer(this);
            Renderer.RenderModel();
            ShaderModel.UploadDefaults(this);
        }

        public override void Setup_Indeces()
        {
            throw new NotImplementedException();
        }

        public override void Setup_Normals()
        {
            throw new NotImplementedException();
        }

        public override void Setup_Position()
        {
            throw new NotImplementedException();
        }

        public override void Setup_TextureCoordinates(float xScale = 1, float yScale = 1)
        {
            throw new NotImplementedException();
        }
    }
}