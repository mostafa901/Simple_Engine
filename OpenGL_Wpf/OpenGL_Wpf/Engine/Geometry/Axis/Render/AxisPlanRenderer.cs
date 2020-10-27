using InSitU.Views.ThreeD.Engine.Core.Abstracts;
using InSitU.Views.ThreeD.Engine.Core.Interfaces;
using InSitU.Views.ThreeD.Engine.Render;
using InSitU.Views.ThreeD.Extentions;
using InSitU.Views.ThreeD.ToolBox;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InSitU.Views.ThreeD.Engine.Geometry.Axis.Render
{
    public class AxisPlanRenderer : EngineRenderer
    {
        public AxisPlanRenderer(Base_Geo3D _model) : base(_model)
        {
        }

        public override void DrawModel()
        {
            GL.DrawElements(((IDrawable3D)geometryModel).DrawType, ((IDrawable3D)geometryModel).Indeces.Count, DrawElementsType.UnsignedInt, 0);
        }

        public override void EndDraw()
        {
            GL.BindVertexArray(0);
            GL.DisableVertexAttribArray(PositionLocation);
        }

        public override void PreDraw()
        {
            base.PreDraw();
            GL.BindVertexArray(VAO);
            GL.EnableVertexAttribArray(PositionLocation);
            DisableCulling();
        }

        public override void RenderModel()
        {
            VAO = CreateVAO();
            var model3D = (IDrawable3D)geometryModel;
            BindIndicesBuffer(model3D.Indeces.ToArray());
            StoreDataInAttributeList(PositionLocation, model3D.Positions.GetArray(), 3, 0);

            GL.BindVertexArray(0);
        }
    }
}