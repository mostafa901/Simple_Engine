uniform vec4 FogColor;
uniform bool HasFog;

in float fogVisibility; 

vec4 AddFog(vec4 currentPixel)
{
 if(HasFog)
    {
        currentPixel = mix(SkyColor,currentPixel,.2f);
        currentPixel = mix(FogColor,currentPixel,fogVisibility);
    }
	return currentPixel;
}