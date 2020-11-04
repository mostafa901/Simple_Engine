using ImGuiNET;
using OpenTK;
using Simple_Engine.Extentions;
using System;

namespace Simple_Engine.Engine.ImGui_Set.Controls
{
    internal class Imgui_Vector3 : ImgUI_Controls
    {
        public Imgui_Vector3(ImgUI_Controls guiWindow, string name, Vector3 initialValue, Action<Vector3> buttonAction) : base(guiWindow)
        {
            Name = name;
            InitialValue = initialValue.ToSystemNumeric();
            ButtonAction = buttonAction;
            Width = 150;
        }

        private System.Numerics.Vector3 InitialValue;
        public Action<Vector3> ButtonAction { get; set; }

        public override void BuildModel()
        {
            if (ImGui.InputFloat3(Name, ref InitialValue))
            {
                var vec3 = InitialValue.ToVector();
                ButtonAction(vec3);
            }
        }

        public override void EndModel()
        {
        }
    }
}