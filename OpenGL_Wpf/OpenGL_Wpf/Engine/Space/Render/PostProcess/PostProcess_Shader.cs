using InSitU.Views.ThreeD.Engine.Render;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InSitU.Views.ThreeD.Engine.Space.Render.PostProcess
{
    public class PostProcess_Shader : Shader
    {
        public enum PostProcessName
        {
            Contrast,
            Sepia,
            hBlure
        }

        public PostProcess_Shader(PostProcessName postProcessType) : base(ShaderMapType.LightnColor, ShaderPath.PostProcess)
        {
            var path = @"./Views/ThreeD/Engine/Space/Render/PostProcess/Source/";

            string vertPath = $"{path}PostProcess_Vert.vert";
            string fragPath = "";
            switch (postProcessType)
            {
                case PostProcessName.Contrast:
                    fragPath = $"{path}Contrast_Frag.frag";
                    break;

                case PostProcessName.Sepia:
                    fragPath = $"{path}Sepia_Frag.frag";
                    break;

                case PostProcessName.hBlure:
                    fragPath = $"{path}Blur_Frag.frag";
                    vertPath = $"{path}PostProcess_HBlurVert.vert"; 
                    break;

                default:
                    break;
            }

            Setup_Shader(vertPath, fragPath);
        }
    }
}