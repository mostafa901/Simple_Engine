using Simple_Engine.Engine.ImGui_Set.Controls;
using System;

namespace Simple_Engine.Engine.WCF_System
{
    internal class Wcf_EngineService : IWcf_Engine
    {
         

        public void DisplayMessage(string message)
        {
            new Imgui_String(Game.Context.uiGame.ui_Control, "Message from Base: ", () => message, (x) => { });
        }
    }
}