using Simple_Engine.Views.ThreeD.Engine.Core.Abstracts;
using Simple_Engine.Views.ThreeD.Engine.Core.Interfaces;
using Newtonsoft.Json;
using Shared_Lib.Extention.Serialize_Ex;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Engine.Views.ThreeD.Engine.Core.Serialize
{
    public static class IO
    {
        public static T LoadModel<T>(string path) where T : IRenderable
        {
            return File.ReadAllText(path).JDeserialize<T>(JsonTools.GetSettings());
        }

        public static T LoadModels<T>(string path) where T : IEnumerable<IDrawable>
        {
            return File.ReadAllText(path).JDeserialize<T>(JsonTools.GetSettings());
        }

        public static void Save(this IEnumerable<IDrawable> models, string path)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            foreach (var model in models)
            {
                if (!model.CanBeSaved) continue;
                sb.Append(GetJsString(model));
                sb.Append(",");
            }
            sb.Append("]");

            File.WriteAllText(path, sb.ToString());
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