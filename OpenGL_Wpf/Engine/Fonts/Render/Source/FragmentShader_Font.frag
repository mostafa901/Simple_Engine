#version 400 core

#include "Frag_Default.glsl" //! #include "D:\Revit_API\Projects\Simple_Engine\OpenGL_Wpf\Engine\Space\Render\ShaderSystem\Source\lib\Frag_Default.glsl"
#include "Frag_Texture2D.glsl" //! #include "D:\Revit_API\Projects\Simple_Engine\OpenGL_Wpf\Engine\Space\Render\ShaderSystem\Source\lib\Frag_Texture2D.glsl"
 

void main(){

	 
	vec4 pixel =  texture(DiffuseMap,textureCoor2);
	if(pixel== vec4(0,0,0,1))
	discard;
	
	FragColor = vec4(1,0,0,1);

}