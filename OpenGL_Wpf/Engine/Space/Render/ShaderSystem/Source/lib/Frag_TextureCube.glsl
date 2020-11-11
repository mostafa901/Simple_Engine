//Texture Cube
uniform bool useCubeSpecularMap;
uniform samplerCube CubeDiffuseMap;
uniform samplerCube CubeNormalMap;
uniform samplerCube CubeSpecularMap;

in vec3 textureCoor3;

vec3 GetCubeNormalMap()
{	 if(EnableNormalMap)
{
	vec4 normalMapValue;
	normalMapValue = 2.0 * texture(CubeNormalMap, textureCoor3, -1.0) - 1.0;
	 
	return normalize(normalMapValue.rgb);
	}
	else
	{
	return surfaceNormal;
	}
}

vec4 GetCubeTexture(vec3 Coor3)
{
    return texture(CubeDiffuseMap, Coor3);
}

float GetCubeSpecularMapValue()
{
	return texture(CubeSpecularMap, textureCoor3).r;	 
}


float AddSpecular( )
{
	if(useCubeSpecularMap)
	{		 
		return GetCubeSpecularMapValue();
	}
		
	else return 0.0;
}