using OpenTK;
using OpenTK.Graphics.OpenGL;
using Simple_Engine.Engine.Core.Abstracts;
using Simple_Engine.Engine.GUI.Render;
using Simple_Engine.Engine.Render;
using Simple_Engine.Engine.Render.ShaderSystem;
using System;
using System.Collections.Generic;

namespace Simple_Engine.Engine.GUI
{
    public class GuiModel : Base_Geo2D
    {
        public GuiModel(float width, float height, float posX, float posY)
        {
            SetWidth(width);
            SetHeight(height);
            PosX = posX;
            PosY = posY;

            DrawType = OpenTK.Graphics.OpenGL.PrimitiveType.TriangleStrip;
            CullMode = CullFaceMode.FrontAndBack;
        }

        public override void BuildModel()
        {
            Build_DefaultModel();
            MoveWorld(new Vector3(PosX, PosY, 0));
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

        public override void Live_Update(Base_Shader ShaderModel)
        {
            TextureModel?.Live_Update(ShaderModel);
        }

        public override void UploadVAO()
        {
            if (GetShaderModel() == null)
            {
                var shader = new GUIShader(ShaderPath.GUI);
                SetShaderModel(shader);
            }

            Renderer = new GUIRenderer(this);
            Default_RenderModel();
        }

        public override List<face> generatefaces()
        {
            throw new NotImplementedException();
        }

        public float PosX { get; }
        public float PosY { get; }
    }
}