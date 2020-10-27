using InSitU.Views.ThreeD.Engine.Space.Render.PostProcess;
using InSitU.Views.ThreeD.Engine.Water.Render;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InSitU.Views.ThreeD.Engine.GameSystem
{
    public class Game_FBOs
    {
        public FBO_Texture texture_FBO;
        public FBO_MTargets mTargets_FBO;
        public PostRender_Effects sepiaEffect;
        public PostRender_Effects contrastEffect;
        public PostRender_Effects hBlureEffect;
        private Game game;

        public Game_FBOs(Game game)
        {
            this.game = game;
            contrastEffect = new PostRender_Effects(PostProcess_Shader.PostProcessName.Contrast);
            sepiaEffect = new PostRender_Effects(PostProcess_Shader.PostProcessName.Sepia);
            hBlureEffect = new PostRender_Effects(PostProcess_Shader.PostProcessName.hBlure);
            texture_FBO = new FBO_Texture(game.Width, game.Height);
            mTargets_FBO = new FBO_MTargets(game.Width, game.Height);
        }
    }
}
