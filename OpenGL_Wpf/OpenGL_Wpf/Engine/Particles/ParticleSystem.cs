using InSitU.Views.ThreeD.Engine.Core.Interfaces;
using InSitU.Views.ThreeD.Engine.Render;
using OpenTK;
using Shared_Lib;
using Shared_Lib.Extention;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InSitU.Views.ThreeD.Engine.Particles.Render
{
    public static class ParticleSystem
    {


        public static void Draw(Space.Scene scene, IDrawable sourceModel)
        {
            foreach (var model in sourceModel.Particles)
            {
                if (model.Meshes.Any())
                {
                    model.ShaderModel.Use();
                    scene.Live_Update(model.ShaderModel);
                    model.Meshes.RemoveAll(o => ((ParticleMesh)o).Live_Update());
                    model.Renderer.Draw();
                    model.ShaderModel.Stop();
                }
            }
        } 
    }
}