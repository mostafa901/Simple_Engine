using Simple_Engine.Engine.Core.Abstracts;
using Simple_Engine.Engine.Core.Interfaces;
using Simple_Engine.Engine.Geometry.Render;
using Simple_Engine.Engine.Render;
using Simple_Engine.Engine.Render.ShaderSystem;
using System;
using System.Linq;

namespace Simple_Engine.Engine.Geometry.Core
{
    public class GeometryModel : Base_Geo3D
    {
        public byte[] vertexBuffer;

        public GeometryModel()
        {
            CastShadow = true;
        }

        public override float GetWidth()
        {
            return Positions.Max(o => o.X) - Positions.Min(o => o.X);
        }

        public override float GetHeight()
        {
            return Positions.Max(o => o.Y) - Positions.Min(o => o.Y);
        }

        public override float GetDepth()
        {
            return Positions.Max(o => o.Z) - Positions.Min(o => o.Z);
        }

        public override void BuildModel()
        {
            throw new NotImplementedException();
        }

        public override void Setup_Indeces()
        {
            throw new NotImplementedException();
        }

        public override void Setup_Normals()
        {
            throw new NotImplementedException();
        }

        public override void Setup_Position()
        {
            throw new NotImplementedException();
        }

        public override void Setup_TextureCoordinates(float xScale = 1, float yScale = 1)
        {
            throw new NotImplementedException();
        }

        public override void UploadVAO()
        {
            Renderer = new GeometryRenderer(this);
            Default_RenderModel();
        }

        public override void Live_Update(Base_Shader ShaderModel)
        {
            base.Live_Update(ShaderModel);
            ShaderModel.SetMatrix4(ShaderModel.Location_LocalTransform, LocalTransform);
        }
    }
}