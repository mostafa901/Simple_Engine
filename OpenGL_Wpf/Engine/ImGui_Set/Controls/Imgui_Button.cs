using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Engine.Engine.ImGui_Set.Controls
{
    internal class Imgui_Button : ImgUI_Controls
    {
        public Imgui_Button(ImgUI_Controls guiWindow, string name, Action<object> buttonAction) : base(guiWindow)
        {
            Name = name;
            ButtonAction = buttonAction;
            Width = 150;
            color = defaultColor;
        }

        public Action<object> ButtonAction { get; set; }

        public static readonly Vector4 defaultColor = new Vector4(114.0f / 255.0f, 144.0f / 255.0f, 154.0f / 255.0f, 200.0f / 255.0f);
        public Vector4 color;
        public override void BuildModel()
        {
            ImGui.PushStyleColor( ImGuiCol.Button,color);
            if (ImGui.Button(Name))
            {
                ButtonAction(null);
            }
        }

        public override void EndModel()
        {
        }
    }
}