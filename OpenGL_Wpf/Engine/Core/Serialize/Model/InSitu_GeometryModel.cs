
using System.Collections.Generic;

namespace Simple_Engine.Engine.Importer.Model
{
    public class Simple_Engine_GeometryModel
    {
        public List<float> Positions;

        public float[] PivotPoint { get;   set; }
        public float[] Rotate { get;   set; }
        public string Name { get;   set; }
        public List<float> FacesColor { get;   set; } 
        public float[] Origin { get;   set; }
        public List<int> Indeces { get; set; }
        public List<float> Normals { get; set; }
        public List<float> TextureCoordinates { get; set; }

        public bool isFamilyInstance { get; set; } = false;
        public Simple_Engine_GeometryModel()
        {
            Indeces = new List<int>();
            Positions = new List<float>();
            Normals = new List<float>();
            TextureCoordinates = new List<float>();
            FacesColor= new List<float>();
        }
    }
}

