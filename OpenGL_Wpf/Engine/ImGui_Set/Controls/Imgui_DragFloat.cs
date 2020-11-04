using ImGuiNET;
using System;

namespace Simple_Engine.Engine.ImGui_Set.Controls
{
    internal class Imgui_DragFloat : ImgUI_Controls
    {
        public Imgui_DragFloat(ImgUI_Controls guiWindow, string name, Func<float> initialValue, Action<float> buttonAction) : base(guiWindow)
        {
            Name = name;
            InitialValue = initialValue;

            ButtonAction = buttonAction;
        }

        public float Min = 0;
        public float Max = float.PositiveInfinity;
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

            if (ImGui.DragFloat(Name, ref val, .01f, Min, Max))
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