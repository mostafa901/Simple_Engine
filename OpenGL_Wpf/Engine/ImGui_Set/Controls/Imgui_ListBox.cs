using ImGuiNET;
using Simple_Engine.Views.ThreeD.Engine.Core.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Engine.Views.ThreeD.Engine.ImGui_Set.Controls
{
    internal class Imgui_ListBox : ImgUI_Controls
    {
        public Imgui_ListBox(ImgUI_Controls guiWindow, string name, Base_Geo[] values, Action<Base_Geo> unSelectedAction, Action<Base_Geo> selectedAction) : base(guiWindow)
        {
            Name = name;
            objValues = values;
            UnSelectedAction = unSelectedAction;
            SelectedAction = selectedAction;
            Values = values.Select(o => o.Name ?? "element").ToArray();
        }

        public Base_Geo[] objValues { get; set; }
        public Action<Base_Geo> UnSelectedAction { get; }
        public Action<Base_Geo> SelectedAction { get; }
        public string[] Values { get; set; }
        private int previousSelection = -1;
        private int currentSelection = -1;

        public override void BuildModel()
        {
            if (ImGui.ListBox(Name, ref currentSelection, Values, Values.Length))
            {
                if (previousSelection != -1)
                {
                    UnSelectedAction(objValues[previousSelection]);
                }
                previousSelection = currentSelection;
                
                if (currentSelection != -1)
                {
                    SelectedAction(objValues[currentSelection]);
                }
            }
        }

        public override void EndModel()
        {
        }
    }
}