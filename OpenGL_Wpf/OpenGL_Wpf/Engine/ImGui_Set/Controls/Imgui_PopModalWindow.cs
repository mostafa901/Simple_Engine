using DocumentFormat.OpenXml.Office.CustomUI;
using ImGuiNET;
using OpenTK;
using org.apache.poi.poifs.property;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InSitU.Views.ThreeD.Engine.ImGui_Set.Controls
{
    //for more examples:
    //https://github.com/ocornut/imgui/blob/master/imgui_demo.cpp
    public class Imgui_PopModalWindow : ImgUI_Controls
    {
        public Imgui_PopModalWindow(ImgUI_Controls Parent,string name,string message, Func<bool> openCondition,Action<bool> acceptedAction) : base(Parent)
        {
            Name = name;
           
            Message = message;
            OpenCondition = openCondition;
            AcceptedAction = acceptedAction;
        }

        private bool isOpen = true;

        public ImGuiWindowFlags flag = ImGuiWindowFlags.AlwaysAutoResize;

        public string Title { get; }
        public string Message { get; }
        public Func<bool> OpenCondition { get; }
        public Action<bool> AcceptedAction { get; }

        public override void BuildModel()
        {
            isOpen = OpenCondition();
            if (isOpen)
            {
                ImGui.OpenPopup(Name);
            }

            // Always center this window when appearing
            System.Numerics.Vector2 center = new System.Numerics.Vector2(ImGui.GetIO().DisplaySize.X * 0.5f, ImGui.GetIO().DisplaySize.Y * 0.5f);
            ImGui.SetNextWindowPos(center, ImGuiCond.Appearing, new System.Numerics.Vector2(0.5f, 0.5f));

            if (ImGui.BeginPopupModal(Name, ref isOpen, flag))
            {
                ImGui.Text(Name);
                ImGui.Text(Message);
                ImGui.Separator();

                //bool dont_ask_me_next_time = false;
                //ImGui.PushStyleVar(ImGuiStyleVar.FramePadding, new System.Numerics.Vector2(0, 0));
                //ImGui.Checkbox("Don't ask me next time", ref dont_ask_me_next_time);
                //ImGui.PopStyleVar();

                if (ImGui.Button("OK", new System.Numerics.Vector2(120, 0)))
                {
                    AcceptedAction(true);
                    ImGui.CloseCurrentPopup();
                    isOpen = false;
                }
                ImGui.SetItemDefaultFocus();
                ImGui.SameLine();
                if (ImGui.Button("Cancel", new System.Numerics.Vector2(120, 0)))
                {
                    AcceptedAction(false);
                    ImGui.CloseCurrentPopup();
                    isOpen = false;
                }
                 
                ImGui.EndPopup();
            }
        }

        public override void EndModel()
        {
            throw new NotImplementedException();
        }
    }
}