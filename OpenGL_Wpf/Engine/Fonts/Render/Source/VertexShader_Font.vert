#version 400 core 
 
#include "Vert_Default.glsl" //! #include "D:\Revit_API\Projects\Simple_Engine\OpenGL_Wpf\Engine\Space\Render\Source\lib\Vert_Default.glsl"
#include "Vert_2DTexture.glsl" //! #include "D:\Revit_API\Projects\Simple_Engine\OpenGL_Wpf\Engine\Space\Render\Source\lib\Vert_2DTexture.glsl"


void main()
{
	mat4 modelTransform = GetLocalMatrix();
	gl_Position = modelTransform * vec4(aPosition2*vec2(1, -1), 0.0f, 1.0f);
	textureCoor2 = aTextureCoor;
}