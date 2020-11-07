using OpenTK;
using Shared_Lib.IO;
using System.Collections.Generic;

namespace Simple_Engine.Extentions
{
    public static class Matrix_Ex
    {
        public static Matrix4 CreateTranslation(this Matrix4 mat, Vector3 translationVector)
        {
            mat = new Matrix4(new Vector4(), new Vector4(), new Vector4(), new Vector4(translationVector));

            return mat;
        }

        public static float[] GetArray(this Matrix4 mat)
        {
            List<Vector4> vecs = new List<Vector4>();
            vecs.Add(mat.Row0);
            vecs.Add(mat.Row1);
            vecs.Add(mat.Row2);
            vecs.Add(mat.Row3);
            return vecs.GetArray();
        }

        public static Matrix4 GetMatrix(this byte[] byts)
        {
            float[] matfloat = byts.ToFloatArray();
            return matfloat.ToMatrix4();
        }

        public static Matrix4 ToMatrix4(this float[] matfloat)
        {
            int i = -1;
            return new Matrix4(
            matfloat[++i],
            matfloat[++i],
            matfloat[++i],
            matfloat[++i],

            matfloat[++i],
            matfloat[++i],
            matfloat[++i],
            matfloat[++i],

            matfloat[++i],
            matfloat[++i],
            matfloat[++i],
            matfloat[++i],

            matfloat[++i],
            matfloat[++i],
            matfloat[++i],
            matfloat[++i]

            );
        }
    }
}