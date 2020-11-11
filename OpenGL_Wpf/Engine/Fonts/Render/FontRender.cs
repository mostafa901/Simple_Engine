using OpenTK.Graphics.OpenGL;
using Simple_Engine.Engine.Core.Interfaces;
using Simple_Engine.Engine.Render;
using Simple_Engine.Extentions;
using System.Collections.Generic;

namespace Simple_Engine.Engine.Fonts
{
    public class FontRender : EngineRenderer
    {
        private int PositionLocation;
        private int TextureLocation;
        public List<int> VBOs { get; set; } = new List<int>();
        public int EBO { get; set; }
        private int VAO { get; set; }
        private CharacterModel model { get; set; }

        public FontRender(IDrawable2D model) : base(model)
        {
            geometryModel.CullMode = CullFaceMode.Back;
        }

        public override void RenderModel()
        {
            model = geometryModel as CharacterModel;
            PositionLocation = model.GetShaderModel().PositionLayoutId;
            TextureLocation = model.GetShaderModel().TextureLayoutId;

            VAO = CreateVAO();

            VBOs.Add(StoreDataInAttributeList(PositionLocation, model.Positions.GetArray(), 2));
            VBOs.Add(StoreDataInAttributeList(TextureLocation, model.TextureCoordinates.GetArray(), 2));

            GL.BindVertexArray(0);
        }

        public override void PreDraw()
        {
            base.PreDraw();
            GL.BindVertexArray(VAO);//access to memory location
            GL.EnableVertexAttribArray(PositionLocation);//position
            GL.EnableVertexAttribArray(TextureLocation);//texture
            DisableCulling();
        }

        public override void DrawModel()
        {
            model.Live_Update(model.GetShaderModel());
            GL.DrawArrays(PrimitiveType.TriangleStrip, 0, model.Positions.Count);
        }

        public override void EndDraw()
        {
            GL.DisableVertexAttribArray(PositionLocation);
            GL.DisableVertexAttribArray(TextureLocation);
            GL.BindVertexArray(0);
        }

        public override void Dispose()
        {
            GL.DeleteVertexArray(VAO);

            foreach (int vbo in VBOs)
            {
                GL.DeleteBuffer(vbo);
            }
            VBOs.Clear();
        }
    }
}