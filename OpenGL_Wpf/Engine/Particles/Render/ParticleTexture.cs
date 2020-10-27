
using Simple_Engine.Views.ThreeD.Engine.Core.Abstracts;
using Simple_Engine.Views.ThreeD.Engine.Render;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Engine.Views.ThreeD.Engine.Particles.Render
{
    public class ParticleTexture : Base_Texture
    {
        public ParticleTexture(string imgPath, TextureMode textureTargetType) : base(textureTargetType)
        {
            Setup_2DTexture(imgPath, TextureUnit.Texture0);
            numberOfRows = 4;
        }

        
    }
}