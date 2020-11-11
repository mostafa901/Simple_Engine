#version 400 core
 
#include "Frag_Default.glsl" //! #include "D:\Revit_API\Projects\Simple_Engine\OpenGL_Wpf\Engine\Space\Render\ShaderSystem\Source\lib\Frag_Default.glsl"
#include "Frag_Texture2D.glsl" //! #include "D:\Revit_API\Projects\Simple_Engine\OpenGL_Wpf\Engine\Space\Render\ShaderSystem\Source\lib\Frag_Texture2D.glsl"
#include "Frag_Light.glsl" //! #include "D:\Revit_API\Projects\Simple_Engine\OpenGL_Wpf\Engine\Space\Render\ShaderSystem\Source\lib\Frag_Light.glsl"
#include "Frag_Fog.glsl" //! #include "D:\Revit_API\Projects\Simple_Engine\OpenGL_Wpf\Engine\Space\Render\ShaderSystem\Source\lib\Frag_Fog.glsl"
#include "Frag_Shadow.glsl" //! #include "D:\Revit_API\Projects\Simple_Engine\OpenGL_Wpf\Engine\Space\Render\ShaderSystem\Source\lib\Frag_Shadow.glsl"

in vec4 Color;
 
void main()
{
    //vec4 pixelColor =  normalize(vec4((surfaceNormal*.5 +1)*2,1));
    if(Color != DefaultColor)
    {
        FragColor = Finalize(Color);
        return;
    }
    else
    {
        FragColor = Finalize(DefaultColor);
        return;
    }
    vec4 pixelColor =  Color;
    //vec4 pixelColor =  Color;

    float shadow = AddShadow();

    pixelColor = AddLight(pixelColor, shadow, surfaceNormal);

    pixelColor= AddFog(pixelColor);
   
    FragColor =  Finalize(pixelColor);
}