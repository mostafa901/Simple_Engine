using OpenTK;
using Simple_Engine.Engine.Core;
using Simple_Engine.Engine.Core.AnimationSystem;
using Simple_Engine.Engine.Core.Interfaces;
using Simple_Engine.Engine.Space.Scene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Engine.Engine.Space.Camera
{
  public  partial class CameraModel 
    {
        public AnimationComponent Animate { get; set; }

        public void UpdateViewTo(CameraModel cameraDistination)
        {
            Animate.OnFinished += (s, e) =>
            {
                if (cameraDistination.IsPerspective)
                {
                    CameraModel.ActiveCamera.ActivatePrespective();
                }
                else
                {
                    CameraModel.ActiveCamera.Activate_Ortho();
                }
                CameraModel.ActiveCamera.ViewType = cameraDistination.ViewType;
            };
            CameraModel.ActiveCamera.ViewType = CameraType.None;
          
            Animate.AnimFloat.Add(new AnimFloat(this, 1000, CameraModel.ActiveCamera.Width, cameraDistination.Width, (X)=>CameraModel.ActiveCamera.Width = X));
            Animate.AnimFloat.Add(new AnimFloat(this, 1000, CameraModel.ActiveCamera.Height, cameraDistination.Height, (X) => CameraModel.ActiveCamera.Height = X));
            //todo: animate Float Width and height
            CameraModel.ActiveCamera.AnimateCameraPosition(cameraDistination.Position);
            CameraModel.ActiveCamera.AnimateCameraTarget(cameraDistination.Target);
            CameraModel.ActiveCamera.AnimateCameraUP(cameraDistination.UP);

        }

       
      

        public void AnimateCameraPosition(Vector3 ToPosition, float duration = 1000)
        {
            Animate.AnimPositions.Add(new AnimVector3(this, duration, Position, ToPosition, (x) =>
            {
                Position = x;
                UpdateCamera();
            }));
        }
       

        public void AnimateCameraTarget(Vector3 ToTargetPosition, float duration = 1000)
        {
            Animate.AnimPositions.Add(new AnimVector3(this, duration, Target, ToTargetPosition, (x) =>
            {
                Target = x;
                UpdateCamera();
            }));
        }

        private void AnimateCameraUP(Vector3 target, float duration = 1000)
        {
            Animate.AnimPositions.Add(new AnimVector3(this, duration, UP, target, (x) =>
            {
                UP = x;
                ViewType = CameraType.None;
                UpdateCamera();
            }));
        }

    }
}
