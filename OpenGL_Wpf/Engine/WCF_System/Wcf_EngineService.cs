using Simple_Engine.Engine.Core.Static;
using Simple_Engine.Engine.GameSystem;
using Simple_Engine.Engine.ImGui_Set.Controls;
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
            if(geo==null)
            {
                return "Model not Found";
            }
            else
            {
                CameraModel.ActiveCamera.ScopeTo(geo.BBX);
                return "";
            }


           
        }
    }
}