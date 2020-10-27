using ImGuiNET;
using Simple_Engine.Views.ThreeD.Extentions;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Engine.Views.ThreeD.Engine.ImGui_Set.Controls
{
    internal class Imgui_SliderFloat4 : ImgUI_Controls
    {
        public Imgui_SliderFloat4(ImgUI_Controls guiWindow, string name, Vector4 initialValue, Action<Vector4> buttonAction) : base(guiWindow)
        {
            Name = name;
            InitialValue = initialValue.ToSystemNumeric();
            ButtonAction = buttonAction;
            Width = 150;
        }

        private System.Numerics.Vector4 InitialValue;
        public Action<Vector4> ButtonAction { get; set; }

        public override void BuildModel()
        {
           
            if (ImGui.SliderFloat4(Name, ref InitialValue,0,2))
            {
                var vec4 = InitialValue.ToVector();
                ButtonAction(vec4);
            }
        }

        public override void EndModel()
        {
        }
    }
}