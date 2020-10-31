using OpenTK;
using OpenTK.Graphics.OpenGL;
using Simple_Engine.Engine.Core.Abstracts;
using Simple_Engine.Engine.Geometry.ThreeDModels.Clips;
using Simple_Engine.Engine.Illumination;
using Simple_Engine.Engine.Opticals;
using Simple_Engine.Engine.Space.Camera;
using Simple_Engine.Engine.Space.Environment;
using Simple_Engine.Engine.Space.Scene;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Simple_Engine.Engine.Render
{
    public enum ShaderMapType
    {
        LoadLight = 1,
        LoadColor = 2,
        LightnColor = 3,
        LightnTexture = 4,
        LightnLoadTexture = 5,
        LoadCubeTexture = 6,
        LightnLoadCubeTexture = 7,
        Blend = 8,
        LightnBlend = 9,
        SkyBox = 10,
        Texture = 11,
    }

    public enum ShaderPath
    {
        Default,
        Combined,
        GUI,
        Font,
        SkyBox,
        Cube,
        Terrain,
        Particle,
        Water,
        SingleColor,
        Shadow,
        PostProcess,
        Color
    }

    public class Shader
    {
        #region Properties

        
        //Lights
        public static int MaximumLight = 4;

        public int MatrixLayoutId = 3;

        public int NormalLayoutId = 2;

        //layoutLocations
        public int PositionLayoutId = 0;

        public int ProgramID;

        //4,5,6
        public int SelectedLayoutId = 7;

        public int TangentLayoutId = 8;
        public int TextureLayoutId = 1;
        public int VertexColorLayoutId = 9;
        public int vertexShaderId, fragmentShaderId;

        public List<int> AttenuationLightLocation { get; set; }

        //Material
        public float BackLite { get; set; } = .2f;

        public int BackLiteLocation { get; set; } = -1;

        public int BlendFactorLocation { get; set; } = -1;

        public int BrightnessLevels { get; set; } = 4;

        public int dayTexureLocation { get; set; } = -1;

        public bool EnableInstancing { get; internal set; } = false;

        public int FogColorLocation { get; set; } = -1;

        //Envirnoment
        public int FogDensityLocation { get; set; } = -1;

        public int FogSpeedLocation { get; set; } = -1;

        public int HasFogLocation { get; set; } = -1;

        //RenderMode
        public int IsToonRenderLocation { get; set; } = -1;

        public int IsTransparentLocation { get; set; } = -1;

        public List<int> LightColorLocation { get; set; }

        public List<int> LightEyePositionLocation { get; set; }

        public List<int> LightPositionsLocation { get; set; }

        public int Location_ClipPlanX { get; private set; } = -1;

        public int Location_ClipPlanY { get; private set; } = -1;

        public int Location_ClipPlanZ { get; private set; } = -1;
        public int Location_EnableClipPlanX { get; private set; } = -1;

        public int Location_EnableClipPlanY { get; private set; } = -1;

        public int Location_EnableClipPlanZ { get; private set; } = -1;

        //Texture
        public int Location_CubeDiffuseMap { get; set; } = -1;

        public int Location_CubeNormalMap { get; set; } = -1;

        public int Location_CubeSpecularMap { get; set; } = -1;

        public int Location_DefaultColor { get; set; } = -1;

        public int Location_DiffuseMap { get; set; } = -1;

        public int Location_EnableNormalMap { get; set; } = -1;

        public int Location_FarDistance { get; set; } = -1;

        public int Location_isReflection { get; private set; } = -1;

        public int Location_IsSelected { get; set; } = -1;

        public int Location_lightProjection { get; set; } = -1;

        public int Location_lightViewTransform { get; set; } = -1;

        //World
        public int Location_LocalTransform { get; set; } = -1;

        public int Location_NearDistance { get; set; } = -1;
        public int Location_NormalMap { get; set; } = -1;
        public int Location_ProjectionTransform { get; set; } = -1;
        public int Location_ShaderType { get; set; } = -1;
        public int Location_ShadowMap { get; set; } = -1;
        public int Location_SpecularMap { get; set; } = -1;
        public int Location_useCubeSpecularMap { get; private set; } = -1;
        public int Location_useSpecularMap { get; private set; } = -1;
        public int Location_ViewTransform { get; set; } = -1;
        public int MaxLightLocation { get; set; } = -1;
        public int nightTexureLocation { get; set; } = -1;
        public int numberOfRowsTextureLocation { get; set; } = -1;
        public int OffsetTextureLocation { get; set; } = -1;
        public int ReflectionIndexLocation { get; set; } = -1;
        public ShaderMapType ShaderType { get; set; }
        public ShaderPath ShaderModelType { get; }
        public int ShiningDampLocation { get; set; } = -1;
        public int ToonBrightnessLevelsLocation { get; set; } = -1;

        #endregion Properties

        public Stack<Action> RunOnUIThread { get; set; } = new Stack<Action>();

        private static string INCLUDEdIRECTIVE = "#include";

        public Shader(ShaderMapType mapType, ShaderPath shaderModelType)
        {
            ShaderType = mapType;
            ShaderModelType = shaderModelType;
        }

        private void InitalizeShader(ShaderPath shaderModelType)
        {
            string path = @"./Engine/Space/Render/Source/";
            Debug.WriteLine($"Loading Shader: {shaderModelType}");
            switch (shaderModelType)
            {
                case ShaderPath.Default:
                    {
                        Setup_Shader($"{path}Default_Vert.vert", $"{path}Default_Frag.frag");
                        break;
                    }
                case ShaderPath.Color:
                    {
                        Setup_Shader($"{path}Color_Vert.vert", $"{path}color_Frag.frag");
                        break;
                    }
                case ShaderPath.SkyBox:
                    {
                        path = @"./Engine/Space/Environment/WorldBox/Render/Source/";

                        Setup_Shader($"{path}SkyBox_Vert.vert", $"{path}SkyBox_Frag.frag");
                        break;
                    }
                case ShaderPath.GUI:
                    {
                        path = @"./Engine/GUI/Render/Source/";

                        Setup_Shader($"{path}VertexShader_GUI.vert", $"{path}FragmentShader_GUI.frag");
                        break;
                    }
                case ShaderPath.Particle:
                    {
                        path = @"./Engine/Particles/Render/Source/";
                        Setup_Shader($"{path}VertexShader_Particle.vert", $"{path}FragmentShader_Particle.frag");
                        break;
                    }
                case ShaderPath.Terrain:
                    {
                        path = @"./Engine/Geometry/Terrain/Render/Source/";

                        Setup_Shader($"{path}VertexShader_Blend.vert", $"{path}FragmentShader_Blend.frag");
                        break;
                    }
                case ShaderPath.Cube:
                    {
                        path = @"./Engine/Geometry/ThreeDModels/Cube/Render/Source/";
                        Setup_Shader($"{path}VertexShader_Cube.vert", $"{path}FragmentShader_Cube.frag");
                        break;
                    }
                case ShaderPath.Water:
                    {
                        path = @"./Engine/water/Render/Source/";
                        Setup_Shader($"{path}VertexShader_Water.vert", $"{path}FragmentShader_Water.frag");
                        break;
                    }
                case ShaderPath.SingleColor:
                    {
                        path = @"./Engine/Space/Render/Source/";
                        Setup_Shader($"{path}SingleColor_Vert.vert", $"{path}SingleColor_Frag.frag");
                        break;
                    }
                case ShaderPath.Shadow:
                    {
                        path = @"./Engine/illumination/Render/Source/";
                        Setup_Shader($"{path}Shadow_vert.vert", $"{path}Shadow_Frag.frag");
                        break;
                    }
                case ShaderPath.PostProcess:
                    {
                        break;
                    }
                default:
                    break;
            }
        }

        public void BindAttribute(int attribute, string variableName)
        {
            GL.BindAttribLocation(ProgramID, attribute, variableName);
        }

        public virtual void BindAttributes()
        {
            BindAttribute(PositionLayoutId, "aPosition");
            BindAttribute(TextureLayoutId, "aTextureCoor");
            BindAttribute(NormalLayoutId, "aNormals");
            BindAttribute(MatrixLayoutId, "InstanceMatrix");
            BindAttribute(SelectedLayoutId, "InstanceSelected");
            BindAttribute(TangentLayoutId, "Tangent");
            BindAttribute(VertexColorLayoutId, "VertexColor");
        }

        public virtual int CreateShader(string path, ShaderType shaderFileType)
        {
            var shaderScript = LoadShader(path);

            var shaderProgram = GL.CreateShader(shaderFileType);
            GL.ShaderSource(shaderProgram, shaderScript.ToString());

            GL.CompileShader(shaderProgram);
            string infologTest = GL.GetShaderInfoLog(shaderProgram);
            if (infologTest != string.Empty)
            {
                new ArgumentException(infologTest);
            }

            GL.AttachShader(ProgramID, shaderProgram);
            return shaderProgram;
        }

        public void Dispose()
        {
            CleanUp();
            GC.SuppressFinalize(this);
        }

        public void GetArrayLocations(string attribute, List<int> storage, int size)
        {
            for (int i = 0; i < size; i++)
            {
                storage.Add(GL.GetUniformLocation(ProgramID, $"{attribute}[{i}]"));
            }
        }

        public int GetLocation(string name)
        {
            return GL.GetUniformLocation(ProgramID, name);
        }

        public virtual void LinkProgram()
        {
            GL.LinkProgram(ProgramID);
            GL.ValidateProgram(ProgramID);

            string log = GL.GetProgramInfoLog(ProgramID);
            if (!string.IsNullOrEmpty(log))
            {
                Debugger.Break();
            }
        }

        public virtual void Live_Update()
        {
            while (RunOnUIThread.Any())
            {
                var action = RunOnUIThread.Pop();
                action();
            }
        }

        public virtual void LoadAllUniforms()
        {
            //Camera
            Location_ProjectionTransform = GetLocation(nameof(CameraModel.ProjectionTransform));
            Location_ViewTransform = GetLocation(nameof(CameraModel.ViewTransform));
            Location_NearDistance = GetLocation(nameof(CameraModel.NearDistance));
            Location_FarDistance = GetLocation(nameof(CameraModel.FarDistance));

            //Model
            Location_DefaultColor = GetLocation(nameof(Base_Geo.DefaultColor));
            Location_LocalTransform = GetLocation(nameof(Base_Geo.LocalTransform));
            Location_ShaderType = GetLocation(nameof(ShaderType));
            Location_IsSelected = GetLocation("IsSelected");

            //Fog
            HasFogLocation = GetLocation("HasFog");
            FogDensityLocation = GetLocation(nameof(Fog.Density));
            FogSpeedLocation = GetLocation(nameof(Fog.FogSpeed));
            FogColorLocation = GetLocation(nameof(Fog.FogColor));

            //Environment
            nightTexureLocation = GetLocation("nightTexture");
            dayTexureLocation = GetLocation("dayTexture");
            BlendFactorLocation = GetLocation(nameof(SkyBox.BlendFactor));

            //Render Mode
            IsToonRenderLocation = GetLocation(nameof(EngineRenderer.IsToonMode));
            ToonBrightnessLevelsLocation = GetLocation(nameof(BrightnessLevels));
            Location_ClipPlanX = GetLocation("ClipPlanX");
            Location_ClipPlanY = GetLocation("ClipPlanY");
            Location_ClipPlanZ = GetLocation("ClipPlanZ");
            
            Location_EnableClipPlanX = GetLocation("Enable_ClipPlanX");
            Location_EnableClipPlanY = GetLocation("Enable_ClipPlanY");
            Location_EnableClipPlanZ = GetLocation("Enable_ClipPlanZ");

            //Texture
            Location_CubeDiffuseMap = GetLocation("CubeDiffuseMap");
            Location_CubeNormalMap = GetLocation("CubeNormalMap");
            Location_CubeSpecularMap = GetLocation("CubeSpecularMap");

            Location_DiffuseMap = GetLocation("DiffuseMap");
            Location_NormalMap = GetLocation("NormalMap");
            Location_SpecularMap = GetLocation("SpecularMap");

            OffsetTextureLocation = GetLocation("Offset");
            numberOfRowsTextureLocation = GetLocation(nameof(Base_Texture.numberOfRows));

            //Material
            ShiningDampLocation = GetLocation(nameof(Gloss.ShiningDamp));
            ReflectionIndexLocation = GetLocation(nameof(Gloss.ReflectionIndex));
            IsTransparentLocation = GetLocation(nameof(Base_Texture.IsTransparent));
            Location_useSpecularMap = GetLocation("useSpecularMap");
            Location_useCubeSpecularMap = GetLocation("useCubeSpecularMap");
            Location_isReflection = GetLocation("isReflection");

            //Light
            Location_EnableNormalMap = GetLocation("EnableNormalMap");
            MaxLightLocation = GetLocation(nameof(MaximumLight));
            BackLiteLocation = GetLocation(nameof(BackLite));
            Location_lightProjection = GetLocation("LightProjectionTransform");
            Location_lightViewTransform = GetLocation("LightViewTransform");
            Location_ShadowMap = GetLocation("ShadowMap");

            LightColorLocation = new List<int>();
            LightPositionsLocation = new List<int>();
            AttenuationLightLocation = new List<int>();
            LightEyePositionLocation = new List<int>();

            GetArrayLocations("LightColor", LightColorLocation, MaximumLight);
            GetArrayLocations(nameof(LightModel.LightPosition), LightPositionsLocation, MaximumLight);
            GetArrayLocations(nameof(LightModel.Attenuation), AttenuationLightLocation, MaximumLight);
            GetArrayLocations("LightEyePosition", LightEyePositionLocation, MaximumLight);
        }

        public void SetArray3(List<int> locationsStore, IEnumerable<Vector3> storage, Vector3 defaultvalue)
        {
            int count = storage.Count();
            for (int i = 0; i < MaximumLight; i++)
            {
                if (i > count - 1)
                {
                    SetVector3(locationsStore[i], defaultvalue);
                }
                else
                {
                    var value = storage.ElementAt(i);
                    SetVector3(locationsStore[i], value);
                }
            }
        }

        public void SetArray4(List<int> locationsStore, IEnumerable<Vector4> storage, Vector4 defaultvalue)
        {
            int count = storage.Count();
            for (int i = 0; i < MaximumLight; i++)
            {
                if (i > count - 1)
                {
                    SetVector4(locationsStore[i], defaultvalue);
                }
                else
                {
                    var value = storage.ElementAt(i);
                    SetVector4(locationsStore[i], value);
                }
            }
        }

        public void SetBool(int attribLocation, bool value)
        {
            GL.Uniform1(attribLocation, Convert.ToInt32(value));
        }

        public void SetFloat(int attribLocation, float value)
        {
            GL.Uniform1(attribLocation, value);
        }

        public void SetMatrix4(int attribLocation, Matrix4 value)
        {
            GL.UniformMatrix4(attribLocation, false, ref value);
        }

        public virtual void Setup_Shader(string vertexPath, string fragmentPath)
        {
            //Link both Shaders to a program
            ProgramID = GL.CreateProgram();

            vertexShaderId = CreateShader(vertexPath, OpenTK.Graphics.OpenGL.ShaderType.VertexShader);
            fragmentShaderId = CreateShader(fragmentPath, OpenTK.Graphics.OpenGL.ShaderType.FragmentShader);

            // LoadCommonFunctions();

            BindAttributes(); //must be before linking program
            LinkProgram();
            LoadAllUniforms();
        }

        public void SetVector2(int attribLocation, Vector2 value)
        {
            GL.Uniform2(attribLocation, value);
        }

        public void SetVector4(int attribLocation, Vector4 value)
        {
            GL.Uniform4(attribLocation, value);
        }

        public void Stop()
        {
            GL.UseProgram(0);
        }

        public virtual void UploadDefaults(Base_Geo model)
        {
            Use();

            model?.UploadDefaults(this);
            model?.Material?.UploadDefaults(this);
            model?.TextureModel?.UploadDefaults(this);
            model?.ActivateShadowMap(SceneModel.ActiveScene.Lights.First());

            SceneModel.ActiveScene.UploadDefaults(this);

            SetInt(ToonBrightnessLevelsLocation, BrightnessLevels);
            SetInt(Location_ShaderType, (int)ShaderType);
            SetFloat(BackLiteLocation, BackLite);

            Stop();
        }

        public void Use()
        {
            if (ProgramID == 0)
            {
                //this is incase the shadermodel is created from a different thread.
                InitalizeShader(ShaderModelType);
            }

            GL.UseProgram(ProgramID);
        }

        internal void SetInt(int attribLocation, int value)
        {
            GL.Uniform1(attribLocation, value);
        }

        internal void SetVector3(int location, Vector3 position)
        {
            GL.Uniform3(location, position);
        }

        protected virtual void CleanUp()
        {
            Stop();
            GL.DetachShader(ProgramID, vertexShaderId);
            GL.DetachShader(ProgramID, fragmentShaderId);
            GL.DeleteShader(fragmentShaderId);
            GL.DeleteShader(vertexShaderId);
            GL.DeleteProgram(ProgramID);
        }

        private string LoadShader(string path)
        {
            StringBuilder sourceBuilder = new StringBuilder();
            sourceBuilder.AppendLine($"//****************{Path.GetFileName(path)}****************");
            var lines = File.ReadAllLines(path);

            foreach (var line in lines)
            {
                if (line.StartsWith(INCLUDEdIRECTIVE))
                {
                    int index = line.IndexOf("//!");

                    string externalPath = line.Substring(index + 4 + INCLUDEdIRECTIVE.Length + 2, -index - 4 + line.Length - INCLUDEdIRECTIVE.Length - 2 - 1);

                    string functions = LoadShader(externalPath);
                    sourceBuilder.AppendLine(functions);
                }
                else
                {
                    sourceBuilder.AppendLine(line);
                }
            }

            return sourceBuilder.ToString();
        }
    }
}