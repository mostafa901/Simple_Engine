#version 400 core
 
#include "Frag_Default.glsl" //! #include "D:\Revit_API\Projects\InSitU\InSitU\Views\ThreeD\Engine\Space\Render\Source\lib\Frag_Default.glsl"

void main()
{
  FragColor =  Finalize(DefaultColor);
}