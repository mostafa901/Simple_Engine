using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InSitU.Views.ThreeD.Engine.ImGui_Set
{
    public static class Imgui_Helper
    {

        public static bool IsAnyCaptured()
        {
            return ImGui.IsAnyItemHovered() || ImGui.IsWindowHovered(ImGuiHoveredFlags.AnyWindow);
        }
    }
}
