using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Engine.Engine.ImGui_Set.Controls
{
    //for more examples:
    //https://github.com/ocornut/imgui/blob/master/imgui_demo.cpp
    public class Imgui_Window : ImgUI_Controls
    {
        public Imgui_Window(string name) : base(null)
        {
            Width = 400;
            Name = name;
            ImGui.GetIO().ConfigFlags = ImGuiConfigFlags.DockingEnable;
        }

        private bool isOpen = false;

        public ImGuiWindowFlags flag { get; set; } = ImGuiWindowFlags.AlwaysUseWindowPadding;

        public override void BuildModel()
        {
            ImGui.SetNextWindowDockID(1, ImGuiCond.Appearing);
            // Main body of the Demo window starts here.
            if (!ImGui.Begin(Name, ref isOpen, flag))
            {
                // Early out if the window is collapsed, as an optimization.
                ImGui.End();
                return;
            }

            var lst = SubControls.ToList();
            foreach (var ctrl in lst)
            {
                ImGui.SetNextWindowSize(new System.Numerics.Vector2(400, 0), ImGuiCond.Always);
                ctrl.BuildModel();
            }
        }

        public override void EndModel()
        {
            throw new NotImplementedException();
        }
    }
}