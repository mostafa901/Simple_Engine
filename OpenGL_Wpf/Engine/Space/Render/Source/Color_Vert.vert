#version 400 core
 
#include "Vert_Default.glsl" //! #include "D:\Revit_API\Projects\Simple_Engine\OpenGL_Wpf\Engine\Space\Render\Source\lib\Vert_Default.glsl"

void main()
{
     mat4 modelTransform = GetLocalMatrix();

     vec4 worldPosition = modelTransform * vec4(aPosition,1.0);
     vec4 positionFromCamera = ViewTransform * worldPosition;
     gl_Position = ProjectionTransform * positionFromCamera ;
 
}