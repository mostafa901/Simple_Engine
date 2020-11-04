using Simple_Engine.Engine.GUI;

namespace Simple_Engine.Engine.Space.Render.PostProcess
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
            vision.UploadVAO();
        }

        public void ProcessEffect(int textureId)
        {
            vision.TextureModel.TextureId = textureId;
            //vision.PrepareForRender(vision.ShaderModel);
            vision.Renderer.Draw();
            vision.ShaderModel.Stop();
        }
    }
}