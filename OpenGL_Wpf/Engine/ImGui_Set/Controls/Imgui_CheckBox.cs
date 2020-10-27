using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Engine.Views.ThreeD.Engine.ImGui_Set.Controls
{
    internal class Imgui_CheckBox : ImgUI_Controls
    {
        public Imgui_CheckBox(ImgUI_Controls guiWindow, string name, Func<bool> initialValue, Action<bool> buttonAction) : base(guiWindow)
        {
            Name = name;
            InitialValue = initialValue;
            ButtonAction = buttonAction;
            Width = 150;

        }

        Func<bool>  InitialValue;
        public Action<bool> ButtonAction { get; }

        public override void BuildModel()
        {
            var val = InitialValue();
            if (ImGui.Checkbox(Name, ref val))
            {
                ButtonAction(val);
            }
        }

        public override void EndModel()
        {
        }
    }
}