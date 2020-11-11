#version 400 core
in vec4 textureoffset;
in float Blend;

out vec2 textureCoords1;
out vec2 textureCoords2;

out float BlendValue;

#include "Vert_Default.glsl" //! #include "D:\Revit_API\Projects\Simple_Engine\OpenGL_Wpf\Engine\Space\Render\ShaderSystem\Source\lib\Vert_Default.glsl"
#include "Vert_2DTexture.glsl" //! #include "D:\Revit_API\Projects\Simple_Engine\OpenGL_Wpf\Engine\Space\Render\ShaderSystem\Source\lib\Vert_2DTexture.glsl"


void main()
{
    mat4 modelTransform = GetLocalMatrix();
    vec4 worldPosition =  modelTransform * vec4(aPosition2,0,1.0); 
    vec4 positionFromCamera= ViewTransform * worldPosition;
    gl_Position = ProjectionTransform * positionFromCamera;
    textureCoords1 =(aTextureCoor/numberOfRows) + textureoffset.xy;
    textureCoords2 =(aTextureCoor/numberOfRows) + textureoffset.zw;
    BlendValue = Blend;
}