using Simple_Engine.Views.ThreeD.Engine.Core.Abstracts;
using Simple_Engine.Views.ThreeD.Engine.Core.Interfaces;
using Simple_Engine.Views.ThreeD.Engine.Core.Events;
using Simple_Engine.Views.ThreeD.Engine.Geometry.InputControls;
using Simple_Engine.Views.ThreeD.Engine.Geometry.TwoD;
using Simple_Engine.Views.ThreeD.Engine.Particles;
using Simple_Engine.Views.ThreeD.Engine.Render;
using Simple_Engine.Views.ThreeD.Engine.Space;
using Simple_Engine.Views.ThreeD.ToolBox;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using PdfSharp.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Engine.Views.ThreeD.Engine.Geometry.Core
{
    public class Mesh3D : Base_Geo3D, ISelectable
    {
        public IDrawable Parent { get; }

        public List<ParticleModel> Particles { get ; set ; }
        public bool Selected { get; set; }

        public Mesh3D(IDrawable parent)
        {
            model_KeyControl = new KeyControl(this);
            Parent = parent;
            var bbx = parent.BBX;
            SetWidth(bbx.Width);
            SetHeight(bbx.Height);
            SetDepth(bbx.Depth);
            MoveEvent += Mesh_Moving;
            UpdateBoundingBox();
        }

        private void Mesh_Moving(object sender, MoveingEvent e)
        {
            var pos = e.Transform.ExtractTranslation();
            LocalTransform = eMath.MoveTo(LocalTransform, pos);
            UpdateBoundingBox();
        }

        public override void BuildModel()
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

        public override void Setup_Normals()
        {
            throw new NotImplementedException();
        }

        public override void Setup_Indeces()
        {
            throw new NotImplementedException();
        }

        public override void RenderModel()
        {
            throw new NotImplementedException();
        }

        public override void Live_Update(Shader ShaderModel)
        {
            ShaderModel.SetMatrix4(ShaderModel.Location_LocalTransform, LocalTransform);
        }
    }
}