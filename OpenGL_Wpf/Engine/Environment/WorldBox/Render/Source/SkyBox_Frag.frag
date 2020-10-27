#version 400 core

uniform samplerCube dayTexture;
uniform samplerCube nightTexture;
uniform float BlendFactor;


#include "Frag_Default.glsl" //! #include "D:\Revit_API\Projects\Simple_Engine\OpenGL_Wpf\Engine\Space\Render\Source\lib\Frag_Default.glsl"
#include "Frag_TextureCube.glsl" //! #include "D:\Revit_API\Projects\Simple_Engine\OpenGL_Wpf\Engine\Space\Render\Source\lib\Frag_TextureCube.glsl"
#include "Frag_Fog.glsl" //! #include "D:\Revit_API\Projects\Simple_Engine\OpenGL_Wpf\Engine\Space\Render\Source\lib\Frag_Fog.glsl"

void main(void)
{
	vec4 dayColor = texture(dayTexture,textureCoor3);
	vec4 nightColor = texture(nightTexture,textureCoor3);
	vec4 pixelColor= mix(dayColor,nightColor,BlendFactor);
		 
	if(HasFog)
	{
		pixelColor = AddFog(pixelColor);	 
	}

	pixelColor = AddToon(pixelColor);
	FragColor =pixelColor;
}
