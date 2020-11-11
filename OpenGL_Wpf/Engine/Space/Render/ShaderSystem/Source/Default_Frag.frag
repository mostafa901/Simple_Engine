#version 400 core

#include "Frag_Default.glsl" //! #include "D:\Revit_API\Projects\Simple_Engine\OpenGL_Wpf\Engine\Space\Render\ShaderSystem\Source\lib\Frag_Default.glsl"
#include "Frag_Texture2D.glsl" //! #include "D:\Revit_API\Projects\Simple_Engine\OpenGL_Wpf\Engine\Space\Render\ShaderSystem\Source\lib\Frag_Texture2D.glsl"
#include "Frag_Light.glsl" //! #include "D:\Revit_API\Projects\Simple_Engine\OpenGL_Wpf\Engine\Space\Render\ShaderSystem\Source\lib\Frag_Light.glsl"
#include "Frag_Fog.glsl" //! #include "D:\Revit_API\Projects\Simple_Engine\OpenGL_Wpf\Engine\Space\Render\ShaderSystem\Source\lib\Frag_Fog.glsl"
#include "Frag_Shadow.glsl" //! #include "D:\Revit_API\Projects\Simple_Engine\OpenGL_Wpf\Engine\Space\Render\ShaderSystem\Source\lib\Frag_Shadow.glsl"

void main()
{
    vec4 pixelColor =  texture(DiffuseMap,textureCoor2);
    if(pixelColor.a < 0.1) 
    {
        discard;
    }


    float shadow = AddShadow();
    vec3 normalMap = GetNormalMap();

    pixelColor = AddLight(pixelColor, shadow, normalMap);

    pixelColor= AddFog(pixelColor);
    FragColor = pixelColor;
}