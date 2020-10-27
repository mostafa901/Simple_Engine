#version 400 core
 
#include "Frag_Default.glsl" //! #include "D:\Revit_API\Projects\Simple_Engine\OpenGL_Wpf\Engine\Space\Render\Source\lib\Frag_Default.glsl"

void main()
{
  FragColor =  Finalize(DefaultColor);
}