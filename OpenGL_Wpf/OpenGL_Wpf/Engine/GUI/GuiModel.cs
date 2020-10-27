using InSitU.Views.ThreeD.Engine.Core.Abstracts;
using InSitU.Views.ThreeD.Engine.Geometry;
using InSitU.Views.ThreeD.Engine.Geometry.Core;
using InSitU.Views.ThreeD.Engine.GUI.Render;
using InSitU.Views.ThreeD.Engine.Render;
using InSitU.Views.ThreeD.ToolBox;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InSitU.Views.ThreeD.Engine.GUI
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

        public override void Live_Update(Shader ShaderModel)
        {
            TextureModel?.Live_Update(ShaderModel);
        }

        public override void RenderModel()
        {
            if (ShaderModel == null)
            {
                ShaderModel = new GUIShader(ShaderMapType.LoadColor, ShaderPath.GUI);
            }

            Renderer = new GUIRenderer(this);
            Default_RenderModel();
        }

        public float PosX { get; }
        public float PosY { get; }
    }
}