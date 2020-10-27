#version 400 core

#include "Frag_Default.glsl" //! #include "D:\Revit_API\Projects\InSitU\InSitU\Views\ThreeD\Engine\Space\Render\Source\lib\Frag_Default.glsl"
#include "Frag_Texture2D.glsl" //! #include "D:\Revit_API\Projects\InSitU\InSitU\Views\ThreeD\Engine\Space\Render\Source\lib\Frag_Texture2D.glsl"


void main(void)
{
 	FragColor = texture(DiffuseMap,textureCoor2);
}

