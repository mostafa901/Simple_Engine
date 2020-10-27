using com.sun.org.apache.bcel.@internal.generic;
using InSitU.Views.ThreeD.Engine.Core.Abstracts;
using InSitU.Views.ThreeD.Engine.Core.Events;
using InSitU.Views.ThreeD.Engine.Core.Interfaces;
using InSitU.Views.ThreeD.Engine.Geometry.Axis;
using InSitU.Views.ThreeD.Engine.Geometry.Core;
using InSitU.Views.ThreeD.Engine.Geometry.InputControls;
using InSitU.Views.ThreeD.Engine.Geometry.Render;
using InSitU.Views.ThreeD.Engine.Illumination;
using InSitU.Views.ThreeD.Engine.Particles;
using InSitU.Views.ThreeD.Engine.Render;
using InSitU.Views.ThreeD.Extentions;
using InSitU.Views.ThreeD.ToolBox;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using org.apache.poi.poifs.property;
using org.apache.poi.ss.formula.functions;
using Shared_Lib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static InSitU.Views.ThreeD.Engine.Core.Interfaces.IDrawable;
using static InSitU.Views.ThreeD.Engine.Core.Interfaces.IRenderable;

namespace InSitU.Views.ThreeD.Engine.Core.Abstracts
{
    public abstract class Base_Geo3D : Base_Geo, IDrawable3D
    {
        public Base_Geo3D(Base_Geo3D sourceModel)
        {
            CloneModel(sourceModel);
        }

        protected Base_Geo3D()
        {
            Positions = new List<Vector3>();
        }

        public List<Vector3> Positions { get; set; }

        private float Depth { get; set; }

        //you need to enable Instancing in Renderer to get this to work
        public virtual Mesh3D AddMesh(Matrix4 mat)
        {
            UpdateBoundingBox();

            var mesh = new Mesh3D((IDrawable3D)this);
            mesh.LocalTransform = mat;
            mesh.Floor = Floor;
            Meshes.Add(mesh);

            ShaderModel.EnableInstancing = true;

            return mesh;
        }

        public override void CloneModel(Base_Geo sourceModel)
        {
            base.CloneModel(sourceModel);
            Positions = ((Base_Geo3D)sourceModel).Positions;
        }

        public void Default_RenderModel()
        {
            if (Renderer == null)
            {
                Renderer = new GeometryRenderer((Base_Geo3D)this);
            }

            Renderer.RenderModel();
            ShaderModel.UploadDefaults(this);
        }

        public virtual float GetDepth()
        {
            return Depth;
        }

        public IntersectionResult Intersect(Vector3 worldRay, Vector3 cameraPosition)
        {
            var objectPos = LocalTransform.ExtractTranslation();
            var rayStart = cameraPosition;

            var faces = generatefaces();

            IntersectionResult res = new IntersectionResult() { Distance = 600 };

            //https://en.wikipedia.org/wiki/Line%E2%80%93plane_intersection#:~:text=In%20analytic%20geometry%2C%20the%20intersection,the%20plane%20but%20outside%20it.

            foreach (var face in faces)
            {
                var normalVector = ToolBox.eMath.GetNormal(face.v0, face.v1, face.v2);

                //Get how far this Plan from Origin
                float d = Vector3.Dot(face.v1 - face.v0, normalVector);

                if (Vector3.Dot(worldRay, normalVector) == 0)
                {
                    //avoid Dividing by Zero
                    return new IntersectionResult();
                }

                // Compute the t value for the directed line ray intersecting the plane
                float t = (Vector3.Dot(face.v0 - rayStart, normalVector)) / Vector3.Dot(worldRay, normalVector);

                //If worldRay Dot Normal = 0 then the line and plane are parallel. (either the line lies in the plan (intersects in each point, or far parallel)

                if (t == 0)
                {
                    //Point is parallel to plan || perpendicular to Normal Vector
                    continue;
                }
                var contact = rayStart + t * worldRay;
                if (eMath.PointinTriangle(face.v0, face.v1, face.v2, contact))
                {
                    if (t < res.Distance)
                    {
                        res = new IntersectionResult
                        {
                            Distance = t,
                            ModelFace = face,
                            NormalPlan = normalVector
                        };
                    }
                }
            }

            return res;
        }

        public virtual void SetDepth(float value)
        {
            Depth = value;
        }

        public override void UpdateBoundingBox()
        {
            base.UpdateBoundingBox();
            var locpos = LocalTransform.ExtractTranslation();
            BBX = new BoundingBox
            {
                Width = BBX.Width,
                Height = BBX.Height,
                Depth = (LocalTransform * new Vector4(0, 0, GetDepth(), 0)).Z,
                Max = Positions.Any() ? locpos + new Vector3(Positions.Max(o => o.X), Positions.Max(o => o.Y), Positions.Max(o => o.Z)) : new Vector3(),
                Min = Positions.Any() ? locpos + new Vector3(Positions.Min(o => o.X), Positions.Min(o => o.Y), Positions.Min(o => o.Z)) : new Vector3(),
                CG = Positions.Any() ? locpos + new Vector3(Positions.Average(o => o.X), Positions.Average(o => o.Y), Positions.Average(o => o.Z)) : new Vector3()
            };
        }

        private List<face> generatefaces()
        {
            List<face> faces = new List<face>();
            var pos = LocalTransform.ExtractTranslation();
            for (int i = 0; i < Indeces.Count; i += 3)
            {
                faces.Add(new face()
                {
                    v0 = pos + Positions.ElementAt(Indeces[i]),
                    v1 = pos + Positions.ElementAt(Indeces[i + 1]),
                    v2 = pos + Positions.ElementAt(Indeces[i + 2])
                });
            }

            return faces;
        }

        public struct face
        {
            public Vector3 v0;
            public Vector3 v1;
            public Vector3 v2;
        }

        public struct IntersectionResult
        {
            public float Distance;
            public face ModelFace;
            public Vector3 NormalPlan;
        }
    }
}