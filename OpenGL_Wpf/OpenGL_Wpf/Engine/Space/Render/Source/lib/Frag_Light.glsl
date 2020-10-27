
//Light
uniform int MaximumLight;
in vec3 toLightSource[4];
uniform vec4 LightColor[4];
uniform vec3 Attenuation[4];
uniform float BackLite;
 

//Material
uniform float ShiningDamp; // how much shiny when the camera is close to the reflected ray
uniform float ReflectionIndex;

in vec3 rayTocameraPosition;

//Requires SurfaceNormal [DefaultFrag] calculated

float GetAttenuation(int i)
{
        float displacement = length(toLightSource[i]);
        float attenuationFactor = Attenuation[i].x + (Attenuation[i].y * displacement) + (Attenuation[i].z * displacement * displacement);
        return attenuationFactor;
}


float CompileLightBrightness(int i,vec3 unitNormal )
{
    vec3 surfacenormalized=normalize(unitNormal);
    vec3 lightnormalized=normalize(toLightSource[i]);
    float nDot = dot(surfacenormalized,lightnormalized);
    float scalerbrightness= max(nDot,0.0);
	if(IsToonMode)
		{
			float toonedBright= floor(scalerbrightness * BrightnessLevels);
			scalerbrightness = toonedBright/BrightnessLevels;
		}
	return scalerbrightness;
}

vec3 CompileLightSpecular(int i,vec3 unitNormal)
{
    if(ShiningDamp==0 || ReflectionIndex==0)
    {
        return vec3(0);
    }
    vec3 LightToVertex = -normalize(toLightSource[i]);
    vec3 surfacenormalized = normalize(unitNormal);
    vec3 reflectedRay = reflect(LightToVertex,surfacenormalized);
    float specularFactor= dot(reflectedRay,normalize(rayTocameraPosition));
    specularFactor = max(specularFactor, 0.0f);
    float dumpness = pow(specularFactor, ShiningDamp);
   
   if(IsToonMode)
    {
        float toonSpecular = floor(dumpness * BrightnessLevels);
        dumpness = toonSpecular/BrightnessLevels;
    }

    float specular = dumpness * ReflectionIndex;
    vec3 coloredSpecular = specular * (LightColor[i]).xyz ;
    return coloredSpecular;
}
 
vec4 AddLight(vec4 currentPixelColor, float shadow,vec3 normalVector)
{
	vec3 specular = vec3(0);
    vec4 diffuse = vec4(0);	
	 
	for (int i=0; i < MaximumLight; i++)
    {
        float attenuationFactor = GetAttenuation(i);
        float brightness = CompileLightBrightness(i,normalVector);
        specular += CompileLightSpecular(i,normalVector)/attenuationFactor;
        diffuse += (brightness * LightColor[i])/attenuationFactor;
    }
	
    diffuse = max(diffuse * shadow, BackLite);    
    diffuse *= currentPixelColor;		
	specular = max(specular,.2f);
	specular *= AddSpecular();	
	currentPixelColor = diffuse + vec4(specular,0);
	
	return currentPixelColor;
}