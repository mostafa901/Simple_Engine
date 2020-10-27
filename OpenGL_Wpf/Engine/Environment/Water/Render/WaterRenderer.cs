using Simple_Engine.Engine.Core.Abstracts;
using Simple_Engine.Engine.Core.Interfaces;
using Simple_Engine.Engine.Render;
using Simple_Engine.Extentions;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Simple_Engine.Engine.Water.Render
{
    internal class WaterRenderer : EngineRenderer
    {
        public Base_Geo2D Model { get; }

        public WaterRenderer(Base_Geo2D _model) : base(_model)
        {
            Model = _model;
        }

        public override void DrawModel()
        {
         
            GL.DrawElements(Model.DrawType, Model.Indeces.Count, DrawElementsType.UnsignedInt, 0);
        }

        public override void EndDraw()
        {
            GL.DisableVertexAttribArray(PositionLocation);
            GL.DisableVertexAttribArray(TextureLocation);
            GL.BindVertexArray(0);
            DisableCulling();
        }

        public override void PreDraw()
        {
            base.PreDraw();
            GL.BindVertexArray(VAO);
            GL.EnableVertexAttribArray(PositionLocation);
            GL.EnableVertexAttribArray(TextureLocation);
            EnableCulling();
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha); //how Blending should work in this Scene
        }

        public override void RenderModel()
        {
            VAO = CreateVAO();
            BindIndicesBuffer(Model.Indeces.ToArray());
            StoreDataInAttributeList(PositionLocation, Model.Positions.GetArray(), 2);
            StoreDataInAttributeList(TextureLocation, Model.TextureCoordinates.GetArray(), 2);

            GL.BindVertexArray(0);
        }

     
    }
}