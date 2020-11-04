using Shared_Lib.IO;
using Simple_Engine.Engine.Core.Serialize;
using Simple_Engine.Engine.Render;
using Simple_Engine.Engine.Space.Scene;
using System.Linq;
using System.Threading.Tasks;
using static Simple_Engine.Engine.Core.Serialize.Importer.Import_Helper;

namespace Simple_Engine.Engine.GameSystem
{
    public partial class Game
    {
        public void ImportModels()
        {
            string path = UT_System.LoadFiles(string.Join("|", GetFilter(filter.Json), GetFilter(filter.OBJ))).FirstOrDefault();

            if (path != null)
            {
                Task.Run(() =>
                {
                    if (path.EndsWith("json"))
                    {
                        Importer.Import.Revit_ExportFile(path);
                    }

                    if (path.EndsWith("obj"))
                    {
                        var geo = Importer.Import.OBJFile(path);
                        geo.ShaderModel = new Shader(ShaderMapType.Blend, ShaderPath.SingleColor);

                        SceneModel.ActiveScene.UpLoadModels(geo);
                    }
                });
            }
        }

        public void LoadModels()
        {
            string path = UT_System.LoadFiles(GetFilter(filter.Simple_EngineModel)).FirstOrDefault();

            if (path != null)
            {
                Task.Run(() => { Loader.Load_GeoFile(path); });
            }
        }

        public void SaveModels()
        {
            string path = UT_System.SaveFilePath(GetFilter(filter.Simple_EngineModel));

            if (path != null)
            {
                Task.Run(() => { IO.Save(SceneModel.ActiveScene.geoModels, path); });
            }
        }
    }
}