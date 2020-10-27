using Simple_Engine.Views.ThreeD.Engine.Core.Abstracts;
using Simple_Engine.Views.ThreeD.Engine.Core.Interfaces;
using Simple_Engine.Views.ThreeD.Engine.Fonts;
using Simple_Engine.Views.ThreeD.Engine.Geometry.Core;
using Simple_Engine.Views.ThreeD.Engine.Geometry.Render;
using Simple_Engine.Views.ThreeD.Engine.GUI.Render;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Engine.Views.ThreeD.Engine.Geometry.TwoD
{
    public class Plan2D : Base_Geo2D
    {
        public Plan2D(float width)
        {
            SetWidth(width);
            SetHeight(width);
        }

        public override void BuildModel()
        {
            Build_DefaultModel();
        }

        public override void RenderModel()
        {
            Renderer = new GUIRenderer(this);
            Default_RenderModel();
        }

        public override void Setup_Indeces()
        {
            throw new NotImplementedException();
        }

        public override void Setup_Normals()
        {
            throw new NotImplementedException();
        }

        public override void Setup_Position()
        {
            throw new NotImplementedException();
        }

        public override void Setup_TextureCoordinates(float xScale = 1, float yScale = 1)
        {
            throw new NotImplementedException();
        }
    }
}