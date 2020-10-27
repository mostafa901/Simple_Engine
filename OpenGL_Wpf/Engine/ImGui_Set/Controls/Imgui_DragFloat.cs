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
    internal class Imgui_DragFloat : ImgUI_Controls
    {
        public Imgui_DragFloat(ImgUI_Controls guiWindow, string name, Func<float> initialValue, Action<float> buttonAction) : base(guiWindow)
        {
            Name = name;
            InitialValue = initialValue;
           
            ButtonAction = buttonAction;
        }

        private Func<float> InitialValue;
        private float PreviousValue = 1 * (float)Math.E;
        public Action<float> ButtonAction { get; set; }

        public override void BuildModel()
        {

            if (PreviousValue == 1 * (float)Math.E)
            {
                PreviousValue = InitialValue();
            }
            var val = InitialValue();
            if (ImGui.DragFloat(Name, ref val, .01f, 0, float.PositiveInfinity))
            {
                var vec3 = (val - PreviousValue);
                ButtonAction(vec3);
                PreviousValue = val;
            }
        }

        public override void EndModel()
        {
        }
    }
}