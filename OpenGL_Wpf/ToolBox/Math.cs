using Simple_Engine.Engine;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Engine.ToolBox
{
    public static class eMath
    {
        public static Matrix4 MoveLocal(Matrix4 transform, Vector3 axis)
        {
            var rot = transform.ExtractRotation();
            transform = Matrix4.CreateFromQuaternion(rot.Inverted()) * transform;
            var matTrans = Matrix4.CreateTranslation(axis);

            transform = matTrans * transform;
            transform = Matrix4.CreateFromQuaternion(rot) * transform;

            return transform;
        }

        public static Matrix4 MoveWorld(Matrix4 transform, Vector3 axis)
        {
            var matTrans = Matrix4.CreateTranslation(axis);
            transform = matTrans * transform;

            return transform;
        }

        public static Matrix4 Rotate(Matrix4 transform, float noOfRounds, Vector3 axisAngles)
        {
            var radianAngleX = MathHelper.DegreesToRadians(noOfRounds * axisAngles.X);
            var radianAngleY = MathHelper.DegreesToRadians(noOfRounds * axisAngles.Y);
            var radianAngleZ = MathHelper.DegreesToRadians(noOfRounds * axisAngles.Z);
            var angMatY = axisAngles.Y != 0 ? Matrix3.CreateRotationY(radianAngleY) : Matrix3.Identity;
            var angMatZ = axisAngles.Z != 0 ? Matrix3.CreateRotationZ(radianAngleZ) : Matrix3.Identity;
            var angMatX = axisAngles.X != 0 ? Matrix3.CreateRotationX(radianAngleX) : Matrix3.Identity;
            var angMat = angMatX * angMatY * angMatZ;

            return new Matrix4(angMat) * transform;
        }

        public static Matrix4 Scale(Matrix4 transform, Vector3 value)
        {
            return new Matrix4(Matrix3.CreateScale(value)) * transform;
        }

        internal static Matrix4 MoveTo(Matrix4 transform, Vector3 position)
        {
            var clearedtran = transform.ClearTranslation();
            clearedtran.Row3 = new Vector4(position, 1);
            return clearedtran;
        }

        public static float GetHeight(Vector3 p1, Vector3 p2, Vector3 p3, Vector2 pos)
        {
            float det = (p2.Z - p3.Z) * (p1.X - p3.X) + (p3.X - p2.X) * (p1.Z - p3.Z);
            float l1 = ((p2.Z - p3.Z) * (pos.X - p3.X) + (p3.X - p2.X) * (pos.Y - p3.Z)) / det;
            float l2 = ((p3.Z - p1.Z) * (pos.X - p3.X) + (p1.X - p3.X) * (pos.Y - p3.Z)) / det;
            float l3 = 1.0f - l1 - l2;
            return l1 * p1.Y + l2 * p2.Y + l3 * p3.Y;
        }

        public static Vector3 GetNormal(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            var l1 = v2 - v1;
            var l2 = v3 - v1;
            return Vector3.Cross(l1, l2).Normalized();
        }

        public static List<Vector3> CalculateTangents(List<Vector3> vertices, List<int> indecis,
         List<Vector2> textures)
        {
            Vector3[] tangents = new Vector3[vertices.Count];
            for (int i = 0; i < indecis.Count; i += 3)
            {
                var ind0 = indecis[i];
                var ind1 = indecis[i + 1];
                var ind2 = indecis[i + 2];

                var v0 = vertices[ind0];
                var v1 = vertices[ind1];
                var v2 = vertices[ind2];

                var uv0 = textures[ind0];
                var uv1 = textures[ind1];
                var uv2 = textures[ind2];

                Vector3 delatPos1 = Vector3.Subtract(v1, v0);
                Vector3 delatPos2 = Vector3.Subtract(v2, v0);

                Vector2 deltaUv1 = Vector2.Subtract(uv1, uv0);
                Vector2 deltaUv2 = Vector2.Subtract(uv2, uv0);

                //Vector3 deltaUv1 = delatPos1;
                //Vector3 deltaUv2 = delatPos2;

                Vector3 tangent = new Vector3();
                float det = (deltaUv1.X * deltaUv2.Y - deltaUv1.Y * deltaUv2.X);
                if (det == 0)
                {
                    tangent = new Vector3(1, 0, 0);
                }
                else
                {
                    float r = 1 / det;

                    delatPos1 *= deltaUv2.Y;
                    delatPos2 *= deltaUv1.Y;
                    tangent = Vector3.Subtract(delatPos1, delatPos2);
                    tangent *= r;

                    tangents[ind0] = (tangents[ind0] + tangent);
                    tangents[ind1] = (tangents[ind1] + tangent);
                    tangents[ind2] = (tangents[ind2] + tangent);
                }
            }

            var normalizedTangents = tangents.ToList();
            return normalizedTangents;
        }

        public static Matrix4 KeepFacingCamera(this Matrix4 cameraTransform)
        {
            //this method returns a transposed transform "inversed to the camera Rotations"
            var v3 = new Matrix3(cameraTransform);
            v3.Transpose();
            return new Matrix4(v3);
        }

        public static List<Vector3> Negate(this List<Vector3> vector3s, Vector3 value)
        {
            for (int i = 0; i < vector3s.Count; i++)
            {
                var norm = vector3s.ElementAt(i);
                vector3s[i] = norm * value;
            }

            return vector3s;
        }

        internal static bool PointinTriangle(Vector3 v0, Vector3 v1, Vector3 v2, Vector3 contact)
        {
            float triangleArea = GetTriangleArea(v0, v1, v2);
            float s1 = GetTriangleArea(v1, contact, v2);
            float s2 = GetTriangleArea(contact, v0, v2);
            float s3 = GetTriangleArea(contact, v0, v1);
            return Math.Round(s1 + s2 + s3, 4) == Math.Round(triangleArea, 4);
        }

        private static float GetTriangleArea(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            return .5f * Vector3.Cross(v2 - v1, v3 - v1).Length;
        }

        public static Vector3 ProjectVectors(this Vector3 vecA, Vector3 vecB)
        {
            return vecB * (Vector3.Dot(vecA, vecB) / Vector3.Dot(vecB, vecB));
        }
    }
}