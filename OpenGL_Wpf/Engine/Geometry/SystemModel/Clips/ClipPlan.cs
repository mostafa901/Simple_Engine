using OpenTK;
using OpenTK.Graphics.OpenGL;
using Simple_Engine.Engine.Render;
using Simple_Engine.Engine.Space.Camera;
using Simple_Engine.Engine.Space.Scene;
using System;

namespace Simple_Engine.Engine.Geometry.SystemModel.Clips
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
                var pos = LocalTransform.ExtractTranslation();
                if (ClipDirection.X > 0)
                {
                    CameraModel.ClipPlanX.MoveTo(pos);
                }
                else if (ClipDirection.Y > 0)
                {
                    CameraModel.ClipPlanY.MoveTo(pos);
                }
                else if (ClipDirection.Z > 0)
                {
                    CameraModel.ClipPlanZ.MoveTo(pos);
                }
                
            }
            else
            {
                CameraModel.ClipPlanX = null;
                CameraModel.ClipPlanY = null;
                CameraModel.ClipPlanZ = null;
            }
            CameraModel.EnableClipPlans = globalValue;
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
                    ShaderModel.SetBool(ShaderModel.Location_EnableClipPlanX, IsActive);
                }
                if (ClipDirection.Y > 0)
                {
                    ClipOffset = trans.Y;

                    ShaderModel.SetVector4(ShaderModel.Location_ClipPlanY, new Vector4(-ClipDirection, ClipOffset));
                    ShaderModel.SetBool(ShaderModel.Location_EnableClipPlanY, IsActive);
                }
                if (ClipDirection.Z > 0)
                {
                    ClipOffset = trans.Z;

                    ShaderModel.SetVector4(ShaderModel.Location_ClipPlanZ, new Vector4(-ClipDirection, ClipOffset));
                    ShaderModel.SetBool(ShaderModel.Location_EnableClipPlanZ, IsActive);
                }
            }
        }

        public Vector3 ClipDirection { get; set; }
        public EnableCap ClipDistance { get; }

        public static ClipPlan Generate_ClipPlan(Vector3 direction, EnableCap ClipDistance)
        {
            var clip = new ClipPlan(direction, ClipDistance, 10f);
            clip.Name = "Clip " + (direction.X > 0 ? "X" : direction.Y > 0 ? "Y" : "Z");
            if (direction == Vector3.UnitY)
            {
                clip.Rotate(90, new Vector3(1, 0, 0));
            }

            if (direction == Vector3.UnitX)
            {
                clip.Rotate(90, new Vector3(1, 0, 1));
            }

            clip.ShaderModel = new Shader(ShaderMapType.Blend, ShaderPath.SingleColor);
            SceneModel.ActiveScene.UpLoadModels(clip);
            return clip;
        }

        
    }
}