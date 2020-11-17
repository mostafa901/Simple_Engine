using Simple_Engine.Engine.Space.Render.PostProcess;
using Simple_Engine.Engine.Water.Render;

namespace Simple_Engine.Engine.GameSystem
{
    public partial class Game
    {
        public FBO_Texture texture_FBO;
        public FBO_MTargets mTargets_FBO;
        public PostRender_Effects sepiaEffect;
        public PostRender_Effects contrastEffect;
        public PostRender_Effects hBlureEffect;

        private void SetupFBOs()
        {
            contrastEffect = new PostRender_Effects(PostProcess_Shader.PostProcessName.Contrast);
            sepiaEffect = new PostRender_Effects(PostProcess_Shader.PostProcessName.Sepia);
            hBlureEffect = new PostRender_Effects(PostProcess_Shader.PostProcessName.hBlure);
            texture_FBO = new FBO_Texture(Width, Height);
            mTargets_FBO = new FBO_MTargets(Width, Height);
        }

        private void Render_PostProcess(int TextureId)
        {
            contrastEffect.ProcessEffect(TextureId);
            hBlureEffect.ProcessEffect(TextureId);
            sepiaEffect.ProcessEffect(TextureId);
        }
    }
}