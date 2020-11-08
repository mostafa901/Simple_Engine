layout(location=0) out vec4 FragColor;
layout(location=1) out vec4 FragDefaultColor;
layout(location=2) out uint FragVertexColor;

#include "Frag_Effects.glsl" //! #include "D:\Revit_API\Projects\Simple_Engine\OpenGL_Wpf\Engine\Space\Render\Source\lib\Frag_Effects.glsl"

//Model
in float ModelSelected;


//Texture
uniform bool EnableNormalMap;

//RenderMode
uniform bool IsToonMode;
uniform int BrightnessLevels;

//Physics
uniform vec4 DefaultColor;
in vec3 VertexPosition;
in vec3 surfaceNormal;

//Environment
uniform vec4 SkyColor;

vec4 AddToon(vec4 currentPixel)
{
	if(IsToonMode)
	{
		float amount =  (currentPixel.r + currentPixel.g + currentPixel.b)/3.0f;
		float bright = floor(amount*BrightnessLevels)/BrightnessLevels;
		currentPixel = bright*currentPixel;		
	}
	
	return currentPixel;
}

vec4 Finalize(vec4 pixelColor)
{
	FragDefaultColor = DefaultColor;
	FragVertexColor = gl_PrimitiveID;
	
	if(ModelSelected>0.0)
    {
        pixelColor = AddSelection(pixelColor);
    }
    
	return pixelColor;
}