using Simple_Engine.Engine.Core.Interfaces;
using Simple_Engine.Engine.Geometry.Core;
using Simple_Engine.Engine.Space;
using Simple_Engine.ToolBox;
using OpenTK;
using Shared_Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simple_Engine.Engine.Space.Camera;
using Simple_Engine.Engine.GameSystem;

namespace Simple_Engine.Engine.Particles.Render
{
    public class ParticleMesh : Mesh3D
    {
        public Vector3 Velocity { get; set; }
        public Vector3 Position { get; set; }
        public float Rotation { get; set; }
        public float ScaleValue { get; set; }
        public float LifeLength { get; set; }

        private float elapsedTime = 0;
        public float GravityEffect = 0;

        private ParticleModel parentParticle;
        public int TextureLocation = 0;
        public Vector4 TextureOffset ;
        public float BlendValue = 0;

        public ParticleMesh(IDrawable2D parent) : base(parent)
        {
            parentParticle = parent as ParticleModel;
            IsBlended = true;
        }

        public void BuildMesh(Vector3 position, Vector3 velocity, float gravityEffect, float rotation, float scale, float lifeLength)
        {
            LocalTransform = Matrix4.Identity;
            Position = position;
            Velocity = velocity;
            GravityEffect = gravityEffect;
            DefaultColor = new Vector4(Randoms.Next(0.0f, 1f), Randoms.Next(0.0f, 1f), Randoms.Next(0.0f, 1f), 1);
            Rotation = rotation;
            this.ScaleValue = scale;
            LifeLength = lifeLength;
        }

        private bool UpdateParticle()
        {
            Velocity = Velocity + new Vector3(0, (float)(WorldSystem.GRAVITY * parentParticle.GravityEffect * (float)Game.Instance.RenderPeriod), 0);
            Vector3 change = new Vector3(Velocity);
            change *= (float)Game.Instance.RenderPeriod;
            Position += change * .1f;
            ScaleValue *= 0.995f;
            elapsedTime += (float)Game.Instance.RenderPeriod;
            if (elapsedTime > LifeLength)
            {
                return true;
            }
            else
            {
                UpdateMatrix();
                return false;
            }
        }

        public bool Live_Update()
        {
            if (UpdateParticle()) return true;
            var life = elapsedTime / LifeLength;
            var slots = Parent.TextureModel.numberOfRows * Parent.TextureModel.numberOfRows;
            var floatPosition = life * slots;
            BlendValue = floatPosition % 1;
            var slotId1 = (int)Math.Floor(floatPosition);
            var slotId2 = slotId1 < slotId1 - 1 ? slotId1 + 1 : slotId1;
            
            var x = Parent.TextureModel.GetTextureXOffset(slotId1);
            var y = Parent.TextureModel.GetTextureYOffset(slotId1);
            var TextureOffset1 = new Vector2(x, y);

             x = Parent.TextureModel.GetTextureXOffset(slotId1);
             y = Parent.TextureModel.GetTextureYOffset(slotId1);
            var TextureOffset2 = new Vector2(x, y);

            TextureOffset = new Vector4(TextureOffset1.X, TextureOffset1.Y, TextureOffset2.X, TextureOffset1.Y);


            return false;
        }

        private void UpdateMatrix()
        {
            LocalTransform = CameraModel.ActiveCamera.ViewTransform.KeepFacingCamera();
            MoveTo(Position);
            Rotate(Rotation += 10, new Vector3(0, 0, 1));
            Scale(ScaleValue);
        }
    }
}