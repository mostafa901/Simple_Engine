using Simple_Engine.Engine.Core.Abstracts;
using Simple_Engine.Engine.Core.Interfaces;
using Simple_Engine.Engine.Geometry.Core;
using Simple_Engine.Engine.Illumination;
using Simple_Engine.ToolBox;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var light = new Light();
            light.LightColor = lightColor;
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