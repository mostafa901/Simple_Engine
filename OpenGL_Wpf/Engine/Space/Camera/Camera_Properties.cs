using Newtonsoft.Json;
using OpenTK;
using Simple_Engine.Engine.Core.Abstracts;
using Simple_Engine.Engine.Core.Interfaces;
using Simple_Engine.Engine.Geometry.ThreeDModels.Clips;
using Simple_Engine.Engine.Space.Scene;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Simple_Engine.Engine.Space.Camera
{
    public partial class CameraModel
    {
        public static CameraModel ActiveCamera;

        public Vector4 DefaultColor { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool CastShadow { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public static List<ClipPlan> ClipPlans { get; set; }

        private Base_Geo hoockedModel;
        private SceneModel scene;

        private Point StartPoint = new Point();

        [JsonIgnore]
        public IRenderable.BoundingBox BBX { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool IsDirectionVisible;
        public Vector3 Direction { get; set; } = new Vector3(0, 0, 1);

        public float FarDistance { get; set; } = 2000f;

        public float FOV { get; private set; } = 45;

        public float Height { get; set; } = 10;

        public int Id { get; set; }

        public bool IsPerspective { get; set; } = false;

        public string Name { get; set; }

        public float NearDistance { get; set; } = .1f;

        public float Pitch { get; set; }

        public Vector3 Position { get; set; }

        public Matrix4 ProjectionTransform { get; set; } = Matrix4.Identity;

        public Vector3 Right { get; set; } = new Vector3(1, 0, 0);

        public float Roll { get; set; }

        public Vector3 Target { get; set; }

        public Vector3 UP { get; set; } = new Vector3(0, 1, 0);

        public Matrix4 ViewTransform { get; set; } = Matrix4.Identity;

        public float Width { get; set; } = 10;

        //how much we are looking up - down wards
        public float Yaw { get; set; }
    }
}