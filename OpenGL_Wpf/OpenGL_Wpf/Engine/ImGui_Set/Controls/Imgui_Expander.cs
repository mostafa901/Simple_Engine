using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InSitU.Views.ThreeD.Engine.ImGui_Set.Controls
{
    internal class Imgui_Expander : ImgUI_Controls
    {

        public Imgui_Expander(ImgUI_Controls guiWindow, string name) : base(guiWindow)
        {
            Name = name;
            Width = 150;
        }
        public bool Expanded = true;
        public override void BuildModel()
        {
             
            if (ImGui.CollapsingHeader(Name, ImGuiTreeNodeFlags.None))
            {

                for (int i = 0; i < SubControls.Count(); i++)
                {
                    var ctrl = SubControls.ElementAt(i);
                    ctrl.BuildModel();
                }
              
            }
            ImGui.SetNextWindowSize(new System.Numerics.Vector2(400, 0), ImGuiCond.None);

        }

        public override void EndModel()
        {
        }
    }
}