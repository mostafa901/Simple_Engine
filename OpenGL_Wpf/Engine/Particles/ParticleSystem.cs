using Simple_Engine.Engine.Core.Interfaces;
using Simple_Engine.Engine.Space.Scene;
using System.Linq;

namespace Simple_Engine.Engine.Particles.Render
{
    public static class ParticleSystem
    {
        public static void Draw(SceneModel scene, IDrawable sourceModel)
        {
            foreach (var model in sourceModel.Particles)
            {
                if (model.Meshes.Any())
                {
                    model.GetShaderModel().Use();
                    scene.Live_Update(model.GetShaderModel());
                    model.Meshes.RemoveAll(o => ((ParticleMesh)o).Live_Update());
                    model.Renderer.Draw();
                    model.GetShaderModel().Stop();
                }
            }
        }
    }
}