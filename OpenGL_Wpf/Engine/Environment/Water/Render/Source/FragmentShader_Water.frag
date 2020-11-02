#version 400 core

#include "Frag_Default.glsl" //! #include "D:\Revit_API\Projects\Simple_Engine\OpenGL_Wpf\Engine\Space\Render\Source\lib\Frag_Default.glsl"
#include "Frag_Texture2D.glsl" //! #include "D:\Revit_API\Projects\Simple_Engine\OpenGL_Wpf\Engine\Space\Render\Source\lib\Frag_Texture2D.glsl"
#include "Frag_Light.glsl" //! #include "D:\Revit_API\Projects\Simple_Engine\OpenGL_Wpf\Engine\Space\Render\Source\lib\Frag_Light.glsl"
#include "Frag_Fog.glsl" //! #include "D:\Revit_API\Projects\Simple_Engine\OpenGL_Wpf\Engine\Space\Render\Source\lib\Frag_Fog.glsl"

//World
in vec4 clipSpace;
in vec3 ToCamera;
uniform float NearDistance;
uniform float FarDistance;

uniform sampler2D Reflection;
uniform sampler2D Refraction;
uniform sampler2D Dudv;
uniform sampler2D DepthMap;

const float WaveLength=0.02f;
uniform float moveFactor;

//Light
in vec3 fromLightSource[4];

//definitions
float LinearizeDepth(float depth)
{
    float z = depth * 2.0 - 1.0; // Back to NDC 
    return (2.0 * NearDistance * FarDistance) / (FarDistance + NearDistance - z * (FarDistance - NearDistance));  
}

void main()
{
	vec2 refractionCoor = (clipSpace.xy/clipSpace.w)/2 + 0.5f;
	vec2 reflectionCoor = refractionCoor * vec2(1, 1);

	float depth = texture(DepthMap,refractionCoor).r;
	float floorDistance = LinearizeDepth(depth);
		 
	depth = gl_FragCoord.z;	
	float waterDistance = LinearizeDepth(depth);
	float waterdepth = floorDistance-waterDistance;
//	FragColor= vec4(waterdepth/10);
//	return;
	
	vec2 distort = texture(Dudv, vec2(textureCoor2.x + moveFactor,textureCoor2.y)).rg * .1;
	distort = textureCoor2 + vec2(distort.x,distort.y + moveFactor);
	distort = (texture(Dudv, distort).rg * 2 - 1) * WaveLength;
	
	refractionCoor += distort;
	refractionCoor = clamp(refractionCoor,0.001,0.999);
	vec4 refractionColour = texture(Refraction,refractionCoor );
//	FragColor= refractionColour;
 //	return;
//	
	reflectionCoor += distort; 
	reflectionCoor = clamp(reflectionCoor,0.001,0.999);
	//reflectionCoor.x = clamp(reflectionCoor.y,.001,.999);
	//reflectionCoor.y = clamp(reflectionCoor.y,-.999,.001);
	vec4 reflectionColour = texture(Reflection,reflectionCoor);
//	FragColor= reflectionColour;
// 	return;

	vec4 textureNormalColor = texture(NormalMap,distort);
	vec3 normalColor = vec3(textureNormalColor.r * 2-1, textureNormalColor.g, textureNormalColor.b * 2 - 1);
	normalColor = normalize(normalColor);
	 
	vec3 reflectedlight = reflect(fromLightSource[0], normalColor);
	float specular = max(dot(reflectedlight, ToCamera),0);
	specular =pow(specular, ShiningDamp);
	vec4 specularHighlight = LightColor[0] * specular * ReflectionIndex;

	float reflectionFactor = dot(ToCamera,vec3(0,1,0));	
 	reflectionFactor  = pow(reflectionFactor, .5f);
	 
	//FragColor = reflectionColour;
	//return;
	FragColor = mix(reflectionColour, refractionColour, reflectionFactor);
	FragColor = mix(FragColor, vec4(0,.2,.5,1), 0.3f) + specularHighlight;
	FragColor.a = clamp(waterdepth/3,0,1);

	 FragColor = Finalize(FragColor);
}
