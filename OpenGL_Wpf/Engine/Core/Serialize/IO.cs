using Shared_Lib.Extention.Serialize_Ex;
using Shared_Lib.MVVM;
using Simple_Engine.Engine.Core.Interfaces;
using Simple_Engine.Engine.Core.Static;
using Simple_Engine.Engine.GameSystem;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Simple_Engine.Engine.Core.Serialize
{
    public static class IO
    {
        public static void Save(this IEnumerable<IDrawable> models, string path)
        {
            var cmd = new cus_CMD();
            Game.Instance.RenderOnUIThread(cmd);
            using (var str = new StreamWriter(path))
            {
                int count = models.Count();
                for (int i = 0; i < count; i++)
                {
                    var model = models.ElementAt(i);
                    if (!model.CanBeSaved) continue;

                    str.WriteLine(GetJsString(model));
                    cmd.Action = (x) =>
                    {
                        UI_Shared.Render_Progress(i, count, $"saving {model.Name}");
                    };
                }
            }

            cmd.Action = (x) =>
            {
                Game.Instance.Dispose_RenderOnUIThread(cmd);
            };
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