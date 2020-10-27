using InSitU.Views.ThreeD.Engine.Core.Interfaces;
using InSitU.Views.ThreeD.Engine.Geometry.TwoD;
using InSitU.Views.ThreeD.Engine.GUI;
using InSitU.Views.ThreeD.Engine.Render;
using InSitU.Views.ThreeD.Engine.Water.Render;
using java.beans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InSitU.Views.ThreeD.Engine.Space.Render.PostProcess
{
    public class PostRender_Effects
    {
        private GuiModel vision;

        public PostRender_Effects(PostProcess_Shader.PostProcessName effectName)
        {
            vision = new GUI.GuiModel(1, 1, 0, 0);
            vision.BuildModel();
            vision.ShaderModel = new PostProcess_Shader(effectName);
            vision.TextureModel = new PostProcess_Texture(Core.Abstracts.TextureMode.Texture2D);
            vision.RenderModel();
        }

        public void ProcessEffect(int textureId)
        {
            vision.TextureModel.TextureId = textureId;
            vision.PrepareForRender(vision.ShaderModel);
            vision.Renderer.Draw();
            vision.ShaderModel.Stop();
        }
    }
}