#version 400 core  
 
#include "Vert_Default.glsl" //! #include "D:\Revit_API\Projects\Simple_Engine\OpenGL_Wpf\Engine\Space\Render\ShaderSystem\Source\lib\Vert_Default.glsl"
#include "Vert_3DTexture.glsl" //! #include "D:\Revit_API\Projects\Simple_Engine\OpenGL_Wpf\Engine\Space\Render\ShaderSystem\Source\lib\Vert_3DTexture.glsl"
#include "Vert_Light.glsl" //! #include "D:\Revit_API\Projects\Simple_Engine\OpenGL_Wpf\Engine\Space\Render\ShaderSystem\Source\lib\Vert_Light.glsl"
#include "Vert_Fog.glsl" //! #include "D:\Revit_API\Projects\Simple_Engine\OpenGL_Wpf\Engine\Space\Render\ShaderSystem\Source\lib\Vert_Fog.glsl"

void main()
{
    textureCoor3 = aPosition  ;
    SetIsSelected();
    mat4 modelTransform = GetLocalMatrix();
    vec4 worldPosition = modelTransform * vec4(aPosition,1.0);

    vec4 positionFromCamera= ViewTransform * worldPosition;
    gl_Position = ProjectionTransform * positionFromCamera;

    LoadSurfaceNormal(modelTransform);

    if(EnableNormalMap)
    {
       mat3 TangentSpace = GetNormalSpace(worldPosition, modelTransform);
       compileToLightSource(worldPosition,TangentSpace);
    }
    else
    {
        mat3 x;
        compileToLightSource(worldPosition,x);
    }

    LoadFog(positionFromCamera);
}
