using ImGuiNET;
using System.Linq;

namespace Simple_Engine.Engine.ImGui_Set.Controls
{
    internal class Imgui_Group : ImgUI_Controls
    {
        public Imgui_Group(ImgUI_Controls guiWindow, string name) : base(guiWindow)
        {
            Name = name;
            Width = 150;
        }

        public bool IsOpen { get; }

        public override void BuildModel()
        {
            ImGui.Text(Name);

            ImGui.BeginGroup();
            {
                for (int i = 0; i < SubControls.Count(); i++)
                {
                    var ctrl = SubControls.ElementAt(i);
                    ctrl.BuildModel();
                }

                ImGui.EndGroup();
            }
        }

        public override void EndModel()
        {
        }
    }
}