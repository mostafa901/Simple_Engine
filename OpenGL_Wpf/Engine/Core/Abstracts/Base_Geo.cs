using Simple_Engine.Engine.Core.Events;
using Simple_Engine.Engine.Core.Interfaces;
using Simple_Engine.Engine.Geometry.Axis;
using Simple_Engine.Engine.Geometry.Core;
using Simple_Engine.Engine.Geometry.Cube;
using Simple_Engine.Engine.Geometry.ThreeDModels.Clips;
using Simple_Engine.Engine.Illumination;
using Simple_Engine.Engine.ImGui_Set;
using Simple_Engine.Engine.ImGui_Set.Controls;
using Simple_Engine.Engine.Particles;
using Simple_Engine.Engine.Render;
using Simple_Engine.Engine.Render.Texture;
using Simple_Engine.ToolBox;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Shared_Lib;
using Shared_Lib.Extention;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Simple_Engine.Engine.Core.Interfaces.IDrawable;
using static Simple_Engine.Engine.Core.Interfaces.IRenderable;
using Newtonsoft.Json;
using Simple_Engine.Engine.Space.Scene;
using Simple_Engine.Engine.Core.Static;
using Shared_Lib.MVVM;
using Simple_Engine.Engine.Static.InputControl;

namespace Simple_Engine.Engine.Core.Abstracts
{
    public abstract class Base_Geo :  IDrawable
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
            ClipPlans = new List<ClipPlan>();
            IsActive = true;
            Animate = new AnimationComponent(this);
            

            onSelectedEvent += delegate
            {

            };
            onDeSelectedEvent += delegate
            {
                Core.Static.UI_Geo.OpenContext = false;
                SelectedModel = null;
            };

            
        }

        public event EventHandler<MoveingEvent> MoveEvent;

        public event EventHandler<SelectedEvent> onDeSelectedEvent;

        public event EventHandler<SelectedEvent> onSelectedEvent;

        public bool AllowReflect { get; set; } = false;
        public AnimationComponent Animate { get; set; }
        public BoundingBox BBX { get; set; }
        public bool CanBeSaved { get; set; }
        public bool CastShadow { get; set; } = false;

        [JsonIgnore]
        public List<ClipPlan> ClipPlans { get; set; }

        public CullFaceMode CullMode { get; set; } = CullFaceMode.Back;
        public Vector4 DefaultColor { get; set; }
        public PrimitiveType DrawType { get; set; } = PrimitiveType.Triangles;

        [JsonIgnore]
        public bool EnableClipPlans { get; private set; } = false;

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
            if (BBX.Width + BBX.Height + BBX.Depth == 0)
            {
                Debugger.Break();
            }
            axises.Add(new AxisPlan(this, Vector3.UnitX, new Vector2(BBX.Width, BBX.Height)));
            axises.Add(new AxisPlan(this, Vector3.UnitY, new Vector2(BBX.Width, BBX.Depth)));
            axises.Add(new AxisPlan(this, Vector3.UnitZ, new Vector2(BBX.Width, BBX.Height)));

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
            KeyControl.Update_ActionKey();
            ShaderModel.Live_Update();
            TextureModel?.Live_Update(ShaderModel);
            Material?.Live_Update();
            Animate?.Update();
            SceneModel.ActiveScene.Live_Update(ShaderModel); //upload sun shadow settings

            if (!ShaderModel.EnableInstancing)
            {
                ShaderModel.SetFloat(ShaderModel.Location_IsSelected, (float)Convert.ToInt32(Selected));
            }

            ShaderModel.SetBool(ShaderModel.Location_EnableClipPlan, EnableClipPlans || Shader.ClipGlobal);
            if ((EnableClipPlans || Shader.ClipGlobal) && !(this is ClipPlan))
            {
                if (Shader.ClipGlobal)
                {
                    Shader.ClipPlanX.Live_Update(ShaderModel);
                    Shader.ClipPlanY.Live_Update(ShaderModel);
                    Shader.ClipPlanZ.Live_Update(ShaderModel);
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
            if (EnableClipPlans || Shader.ClipGlobal)
            {
                foreach (var clip in ClipPlans)
                {
                    GL.Disable(clip.ClipDistance);
                }
            }
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

        public void Render_UIControls()
        {
            if (Selected)
            {
                Ui_Controls.BuildModel();
            }
            else
            {
                if (EnableClipPlans)
                {
                    SetEnableClipPlans(false);
                }
                Ui_Controls?.SubControls.Clear();
            }
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

        public void Scale(float x)
        {
            Scale(x, x, x);
        }

        public void Scale(Vector3 scalarVector)
        {
            Scale(scalarVector.X, scalarVector.Y, scalarVector.Z);
        }

        public void SetEnableClipPlans(bool enableValue)
        {
            if (enableValue)
            {
                ClipPlans.Clear();
                Add_Clips();
            }
            else
            {
                RemoveClips();
            }
            EnableClipPlans = enableValue;
            CullMode = EnableClipPlans ? CullFaceMode.FrontAndBack : CullFaceMode.Back;
        }

        public virtual void SetHeight(float height)
        {
            Height = height;
        }

        public virtual void Set_Selected(bool value)
        {
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
            SelectionBox.SetHeight(BBX.Height);
            SelectionBox.SetWidth(BBX.Width);
            SelectionBox.SetDepth(BBX.Depth);
            SelectionBox.BuildModel();
            SelectionBox.ShaderModel = new Shader(ShaderMapType.Blend, ShaderPath.SingleColor);
            SceneModel.ActiveScene.UpLoadModels(SelectionBox);
        }

        public virtual void UpdateBoundingBox()
        {
            BBX = new BoundingBox
            {
                Width = 2 * (LocalTransform * new Vector4(GetWidth() / 2, 0, 0, 0)).X,
                Height = 2 * (LocalTransform * new Vector4(0, GetHeight() / 2, 0, 0)).Y,
            };
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
                onSelectedEvent?.Invoke(this, e);
            }
        }

        private void Add_Clips()
        {
            ClipPlans.Add(Generate_ClipPlan(Vector3.UnitX, OpenTK.Graphics.OpenGL.EnableCap.ClipDistance0));
            ClipPlans.Add(Generate_ClipPlan(Vector3.UnitY, OpenTK.Graphics.OpenGL.EnableCap.ClipDistance1));
            ClipPlans.Add(Generate_ClipPlan(Vector3.UnitZ, OpenTK.Graphics.OpenGL.EnableCap.ClipDistance2));
        }

        private ClipPlan Generate_ClipPlan(Vector3 direction, EnableCap ClipDistance)
        {
            var clip = new ClipPlan(direction, ClipDistance, 10f);
            if (direction == Vector3.UnitY)
            {
                clip.Rotate(90, new Vector3(1, 0, 0));
            }

            if (direction == Vector3.UnitX)
            {
                clip.Rotate(90, new Vector3(0, 1, 0));
            }
            clip.MoveTo(BBX.CG);
            clip.ShaderModel = new Shader(ShaderMapType.Blend, ShaderPath.SingleColor);
            SceneModel.ActiveScene.UpLoadModels(clip);
            return clip;
        }

        private void RemoveClips()
        {
            foreach (var clip in ClipPlans)
            {
                SceneModel.ActiveScene.RemoveModels(clip);
            }
        }

        internal void Delete()
        {
            UI_Geo.DeleteModel(this);
        }
    }
}