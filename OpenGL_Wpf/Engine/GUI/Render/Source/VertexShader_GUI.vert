#version 400 core

#include "Vert_Default.glsl" //! #include "D:\Revit_API\Projects\Simple_Engine\OpenGL_Wpf\Engine\Space\Render\ShaderSystem\Source\lib\Vert_Default.glsl"
#include "Vert_2DTexture.glsl" //! #include "D:\Revit_API\Projects\Simple_Engine\OpenGL_Wpf\Engine\Space\Render\ShaderSystem\Source\lib\Vert_2DTexture.glsl"

void main(void)
{
	mat4 modelTransform = GetLocalMatrix();
	gl_Position = modelTransform * vec4(aPosition2,0, 1.0);
	//textureCoords = vec2((aPosition.x+1.0)/2.0, 1 - (aPosition.y+1.0)/2.0);
	textureCoor2 = aTextureCoor;
}
