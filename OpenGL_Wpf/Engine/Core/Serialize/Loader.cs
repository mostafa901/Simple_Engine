using Shared_Lib.Extention.Serialize_Ex;
using Shared_Lib.MVVM;
using Simple_Engine.Engine.Core.Static;
using Simple_Engine.Engine.GameSystem;
using Simple_Engine.Engine.Geometry.Core;
using Simple_Engine.Engine.Space.Scene;
using System.IO;
using System.Linq;

namespace Simple_Engine.Engine.Core.Serialize
{
    public static partial class Loader
    {
        private static int linecount = 0;

        public static void Load_GeoFile(string filename)
        {
            var cmd = new cus_CMD();
            Game.Instance.RenderOnUIThread(cmd);

            var count = File.ReadLines(filename).Count();
            using (var reader = new StreamReader(filename))
            {
                string line = "";

                while ((line = reader.ReadLine()) != null)
                {
                    linecount += 1;

                    if (string.IsNullOrEmpty(line)) continue;

                    cmd.Action = (x) =>
                    {
                        UI_Shared.Render_Progress(linecount, count, $"Loading...");
                    };
                    GeometryModel geo = line.JDeserialize<GeometryModel>(JsonTools.GetSettings());

                    geo.ShaderModel = new Render.Shader(Render.ShaderMapType.Blend, Render.ShaderPath.SingleColor);

                    SceneModel.ActiveScene.UpLoadModels(geo);
                }

                cmd.Action = (x) =>
                {
                    Game.Instance.Dispose_RenderOnUIThread(cmd);
                };
            }
        }
    }
}