﻿using Simple_Engine.Engine.Space.Render.PostProcess;
using Simple_Engine.Engine.Water.Render;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Engine.Engine.GameSystem
{
    public partial class Game
    {
        public FBO_Texture texture_FBO;
        public FBO_MTargets mTargets_FBO;
        public PostRender_Effects sepiaEffect;
        public PostRender_Effects contrastEffect;
        public PostRender_Effects hBlureEffect;

        public void SetupFBOs()
        {
            
            contrastEffect = new PostRender_Effects(PostProcess_Shader.PostProcessName.Contrast);
            sepiaEffect = new PostRender_Effects(PostProcess_Shader.PostProcessName.Sepia);
            hBlureEffect = new PostRender_Effects(PostProcess_Shader.PostProcessName.hBlure);
            texture_FBO = new FBO_Texture(Width, Height);
            mTargets_FBO = new FBO_MTargets(Width, Height);
        }
    }
}