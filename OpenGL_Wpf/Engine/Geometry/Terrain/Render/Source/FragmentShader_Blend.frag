#version 400 core


//Textures
uniform sampler2D GrassFlowerTexture;
uniform sampler2D GrassTexture;
uniform sampler2D RoadTexture;
uniform sampler2D DirtTexure;
uniform bool IsTransparent;
uniform bool LoadNormalMaps;

//definitions
 
vec4 Blend2DTextures();

#include "Frag_Default.glsl" //! #include "D:\Revit_API\Projects\Simple_Engine\OpenGL_Wpf\Engine\Space\Render\ShaderSystem\Source\lib\Frag_Default.glsl"
#include "Frag_Texture2D.glsl" //! #include "D:\Revit_API\Projects\Simple_Engine\OpenGL_Wpf\Engine\Space\Render\ShaderSystem\Source\lib\Frag_Texture2D.glsl"
#include "Frag_Light.glsl" //! #include "D:\Revit_API\Projects\Simple_Engine\OpenGL_Wpf\Engine\Space\Render\ShaderSystem\Source\lib\Frag_Light.glsl"
#include "Frag_Fog.glsl" //! #include "D:\Revit_API\Projects\Simple_Engine\OpenGL_Wpf\Engine\Space\Render\ShaderSystem\Source\lib\Frag_Fog.glsl"
#include "Frag_Shadow.glsl" //! #include "D:\Revit_API\Projects\Simple_Engine\OpenGL_Wpf\Engine\Space\Render\ShaderSystem\Source\lib\Frag_Shadow.glsl"


void main()
{ 
    vec4 pixelColor;
    pixelColor = Blend2DTextures();

    float shadow = AddShadow();
    pixelColor = AddLight(pixelColor, shadow, surfaceNormal); //no shadow

    pixelColor = AddFog(pixelColor);

    FragColor =  Finalize(pixelColor);
}

vec4 Blend2DTextures()
{
    //get the blend value
    //10 here is the number of tiles this texture is repeated. the blend texture should be always 1 to 1 to get this case working
    vec4 blendMapColor = texture(DiffuseMap,textureCoor2/10);
    float blendAmount = 1 - (blendMapColor.r + blendMapColor.g + blendMapColor.b);
    vec4 backGround = texture(GrassTexture,textureCoor2) * blendAmount;
    vec4 roadTexture = texture(RoadTexture,textureCoor2)*(blendMapColor.b);
    vec4 grassTexture = texture(GrassFlowerTexture ,textureCoor2)*(blendMapColor.g);
    vec4 dirtTexture = texture(DirtTexure,textureCoor2)*(blendMapColor.r);
    vec4  pixelColor = backGround + grassTexture + dirtTexture + roadTexture;
    return pixelColor;
}