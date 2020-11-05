using Shared_Lib.Extention;
using Shared_Lib.Extention.Serialize_Ex;
using Shared_Lib.MVVM;
using Simple_Engine.Engine.Core.Abstracts;
using Simple_Engine.Engine.Core.Interfaces;
using Simple_Engine.Engine.Core.Static;
using Simple_Engine.Engine.GameSystem;
using Simple_Engine.Extentions;
using System;
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

        public static void SaveBinary(Base_Geo3D model)
        {
            string path = "";

            {
                string name = "position";
                var namebuffer = name.ToByteArray(System.Text.Encoding.ASCII);
                byte[] namelengthBuffer = namebuffer.Length.ToBytes();

                var pos = model.Positions.GetArray();
                var poslength = pos.Length * sizeof(float);
                byte[] posLengthBuffer = poslength.ToBytes();
                byte[] posbuffer = new byte[poslength];

                Buffer.BlockCopy(pos, 0, posbuffer, 0, poslength);

                //size of name + name + size of pos + pos
                byte[] buffer = new byte[4 + namebuffer.Length + 4 + poslength];
                int index = 0;

                Array.Copy(namelengthBuffer, 0, buffer, index, sizeof(int));
                index += sizeof(int);
                Array.Copy(namebuffer, 0, buffer, index, namebuffer.Length);
                index += namebuffer.Length;

                Array.Copy(posLengthBuffer, 0, buffer, index, sizeof(int));
                index += sizeof(int);

                Array.Copy(posbuffer, 0, buffer, index, posbuffer.Length);

                using (var fs = new FileStream(path, FileMode.Create))
                {
                    fs.Write(buffer, 0, buffer.Length);
                }
            }
            using (var fr = new FileStream(path, FileMode.Open))
            {
                int index = 0;
                int size = sizeof(float);
                byte[] typeNamelength = new byte[size];
                fr.Read(typeNamelength, index, size);
                index += size;

                size = Convert.ToInt32(typeNamelength);
                byte[] namebuffer = new byte[size];
                fr.Read(typeNamelength, index, size);
                index += size;
                 
                size = sizeof(float);
                byte[] poslength = new byte[size];
                fr.Read(poslength, index, size);
                index += size;

                size = Convert.ToInt32(poslength);
                byte[] posBuffer = new byte[size];
                fr.Read(posBuffer, index, size);
                index += size;

            }
        }
    }
}