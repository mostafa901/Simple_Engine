using ImGuiNET;
using Simple_Engine.Extentions;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Engine.Engine.ImGui_Set.Controls
{
    internal class Imgui_SliderInt : ImgUI_Controls
    {
        public Imgui_SliderInt(ImgUI_Controls guiWindow, string name, int initialValue, Action<int> buttonAction) : base(guiWindow)
        {
            Name = name;
            InitialValue = initialValue;
            ButtonAction = buttonAction;
            Width = 150;
        }

        private int InitialValue;
        private int Previousevalue;
        public Action<int> ButtonAction { get; set; }

        public override void BuildModel()
        {
           
            if (ImGui.SliderInt(Name, ref InitialValue, 0, 10))
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