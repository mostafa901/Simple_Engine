using OpenTK;
using OpenTK.Graphics.OpenGL;
using Simple_Engine.Engine.Core.Abstracts;
using Simple_Engine.Engine.Core.Interfaces;
using Simple_Engine.Engine.Geometry.ThreeDModels;
using Simple_Engine.Engine.Geometry.TwoD;
using Simple_Engine.Engine.Render;
using Simple_Engine.Engine.Space.Scene;
using Simple_Engine.Engine.Water.Render;

namespace Simple_Engine.Engine.Water
{
    public class WaterModel : Plan2D, ISelectable
    {
        public WaterModel(float width) : base(width)
        {
        }

        //todo: try this method
        //https://blog.bonzaisoftware.com/tnp/gl-water-tutorial/
        public override void BuildModel()
        {
            Build_DefaultModel();
            CullMode = CullFaceMode.Back;

            MoveTo(new Vector3(GetWidth(), 0, GetHeight()));
            Rotate(90, new Vector3(1, 0, 0));
            DefaultColor = new Vector4(.1f, .1f, 1, 1);
            ShaderModel = new WaterShader(ShaderMapType.LoadColor, ShaderPath.Water);
            TextureModel = new WaterTexture(TextureMode.Texture2D);
            SceneModel.ActiveScene.FBOs.ForEach(o => o.StenciledModel = this);
            Material = new WaterMaterial();
            Material.Glossiness = new Opticals.Gloss(.6f, 20f);
            AllowReflect = true;
            IsBlended = true;
        }

        public override void UploadVAO()
        {
            Renderer = new WaterRenderer(this);
            Renderer.RenderModel();
            ShaderModel.UploadDefaults(this);
        }

        
    }
}