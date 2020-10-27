#version 400 core

#include "Vert_Default.glsl" //! #include "D:\Revit_API\Projects\Simple_Engine\OpenGL_Wpf\Engine\Space\Render\Source\lib\Vert_Default.glsl"
#include "Vert_3DTexture.glsl" //! #include "D:\Revit_API\Projects\Simple_Engine\OpenGL_Wpf\Engine\Space\Render\Source\lib\Vert_3DTexture.glsl"
#include "Vert_Fog.glsl" //! #include "D:\Revit_API\Projects\Simple_Engine\OpenGL_Wpf\Engine\Space\Render\Source\lib\Vert_Fog.glsl"

const float lowerlimit = .2f;
const float upperlimit = 50;

void main(void)
{
	mat4 modelTransform = GetLocalMatrix();
	vec4 worldPosition = modelTransform *  vec4(aPosition,1.0);
	vec4 positionFromCamera= ViewTransform * worldPosition;
    
	gl_Position = ProjectionTransform * positionFromCamera;
	
	textureCoor3 = aPosition;

	if(HasFog)
	{
		fogVisibility = (textureCoor3.y-lowerlimit)/upperlimit-lowerlimit;
		fogVisibility = clamp(fogVisibility,0,1);
	}
}