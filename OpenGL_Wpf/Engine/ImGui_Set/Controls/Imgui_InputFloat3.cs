using ImGuiNET;
using Simple_Engine.Views.ThreeD.Extentions;
using OpenTK;
using Shared_Lib.Extention;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Engine.Views.ThreeD.Engine.ImGui_Set.Controls
{
    internal class Imgui_InputFloat3 : ImgUI_Controls
    {
        public Imgui_InputFloat3(ImgUI_Controls guiWindow, string name, Vector3 initialValue, Action<Vector3> buttonAction) : base(guiWindow)
        {
            Name = name;
            InitialValue = initialValue.ToSystemNumeric();
            originalValue = initialValue.ToSystemNumeric();
            ButtonAction = buttonAction;
            Width = 100;
        }

        private System.Numerics.Vector3 InitialValue;
        private System.Numerics.Vector3 originalValue;

        public Action<Vector3> ButtonAction { get; set; }

        public override void BuildModel()
        {
           
            if (ImGui.InputFloat3(Name, ref InitialValue))
            {
                ButtonAction((InitialValue).ToVector());
                //originalValue = InitialValue;
            }
        }

        public override void EndModel()
        {
        }
    }
}