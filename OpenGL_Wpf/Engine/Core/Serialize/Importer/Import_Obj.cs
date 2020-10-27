using Simple_Engine.Engine.Core.Serialize;
using Simple_Engine.Engine.Geometry;
using Simple_Engine.Engine.Geometry.Core;
using OpenTK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Engine.Engine.Importer
{
    public static partial class Import
    {
        public static GeometryModel OBJFile(string path)
        {
           
            var data = File.ReadAllLines(path);
            List<Vector3> vertices = new List<Vector3>();
            List<Vector2> textures = new List<Vector2>();
            List<Vector3> normals = new List<Vector3>();
            GeometryModel geo = new GeometryModel();
            foreach (var line in data)
            {
                if (line.StartsWith("vn "))
                {
                    var v = ExtractV3(line);
                    normals.Add(v);
                }
                else if (line.StartsWith("vt "))
                {
                    var v = ExtractV2(line);
                    textures.Add(v);
                }
                else if (line.StartsWith("v "))
                {
                    var v = ExtractV3(line);
                    vertices.Add(v);
                }
            }
            geo.Positions.AddRange(vertices);
            Vector2[] textureArray = new Vector2[vertices.Count];
            Vector3[] normalsArray = new Vector3[vertices.Count];
            Vector3[] normalTangentsArray = new Vector3[vertices.Count];
            foreach (var line in data)
            {
                if (line.StartsWith("f "))
                {
                    var splited = line.Split(' ');
                    List<int> points = new List<int>();
                    for (int i = 1; i < 4; i++)
                    {
                        var vtxt = splited[i];
                        var slashed = vtxt.Split('/');
                        //vertix  - texture - normal
                        int pointer = int.Parse(slashed[0]) - 1;
                        geo.Indeces.Add(pointer);
                        textureArray[pointer] = textures[int.Parse(slashed[1]) - 1];
                        normalsArray[pointer] = normals[int.Parse(slashed[2]) - 1];
                        points.Add(pointer);
                    }
                }
            }
            geo.TextureCoordinates = textureArray.ToList();
            geo.Normals = normalsArray.ToList();

          

            return geo;
        }

        private static Vector3 ExtractV3(string line)
        {
            string[] floats = line.Split(' ');
            var v = new Vector3(float.Parse(floats[1]), float.Parse(floats[2]), float.Parse(floats[3]));
            return v; ;
        }

        private static Vector2 ExtractV2(string line)
        {
            string[] floats = line.Split(' ');
            var v = new Vector2(float.Parse(floats[1]), float.Parse(floats[2]));
            return v; ;
        }
    }
}