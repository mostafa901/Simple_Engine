using OpenTK;
using Shared_Lib;
using Simple_Engine.Engine.Core.Abstracts;
using Simple_Engine.Engine.Geometry.Core;
using Simple_Engine.Engine.Particles.Render;
using Simple_Engine.Engine.Render;
using System;
using System.Collections.Generic;

namespace Simple_Engine.Engine.Particles
{
    public class ParticleModel : Base_Geo2D
    {
        public List<ParticleModel> Particles { get; set; }

        public float GravityEffect = 0;

        public ParticleModel()
        {
            BuildModel();
            ShaderModel = new ParticleShader(ShaderMapType.Texture, ShaderPath.Particle);
            ShaderModel.EnableInstancing = true;
            TextureModel = new ParticleTexture(@"D:\Revit_API\Projects\Simple_Engine\OpenGL_Wpf\SampleModels\Texture\FireAtlas.png", TextureMode.Texture2D);

            Meshes = new List<Mesh3D>();

            UploadVAO();
        }

        public override void BuildModel()
        {
            Build_DefaultModel();
        }

        public override void Live_Update(Shader ShaderModel)
        {
            ShaderModel.SetVector4(ShaderModel.Location_DefaultColor, DefaultColor);
            TextureModel?.Live_Update(ShaderModel);
        }

        public override void UploadVAO()
        {
            Renderer = new ParticleRenderer(this);
            Renderer.RenderModel();
            ShaderModel.UploadDefaults(this);
        }

        public override void UploadDefaults(Shader ShaderModel)
        {
            ShaderModel.SetMatrix4(ShaderModel.Location_LocalTransform, LocalTransform);
            ShaderModel.SetVector4(ShaderModel.Location_DefaultColor, DefaultColor);
        }

        public void AddParticles(Vector3 pos, int count)
        {
            for (int i = 0; i < count; i++)
            {
                AddInstance(pos);
            }
        }

        private void AddInstance(Vector3 pos)
        {
            var mesh = new ParticleMesh(this);
            mesh.BuildMesh(pos, new Vector3(Randoms.Next(-20, 20), Randoms.Next(-5, 5), Randoms.Next(-30, 30)), .01f, Randoms.Next(1, 80), 1, 2f);

            Meshes.Add(mesh);
        }

        public override void Setup_Position()
        {
        }

        public override void Setup_TextureCoordinates(float xScale = 1, float yScale = 1)
        {
        }

        public override void Setup_Normals()
        {
        }

        public override void Setup_Indeces()
        {
        }

        public override List<face> generatefaces()
        {
            throw new NotImplementedException();
        }
    }
}