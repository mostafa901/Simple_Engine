using OpenTK.Graphics.OpenGL;
using Simple_Engine.Engine.Core.Abstracts;
using Simple_Engine.Engine.Render;
using Simple_Engine.Extentions;
using System;
using System.Linq;

namespace Simple_Engine.Engine.Particles.Render
{
    public class ParticleRenderer : EngineRenderer
    {
        private Base_Geo2D particleModel;

        public ParticleRenderer(Base_Geo2D _model) : base(_model)
        {
            particleModel = _model;
        }

        public override void DrawModel()
        {
            GL.DrawElementsInstanced(geometryModel.DrawType, geometryModel.Indeces.Count, DrawElementsType.UnsignedInt, IntPtr.Zero, particleModel.Meshes.Count);
        }

        public override void EndDraw()
        {
            GL.DisableVertexAttribArray(PositionLocation);
            GL.DisableVertexAttribArray(1);
            foreach (var i in MatrixLocations)
            {
                GL.DisableVertexAttribArray(i);
            }

            //  GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusDstAlpha); //how Blending should work in this Scene

            GL.DepthMask(true);
            GL.BindVertexArray(0);
        }

        public override void PreDraw()
        {
            base.PreDraw();
            GL.BindVertexArray(VAO);
            GL.EnableVertexAttribArray(PositionLocation);
            GL.EnableVertexAttribArray(1);
            foreach (var i in MatrixLocations)
            {
                GL.EnableVertexAttribArray(i);
            }

            GL.DepthMask(false);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.One); //how Blending should work in this Scene
            particleModel.Live_Update(particleModel.GetShaderModel());
            UploadMeshes(InstancesMatrix, particleModel.Meshes);
            UploadTextureOffset(3);
            UploadBlend(4);
        }

        private void UploadBlend(int vboIndex)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBOs.ElementAt(vboIndex));  //define the type of buffer in the GPU
            var dataArray = particleModel.Meshes.Select(o => ((ParticleMesh)o).BlendValue).ToArray();

            //now stream these vertex (array type) to the located buffer in the GPU
            GL.BufferData(BufferTarget.ArrayBuffer, particleModel.Meshes.Count * sizeof(float), dataArray, BufferUsageHint.DynamicDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        private void UploadTextureOffset(int vboIndex)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBOs.ElementAt(vboIndex));  //define the type of buffer in the GPU
            var dataArray = particleModel.Meshes.Select(o => ((ParticleMesh)o).TextureOffset).ToArray();

            //now stream these vertex (array type) to the located buffer in the GPU
            GL.BufferData(BufferTarget.ArrayBuffer, particleModel.Meshes.Count * sizeof(float) * 4, dataArray, BufferUsageHint.DynamicDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        private int textureOffsetLocation;
        private int blendLocation;

        public override void RenderModel()
        {
            VAO = CreateVAO();
            BindIndicesBuffer(particleModel.Indeces.ToArray());
            StoreDataInAttributeList(PositionLocation, particleModel.Positions.GetArray(), 2, 0);
            StoreDataInAttributeList(TextureLocation, particleModel.TextureCoordinates.GetArray(), 2, 0);
            //StoreDataInAttributeList(NormalLocation, particleModel.Normals.GetArray(), 2, 0);
            StoreDataInAttributeList(InstancesMatrix, MatrixLocations, 1, 4, 4);

            textureOffsetLocation = InstancesMatrix + MatrixLocations.Count;
            StoreDataInAttributeList(textureOffsetLocation, MatrixLocations, 1, 1, 4); //textureoffset1,textureoffset2

            blendLocation = InstancesMatrix + MatrixLocations.Count;
            StoreDataInAttributeList(blendLocation, MatrixLocations, 1, 1, 1); //blend

            GL.BindVertexArray(0);
        }
    }
}