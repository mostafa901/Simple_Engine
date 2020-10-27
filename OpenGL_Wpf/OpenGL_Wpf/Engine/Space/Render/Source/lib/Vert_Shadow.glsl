 out vec4 ShadowPosition;
uniform mat4 LightProjectionTransform;
uniform mat4 LightViewTransform;

void LoadShadowPosition(vec4 worldPosition)
{
    ShadowPosition = LightProjectionTransform * LightViewTransform * worldPosition;
}