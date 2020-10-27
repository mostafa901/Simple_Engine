//Light
uniform int MaximumLight;
uniform vec3 LightPosition[4];
uniform vec3 LightEyePosition[4]; //used for Normal textures
uniform bool EnableNormalMap;

out vec3 toLightSource[4]; //used for Normal textures
out vec3 rayTocameraPosition;
out vec3 fromLightSource[4]; //used for Reflections



void compileToLightSource(vec4 worldPosition, mat3 toTangentSpace)
{
	if(EnableNormalMap)
	{
		 for(int i=0;i<MaximumLight;i++)
        {
		    toLightSource[i] = toTangentSpace * (LightEyePosition[i] - worldPosition.xyz);
	    }
	    rayTocameraPosition = toTangentSpace * (-worldPosition.xyz);
	}	
	else
	{
		for(int i=0;i<MaximumLight;i++)
		{
			toLightSource[i] = LightPosition[i] - worldPosition.xyz;
		}
		
		rayTocameraPosition = (inverse(ViewTransform) * vec4(0,0,0,1)).xyz - worldPosition.xyz;
	}
}	

//used for water Refraction
void compileFromLightSource(vec4 worldPosition)
{
    
        for(int i=0;i<MaximumLight;i++)
        {
            fromLightSource[i] = normalize(worldPosition.xyz-LightPosition[i]);
        }
    
}