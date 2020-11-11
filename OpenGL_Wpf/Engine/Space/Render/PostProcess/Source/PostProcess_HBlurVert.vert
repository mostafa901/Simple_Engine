#version 400 core

#include "Vert_Default.glsl" //! #include "D:\Revit_API\Projects\Simple_Engine\OpenGL_Wpf\Engine\Space\Render\ShaderSystem\Source\lib\Vert_Default.glsl"
#include "Vert_2DTexture.glsl" //! #include "D:\Revit_API\Projects\Simple_Engine\OpenGL_Wpf\Engine\Space\Render\ShaderSystem\Source\lib\Vert_2DTexture.glsl"

out vec2 blureCoor2[11];
const float screenWidth=800;
void main(void)
{
	mat4 modelTransform = GetLocalMatrix();
	gl_Position = modelTransform * vec4(aPosition2,0, 1.0);
	//textureCoords = vec2((aPosition.x+1.0)/2.0, 1 - (aPosition.y+1.0)/2.0);
	vec2 coor = aPosition2*.5 +.5;
	float pixelSize = 1/screenWidth;

	for(int i=-5;i<5;i++)
	{
		blureCoor2[i+5] = coor + vec2(i*pixelSize,0);
	}
}
