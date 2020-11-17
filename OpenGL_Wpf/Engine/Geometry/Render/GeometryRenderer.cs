using OpenTK.Graphics.OpenGL;
using Simple_Engine.Engine.Core.Abstracts;
using Simple_Engine.Engine.Core.Interfaces;
using Simple_Engine.Engine.Render;
using Simple_Engine.Extentions;
using System;
using System.Linq;

namespace Simple_Engine.Engine.Geometry.Render
{
    public class GeometryRenderer : EngineRenderer
    {
        public Base_Geo3D Model { get; }

        public GeometryRenderer(Base_Geo3D _model) : base(_model)
        {
            Model = _model;
        }

        public override void RenderModel()
        {
            VAO = CreateVAO();
            BindIndicesBuffer(((IDrawable3D)Model).Indeces.ToArray());

            StoreDataInAttributeList(Get_VBO_Position(), PositionLocation, ((IDrawable3D)Model).Positions.GetArray(), 3);
            PositionBufferLength = ((IDrawable3D)Model).Positions.Count;

            StoreDataInAttributeList(Get_VBO_Texture(), TextureLocation, Model.TextureCoordinates.GetArray(), 2);
            StoreDataInAttributeList(Get_VBO_Normals(), NormalLocation, Model.Normals.GetArray(), 3);
            if (geometryModel.GetShaderModel().EnableInstancing)
            {
                if (Model.Meshes.Any())
                {
                    StoreDataInAttributeList(InstancesMatrix, MatrixLocations, 1, 4, 4);
                }

                StoreDataInAttributeList(Get_VBO_Instance(), InstancesSelectedLocation, Model.Meshes.Select(o => (float)Convert.ToInt32(o.Selected)).ToArray(), 1, 1);
            }
            else
            {
                StoreDataInAttributeList(Get_VBO_Instance(), InstancesSelectedLocation, new float[] { (float)Convert.ToInt32(Model.GetSelected()) }, 1);
            }
            if (Model.NormalTangent.Any())
            {
                EnableNormalTangent = true;
                StoreDataInAttributeList(Get_VBO_Tangent(), TangentsLocation, Model.NormalTangent.GetArray(), 3);
            }

            GL.BindVertexArray(0);
        }

        public override void PreDraw()
        {
            base.PreDraw();
            GL.BindVertexArray(VAO);//access to memory location
            GL.EnableVertexAttribArray(PositionLocation);//position
            if (geometryModel.Dynamic.HasFlag(IDrawable.DynamicFlag.Positions))
            {
                StoreDataInAttributeList(Get_VBO_Position(), PositionLocation, ((IDrawable3D)Model).Positions.GetArray(), 3);
            }
            GL.EnableVertexAttribArray(TextureLocation);//texture
            GL.EnableVertexAttribArray(NormalLocation);//normal

            if (EnableNormalTangent)
            {
                GL.EnableVertexAttribArray(TangentsLocation);
            }

            if (Model.VertixColor.Any())
            {
                GL.EnableVertexAttribArray(VertexColorLocation);
            }

            if (geometryModel.GetShaderModel().EnableInstancing)
            {
                GL.EnableVertexAttribArray(InstancesSelectedLocation);//ISSelected

                for (int i = 0; i < MatrixLocations.Count; i++)
                {
                    GL.EnableVertexAttribArray(MatrixLocations[i]); //Matrix Rows
                }

                UploadMeshes(3, Model.Meshes);
                UpdateSelectedMeshes(4, Model.Meshes);
            }

            EnableCulling();
        }

        public override void DrawModel()
        {
            if (Model.Meshes.Any())
            {
                GL.DrawElementsInstanced(
                                       //  PrimitiveType.Triangles, //this doesn't work with instancing
                                       BeginMode.Triangles,
                                        ((IDrawable3D)Model).Indeces.Count,
                                        DrawElementsType.UnsignedInt,
                                        // ((IDrawable3D) Model).Indeces.ToArray(),
                                        IntPtr.Zero,
                                        Model.Meshes.Count
                                        );
            }
            else if (EBO != -1)
            {
                GL.DrawElements(Model.DrawType, IndexBufferLength, DrawElementsType.UnsignedInt, 0);
            }
            else
            {
                GL.DrawArrays(Model.DrawType, 0, PositionBufferLength);
            }
        }

        public override void EndDraw()
        {
            GL.DisableVertexAttribArray(PositionLocation);
            GL.DisableVertexAttribArray(TextureLocation);
            GL.DisableVertexAttribArray(NormalLocation);
            GL.DisableVertexAttribArray(InstancesSelectedLocation);
            GL.DisableVertexAttribArray(TangentsLocation);
            GL.DisableVertexAttribArray(VertexColorLocation);

            for (int i = 0; i < MatrixLocations.Count; i++)
            {
                GL.DisableVertexAttribArray(MatrixLocations[i]);
            }
            GL.BindVertexArray(0);

            DisableCulling();
            Model.PostRender(Model.GetShaderModel());
            DisableBlending();
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}