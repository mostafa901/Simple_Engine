using Simple_Engine.Engine.ImGui_Set.Controls;
using Simple_Engine.Engine.Render;
using OpenTK;

using OpenTK;

using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Engine.Engine.Geometry.ThreeDModels.Clips
{

    public class ClipPlan : Plan3D
    {
        //todo: shade the clipped part
        //http://glbook.gamedev.net/GLBOOK/glbook.gamedev.net/moglgp/advclip.html
        public ClipPlan(Vector3 clipDirection, EnableCap clipDistance, float offsetValue) : base(5)
        {
            CanBeSaved = false;

            ClipDirection = clipDirection;
            ClipDistance = clipDistance;
            CullMode = CullFaceMode.FrontAndBack;
            BuildModel();
            DefaultColor = new Vector4(clipDirection, .5f);
            MoveWorld(clipDirection * offsetValue);
            IsBlended = true;
        }

  

        public void SetAsGlobal(bool globalValue)
        {
            if (globalValue)
            {
                if (ClipDirection.X > 0)
                {
                    Shader.ClipPlanX = this;
                }
                else if (ClipDirection.Y > 0)
                {
                    Shader.ClipPlanY = this;
                }
                else if (ClipDirection.Z > 0)
                {
                    Shader.ClipPlanZ = this;
                }
            }
            else
            {
                Shader.ClipPlanX = null;
                Shader.ClipPlanY = null;
                Shader.ClipPlanZ = null;
            }
            Shader.ClipGlobal = globalValue;
        }

        public override void Live_Update(Shader ShaderModel)
        {
            if (ShaderModel == this.ShaderModel)
            {
                base.Live_Update(ShaderModel);

                GL.BlendFunc(BlendingFactor.One, BlendingFactor.SrcColor);
                ShaderModel.SetMatrix4(ShaderModel.Location_LocalTransform, LocalTransform);
            }
            else
            {
                var trans = LocalTransform.ExtractTranslation();
                float ClipOffset = 0;

                GL.Enable(ClipDistance);

                if (ClipDirection.X > 0)
                {
                    ClipOffset = trans.X;

                    ShaderModel.SetVector4(ShaderModel.Location_ClipPlanX, new Vector4(-ClipDirection, ClipOffset));
                }
                if (ClipDirection.Y > 0)
                {
                    ClipOffset = trans.Y;

                    ShaderModel.SetVector4(ShaderModel.Location_ClipPlanY, new Vector4(-ClipDirection, ClipOffset));
                }
                if (ClipDirection.Z > 0)
                {
                    ClipOffset = trans.Z;

                    ShaderModel.SetVector4(ShaderModel.Location_ClipPlanZ, new Vector4(-ClipDirection, ClipOffset));
                }
            }
        }

        public Vector3 ClipDirection { get; set; }
        public EnableCap ClipDistance { get; }
    }
}