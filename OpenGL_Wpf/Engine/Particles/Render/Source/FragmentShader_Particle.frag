#version 400 core

in float BlendValue;
in vec2 textureCoords1;
in vec2 textureCoords2;

#include "Frag_Default.glsl" //! #include "D:\Revit_API\Projects\Simple_Engine\OpenGL_Wpf\Engine\Space\Render\ShaderSystem\Source\lib\Frag_Default.glsl"
#include "Frag_Texture2D.glsl" //! #include "D:\Revit_API\Projects\Simple_Engine\OpenGL_Wpf\Engine\Space\Render\ShaderSystem\Source\lib\Frag_Texture2D.glsl"


void main()
{
	vec4 pixelColor1 = texture(DiffuseMap,textureCoords1); 
	vec4 pixelColor2 = texture(DiffuseMap,textureCoords2); 
	vec4 pixelColor = mix(pixelColor1,pixelColor2,BlendValue);
	if(pixelColor.a<.1f)
	{
		discard;
	}
	FragColor=pixelColor;
}