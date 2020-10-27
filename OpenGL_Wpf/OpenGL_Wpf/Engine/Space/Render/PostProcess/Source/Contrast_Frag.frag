#version 400 core


#include "Frag_Default.glsl" //! #include "D:\Revit_API\Projects\InSitU\InSitU\Views\ThreeD\Engine\Space\Render\Source\lib\Frag_Default.glsl"
#include "Frag_Texture2D.glsl" //! #include "D:\Revit_API\Projects\InSitU\InSitU\Views\ThreeD\Engine\Space\Render\Source\lib\Frag_Texture2D.glsl"


void main(void)
{
 	//FragColor = vec4(1,1,0,1);
 	FragColor = texture(DiffuseMap,textureCoor2);
    FragColor.rgb = (FragColor.rgb - 0.5) * (1.0 + 0.3) + 0.5;
}
