using Shared_Lib.IO;
using Simple_Engine.Engine.Core.Serialize;
using Simple_Engine.Engine.Core.Serialize.Importer;
using Simple_Engine.Engine.Geometry.Core;
using Simple_Engine.Engine.Render;
using Simple_Engine.Engine.Space.Scene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static Simple_Engine.Engine.Core.Serialize.Importer.Import_Helper;

namespace Simple_Engine.Engine.GameSystem
{
    public partial class Game
    {
        public void ImportModels()
        {
            string path = UT_System.LoadFiles(GetFilter(filter.Json)).FirstOrDefault();

            if (path != null)
            {
                Task.Run(() => { Loader.Load_GeoFile(path); });
            }

        }

        public void SaveModels()
        {
            string path = UT_System.LoadFiles(GetFilter(filter.Json)).FirstOrDefault();

            if (path != null)
            {
                Task.Run(() => { IO.Save(SceneModel.ActiveScene.geoModels, path); });
            }

        }


    }
}
