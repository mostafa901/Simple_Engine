using ImGuiNET;

namespace Simple_Engine.Engine.ImGui_Set.Controls
{
    internal class Imgui_MenuItem : ImgUI_Controls
    {
        public Imgui_MenuItem(ImgUI_Controls guiWindow, string name) : base(guiWindow)
        {
            Name = name;
        }

        public string Name { get; }

        public override void BuildModel()
        {
            if (ImGui.BeginMenu(Name))
            {
                foreach (var ctrl in SubControls)
                {
                    ctrl.BuildModel();
                }
                ImGui.EndMenu();
            }
        }

        public override void EndModel()
        {
            ImGui.EndMenu();
        }
    }
}