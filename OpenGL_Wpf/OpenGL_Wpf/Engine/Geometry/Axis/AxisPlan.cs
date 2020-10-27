using InSitU.Views.ThreeD.Engine.Core.Abstracts;
using InSitU.Views.ThreeD.Engine.Core.Interfaces;
using InSitU.Views.ThreeD.Engine.Geometry.Axis.Render;
using InSitU.Views.ThreeD.Engine.Geometry.Core;
using InSitU.Views.ThreeD.Engine.Geometry.ThreeDModels;
using InSitU.Views.ThreeD.Engine.Geometry.TwoD;
using InSitU.Views.ThreeD.Engine.Render;
using InSitU.Views.ThreeD.ToolBox;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InSitU.Views.ThreeD.Engine.Geometry.Axis
{
    public class AxisPlan : Plan3D
    {
        public AxisPlan(IDrawable parentGeometry, Vector3 direction, Vector2 size) : base(1)
        {
            ParentGeometry = parentGeometry;
            SetWidth(size.X);
            SetHeight(size.Y);
            Direction = direction;
            DefaultColor = new Vector4(direction, 1);
            LocalTransform = parentGeometry.LocalTransform;
            IsBlended = true;

        }

        public IDrawable ParentGeometry { get; }
        public Vector3 Direction { get; }

        public override void BuildModel()
        {
            base.BuildModel();

            CullMode = CullFaceMode.FrontAndBack;

            
            LocalTransform = eMath.MoveLocal(LocalTransform, ParentGeometry.PivotPoint);

            if (Direction == Vector3.UnitY)
            {
                LocalTransform = eMath.Rotate(LocalTransform, 90, Vector3.UnitX);
            }
            if (Direction == Vector3.UnitZ)
            {
                LocalTransform = eMath.Rotate(LocalTransform, 90, new Vector3(0, 1, 0));
            }
        }

        public override void Live_Update(Shader ShaderModel)
        {
            base.Live_Update(ShaderModel);
            ShaderModel.SetMatrix4(ShaderModel.Location_LocalTransform, LocalTransform);
            GL.BlendFunc(BlendingFactor.One, BlendingFactor.SrcColor);
        }

        public override void RenderModel()
        {
            Renderer = new AxisPlanRenderer(this);
            Renderer.RenderModel();
            ShaderModel.UploadDefaults(this);
        }
    }
}