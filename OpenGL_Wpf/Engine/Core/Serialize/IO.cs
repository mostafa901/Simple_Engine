using Shared_Lib.Extention.Serialize_Ex;
using Simple_Engine.Engine.Core.Interfaces;
using System.Collections.Generic;
using System.IO;

namespace Simple_Engine.Engine.Core.Serialize
{
    public static class IO
    {
   
        public static void Save(this IEnumerable<IDrawable> models, string path)
        {
            using (var str = new StreamWriter(path))
            {
                foreach (var model in models)
                {
                    if (!model.CanBeSaved) continue;
                    str.WriteLine(GetJsString(model));
                }
            }
        }

        public static void Save(this IRenderable model, string path)
        {
            File.WriteAllText(path, GetJsString(model));
        }

        private static string GetJsString(this IRenderable model)
        {
            return model.JSerialize(JsonTools.GetSettings());
        }
    }
}