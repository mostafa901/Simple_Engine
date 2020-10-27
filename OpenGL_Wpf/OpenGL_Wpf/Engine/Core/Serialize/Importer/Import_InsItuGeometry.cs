using DocumentFormat.OpenXml.VariantTypes;
using InSitU.Views.ThreeD.Engine.Geometry;
using InSitU.Views.ThreeD.Engine.Geometry.Core;
using InSitU.Views.ThreeD.Extentions;
using OpenTK;
using Shared_Lib.Extention.Serialize_Ex;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InSitU.Views.ThreeD.Engine.Importer
{
    public static partial class Import
    {
        public static List<GeometryModel> InSituGeoFile(string path)
        {
            var datas = File.ReadAllText(path).JDeserialize<List<InSitu_GeometryModel>>();
            var geos = new List<GeometryModel>();
            var rmat = Matrix3.CreateRotationX(MathHelper.DegreesToRadians(90));

            foreach (var data in datas)
            {
                GeometryModel geo = new GeometryModel();
                geo.Name = data.Name;
                var dataPosotopns = data.Positions.GetVector3Array();
                foreach (var datapos in dataPosotopns)
                {
                    geo.Positions.Add(rmat * datapos);
                }
                var orgin = data.Origin.ToVector3();
                var pivot = data.PivotPoint.ToVector3();

                geo.Indeces = data.Indeces;
                geo.Normals.AddRange(data.Normals.GetVector3Array());
                geo.TextureCoordinates.AddRange(data.TextureCoordinates.GetVector2Array());

                geo.VertixColor.AddRange(data.FacesColor.GetVector4Array());

                 geos.Add(geo);
            }

            return geos;
        }
    }
}