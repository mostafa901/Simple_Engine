using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Engine.Views.ThreeD.Engine.ImGui_Set.Controls
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