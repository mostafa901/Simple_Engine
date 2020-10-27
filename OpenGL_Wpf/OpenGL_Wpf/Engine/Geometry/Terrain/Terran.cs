using InSitU.Views.ThreeD.Engine.Core.Abstracts;
using InSitU.Views.ThreeD.Engine.Core.Interfaces;
using InSitU.Views.ThreeD.Engine.Geometry.Core;
using InSitU.Views.ThreeD.Engine.Geometry.Terrain.Render;
using InSitU.Views.ThreeD.Engine.Illumination.Render;
using InSitU.Views.ThreeD.Engine.Render;
using InSitU.Views.ThreeD.ToolBox;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Shared_Lib.Extention;
using sun.nio.fs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InSitU.Views.ThreeD.Engine.Geometry.ThreeDModels
{
    public class Terran : Base_Geo3D, ISelectable
    {
        public Terran(float width, float height)
        {
            SetWidth(width);
            SetDepth(height);
            RecieveShadow = true;
            CastShadow = true;
        }

        public override void BuildModel()
        {
            ShaderModel = new Shader(ShaderMapType.LightnBlend, ShaderPath.Terrain);
            DefaultColor = new Vector4(.15f, .16f, .18f, 1);
            TextureModel = new TerrainTexture();
            generateTerrain();
        }

 
        public override float GetHeight()
        {
            return Positions.Max(o=>o.Y)- Positions.Min(o => o.Y);
        }
        public override void Setup_Indeces()
        {
            Indeces.AddRange(new int[] { 0, 1, 2, 0, 2, 3 });
        }

        public override void Setup_Position()
        {
            var Width = GetWidth();
            var Height = GetDepth();
            var point0 = new Vector3(-Width, 0, Height);
            var point1 = new Vector3(Width, 0, Height);
            var point2 = new Vector3(Width, 0, -Height);
            var point3 = new Vector3(-Width, 0, -Height);

            List<Vector3> vertices = new List<Vector3>();

            vertices.Add(point0);
            vertices.Add(point1);
            vertices.Add(point2);
            vertices.Add(point3);

            Positions = vertices;
        }

        private float maxPixel = 255 * 255 * 255;
        private float maxHeight = 10;

        private float GetPixelHeight(float x, float y, Bitmap img)
        {
            if (x < 0 || x >= img.Width || y < 0 || y >= img.Height)
            {
                return 0;
            }
            float height = img.GetPixel((int)x, (int)y).ToArgb();

            height += maxPixel / 2f;
            height /= maxPixel / 2;
            height *= maxHeight;
            return height;
        }

        public float GetTerrainHeight(float worldX, float worldY)
        {
            //translate world coordinates to the Zero based on the Terrain Location
            //since out Terrain starts from 0,0 this step is ignored
            var sqx = worldX - 0;
            var sqy = worldY - 0;

            //Get the relative position to the Square this point is
            var squareWidth = GetWidth() / (float)heightCoor.Count;
            var squareHeight = GetDepth() / (float)heightCoor[0].Count;

            //verify the point is over the plan
            var gX = (int)Math.Floor(sqx / squareWidth);
            var gy = (int)Math.Floor(sqy / squareHeight);
            if (gX >= heightCoor.Count - 1 || gy >= heightCoor[0].Count - 1 || gX < 0 || gy < 0)
            {
                return 0;
            }

            //Get the Coordinates of this point relative to the square
            float coorx = (sqx % squareWidth) / squareWidth;
            float coory = (sqy % squareHeight) / squareHeight;

            float height = 0;
            if (coorx <= (1 - coory))
            {
                height = eMath.GetHeight(
                new Vector3(0, heightCoor[gX][gy], 0),
                new Vector3(1, heightCoor[gX + 1][gy], 0),
                new Vector3(0, heightCoor[gX][gy + 1], 1),
                new Vector2(coorx, coory));
            }
            else
            {
                height = eMath.GetHeight(
                    new Vector3(1, heightCoor[gX + 1][gy], 0),
                    new Vector3(1, heightCoor[gX + 1][gy + 1], 1),
                    new Vector3(0, heightCoor[gX][gy + 1], 1),
                    new Vector2(coorx, coory));
            }

            return float.IsNaN(height) ? 0 : height;
        }

        private List<List<float>> heightCoor = new List<List<float>>();

       
        private void generateTerrain()
        {
            var himg = new Bitmap(Bitmap.FromFile(@"D:\Revit_API\Projects\InSitU\InSitU\Views\ThreeD\SampleModels\LandScape\Texture\HeightMap.png"));

            int count = himg.Width * himg.Height;
            Vector3[] vertices = new Vector3[count];
            Vector3[] normals = new Vector3[count];
            Vector2[] textureCoords = new Vector2[count];
            int[] indices = new int[6 * (himg.Width - 1) * (himg.Height - 1)];
            int vertexPointer = 0;
            for (int i = 0; i < himg.Width; i++)
            {
                heightCoor.Add(new List<float>());
                for (int j = 0; j < himg.Height; j++)
                {
                    float height = GetPixelHeight(i, j, himg);
                    //float height = 0;
                    heightCoor[i].Add(height);
                    vertices[vertexPointer] =
                    new Vector3(
                    (float)j / ((float)himg.Width - 1) * GetWidth(),
                    height,
                    (float)i / ((float)himg.Height - 1) * GetDepth()
                    );

                    normals[vertexPointer] = CalculateNormal(i, j, himg);

                    textureCoords[vertexPointer] = new Vector2
                    (
                        20 * (float)j / ((float)himg.Width - 1),
                        20 * (float)i / ((float)himg.Height - 1)
                    );

                    vertexPointer++;
                }
            }

            int pointer = 0;
            for (int gz = 0; gz < himg.Height - 1; gz++)
            {
                for (int gx = 0; gx < himg.Width - 1; gx++)
                {
                    int topLeft = (gz * himg.Width) + gx;
                    int topRight = topLeft + 1;
                    int bottomLeft = ((gz + 1) * himg.Height) + gx;
                    int bottomRight = bottomLeft + 1;
                    indices[pointer++] = topLeft;
                    indices[pointer++] = bottomLeft;
                    indices[pointer++] = topRight;
                    indices[pointer++] = topRight;
                    indices[pointer++] = bottomLeft;
                    indices[pointer++] = bottomRight;
                }
            }
            Positions.AddRange(vertices);
            TextureCoordinates.AddRange(textureCoords);
            Normals.AddRange(normals);
            Indeces.AddRange(indices);
        }

        private Vector3 CalculateNormal(int x, int z, Bitmap img)
        {
            float heightL = GetPixelHeight(x - 1, z, img);
            float heightR = GetPixelHeight(x + 1, z, img);
            float heightD = GetPixelHeight(x, z - 1, img);
            float heightU = GetPixelHeight(x, z + 1, img);
            Vector3 normal = new Vector3(heightL - heightR, 2f, heightD - heightU);
            normal.Normalize();
            return normal;
        }

        public override void Setup_TextureCoordinates(float xScale = 1, float yScale = 1)
        {
            TextureCoordinates.Add(new Vector2(0, 0));
            TextureCoordinates.Add(new Vector2(0, 1 * yScale));
            TextureCoordinates.Add(new Vector2(1 * xScale, 1 * yScale));
            TextureCoordinates.Add(new Vector2(1 * xScale, 0));
        }

        public override void Setup_Normals()
        {
            Normals.Add(Vector3.UnitY);
            Normals.Add(Vector3.UnitY);
            Normals.Add(Vector3.UnitY);
            Normals.Add(Vector3.UnitY);
        }

        public override void RenderModel()
        {
            Renderer = new TerrainRenderer(this);
            Default_RenderModel();
        }

        public override void Live_Update(Shader ShaderModel)
        {
            base.Live_Update(ShaderModel);
            ShaderModel.SetInt(ShaderModel.Location_ShaderType, (int)ShaderModel.ShaderType);
            ShaderModel.SetMatrix4(ShaderModel.Location_LocalTransform, LocalTransform);
        }
    }
}