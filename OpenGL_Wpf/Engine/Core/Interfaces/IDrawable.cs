﻿using OpenTK;
using OpenTK.Graphics.OpenGL;
using Simple_Engine.Engine.Core.Abstracts;
using Simple_Engine.Engine.Core.Events;
using Simple_Engine.Engine.Illumination;
using Simple_Engine.Engine.Particles;
using Simple_Engine.Engine.Render;
using System;
using System.Collections.Generic;

namespace Simple_Engine.Engine.Core.Interfaces
{
    public interface IDrawable : IRenderable
    {
        event EventHandler<MoveingEvent> MoveEvent;

        [Flags]
        public enum DynamicFlag
        {
            Static,
            Positions,
            Normals,
            TextureCoordinates
        }

        public DynamicFlag Dynamic { get; set; }

        public bool AllowReflect { get; set; }
        public bool CanBeSaved { get; set; }
        CullFaceMode CullMode { get; set; }
        PrimitiveType DrawType { get; set; }

        List<int> Indeces { get; set; }
        public bool IsActive { get; set; }
        public bool IsBlended { get; set; }
        bool IsSystemModel { get; set; }
        Matrix4 LocalTransform { get; set; }
        List<Vector3> Normals { get; set; }
        List<Vector3> NormalTangent { get; set; }
        public List<ParticleModel> Particles { get; set; }
        public Vector3 PivotPoint { get; set; }
        public bool RecieveShadow { get; set; }
        EngineRenderer Renderer { get; set; }
        Shader ShaderModel { get; set; }
        List<Vector2> TextureCoordinates { get; set; }
        Base_Texture TextureModel { get; set; }
        public string Uid { get; set; }

        void ActivateShadowMap(LightModel lightSource);

        void MoveLocal(Vector3 displacement);

        void MoveTo(Vector3 position);

        void MoveTo(float x, float y, float z);

        void MoveWorld(Vector3 displacement);

        void Rotate(float angle, Vector3 axis);

        void Scale(float x);

        void Scale(float x, float y, float z);

        void Scale(Vector3 scalarVector);

        void SetHeight(float height);

        void Setup_Indeces();

        void Setup_Normals();

        void Setup_Position();

        void Setup_TextureCoordinates(float xScale = 1, float yScale = 1);

        void SetWidth(float width);

        void ScaleTo(float x, float y, float z);
    }
}