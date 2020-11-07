using OpenTK;
using OpenTK.Graphics.OpenGL;
using Shared_Lib.Extention;
using Shared_Lib.Extention.Serialize_Ex;
using Shared_Lib.IO;
using Shared_Lib.MVVM;
using Simple_Engine.Engine.Core.Interfaces;
using Simple_Engine.Engine.Core.Static;
using Simple_Engine.Engine.GameSystem;
using Simple_Engine.Engine.Geometry.Core;
using Simple_Engine.Engine.Space.Scene;
using Simple_Engine.Extentions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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

        public static void SaveBinary(IEnumerable<IDrawable> models, string path)
        {
            using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                var numberofModels = models.Count().ToBytes();
                fs.Write(numberofModels, 0, 4);

                foreach (var model in models)
                {
                    var buffers = model.GetAsBinary();
                    foreach (var buf in buffers)
                    {
                        fs.Write(buf, 0, buf.Length);
                    }
                }
            }
        }

        private enum BufferName
        {
            Positions = 0,
            Indeces = 1,
            Normals = 2,
            Shader = 4,
            TangentNormal = 8,
            DefaultColor = 16,
            Transform = 32
        }

        public static List<byte[]> GetAsBinary(this IDrawable model)
        {
            byte[] posbuffer = GetBuffer(((List<Vector3>)((dynamic)model).Positions).GetArray(), BufferName.Positions);
            byte[] indexbuffer = GetBuffer(model.Indeces.ToArray(), BufferName.Indeces);
            byte[] normalbuffer = GetBuffer(model.Normals.GetArray(), BufferName.Normals);
            byte[] matbuffer = GetBuffer(model.LocalTransform.GetArray(), BufferName.Transform);
            byte[] defaultColorbuffer = GetBuffer(model.DefaultColor.ToFloat(), BufferName.DefaultColor);
            byte[] normalTangentbuffer = GetBuffer(model.NormalTangent.GetArray(), BufferName.TangentNormal);
            byte[] shaderbuffer = GetBuffer(new int[] { (int)model.ShaderModel.ShaderModelType }, BufferName.Shader);

            return new List<byte[]> { posbuffer, indexbuffer, normalbuffer, matbuffer, defaultColorbuffer, normalTangentbuffer, shaderbuffer };
        }

        async public static void LoadBinary(string path)
        {
            var cmd = new cus_CMD();
            Game.Instance.RenderOnUIThread(cmd);
            cmd.Action = (a) =>
            {
                UI_Shared.Render_Progress(0, 100, "Loading File Please wait");
            };
            await Task.Delay(7);

            await Task.Run(async () =>
             {
                 using (var fr = new FileStream(path, FileMode.Open, FileAccess.Read))
                 {
                     byte[] buffernumbderofModels = new byte[4];
                     fr.Read(buffernumbderofModels, 0, 4);
                     int numbers = BitConverter.ToInt16(buffernumbderofModels, 0);

                     int sectionscount = Enum.GetNames(typeof(BufferName)).Length;

                     for (int i = 0; i < numbers; i++)
                     {
                         cmd.Action = (a) =>
                         {
                             UI_Shared.Render_Progress(i, numbers, "Loading Model");
                         };
                         int index = i * sectionscount;

                         //the more sections you add, you need to update section count
                         int x = -1;
                         byte[] posBuffer = ReadBuffer(fr, ++x + index, BufferName.Positions);
                         byte[] indexBuffer = ReadBuffer(fr, ++x + index, BufferName.Indeces);
                         byte[] normalsBuffer = ReadBuffer(fr, ++x + index, BufferName.Normals);
                         byte[] matsBuffer = ReadBuffer(fr, ++x + index, BufferName.Transform);
                         byte[] defaultColorBuffer = ReadBuffer(fr, ++x + index, BufferName.DefaultColor);
                         byte[] normalTangentBuffer = ReadBuffer(fr, ++x + index, BufferName.TangentNormal);
                         byte[] shaderBuffer = ReadBuffer(fr, ++x + index, BufferName.Shader);

                         int shadertype = shaderBuffer.Select(o => (int)Convert.ToInt16(o)).First();

                         Game.Instance.RunOnUIThread(() =>
                         {
                             var line = new GeometryModel();
                             line.DefaultColor = defaultColorBuffer != null ? defaultColorBuffer.ToFloatArray().ToVector4() : new Vector4(.5f);
                             line.LocalTransform = matsBuffer != null ? matsBuffer.GetMatrix() : Matrix4.Identity;

                             line.ShaderModel = new Render.Shader(Render.ShaderPath.SingleColor);
                             line.UploadVAO();
                             GL.BindVertexArray(line.Renderer.VAO);
                             line.Renderer.StoreDataInAttributeList(line.ShaderModel.PositionLayoutId, posBuffer, 3, 0);
                             if (normalsBuffer != null && normalsBuffer.Any())
                             {
                                 line.Renderer.StoreDataInAttributeList(line.ShaderModel.NormalLayoutId, normalsBuffer, 3, 0);
                             }
                             if (normalTangentBuffer != null && normalTangentBuffer.Any())
                             {
                                 line.Renderer.EnableNormalTangent = true;
                                 line.Renderer.StoreDataInAttributeList(line.ShaderModel.TangentLayoutId, normalTangentBuffer, 3, 0);
                             }
                             line.Renderer.BindIndicesBuffer<int>(indexBuffer);

                             line.Renderer.PositionBufferLength = posBuffer.Length / (3 * sizeof(float));
                             GL.BindVertexArray(0);

                             SceneModel.ActiveScene.geoModels.Add(line);
                         });

                         await Task.Delay(5);
                     }
                     cmd.Action = (a) =>
                     {
                         Game.Instance.Dispose_RenderOnUIThread(cmd);
                     };
                 }
             });
        }

        private static byte[] ReadBuffer(FileStream fr, int index, BufferName buffname)
        {
            fr.Position = 0;
            fr.Position += 4; //reserved for the number of models

            for (int i = 0; i < index; i++)
            {
                //set position in a reference to the header length
                int headersize = sizeof(int);
                byte[] headerlength = new byte[headersize];
                fr.Read(headerlength, 0, headersize);
                fr.Position += BitConverter.ToInt32(headerlength, 0) - 4;
            }

            fr.Position += 4; //skip header length

            int size = sizeof(int);
            byte[] typeNamelength = new byte[size];
            fr.Read(typeNamelength, 0, size);

            size = BitConverter.ToInt32(typeNamelength, 0);
            byte[] namebuffer = new byte[size];
            fr.Read(namebuffer, 0, size);
            if (BitConverter.ToInt32(namebuffer, 0) != (int)buffname) return null;

            size = sizeof(int);
            byte[] poslength = new byte[size];
            fr.Read(poslength, 0, size);

            size = BitConverter.ToInt32(poslength, 0);
            byte[] posBuffer = new byte[size];
            fr.Read(posBuffer, 0, size);

            return posBuffer;
        }

        private static byte[] GetBuffer<T>(T[] data, BufferName buffname)
        {
            var namebuffer = ((int)buffname).ToBytes();
            byte[] namelengthBuffer = namebuffer.Length.ToBytes();

            var databytes = data.ToByteArray();
            byte[] posLengthBuffer = databytes.Length.ToBytes();

            int headerintLength = 4;
            int nameintLength = 4;
            int namebyteLength = namebuffer.Length;
            int posintLength = 4;
            int posbyteLength = databytes.Length;

            int totalBufferLength = headerintLength + nameintLength + namebyteLength + posintLength + posbyteLength;

            byte[] headerlengthBuffer = totalBufferLength.ToBytes();

            byte[] buffer = new byte[totalBufferLength];
            int index = 0;

            Array.Copy(headerlengthBuffer, 0, buffer, index, headerlengthBuffer.Length);
            index += headerlengthBuffer.Length;

            Array.Copy(namelengthBuffer, 0, buffer, index, namelengthBuffer.Length);
            index += namelengthBuffer.Length;

            Array.Copy(namebuffer, 0, buffer, index, namebuffer.Length);
            index += namebuffer.Length;

            Array.Copy(posLengthBuffer, 0, buffer, index, posLengthBuffer.Length);
            index += posLengthBuffer.Length;

            Array.Copy(databytes, 0, buffer, index, databytes.Length);

            return buffer;
        }
    }
}