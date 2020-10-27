#version 400 core


#include "Frag_Default.glsl" //! #include "D:\Revit_API\Projects\InSitU\InSitU\Views\ThreeD\Engine\Space\Render\Source\lib\Frag_Default.glsl"
#include "Frag_Texture2D.glsl" //! #include "D:\Revit_API\Projects\InSitU\InSitU\Views\ThreeD\Engine\Space\Render\Source\lib\Frag_Texture2D.glsl"

in vec2 blureCoor2[11];

//http://dev.theomader.com/gaussian-kernel-calculator/
void main(void)
{
 	FragColor = vec4(0);
    int i=-1;
 	FragColor += texture(DiffuseMap,blureCoor2[++i]) * 0.0093;
 	FragColor += texture(DiffuseMap,blureCoor2[++i]) * 0.028002;
 	FragColor += texture(DiffuseMap,blureCoor2[++i]) * 0.065984;
 	FragColor += texture(DiffuseMap,blureCoor2[++i]) * 0.121703;
 	FragColor += texture(DiffuseMap,blureCoor2[++i]) * 0.175713;
 	FragColor += texture(DiffuseMap,blureCoor2[++i]) * 0.198596;
 	FragColor += texture(DiffuseMap,blureCoor2[++i]) * 0.175713;
 	FragColor += texture(DiffuseMap,blureCoor2[++i]) * 0.121703;
 	FragColor += texture(DiffuseMap,blureCoor2[++i]) * 0.065984;
 	FragColor += texture(DiffuseMap,blureCoor2[++i]) * 0.028002;
 	FragColor += texture(DiffuseMap,blureCoor2[++i]) * 0.0093;
    
}