using OpenTK;
using OpenTK.Graphics;
using Shared_Lib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Simple_Engine.Extentions
{
    public static class Vector_Ex
    {
        public static string String_ex(this Vector3D vector)
        {
            return $"X:{vector.X}\nY: { vector.Y}\nZ: { vector.Z}";
        }

        public static string String_ex(this Point point)
        {
            return $"X:{point.X}\nY: { point.Y}";
        }

        public static string String_ex(this System.Windows.Point point)
        {
            return $"X:{point.X}\nY: { point.Y}";
        }

        public static Point3D ToPoint3D(this Vector3D vec)
        {
            return new Point3D(vec.X, vec.Y, vec.Z);
        }

        public static float[] ToFloat(this Vector3 vec)
        {
            return new float[3] { vec.X, vec.Y, vec.Z };
        }

        public static float[] ToFloat(this Vector2 vec)
        {
            return new float[2] { vec.X, vec.Y };
        }

        public static float[] ToFloat(this Vector4 vec)
        {
            return new float[4] { vec.X, vec.Y, vec.Z, vec.W };
        }

        public static float[] ToFloat(this Color4 vec)
        {
            return new float[4] { vec.R, vec.G, vec.B, vec.A };
        }

        public static Color4 Randomize(this Color4 vec)
        {
            return new Color4(vec.R * Randoms.Next(0, 1), vec.G * Randoms.Next(0, 1), vec.B * Randoms.Next(0, 1), vec.A);
        }

        public static Vector4 ToVector4(this Color4 color)
        {
            return new Vector4(color.R, color.G, color.B, color.A);
        }

        public static float[] GetArray(this IEnumerable<Vector2> list)
        {
            List<float> points = new List<float>();
            for (int i = 0; i < list.Count(); i++)
            {
                points.AddRange(list.ElementAt(i).ToFloat());
            }
            return points.ToArray();
        }

        public static float[] GetArray(this IEnumerable<Vector3> list)
        {
            List<float> points = new List<float>();
            for (int i = 0; i < list.Count(); i++)
            {
                points.AddRange(list.ElementAt(i).ToFloat());
            }
            return points.ToArray();
        }

        public static float[] GetArray(this IEnumerable<Vector4> list)
        {
            List<float> points = new List<float>();
            for (int i = 0; i < list.Count(); i++)
            {
                points.AddRange(list.ElementAt(i).ToFloat());
            }
            return points.ToArray();
        }

        public static List<Vector3> GetVector3Array(this IEnumerable<float> list)
        {
            List<Vector3> points = new List<Vector3>();
            for (int i = 0; i < list.Count(); i += 3)
            {
                points.Add(list.Skip(i).Take(3).ToVector3());
            }
            return points;
        }

        public static List<Vector2> GetVector2Array(this IEnumerable<float> list)
        {
            List<Vector2> points = new List<Vector2>();
            for (int i = 0; i < list.Count(); i += 3)
            {
                points.Add(list.Skip(i).Take(2).ToVector2());
            }
            return points;
        }

        public static List<Vector4> GetVector4Array(this IEnumerable<float> list)
        {
            List<Vector4> points = new List<Vector4>();
            for (int i = 0; i < list.Count(); i += 4)
            {
                points.Add(list.Skip(i).Take(4).ToVector4());
            }
            return points;
        }

        public static Vector2 ToVector2(this IEnumerable<float> list)
        {
            return new Vector2(list.ElementAt(0), list.ElementAt(1));
        }

        public static Vector3 ToVector3(this IEnumerable<float> list)
        {
            return new Vector3(list.ElementAt(0), list.ElementAt(1), list.ElementAt(2));
        }

        public static Vector4 ToVector4(this IEnumerable<float> list)
        {
            return new Vector4(list.ElementAt(0), list.ElementAt(1), list.ElementAt(2), list.ElementAt(3));
        }

        public static System.Numerics.Vector3 ToSystemNumeric(this Vector3 vec3)
        {
            return new System.Numerics.Vector3(vec3.X, vec3.Y, vec3.Z);
        }

        public static Vector3 ToVector(this System.Numerics.Vector3 vec3)
        {
            return new Vector3(vec3.X, vec3.Y, vec3.Z);
        }

        public static System.Numerics.Vector4 ToSystemNumeric(this Vector4 vec4)
        {
            return new System.Numerics.Vector4(vec4.X, vec4.Y, vec4.Z, vec4.W);
        }

        public static Vector4 ToVector(this System.Numerics.Vector4 vec4)
        {
            return new Vector4(vec4.X, vec4.Y, vec4.Z, vec4.W);
        }

        public static float Max_X(this Vector3 v1, Vector3 v2)
        {
            return Math.Max(v1.X, v2.X);
        }

        public static float Min_X(this Vector3 v1, Vector3 v2)
        {
            return Math.Min(v1.X, v2.X);
        }

        public static float Max_Y(this Vector3 v1, Vector3 v2)
        {
            return Math.Max(v1.Y, v2.Y);
        }

        public static float Min_Y(this Vector3 v1, Vector3 v2)
        {
            return Math.Min(v1.Y, v2.Y);
        }

        public static float Max_Z(this Vector3 v1, Vector3 v2)
        {
            return Math.Max(v1.Z, v2.Z);
        }

        public static float Min_Z(this Vector3 v1, Vector3 v2)
        {
            return Math.Min(v1.Z, v2.Z);
        }

        public static Vector4 Round(this Vector4 vec, int round)
        {
            return new Vector4(

            (float)Math.Round(vec.X, round),
            (float)Math.Round(vec.Y, round),
            (float)Math.Round(vec.Z, round),
            (float)Math.Round(vec.W, round)
            );
        }
    }
}