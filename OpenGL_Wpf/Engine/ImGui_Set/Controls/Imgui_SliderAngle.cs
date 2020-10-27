using ImGuiNET;
using Simple_Engine.Views.ThreeD.Extentions;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Engine.Views.ThreeD.Engine.ImGui_Set.Controls
{
    internal class Imgui_SliderAngle : ImgUI_Controls
    {
        public Imgui_SliderAngle(ImgUI_Controls guiWindow, string name, float initialValue, Action<float> buttonAction) : base(guiWindow)
        {
            Name = name;
            InitialValue = initialValue;
            ButtonAction = buttonAction;
            Width = 150;
        }

        private float InitialValue;
        private float Previousevalue;
        public Action<float> ButtonAction { get; set; }

        public override void BuildModel()
        {
           
            if (ImGui.SliderAngle(Name, ref InitialValue))
            {
                ButtonAction(InitialValue - Previousevalue);
                Previousevalue = InitialValue;
            }
        }

        public override void EndModel()
        {
        }
    }
}