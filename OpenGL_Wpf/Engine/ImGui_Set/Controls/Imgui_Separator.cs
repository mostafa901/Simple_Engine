using ImGuiNET;

namespace Simple_Engine.Engine.ImGui_Set.Controls
{
    internal class Imgui_Separator : ImgUI_Controls
    {
        public Imgui_Separator(ImgUI_Controls guiWindow) : base(guiWindow)
        {
        }

        public override void BuildModel()
        {
            ImGui.Separator();
        }

        public override void EndModel()
        {
        }
    }
}