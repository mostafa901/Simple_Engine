using InSitU.Views.ThreeD.Engine.Core.Interfaces;
using InSitU.Views.ThreeD.Engine.Geometry;
using InSitU.Views.ThreeD.Engine.Geometry.TwoD;
using InSitU.Views.ThreeD.Engine.Render;
using InSitU.Views.ThreeD.Engine.Space;
using InSitU.Views.ThreeD.Extentions;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InSitU.Views.ThreeD.Engine.Core.Abstracts
{
    public enum TextureMode
    {
        Texture2D,
        TextureCube,
        Blend
    }

    public abstract class Base_Texture
    {
        public string Name;
        public List<Base_Texture> TextureIds = new List<Base_Texture>();
        public int TextureId;
        public TextureUnit textureSlot;
        public TextureTarget TextureTargetType { get; }
        public bool IsTransparent { get; set; } = false;
        public int NormalMap { get; private set; }
        private bool EnableNormalMap { get; set; } = false;
        public bool useSpecularMap { get; set; } = false;
        private bool useCubeSpecularMap { get; set; } = false;
        public int numberOfRows { get; set; } = 1;
        public int TextureAtlasId { get; set; } = 0;

        public Base_Texture(TextureMode textureTargetType)
        {
            TextureIds = new List<Base_Texture>();

            switch (textureTargetType)
            {
                case TextureMode.Texture2D:
                    TextureTargetType = TextureTarget.Texture2D;
                    break;

                case TextureMode.TextureCube:
                    TextureTargetType = TextureTarget.TextureCubeMap;
                    break;

                case TextureMode.Blend:
                    {
                        TextureTargetType = TextureTarget.Texture2D;
                        break;
                    }
            }
        }

        protected virtual void Setup_2DTexture(string imgPath, TextureUnit textureUnit)
        {
            // https://stackoverflow.com/questions/6803685/texture-is-lacking-colors
            //WE are using bgra instead of RGBA, Windows bitmaps and opengl have a different endianess on pixels i.e. the bytes are reversed.
            Create_TextureModel(textureUnit);
            if (!string.IsNullOrEmpty(imgPath))
            {
                Bitmap img;
                var data = GetImgData(imgPath, out img);
                CompileImage(data, TextureTarget.Texture2D);
                img.Dispose();
            }
            Set_TextureFilter(TextureMinFilter.Nearest);
            Set_Texture_Tiling(TextureWrapMode.Repeat);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            GL.TexParameter(TextureTargetType, TextureParameterName.TextureLodBias, -0.4f);
            Activate_Ansitropic();
        }

        public void Activate_Ansitropic()
        {
            /*It's very unlikely that your card does not support Anisotropic filtering:
            "Anisotropic filtering is relatively intensive (primarily memory bandwidth and to some degree computationally, though the standard space–time tradeoff rules apply) and only became a standard feature of consumer-level graphics cards in the late 1990s."
https://en.wikipedia.org/wiki/Anisotropic_filtering

            //todo: can't find the api access to this feature
            */

            //this is to get the max
            //float maxAniso;
            //GL.GetFloat((GetPName)ExtTextureFilterAnisotropic.MaxTextureMaxAnisotropyExt, out maxAniso);

            GL.TexParameter(TextureTarget.Texture2D, (TextureParameterName)ExtTextureFilterAnisotropic.TextureMaxAnisotropyExt, 4);
        }

        protected void CompileImage(BitmapData data, TextureTarget textureTarget)
        {
            GL.TexImage2D(textureTarget, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
        }

        protected void Create_TextureModel(TextureUnit textureUnit)
        {
            TextureId = GL.GenTexture();
            this.textureSlot = textureUnit;
            TextureIds.Add(this);
            BindTexture(TextureId, textureUnit);
        }

        private void BindTexture(int textureID, TextureUnit textureUnit)
        {
            GL.ActiveTexture(textureUnit);
            GL.BindTexture(TextureTargetType, textureID);
        }

        protected void Setup_CubeTexture(List<string> imgPaths, TextureUnit textureUnit)
        {
            Create_TextureModel(textureUnit);

            while (imgPaths.Count < 6)
            {
                imgPaths.Add(imgPaths[0]);
            }
            //store the pixels at location 0
            for (int i = 0; i < 6; i++)
            {
                Bitmap img;

                var data = GetImgData(imgPaths[i], out img);
                CompileImage(data, TextureTarget.TextureCubeMapPositiveX + i);
                img.Dispose();
            }

            Set_TextureFilter(TextureMinFilter.Linear);
            Set_Texture_Tiling(TextureWrapMode.ClampToEdge);

            GL.GenerateMipmap(GenerateMipmapTarget.TextureCubeMap);
            //-1, is to high res.closer to 0 is low res
            GL.TexParameter(TextureTargetType, TextureParameterName.TextureLodBias, -0.4f);
            Activate_Ansitropic();
        }

        public void Set_Texture_Tiling(TextureWrapMode wrapMode)
        {
            ////Setup Wrapping mode
            GL.TexParameter(TextureTargetType, TextureParameterName.TextureWrapS, (int)wrapMode);
            GL.TexParameter(TextureTargetType, TextureParameterName.TextureWrapT, (int)wrapMode);
            GL.TexParameter(TextureTargetType, TextureParameterName.TextureWrapR, (int)wrapMode);
        }

        public void Set_TextureFilter(TextureMinFilter filterType)
        {
            //Setup how to pick the pixel is it blended or exact color
            GL.TexParameter(TextureTargetType, TextureParameterName.TextureMinFilter, (int)filterType);
            GL.TexParameter(TextureTargetType, TextureParameterName.TextureMagFilter, (int)filterType);
        }

        protected static BitmapData GetImgData(string imgPath, out Bitmap img)
        {
            img = Bitmap.FromFile(imgPath) as Bitmap;

            var data = img.LockBits(
                  new Rectangle(0, 0, img.Width, img.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly,
                  System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            return data;
        }

        public virtual void Live_Update(Shader shaderModel)
        {
            foreach (var texture in TextureIds)
            {
                BindTexture(texture.TextureId, texture.textureSlot);
            }
        }

        public float GetTextureXOffset(int slotNumber)
        {
            int column = slotNumber % numberOfRows;
            return (float)column / (float)numberOfRows;
        }

        public float GetTextureYOffset(int slotNumber)
        {
            int row = slotNumber / numberOfRows;
            return (float)row / (float)numberOfRows;
        }

        public virtual void Dispose()
        {
            foreach (var texture in TextureIds)
            {
                if (texture.Name == "ShadowMap") continue;
                DisposeTexture(texture);
            }
        }

        public static void DisposeTexture(Base_Texture texture)
        {
            GL.DeleteTexture(texture.TextureId);
        }

        /// <summary>
        /// ensure Loading Glossiness and Tangents
        /// </summary>
        /// <param name="LoadMaps"></param>
        public virtual void Set_LoadNormalMap(bool LoadMaps)
        {
            foreach (var textureModel in TextureIds)
            {
                textureModel.EnableNormalMap = LoadMaps;
            }
            EnableNormalMap = LoadMaps;
        }

        public virtual void UploadDefaults(Shader ShaderModel)
        {
            ShaderModel.SetBool(ShaderModel.IsTransparentLocation, IsTransparent);

            if (EnableNormalMap)
            {
                ShaderModel.SetBool(ShaderModel.Location_EnableNormalMap, EnableNormalMap);

                ShaderModel.SetArray3(ShaderModel.LightEyePositionLocation, Game.Context.ActiveScene.Lights.Select(o => o.GetEyeSpacePosition(CameraModel.ActiveCamera.ViewTransform)), new Vector3());
            }

            ShaderModel.SetInt(ShaderModel.Location_DiffuseMap, 0);
            ShaderModel.SetInt(ShaderModel.Location_SpecularMap, 1);
            ShaderModel.SetInt(ShaderModel.Location_NormalMap, 2);

            ShaderModel.SetBool(ShaderModel.Location_useSpecularMap, useSpecularMap);
            ShaderModel.SetBool(ShaderModel.Location_useCubeSpecularMap, useCubeSpecularMap);
            ShaderModel.SetInt(ShaderModel.numberOfRowsTextureLocation, numberOfRows);
            ShaderModel.SetVector2(ShaderModel.OffsetTextureLocation, new Vector2(GetTextureXOffset(TextureAtlasId), GetTextureYOffset(TextureAtlasId)));


        }
    }
}