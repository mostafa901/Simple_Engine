using ImGuiNET;
using InSitU.Views.ThreeD.Extentions;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InSitU.Views.ThreeD.Engine.ImGui_Set.Controls
{
    internal class Imgui_DragFloat3 : ImgUI_Controls
    {
        public Imgui_DragFloat3(ImgUI_Controls guiWindow, string name, Func<Vector3> initialValue, Action<Vector3> buttonAction) : base(guiWindow)
        {
            Name = name;
            InitialValue = initialValue;
            previouse = initialValue();
            ButtonAction = buttonAction;
        }

        public Func<Vector3> InitialValue { get; }
        Vector3 previouse;
        public Action<Vector3> ButtonAction { get; set; }

        public override void BuildModel()
        {
            var val = InitialValue().ToSystemNumeric();
            float scale = 1;
            if(ImGui.GetIO().KeyShift)
            {
                scale = 3;
            }
            if (ImGui.GetIO().KeyAlt)
            {
                scale = .5f;
            }
            if (ImGui.DragFloat3(Name, ref val, 1*scale, float.NegativeInfinity, float.PositiveInfinity))
            {
                var vec3 = val.ToVector();
                ButtonAction(vec3-previouse);
                previouse = vec3;
            }
        }

        public override void EndModel()
        {
        }
    }
}