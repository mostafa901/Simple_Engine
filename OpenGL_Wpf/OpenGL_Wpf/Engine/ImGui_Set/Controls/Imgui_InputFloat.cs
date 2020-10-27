using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InSitU.Views.ThreeD.Engine.ImGui_Set.Controls
{
    internal class Imgui_InputFloat : ImgUI_Controls
    {
        public Imgui_InputFloat(ImgUI_Controls guiWindow, string name, float initialValue, Action<float> buttonAction) : base(guiWindow)
        {
            Name = name;
            InitialValue = initialValue;
            originalValue = initialValue;
            ButtonAction = buttonAction;
            Width = 100;
        }

        private float InitialValue;
        private float originalValue;

        public Action<float> ButtonAction { get; set; }

        public override void BuildModel()
        {
           
            if (ImGui.InputFloat(Name, ref InitialValue, 1, 5))
            {
                ButtonAction(InitialValue-originalValue);
                originalValue = InitialValue;
            }
        }

        public override void EndModel()
        {
        }
    }
}