using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Engine.Views.ThreeD.Engine.ImGui_Set.Controls
{
    internal class Imgui_Button : ImgUI_Controls
    {
        public Imgui_Button(ImgUI_Controls guiWindow, string name, Action<object> buttonAction) : base(guiWindow)
        {
            Name = name;
            ButtonAction = buttonAction;
            Width = 150;
        }

        public Action<object> ButtonAction { get; }

        public override void BuildModel()
        {
            if (ImGui.Button(Name))
            {
                ButtonAction(null);
            }
        }

        public override void EndModel()
        {
        }
    }
}