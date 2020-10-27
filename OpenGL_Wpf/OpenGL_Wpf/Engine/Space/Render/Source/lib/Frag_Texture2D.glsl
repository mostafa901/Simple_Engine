 

//Texture2D
uniform bool useSpecularMap;
uniform sampler2D DiffuseMap;
uniform sampler2D NormalMap;
uniform sampler2D SpecularMap;

in vec2 textureCoor2;

vec3 GetNormalMap()
{	 
	if(EnableNormalMap)
	{
		vec4 normalMapValue;
		normalMapValue = 2.0 * texture(NormalMap, textureCoor2) - 1.0;
		 
		return normalize(normalMapValue.rgb);	 	
	}
	else
	{
		return surfaceNormal;
	}
}

float GetSpecularMapValue()
{
	return  texture(SpecularMap,textureCoor2).r;	 
}

float AddSpecular( )
{
	  if(useSpecularMap)
	{		 
		return GetSpecularMapValue();
	}
	else return 0.0;
}