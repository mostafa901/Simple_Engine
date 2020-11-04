using OpenTK;
using Simple_Engine.Engine.Core;
using Simple_Engine.Engine.Core.AnimationSystem;

namespace Simple_Engine.Engine.Space.Camera
{
    public partial class CameraModel
    {
        public void UpdateViewTo(CameraModel cameraDistination)
        {
            CameraModel.ActiveCamera.ViewType = CameraType.None;
            if (!cameraDistination.IsPerspective)
            {
                AnimateCameraHeight(cameraDistination.height);
            }

            var posanimate = CameraModel.ActiveCamera.AnimateCameraPosition(cameraDistination.Position);
            CameraModel.ActiveCamera.AnimateCameraTarget(cameraDistination.Target);
            CameraModel.ActiveCamera.AnimateCameraUP(cameraDistination.UP); posanimate.OnFinish += (s, e) =>
            {
                CameraModel.ActiveCamera.ViewType = cameraDistination.ViewType;
                if (cameraDistination.IsPerspective)
                {
                    CameraModel.ActiveCamera.ActivatePrespective();
                }
                else
                {
                    CameraModel.ActiveCamera.Activate_Ortho();
                }
            };
        }

        private void AnimateCameraHeight(float toHeight)
        {
            new AnimFloat(this, 1000, CameraModel.ActiveCamera.height, toHeight, (X) =>
            {
                CameraModel.ActiveCamera.SetHeight(X);
                if (!CameraModel.ActiveCamera.IsPerspective)
                {
                    CameraModel.ActiveCamera.Activate_Ortho();
                }
            });
        }

        public AnimVector3 AnimateCameraPosition(Vector3 ToPosition, float duration = 1000)
        {
            return new AnimVector3(this, duration, Position, ToPosition, (x) =>
               {
                   Position = x;
                   UpdateCamera();
               });
        }

        public AnimVector3 AnimateCameraTarget(Vector3 ToTargetPosition, float duration = 1000)
        {
            return new AnimVector3(this, duration, Target, ToTargetPosition, (x) =>
            {
                Target = x;
                UpdateCamera();
            });
        }

        private AnimVector3 AnimateCameraUP(Vector3 target, float duration = 1000)
        {
            return new AnimVector3(this, duration, UP, target, (x) =>
            {
                UP = x;
                ViewType = CameraType.None;
                UpdateCamera();
            });
        }
    }
}