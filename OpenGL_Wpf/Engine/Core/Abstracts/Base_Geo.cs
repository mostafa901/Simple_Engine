using Newtonsoft.Json;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Shared_Lib;
using Simple_Engine.Engine.Core.AnimationSystem;
using Simple_Engine.Engine.Core.Events;
using Simple_Engine.Engine.Core.Interfaces;
using Simple_Engine.Engine.Core.Static;
using Simple_Engine.Engine.Geometry.Axis;
using Simple_Engine.Engine.Geometry.Core;
using Simple_Engine.Engine.Geometry.Cube;
using Simple_Engine.Engine.Geometry.ThreeDModels.Clips;
using Simple_Engine.Engine.Illumination;
using Simple_Engine.Engine.ImGui_Set.Controls;
using Simple_Engine.Engine.Particles;
using Simple_Engine.Engine.Render;
using Simple_Engine.Engine.Render.Texture;
using Simple_Engine.Engine.Space.Scene;
using Simple_Engine.Engine.Static.InputControl;
using Simple_Engine.ToolBox;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static Simple_Engine.Engine.Core.Interfaces.IRenderable;

namespace Simple_Engine.Engine.Core.Abstracts
{
    public abstract class Base_Geo : IDrawable
    {
        public static Base_Geo SelectedModel;
        private CubeModel SelectionBox;

        
        public Base_Geo()
        {
            Name = this.GetType().Name;
            IsSystemModel = false;
            CanBeSaved = true;
            TextureCoordinates = new List<Vector2>();
            Normals = new List<Vector3>();
            NormalTangent = new List<Vector3>();
            Indeces = new List<int>();
            Meshes = new List<Mesh3D>();
            VertixColor = new List<Vector4>();

            LocalTransform = Matrix4.Identity;
            DefaultColor = new Vector4(Randoms.Next(.1f, 1f, 2), Randoms.Next(.1f, 1f, 2), Randoms.Next(.1f, 1f, 2), 1);
            Meshes = new List<Mesh3D>();
            PivotPoint = new Vector3();
            IsActive = true;

            Dynamic = IDrawable.DynamicFlag.Static;
            onSelectedEvent += delegate
            {
            };
            onDeSelectedEvent += delegate
            {
                UI_Shared.OpenContext = false;
                SelectedModel = null;
            };
        }

        public event EventHandler<MoveingEvent> MoveEvent;

        public event EventHandler<SelectedEvent> onDeSelectedEvent;

        public event EventHandler<SelectedEvent> onSelectedEvent;

        public bool AllowReflect { get; set; } = false;
        public BoundingBox BBX { get; set; }
        public bool CanBeSaved { get; set; }
        public bool CastShadow { get; set; } = false;



        public CullFaceMode CullMode { get; set; } = CullFaceMode.Back;
        public Vector4 DefaultColor { get; set; }
        public PrimitiveType DrawType { get; set; } = PrimitiveType.Triangles;

        public FloorModel Floor { get; set; }
        public int Id { get; set; }
        public List<int> Indeces { get; set; } = new List<int>();
        public bool IsActive { get; set; }
        public bool IsBlended { get; set; }
        public bool IsSystemModel { get; set; }
        public Matrix4 LocalTransform { get; set; }
        public Base_Material Material { get; set; }
        public List<Mesh3D> Meshes { get; set; }

        public string Name { get; set; }
        public List<Vector3> Normals { get; set; }

        public List<Vector3> NormalTangent { get; set; }

        public List<ParticleModel> Particles { get; set; }

        public Vector3 PivotPoint { get; set; }

        public bool RecieveShadow { get; set; } = false;

        [JsonIgnore]
        public EngineRenderer Renderer { get; set; }

        [JsonIgnore]
        public Shader ShaderModel { get; set; }

        public List<Vector2> TextureCoordinates { get; set; }

        [JsonIgnore]
        public Base_Texture TextureModel { get; set; }

        [JsonIgnore]
        public ImgUI_Controls Ui_Controls { get; set; }

        public List<Vector4> VertixColor { get; set; }

        private float Height { get; set; } = 1;

        [JsonIgnore]
        private bool Selected { get; set; }

        private float Width { get; set; } = 1;
        public string Uid { get; set; }
        public IDrawable.DynamicFlag Dynamic { get ; set ; }

        public void ActivateShadowMap(LightModel lightSource)
        {
            if (CastShadow)
            {
                //here we are setting all models to receive shadows.
                if (TextureModel == null)
                {
                    TextureModel = new TextureSample2D(TextureUnit.Texture0, DefaultColor);
                }
                var count = TextureModel.TextureIds.Count;

                var textureUnit = OpenTK.Graphics.OpenGL.TextureUnit.Texture0 + count;
                var shadowTexture = TextureModel.TextureIds.FirstOrDefault(o => o.Name == "ShadowMap");
                if (shadowTexture == null)
                {
                    shadowTexture = new TextureSample2D(textureUnit);
                    shadowTexture.Name = "ShadowMap";
                    TextureModel.TextureIds.Add(shadowTexture);
                }

                shadowTexture.TextureId = lightSource.ShadowMapId;
                ShaderModel.SetInt(ShaderModel.Location_ShadowMap, count);
            }
        }

        public abstract void BuildModel();

        public virtual void CloneModel(Base_Geo sourceModel)
        {
            Meshes = new List<Mesh3D>();
            Indeces = sourceModel.Indeces;
            Normals = sourceModel.Normals;
            TextureCoordinates = sourceModel.TextureCoordinates;
            LocalTransform = sourceModel.LocalTransform;
            PivotPoint = sourceModel.PivotPoint;
        }

        public virtual void Create_UIControls()
        {
            //Imgui_Generator.GenerateControls(this, Ui_Controls);
        }

        public void Dispose()
        {
            ShaderModel.Dispose();
            Renderer.Dispose();
            TextureModel?.Dispose();
        }

        public void DrawAxis()
        {
            List<AxisPlan> axises = new List<AxisPlan>();
            var dim = BBX.GetDimensions();
            if (dim.Length == 0)
            {
                Debugger.Break();
            }
            axises.Add(new AxisPlan(this, Vector3.UnitX, new Vector2(dim.X, dim.Y)));
            axises.Add(new AxisPlan(this, Vector3.UnitY, new Vector2(dim.X, dim.Z)));
            axises.Add(new AxisPlan(this, Vector3.UnitZ, new Vector2(dim.X, dim.Y)));

            foreach (var axis in axises)
            {
                axis.BuildModel();
                axis.ShaderModel = new Shader(ShaderMapType.LoadColor, ShaderPath.Color);
                SceneModel.ActiveScene.ModelsforUpload.Push(axis);
                MoveEvent += (s, e) =>
                {
                    axis.LocalTransform = eMath.MoveTo(axis.LocalTransform, e.Transform.ExtractTranslation());
                };
            }
        }

        public virtual float GetHeight()
        {
            return Height;
        }

        public virtual bool GetSelected()
        {
            return Selected;
        }

        public virtual float GetWidth()
        {
            return Width;
        }

        public virtual void Live_Update(Shader ShaderModel)
        {
            ShaderModel.Live_Update();
            TextureModel?.Live_Update(ShaderModel);
            Material?.Live_Update();
            SceneModel.ActiveScene.Live_Update(ShaderModel); //upload sun shadow settings

            if (!ShaderModel.EnableInstancing)
            {
                ShaderModel.SetFloat(ShaderModel.Location_IsSelected, (float)Convert.ToInt32(Selected));
            }
        }

        public IRenderable Load(string path)
        {
            throw new NotImplementedException();
        }

        public virtual void MoveLocal(Vector3 displacement)
        {
            LocalTransform = eMath.MoveLocal(LocalTransform, displacement);
            OnMoving(new MoveingEvent(LocalTransform));
            UpdateBoundingBox();
        }

        public void MovePivotPoint(Vector3 displacement)
        {
            PivotPoint += displacement;
        }

        public void MoveTo(float x, float y, float z)
        {
            MoveTo(new Vector3(x, y, z));
        }

        public void MoveTo(Vector3 position)
        {
            LocalTransform = eMath.MoveTo(LocalTransform, position);
            OnMoving(new MoveingEvent(LocalTransform));
            UpdateBoundingBox();
        }

        public virtual void MoveWorld(Vector3 displacement)
        {
            LocalTransform = eMath.MoveWorld(LocalTransform, displacement);
            OnMoving(new MoveingEvent(LocalTransform));
            UpdateBoundingBox();
        }

        public virtual void PostRender(Shader ShaderModel)
        {
        }

        public virtual void PrepareForRender(Shader shaderModel)
        {
            if (shaderModel != null && shaderModel != ShaderModel)
            {
                shaderModel.UploadDefaults(this);
            }
            shaderModel.Use();

            Live_Update(shaderModel);
        }

        public abstract void RenderModel();

        public void Rotate(float angle, Vector3 axis)
        {
            LocalTransform = eMath.Rotate(LocalTransform, angle, axis);
            UpdateBoundingBox();
        }

        public virtual string Save()
        {
            throw new NotImplementedException();
        }

        public void Scale(float x, float y, float z)
        {
            Vector3 scaledAxis = new Vector3(x, y, z);
            LocalTransform = eMath.Scale(LocalTransform, scaledAxis);
            UpdateBoundingBox();
        }

        public void ScaleTo(float x, float y, float z)
        {
            Vector3 scaledAxis = new Vector3(x, y, z);
            LocalTransform = eMath.ScaleTo(LocalTransform, scaledAxis);
            UpdateBoundingBox();
        }

        public void Scale(float x)
        {
            Scale(x, x, x);
        }

        public void Scale(Vector3 scalarVector)
        {
            Scale(scalarVector.X, scalarVector.Y, scalarVector.Z);
        }

        public virtual void SetHeight(float height)
        {
            Height = height;
        }

        public virtual void Set_Selected(bool value)
        {
            if (SelectedModel != null && SelectedModel != this)
            {
                SelectedModel.Set_Selected(false);
            }

            if (Selected != value)
            {
                Selected = value;

                OnSelected(new SelectedEvent(this as ISelectable));
            }
        }

        public abstract void Setup_Indeces();

        public abstract void Setup_Normals();

        public abstract void Setup_Position();

        public abstract void Setup_TextureCoordinates(float xScale = 1, float yScale = 1);

        public void SetWidth(float width)
        {
            Width = width;
        }

        public void ShowSelectionBox()
        {
            var objectPos = LocalTransform.ExtractTranslation();

            SelectionBox = new CubeModel();
            SelectionBox.LocalTransform = LocalTransform;
            var dim = BBX.GetDimensions();
            SelectionBox.SetHeight(dim.Y);
            SelectionBox.SetWidth(dim.X);
            SelectionBox.SetDepth(dim.Z);
            SelectionBox.BuildModel();
            SelectionBox.ShaderModel = new Shader(ShaderMapType.Blend, ShaderPath.SingleColor);
            SceneModel.ActiveScene.UpLoadModels(SelectionBox);
        }

        public virtual void UpdateBoundingBox()
        {

        }

        public virtual void UploadDefaults(Shader ShaderModel)
        {
            UpdateBoundingBox();

            ShaderModel.SetMatrix4(ShaderModel.Location_LocalTransform, LocalTransform);
            ShaderModel.SetVector4(ShaderModel.Location_DefaultColor, DefaultColor);
        }

        protected void OnMoving(MoveingEvent e)
        {
            MoveEvent?.Invoke(this, e);
        }

        protected void OnSelected(SelectedEvent e)
        {
            if (e.Model == null) return;
            if (!e.Model.GetSelected())
            {
                onDeSelectedEvent?.Invoke(this, e);
            }
            else
            {
                SelectedModel = this;
                onSelectedEvent?.Invoke(this, e);
            }
        }

        internal void Delete()
        {
            UI_Geo.DeleteModel(this);
        }

        public void Render_UIControls()
        {
            throw new NotImplementedException();
        }

        public abstract List<face> generatefaces();
        

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

    }
}