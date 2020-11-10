using Simple_Engine.Engine.GUI;

namespace Simple_Engine.Engine.Space.Render.PostProcess
{
    public class PostRender_Effects
    {
        public GuiModel vision;
        public bool IsActive = false;

        public PostRender_Effects(PostProcess_Shader.PostProcessName effectName)
        {
            vision = new GUI.GuiModel(1, 1, 0, 0);
            vision.BuildModel();
            vision.SetShaderModel(new PostProcess_Shader(effectName));
            vision.TextureModel = new PostProcess_Texture(Core.Abstracts.TextureMode.Texture2D);
            vision.UploadVAO();
        }

        public void ProcessEffect(int textureId)
        {
            vision.TextureModel.TextureId = textureId;
            if (!IsActive) return;

            vision.PrepareForRender(vision.GetShaderModel());

            vision.Renderer.Draw();
            vision.GetShaderModel().Stop();
        }
    }
}