using Simple_Engine.Engine.Space.Camera;
using Simple_Engine.Engine.Space.Scene;
using System.Linq;

namespace Simple_Engine.Engine.WCF_System
{
    internal class Wcf_EngineService : IWcf_Engine
    {
        public void DisplayMessage(string message)
        {
            //new Imgui_InputString(UI_Game. Game.Instance.uiGame.ui_Control, "Message from Base: ", () => message, (x) => { });
        }

        public string ScopeToModel(string model_Uid)
        {
            var geo = SceneModel.ActiveScene.geoModels.FirstOrDefault(o => o.Uid == model_Uid);
            if (geo == null)
            {
                return "Model not Found";
            }
            else
            {
                CameraModel.ActiveCamera.ScopeTo(geo.BBX);
                return "";
            }
        }

        public void Show_Level(double level)
        {
            var pos = SceneModel.ActiveScene.BBX.GetCG() * new OpenTK.Vector3(1, 0, 1) + new OpenTK.Vector3(0, (float)level, 0);
            CameraModel.ActiveCamera.AnimateCameraPosition(pos);
            CameraModel.ActiveCamera.AnimateCameraTarget(pos * new OpenTK.Vector3(1, 0, 1));

            CameraModel.ClipPlanY.MoveTo(pos);
            CameraModel.ClipPlanY.SetAsGlobal(true);
            CameraModel.ClipPlanY.IsActive = true;
        }
    }
}