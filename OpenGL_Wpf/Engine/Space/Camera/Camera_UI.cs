using OpenTK;
using Simple_Engine.Engine.ImGui_Set.Controls;

namespace Simple_Engine.Engine.Space.Camera
{
    public partial class CameraModel
    {
        public ImgUI_Controls Ui_Controls { get; set; }
        private bool loadUI;

        public void Create_UIControls()
        {
            Ui_Controls = new Imgui_Window($"Camera {Name}");

            Add_Name();
            Add_Width();
            Add_Height();
            Add_Position();
            Add_Target();
            Add_FOV();
            Add_DisplayMode();
            Add_CameraLine();
        }

        private void Add_FOV()
        {
            var imgui_fov = new Imgui_DragFloat(Ui_Controls, "FOV", () => FOV, (x) =>
            {
                UpdateFOV(FOV+x);
            });
            imgui_fov.Max = 90;
            imgui_fov.Min = 1;
        }

        private void Add_Target()
        {
            new Imgui_DragFloat3(Ui_Controls, "Target", () => Target, (x) =>
            {
                Target += x;
                UpdateCamera();
            });
        }

        private void Add_CameraLine()
        {
            new Imgui_CheckBox(Ui_Controls, "Show Camera Line", () => IsDirectionVisible, (x) =>
            {
                set_IsDirectionVisible(x); ;
            });
        }

        private void Add_DisplayMode()
        {
            new Imgui_CheckBox(Ui_Controls, "Display Mode", () => IsPrespective, (x) =>
            {
                if (x)
                {
                    ActivatePrespective();
                }
                else
                {
                    Activate_Ortho();
                }
            });
        }

        private void Add_Position()
        {
            new Imgui_DragFloat3(Ui_Controls, "Position", () => Position, (x) =>
            {
                Position += x;
                UpdateCamera();
            });
        }

        private void Add_Height()
        {
            new Imgui_DragFloat(Ui_Controls, "Height", () => Height, (x) =>
            {
                Height += x;
                Height = MathHelper.Clamp(Height, 50, 2000);
                UpdateViewMode();
            });
        }

        private void Add_Width()
        {
            new Imgui_DragFloat(Ui_Controls, "Width", () => Width, (x) =>
            {
                Width += x;
                Width = MathHelper.Clamp(Width, 50, 2000);
                UpdateViewMode();
            });
        }

        private void Add_Name()
        {
            new Imgui_String(Ui_Controls, "Name", () => Name, (x) =>
            {
                Name = x;
            });
        }

        public void Render_UIControls()
        {
            Ui_Controls?.BuildModel();
        }
    }
}