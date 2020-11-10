using OpenTK.Graphics.OpenGL;
using Simple_Engine.Engine.Core.Interfaces;
using Simple_Engine.Engine.Render;
using Simple_Engine.Extentions;

namespace Simple_Engine.Engine.Illumination.Render
{
    public class SkyBoxRenderer : EngineRenderer
    {
        public SkyBoxRenderer(IDrawable3D _model) : base(_model)
        {
        }

        public override void Dispose()
        {
            GL.DeleteBuffer(EBO);
            GL.DeleteVertexArray(VAO);

            foreach (int vbo in VBOs)
            {
                GL.DeleteBuffer(vbo);
            }
            VBOs.Clear();
        }

        public override void PreDraw()
        {
            base.PreDraw();
            GL.BindVertexArray(VAO);//access to memory location
            GL.EnableVertexAttribArray(PositionLocation);//position
            EnableCulling();
        }

        public override void DrawModel()
        {
            GL.DrawElements(geometryModel.DrawType, ((IDrawable3D)geometryModel).Indeces.Count, DrawElementsType.UnsignedInt, 0);
        }

        public override void EndDraw()
        {
            GL.DisableVertexAttribArray(PositionLocation);
            GL.BindVertexArray(0);
            EnableCulling();
        }

        public override void RenderModel()
        {
            PositionLocation = geometryModel.GetShaderModel().PositionLayoutId;

            VAO = CreateVAO();
            BindIndicesBuffer(((IDrawable3D)geometryModel).Indeces.ToArray());

            StoreDataInAttributeList(PositionLocation, ((IDrawable3D)geometryModel).Positions.GetArray(), 3);

            GL.BindVertexArray(0);
        }
    }
}