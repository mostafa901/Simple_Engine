#version 400 core
 
out vec4 Color;
in vec4 VertexColor;

#include "Vert_Default.glsl" //! #include "D:\Revit_API\Projects\Simple_Engine\OpenGL_Wpf\Engine\Space\Render\Source\lib\Vert_Default.glsl"
#include "Vert_2DTexture.glsl" //! #include "D:\Revit_API\Projects\Simple_Engine\OpenGL_Wpf\Engine\Space\Render\Source\lib\Vert_2DTexture.glsl"
#include "Vert_Light.glsl" //! #include "D:\Revit_API\Projects\Simple_Engine\OpenGL_Wpf\Engine\Space\Render\Source\lib\Vert_Light.glsl"
#include "Vert_Fog.glsl" //! #include "D:\Revit_API\Projects\Simple_Engine\OpenGL_Wpf\Engine\Space\Render\Source\lib\Vert_Fog.glsl"
#include "Vert_Shadow.glsl" //! #include "D:\Revit_API\Projects\Simple_Engine\OpenGL_Wpf\Engine\Space\Render\Source\lib\Vert_Shadow.glsl"


void main()
{
    mat4 modelTransform = GetLocalMatrix();
    SetIsSelected();

    vec4 worldPosition = modelTransform * vec4(aPosition,1.0);
    vec4 positionFromCamera = ViewTransform * worldPosition;
    
    CheckClipPlan(worldPosition);
    LoadShadowPosition(worldPosition);
    
    gl_Position = ProjectionTransform * positionFromCamera ;

     
    LoadSurfaceNormal(modelTransform);
   
    if(EnableNormalMap)
    {
        //requires:
        // 1. SurfaceNormal
        // 2. in Tangent
        // 3. Enable NormalMap
        // 4. Shinning and damp
        mat3 TangentSpace =  GetNormalSpace(worldPosition, modelTransform);
        compileToLightSource(worldPosition,TangentSpace);
    }
    else
    {
        mat3 x;
        compileToLightSource(worldPosition,x);
    }

    Color = VertexColor;

    LoadFog(positionFromCamera);
}