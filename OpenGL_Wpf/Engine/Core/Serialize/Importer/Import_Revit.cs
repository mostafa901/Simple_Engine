using OpenTK;
using Shared_Lib.Extention.Serialize_Ex;
using Shared_Lib.MVVM;
using Simple_Engine.Engine.Core.Static;
using Simple_Engine.Engine.GameSystem;
using Simple_Engine.Engine.Geometry.Core;
using Simple_Engine.Engine.Importer.Model;
using Simple_Engine.Engine.Space.Scene;
using Simple_Engine.Extentions;
using System.Collections.Generic;
using System.IO;

namespace Simple_Engine.Engine.Importer
{
    public static partial class Import
    {
        private static int linecount = 0;

        public static void Revit_ExportFile(string filename)
        {
            

            var cmd = new cus_CMD();
            Game.Instance.RenderOnUIThread(cmd);

            using (var reader = new StreamReader(filename))
            {
                string line = "";

                while ((line = reader.ReadLine()) != null)
                {
                    linecount += 1;

                    if (string.IsNullOrEmpty(line)) continue;
                    GeometryModel geo = ImportRevitJsonString(line, cmd);
                    
                    SceneModel.ActiveScene.UpLoadModels(geo);

                }

                cmd.Action = (x) =>
                {
                    Game.Instance.Dispose_RenderOnUIThread(cmd);
                };
            }

           
        }

        private static GeometryModel ImportRevitJsonString(string line, cus_CMD cmd)
        {
            cmd.Action = (x) =>
            {
                UI_Shared.Render_Progress(linecount, 350, $"Parsing...");
            };

            var data = line.JDeserialize<Simple_Engine_GeometryModel>(Core.Serialize.JsonTools.GetSettings());

            var rmat = Matrix3.CreateRotationX(MathHelper.DegreesToRadians(90));

            GeometryModel geo = new GeometryModel();
            geo.ShaderModel = new Render.Shader(Render.ShaderMapType.Blend, Render.ShaderPath.SingleColor);
            geo.Name = data.Name;
            geo.Uid = data.Uid;
            cmd.Action = (x) =>
            {
                UI_Shared.Render_Progress(linecount, 350, $"Loading {geo.Name}");
            };
            var dataPosotopns = data.Positions.GetVector3Array();
            foreach (var datapos in dataPosotopns)
            {
                geo.Positions.Add(rmat * datapos);
            }

            geo.Indeces = data.Indeces;
            geo.Normals.AddRange(data.Normals.GetVector3Array());
            geo.TextureCoordinates.AddRange(data.TextureCoordinates.GetVector2Array());

            geo.VertixColor.AddRange(data.FacesColor.GetVector4Array());

            return geo;
        }
    }
}