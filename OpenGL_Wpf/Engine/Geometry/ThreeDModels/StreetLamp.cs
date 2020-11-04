using OpenTK;
using Simple_Engine.Engine.Core.Abstracts;
using Simple_Engine.Engine.Geometry.Core;
using Simple_Engine.Engine.Illumination;

namespace Simple_Engine.Engine.Geometry.ThreeDModels
{
    public class StreetLamp : GeometryModel
    {
        public Vector3 Attenuation = new Vector3(1, .1f, .02f);

        public StreetLamp(Base_Geo3D geo)
        {
            CloneModel(geo);
        }

        public void AddInstance(Vector4 lightColor, Matrix4 transform)
        {
            var light = new LightModel();
            light.DefaultColor = lightColor;
            light.Attenuation = Attenuation;

            light.LightPosition = transform.ExtractTranslation() + new Vector3(0, 10, 0);

            var mesh = AddMesh(transform);

            mesh.MoveEvent += (s, e) =>
              {
                  light.LightPosition = e.Transform.ExtractTranslation() + new Vector3(0, 10, 0);
              };
        }
    }
}