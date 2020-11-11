using Newtonsoft.Json;
using OpenTK;

using OpenTK.Graphics.OpenGL;
using Simple_Engine.Engine.Core.Interfaces;
using Simple_Engine.Engine.Geometry.Core;
using Simple_Engine.Engine.Geometry.Render;
using Simple_Engine.Engine.Geometry.SystemModel.Clips;
using Simple_Engine.Engine.Render;
using Simple_Engine.Engine.Render.ShaderSystem;
using Simple_Engine.Engine.Space.Camera;
using Simple_Engine.Engine.Space.Scene;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using static Simple_Engine.Engine.Core.Interfaces.IRenderable;

namespace Simple_Engine.Engine.Core.Abstracts
{
    public abstract class Base_Geo3D : Base_Geo, IDrawable3D
    {
        public Base_Geo3D(Base_Geo3D sourceModel)
        {
            CloneModel(sourceModel);
        }

        [JsonIgnore]
        public bool EnableClipPlans { get; private set; } = false;

        [JsonIgnore]
        public List<ClipPlan> ClipPlans { get; set; }

        protected Base_Geo3D()
        {
            Positions = new List<Vector3>();
            ClipPlans = new List<ClipPlan>();
        }

        public List<Vector3> Positions { get; set; }

        private float Depth { get; set; }
        public static Vector3 SelectedVertex { get; internal set; }

        //you need to enable Instancing in Renderer to get this to work
        public virtual Mesh3D AddMesh(Matrix4 mat)
        {
            UpdateBoundingBox();

            var mesh = new Mesh3D((IDrawable3D)this);
            mesh.LocalTransform = mat;
            mesh.Floor = Floor;
            Meshes.Add(mesh);

            GetShaderModel().EnableInstancing = true;

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
            GetShaderModel().UploadDefaults(this);
        }

        public virtual float GetDepth()
        {
            return Depth;
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
                Max = Positions.Any() ? locpos + new Vector3(Positions.Max(o => o.X), Positions.Max(o => o.Y), Positions.Max(o => o.Z)) : new Vector3(),
                Min = Positions.Any() ? locpos + new Vector3(Positions.Min(o => o.X), Positions.Min(o => o.Y), Positions.Min(o => o.Z)) : new Vector3()
            };
        }

        public override void Live_Update(Base_Shader ShaderModel)
        {
            base.Live_Update(ShaderModel);
            if ((EnableClipPlans || CameraModel.EnableClipPlans) && !(this is ClipPlan))
            {
                if (CameraModel.EnableClipPlans)
                {
                    CameraModel.ClipPlanX?.Live_Update(ShaderModel);
                    CameraModel.ClipPlanY?.Live_Update(ShaderModel);
                    CameraModel.ClipPlanZ?.Live_Update(ShaderModel);
                }
                else
                {
                    foreach (var clip in ClipPlans)
                    {
                        clip.Live_Update(ShaderModel);
                    }
                }
            }
        }

        public override void PostRender(Base_Shader ShaderModel)
        {
            base.PostRender(ShaderModel);
            if (EnableClipPlans || CameraModel.EnableClipPlans)
            {
                foreach (var clip in ClipPlans)
                {
                    GL.Disable(clip.ClipDistance);
                }
            }
        }

        public override void UploadVAO()
        {
            if (Renderer == null)
            {
                Renderer = new GeometryRenderer(this);
            }

            Renderer.RenderModel();
            GetShaderModel().UploadDefaults(this);
        }

        public void SetEnableClipPlans(bool enableValue)
        {
            if (EnableClipPlans == enableValue) return;
            if (enableValue)
            {
                Add_Clips();
            }
            else
            {
                RemoveClips();
            }
            EnableClipPlans = enableValue;
            CullMode = EnableClipPlans ? CullFaceMode.FrontAndBack : CullFaceMode.Back;
        }

        private void Add_Clips()
        {
            if (ClipPlans.Any())
            {
                foreach (var clip in ClipPlans)
                {
                    clip.IsActive = true;
                }
            }
            else
            {
                ClipPlans.Add(Generate_ClipPlan(Vector3.UnitX, OpenTK.Graphics.OpenGL.EnableCap.ClipDistance0));
                ClipPlans.Add(Generate_ClipPlan(Vector3.UnitY, OpenTK.Graphics.OpenGL.EnableCap.ClipDistance1));
                ClipPlans.Add(Generate_ClipPlan(Vector3.UnitZ, OpenTK.Graphics.OpenGL.EnableCap.ClipDistance2));
            }
        }

        private ClipPlan Generate_ClipPlan(Vector3 unitX, EnableCap clipDistance)
        {
            var clip = ClipPlan.Generate_ClipPlan(unitX, clipDistance);
            clip.MoveTo(BBX.GetCG());
            return clip;
        }

        private void RemoveClips()
        {
            foreach (var clip in ClipPlans)
            {
                //SceneModel.ActiveScene.RemoveModels(clip); //removing clips will kill the live update, hence clips will not be activated
                clip.IsActive = false;
                SceneModel.ActiveScene.RunOnAllShaders.Push((x) =>
                {
                    clip.Live_Update(x);
                });
            }
        }

        public override List<face> generatefaces()
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
    }
}