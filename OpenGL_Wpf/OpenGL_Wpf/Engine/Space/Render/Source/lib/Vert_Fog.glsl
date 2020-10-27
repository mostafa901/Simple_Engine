out float fogVisibility;

uniform bool HasFog;
uniform float Density; //how strong the fog is
uniform float FogSpeed; //how fast the color fades with fog

void LoadFog(vec4 positionFromCamera)
{
    float distance = length(positionFromCamera);
    fogVisibility = exp(-pow((distance*Density),FogSpeed));
    fogVisibility = clamp(fogVisibility,0.0f,1.0f);
}