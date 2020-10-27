using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Engine.Views.ThreeD.Engine.ImGui_Set.Controls
{
    public abstract class ImgUI_Controls
    {
        public int Width { get; set; } = 100;
        public string Name { get; set; }
        public ImgUI_Controls GuiParent { get; set; }
        public List<ImgUI_Controls> SubControls = new List<ImgUI_Controls>();

        public ImgUI_Controls(ImgUI_Controls guiParent)
        {
            GuiParent = guiParent;

            GuiParent?.SubControls.Add(this);
        }

        public abstract void BuildModel();

        public abstract void EndModel();

        public virtual void SetWindowFlag(ImGuiWindowFlags flag)
        {
            var win = GuiParent as Imgui_Window;
            win.flag |= flag;
        }
    }
}