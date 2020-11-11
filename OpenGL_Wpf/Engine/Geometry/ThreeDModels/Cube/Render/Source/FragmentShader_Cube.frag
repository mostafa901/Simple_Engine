#version 400 core
 
#include "Frag_Default.glsl" //! #include "D:\Revit_API\Projects\Simple_Engine\OpenGL_Wpf\Engine\Space\Render\ShaderSystem\Source\lib\Frag_Default.glsl"
#include "Frag_TextureCube.glsl" //! #include "D:\Revit_API\Projects\Simple_Engine\OpenGL_Wpf\Engine\Space\Render\ShaderSystem\Source\lib\Frag_TextureCube.glsl"
#include "Frag_Light.glsl" //! #include "D:\Revit_API\Projects\Simple_Engine\OpenGL_Wpf\Engine\Space\Render\ShaderSystem\Source\lib\Frag_Light.glsl"
#include "Frag_Fog.glsl" //! #include "D:\Revit_API\Projects\Simple_Engine\OpenGL_Wpf\Engine\Space\Render\ShaderSystem\Source\lib\Frag_Fog.glsl"
 

void main()
{
    vec4 pixelColor  = GetCubeTexture(textureCoor3);
    
    vec3 normalMapValue = GetCubeNormalMap();
    
    pixelColor = AddLight(pixelColor, 1, normalMapValue);

    FragColor =  Finalize(pixelColor);
}