using OpenTK.Graphics.OpenGL;
using Simple_Engine.Engine.Core.Abstracts;
using Simple_Engine.Engine.Render;
using Simple_Engine.Extentions;

namespace Simple_Engine.Engine.GUI.Render
{
    public class GUIRenderer : EngineRenderer
    {
        public Base_Geo2D Model { get; }

        public GUIRenderer(Base_Geo2D _model) : base(_model)
        {
            Model = _model;
        }

        public override void EndDraw()
        {
            GL.DisableVertexAttribArray(PositionLocation);
            GL.DisableVertexAttribArray(TextureLocation);
            GL.DisableVertexAttribArray(NormalLocation);
            GL.BindVertexArray(0);

            GL.Enable(EnableCap.DepthTest);
        }

        public override void PreDraw()
        {
            base.PreDraw();
            GL.BindVertexArray(VAO);
            GL.EnableVertexAttribArray(PositionLocation);
            GL.EnableVertexAttribArray(TextureLocation);
            GL.EnableVertexAttribArray(NormalLocation);
            DisableCulling();
            GL.Disable(EnableCap.DepthTest);
        }

        public override void RenderModel()
        {
            VAO = CreateVAO();
            StoreDataInAttributeList(Get_VBO_Position(), PositionLocation, Model.Positions.GetArray(), 2);
            StoreDataInAttributeList(Get_VBO_Texture(), TextureLocation, Model.TextureCoordinates.GetArray(), 2);
            StoreDataInAttributeList(Get_VBO_Normals(), NormalLocation, Model.Normals.GetArray(), 3);
            GL.BindVertexArray(0);
        }

        public override void DrawModel()
        {
            //PreDraw();
            GL.DrawArrays(Model.DrawType, 0, Model.Positions.Count);
            //EndDraw();
        }
    }
}