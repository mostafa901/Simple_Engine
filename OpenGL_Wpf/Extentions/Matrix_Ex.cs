using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

namespace Simple_Engine.Extentions
{
    public static class Matrix_Ex
    {
        public static Matrix4 CreateTranslation(this Matrix4 mat, Vector3 translationVector)
        {
            mat = new Matrix4(new Vector4(), new Vector4(), new Vector4(), new Vector4(translationVector));
   
            return mat;
        }
    }
}