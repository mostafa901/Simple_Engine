using InSitU.Views.ThreeD.Engine.Core.Interfaces;
using InSitU.Views.ThreeD.Engine.Geometry.Core;
using InSitU.Views.ThreeD.Engine.Render;
using InSitU.Views.ThreeD.Engine.Space;
using InSitU.Views.ThreeD.Extentions;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InSitU.Views.ThreeD.Engine.Fonts
{
    public class FontRender : EngineRenderer
    {
        private int PositionLocation;
        private int TextureLocation;
        public List<int> VBOs { get; set; } = new List<int>();
        public int EBO { get; set; }
        private int VAO { get; set; }
        private CharacterModel model { get; set; }
        public FontRender(IDrawable2D model):base(model)
        {
           geometryModel. CullMode = CullFaceMode.Back;
        }

        public override void RenderModel()
        {
            model = geometryModel as CharacterModel;
            PositionLocation = model.ShaderModel.PositionLayoutId;
            TextureLocation = model.ShaderModel.TextureLayoutId;

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
            model.Live_Update(model.ShaderModel);           
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