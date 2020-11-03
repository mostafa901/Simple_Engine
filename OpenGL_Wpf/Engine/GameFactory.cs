using OpenTK;
using OpenTK.Graphics.OpenGL;
using Shared_Lib;
using Simple_Engine.Engine.Core.Abstracts;
using Simple_Engine.Engine.Core.Serialize;
using Simple_Engine.Engine.Fonts;
using Simple_Engine.Engine.Fonts.Core;
using Simple_Engine.Engine.GameSystem;
using Simple_Engine.Engine.Geometry.Core;
using Simple_Engine.Engine.Geometry.Cube;
using Simple_Engine.Engine.Geometry.ThreeDModels;
using Simple_Engine.Engine.Geometry.ThreeDModels.Cube.Render;
using Simple_Engine.Engine.Geometry.TwoD;
using Simple_Engine.Engine.GUI.Render;
using Simple_Engine.Engine.Illumination;
using Simple_Engine.Engine.Opticals;
using Simple_Engine.Engine.Particles;
using Simple_Engine.Engine.Render;
using Simple_Engine.Engine.Render.Texture;
using Simple_Engine.Engine.Space.Scene;
using Simple_Engine.Engine.Water;
using Simple_Engine.ToolBox;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Simple_Engine.Engine
{
    public static class GameFactory
    {
        public static GeometryModel CreateSpheare(SceneModel scene, Vector3 position)
        {
            var PivotModel = Importer.Import.OBJFile(@"./SampleModels/Primatives/Sphere.obj");
            PivotModel.ShaderModel = new Shader(ShaderMapType.LoadColor, ShaderPath.Default);
            PivotModel.DefaultColor = new Vector4(1, 0, 0, 1);

            var pivotemesh = PivotModel.AddMesh(eMath.MoveTo(PivotModel.LocalTransform, position));

            scene.UpLoadModels(PivotModel);
            return PivotModel;
        }

        public static void Draw_Hilbert(SceneModel scene)
        {
            var hilbret = new Hilbert(5);
            scene.UpLoadModels(hilbret);
        }

        public static List<Plan3D> Draw_Pushes(SceneModel scene, Terran terrain)
        {
            List<Plan3D> plans = new List<Plan3D>();
            for (int n = 0; n < 15; n++)
            {
                var greenPush = new Plan3D(2);

                greenPush.CullMode = CullFaceMode.FrontAndBack;
                plans.Add(greenPush); ;
                greenPush.Floor = new FloorModel(terrain);
                greenPush.SetHeight(3);
                greenPush.BuildModel();
                greenPush.ShaderModel = new Shader(ShaderMapType.LightnTexture, ShaderPath.Default);
                greenPush.AllowReflect = false;
                greenPush.CastShadow = false;

                greenPush.TextureModel = new TextureSample2D(@"D:\Revit_API\Projects\Simple_Engine\OpenGL_Wpf\SampleModels\Landscape\Texture\TextureAtlas.png", TextureUnit.Texture0);

                greenPush.TextureModel.IsTransparent = true;
                greenPush.TextureModel.TextureAtlasId = Randoms.Next(0, 15);
                greenPush.TextureModel.numberOfRows = 4;

                for (int j = 0; j < 50; j++)
                {
                    var randomPos = new Vector3(Randoms.Next(10, terrain.GetWidth() - 10), 0, Randoms.Next(10, terrain.GetDepth() - 10));
                    var mat = greenPush.LocalTransform;
                    float height = terrain.GetTerrainHeight(randomPos.Z, randomPos.X);
                    randomPos += new Vector3(0, height, 0);

                    for (int i = 0; i < 2; i++)
                    {
                        var tmat = eMath.MoveLocal(mat, new Vector3(randomPos.X, randomPos.Y
                        + (greenPush.GetHeight() / 2)
                        , randomPos.Z));
                        Vector3 rotation = new Vector3(0, 90, 0);

                        tmat = eMath.Rotate(tmat, 1, rotation);
                        if (i == 0)
                        {
                            tmat = eMath.Rotate(tmat, 1, rotation);
                        }
                        greenPush.AddMesh(tmat);
                    }
                }

                scene.UpLoadModels(greenPush);
            }
            return plans;
        }

        public static void Draw_Rectangle(SceneModel scene)
        {
            var rec = new Plan3D(.25f);

            rec.AllowReflect = false;
            rec.BuildModel();
            rec.NormalTangent = eMath.CalculateTangents(rec.Positions.Select(o => new Vector3(o)).ToList(), rec.Indeces, rec.TextureCoordinates);
            rec.Material = new Base_Material();
            rec.Material.Glossiness = new Gloss(.3f, 3);

            rec.DefaultColor = new Vector4(1, 0, 0, 1);
            rec.ShaderModel = new GUIShader(ShaderMapType.LoadColor, ShaderPath.GUI);

            scene.UpLoadModels(rec);
        }

        public static Base_Geo3D Draw_Terran(SceneModel scene)
        {
            var terran = new Terran(100, 100);
            terran.AllowReflect = true;
            terran.BuildModel();
            scene.UpLoadModels(terran);
            return terran;
        }

        public static Base_Geo3D DrawCube(SceneModel scene)
        {
            var cube = new CubeModel(2);
            cube.BuildModel();
            cube.MoveTo(5, cube.GetWidth() / 2, 30);
            cube.Rotate(30, new Vector3(1, 0, 1));
            cube.NormalTangent = eMath.CalculateTangents(cube.Positions, cube.Indeces, cube.TextureCoordinates);

            cube.Save(@"C:\Users\MOUSTA~1.KHA\AppData\Local\Temp\testSave.ssd");

            cube.Material = new Base_Material();
            cube.Material.Glossiness = new Gloss(.3f, 3f);

            cube.CastShadow = true;
            cube.AllowReflect = true;

            cube.ShaderModel = new CubeShader(ShaderMapType.LoadCubeTexture);
            //  cube.AddMesh(cube.LocalTransform);
            cube.Renderer = new CubeRenderer(cube);

            cube.TextureModel = new CubeTexture(TextureMode.TextureCube);
            cube.TextureModel.Set_LoadNormalMap(true);
            scene.UpLoadModels(cube);

            return cube;
        }

        public static Base_Geo3D DrawDragon(SceneModel scene, Terran terrain)
        {
            var dragon = Importer.Import.OBJFile(@"./SampleModels/Dragon/dragon.obj");

            if (terrain != null)
            {
                dragon.Floor = new FloorModel(terrain);
            }
            dragon.UpdateBoundingBox();
            dragon.SetEnableClipPlans(false);

            dragon.ShaderModel = new Shader(ShaderMapType.LightnColor, ShaderPath.SingleColor);
            dragon.AllowReflect = true;

            dragon.Material = new Core.Abstracts.Base_Material();
            dragon.Material.Glossiness = new Gloss(.3f, 3);
            scene.UpLoadModels(dragon);

            //   var mesh = dragon.AddMesh(eMath.Scale(eMath.Move(dragon.LocalTransform, new Vector3(10, 0, 10)), new Vector3(.2f)));

            var mesh = dragon;

            mesh.Particles = new List<ParticleModel>();
            var parsys = new ParticleModel();
            mesh.Particles.Add(parsys);

            float count = 0;
            //  scene.ActiveCamera.AttachTargetTo(dragon);

            mesh.MoveEvent += (s, e) =>
            {
                mesh.Floor?.OnMoving(dragon);

                count += (float)Game.Instance.RenderPeriod / 2;
                var pps = (int)Math.Ceiling(count);
                if (pps > 0)
                {
                    // parsys.AddParticles(e.Transform.ExtractTranslation() + new Vector3(0, dragon.BBX.Height, 0), pps);
                    count = 0;
                }
            };

            //  dragon.DrawAxis();

            float height = terrain?.GetTerrainHeight(10, 10) ?? 0;
            mesh.MoveWorld(new Vector3(10, height, 10));
            //  dragon.ShowSelectionBox();

            return mesh;
        }

        public static Base_Geo3D DrawEarth(SceneModel scene)
        {
            var earth = new Plan3D(20);
            earth.BuildModel();
            earth.NormalTangent = ToolBox.eMath.CalculateTangents(earth.Positions, earth.Indeces, earth.TextureCoordinates);

            earth.Rotate(90, new Vector3(1, 0, 0));

            earth.Material = new Base_Material();
            earth.Material.Glossiness = new Gloss(.5f, 10f);

            var diff = new TextureSample2D(@"./SampleModels/Texture/earth_diff.jpg", TextureUnit.Texture0);
            var specular = new TextureSample2D(@"./SampleModels/Texture/earth_SpecularMap.jpg", TextureUnit.Texture1);
            var normalMap = new TextureSample2D(@"./SampleModels/Texture/earth_NormalMap.jpg", TextureUnit.Texture2);

            var earthTexture = new TextureSample2D(TextureUnit.Texture0);
            earthTexture.TextureIds.Add(diff);
            earthTexture.TextureIds.Add(specular);
            earthTexture.TextureIds.Add(normalMap);
            earth.TextureModel = earthTexture;
            earth.TextureModel.Set_LoadNormalMap(true);
            earth.TextureModel.useSpecularMap = true;

            earth.ShaderModel = new Shader(ShaderMapType.Blend, ShaderPath.Default);

            scene.UpLoadModels(earth);
            return earth;
        }

        internal static void DrawGrid(SceneModel sceneModel)
        {
            if (Grid.ActiveGrid != null)
            {
                sceneModel.RemoveModels(Grid.ActiveGrid);
            }
            var grid = new Grid(100, 100);
            Grid.ActiveGrid = grid;
            grid.BuildModel();
            sceneModel.UpLoadModels(grid);
        }

        public static void DrawLine(SceneModel scene)
        {
            Line l = new Line(new Vector3(0, 0, 0), new Vector3(1, 0, 1));

            scene.UpLoadModels(l);
        }

        public static SkyBox DrawSkyBox(SceneModel scene)
        {
            var cube = new CubeModel(Game.Instance.Width);
            cube.BuildModel();
            var SkyBoxModel =  new SkyBox(cube);
            SkyBoxModel.AllowReflect = true;
            SkyBoxModel.BuildModel();
            scene.UpLoadModels(SkyBoxModel);

            return SkyBoxModel;
        }

        public static StreetLamp DrawStreetLamp(SceneModel scene, Terran terrain)
        {
            var lamp = Importer.Import.OBJFile(@"D:\Revit_API\Projects\Simple_Engine\OpenGL_Wpf\SampleModels\LandScape\model\lamp.obj");
            StreetLamp stlamp = new StreetLamp(lamp);

            stlamp.Floor = new FloorModel(terrain);
            stlamp.ShaderModel = new Shader(ShaderMapType.LightnColor, ShaderPath.SingleColor);

            stlamp.AddInstance(new Vector4(5, 0, 0, 1), eMath.MoveTo(stlamp.LocalTransform, new Vector3(10, terrain?.GetTerrainHeight(10, 50) ?? 0, 50)));

            stlamp.AddInstance(new Vector4(0, 5, 0, 1), eMath.MoveTo(stlamp.LocalTransform, new Vector3(20, terrain?.GetTerrainHeight(20, 20) ?? 0, 20)));

            stlamp.AddInstance(new Vector4(0, 0, 5, 1), eMath.MoveTo(stlamp.LocalTransform, new Vector3(50, terrain?.GetTerrainHeight(50, 50) ?? 0, 50)));

            stlamp.AllowReflect = true;
            scene.UpLoadModels(stlamp);
            return stlamp;
        }

        public static void DrawWater(SceneModel scene)
        {
            var water = new WaterModel(50);

            water.BuildModel();
            scene.UpLoadModels(water);
        }

        internal static void DrawText(SceneModel scene)
        {
            FontFactory.GenerateFont();

            var GuiTextModel = new GuiFont("This is my first Line!", Game.Instance.Width);
            GuiTextModel.TextPosition = new Vector2(-.751f, .75f);
            GuiTextModel.BuildModel();

            scene.GuiTextModel = GuiTextModel;
        }
    }
}