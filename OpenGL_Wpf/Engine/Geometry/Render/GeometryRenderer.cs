using Simple_Engine.Engine.Core.Abstracts;
using Simple_Engine.Engine.Core.Interfaces;
using Simple_Engine.Engine.Geometry.Core;

using Simple_Engine.Engine.Primitives;
using Simple_Engine.Engine.Render;
using Simple_Engine.Engine.Space;
using Simple_Engine.Extentions;
using Simple_Engine.ToolBox;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Platform.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using static Simple_Engine.Engine.Core.Abstracts.Base_Geo;

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

            StoreDataInAttributeList(PositionLocation, ((IDrawable3D)Model).Positions.GetArray(), 3);
            
            StoreDataInAttributeList(TextureLocation, Model.TextureCoordinates.GetArray(), 2);
            StoreDataInAttributeList(NormalLocation, Model.Normals.GetArray(), 3);
            if (geometryModel.ShaderModel.EnableInstancing)
            {
                if (Model.Meshes.Any())
                {
                    StoreDataInAttributeList(InstancesMatrix, MatrixLocations, 1, 4, 4);
                }

                StoreDataInAttributeList(InstancesSelectedLocation, Model.Meshes.Select(o => (float)Convert.ToInt32(o.Selected)).ToArray(), 1, 1);
            }
            else
            {
                StoreDataInAttributeList(InstancesSelectedLocation,new float[] { (float) Convert.ToInt32(Model.GetSelected()) }, 1);
            }
            if (Model.NormalTangent.Any())
            {
                StoreDataInAttributeList(TangentsLocation, Model.NormalTangent.GetArray(), 3);
            }
            if(Model.VertixColor.Any())
            {
                StoreDataInAttributeList(VertexColorLocation, Model.VertixColor.GetArray(), 4);

            }
            GL.BindVertexArray(0);
        }

        public override void PreDraw()
        {
            base.PreDraw();
            GL.BindVertexArray(VAO);//access to memory location
            GL.EnableVertexAttribArray(PositionLocation);//position
            if(geometryModel.Dynamic.HasFlag(IDrawable.DynamicFlag.Positions))
            {
                StoreDataInAttributeList(PositionLocation, ((IDrawable3D)Model).Positions.GetArray(), 3);

            }
            GL.EnableVertexAttribArray(TextureLocation);//texture
            GL.EnableVertexAttribArray(NormalLocation);//normal

            if (Model.NormalTangent.Any())
            {
                GL.EnableVertexAttribArray(TangentsLocation); 
            }

            if (Model.VertixColor.Any())
            {
                GL.EnableVertexAttribArray(VertexColorLocation); 
            }

            if (geometryModel.ShaderModel.EnableInstancing)
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
            else if (((IDrawable3D)Model).Indeces.Any())
            {
                GL.DrawElements(Model.DrawType, ((IDrawable3D)Model).Indeces.Count, DrawElementsType.UnsignedInt, 0);
            }
            else
            {
                GL.DrawArrays(Model.DrawType, 0, ((IDrawable3D)Model).Positions.Count);
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
            Model.PostRender(Model.ShaderModel);
            DisableBlending();
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}