using ImGuiNET;
using System;

namespace Simple_Engine.Engine.ImGui_Set.Controls
{
    internal class Imgui_InputString : ImgUI_Controls
    {
        public Imgui_InputString(ImgUI_Controls guiWindow, string name, Func<string> initialValue, Action<string> changeAction) : base(guiWindow)
        {
            Name = name;
            InitialValue = initialValue;
            ChangeAction = changeAction;
        }

        private Func<string> InitialValue;
        public Action<string> ChangeAction { get; }

        public override void BuildModel()
        {
            var val = InitialValue() ?? "";

            if (ImGui.InputText(Name, ref val, 200))
            {
                ChangeAction(val);
            }
        }

        public override void EndModel()
        {
        }
    }
}