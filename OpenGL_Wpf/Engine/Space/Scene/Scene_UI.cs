using Simple_Engine.Engine.Core.Abstracts;
using Simple_Engine.Engine.Core.Static;
using Simple_Engine.Engine.Core.Static.UI;
using Simple_Engine.Engine.Geometry.ThreeDModels.Clips;
using Simple_Engine.Engine.Illumination;
using Simple_Engine.Engine.ImGui_Set.Controls;
using Simple_Engine.Engine.Space.Camera;
using System.Linq;

namespace Simple_Engine.Engine.Space.Scene
{
    public partial class SceneModel
    {
        public bool IsolateDisplay { get; internal set; } = false;

        public void Render_UIControls()
        {
            UI_Camera.RenderUI(CameraModel.ActiveCamera);
            UI_Light.RenderUI(LightModel.SelectedLight);
            UI_Fog.RenderUI(SceneFog);
            Core.Static.UI_Geo.RenderUI(Base_Geo.SelectedModel as Base_Geo);

        }
    }
}