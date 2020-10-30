using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Simple_Engine.Engine.ImGui_Set.Controls
{
    public class Imgui_MenuBar : ImgUI_Controls
    {
        public Imgui_MenuBar(ImgUI_Controls guiWindow) : base(guiWindow)
        {
            SetWindowFlag(ImGuiWindowFlags.MenuBar);
        }


        private bool show_app_main_menu_bar = true;

        public override void BuildModel()
        {
            // Menu Bar
            if (ImGui.BeginMenuBar())
            {
                foreach (var ctrl in SubControls)
                {
                    ctrl.BuildModel();
                }
                if (ImGui.BeginMenu("Examples"))
                {
                    ImGui.MenuItem("Main menu bar", null, ref show_app_main_menu_bar);
                    //ImGui.MenuItem("Console", NULL, &show_app_console);
                    //ImGui.MenuItem("Log", NULL, &show_app_log);
                    //ImGui.MenuItem("Simple layout", NULL, &show_app_layout);
                    //ImGui.MenuItem("Property editor", NULL, &show_app_property_editor);
                    //ImGui.MenuItem("Long text display", NULL, &show_app_long_text);
                    //ImGui.MenuItem("Auto-resizing window", NULL, &show_app_auto_resize);
                    //ImGui.MenuItem("Constrained-resizing window", NULL, &show_app_constrained_resize);
                    //ImGui.MenuItem("Simple overlay", NULL, &show_app_simple_overlay);
                    //ImGui.MenuItem("Manipulating window titles", NULL, &show_app_window_titles);
                    //ImGui.MenuItem("Custom rendering", NULL, &show_app_custom_rendering);
                    //ImGui.MenuItem("Documents", NULL, &show_app_documents);
                    ImGui.EndMenu();
                }
                if (ImGui.BeginMenu("Tools"))
                {
                    //ImGui.MenuItem("Metrics", NULL, &show_app_metrics);
                    //ImGui.MenuItem("Style Editor", NULL, &show_app_style_editor);
                    //ImGui.MenuItem("About Dear ImGui", NULL, &show_app_about);
                    ImGui.EndMenu();
                }
                ImGui.EndMenuBar();
            }
        }

        public override void EndModel()
        {
            ImGui.EndMenuBar();

        }
    }
}