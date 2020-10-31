using Simple_Engine.Engine.Core.Abstracts;
using Simple_Engine.Engine.Core.Static;
using Simple_Engine.Engine.Geometry.ThreeDModels.Clips;
using Simple_Engine.Engine.Illumination;
using Simple_Engine.Engine.ImGui_Set.Controls;
using Simple_Engine.Engine.Space.Camera;
using System.Linq;

namespace Simple_Engine.Engine.Space.Scene
{
    public partial class SceneModel
    {
        public ImgUI_Controls Ui_Controls { get; set; }
        public bool IsolateDisplay { get; internal set; } = false;

        public void Create_UIControls()
        {
            Ui_Controls = new Imgui_Window("Scene");
            var models = geoModels.Where(o => o is Base_Geo).Where(o => !(o is ClipPlan)).Cast<Base_Geo>().ToArray();
            var i = new Imgui_ListBox(Ui_Controls, "Elements", models, (x) => { x.Set_Selected(false); }, (x) => { x.Set_Selected(true); });
        }

        public void Render_UIControls()
        {
            UI_Camera.RenderUI(CameraModel.ActiveCamera);
            UI_Light.RenderUI(LightModel.SelectedLight);

        }
    }
}