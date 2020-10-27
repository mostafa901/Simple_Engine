#version 400 core
#include "Frag_Default.glsl" //! #include "D:\Revit_API\Projects\InSitU\InSitU\Views\ThreeD\Engine\Space\Render\Source\lib\Frag_Default.glsl"

void main(void)
{

 //Since we have no color buffer and disabled the draw and read buffers, the resulting fragments do not require any processing so we can simply use an empty fragment shader
 
	FragColor = vec4(gl_FragCoord.z);
	//fragColor = vec4(1,1,0,1);
  
}