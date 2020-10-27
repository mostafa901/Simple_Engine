#version 400 core

#include "Vert_Default" //! #include "D:\Revit_API\Projects\Simple_Engine\OpenGL_Wpf\Engine\Space\Render\Source\lib\Vert_Default.glsl"

//transforms
uniform mat4 LightProjectionTransform;
uniform mat4 LightViewTransform;

void main(void) 
{
	mat4 modelTransform = GetLocalMatrix();
	gl_Position = LightProjectionTransform * LightViewTransform * modelTransform  * vec4(aPosition,1.0);
}