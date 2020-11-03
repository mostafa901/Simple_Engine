#version 400 core

out vec3 ToCamera;
out vec4 clipSpace;

//World
uniform vec3 CameraPosition;

//Texture
const int tiling = 3;

#include "Vert_Default.glsl" //! #include "D:\Revit_API\Projects\Simple_Engine\OpenGL_Wpf\Engine\Space\Render\Source\lib\Vert_Default.glsl"
#include "Vert_2DTexture.glsl" //! #include "D:\Revit_API\Projects\Simple_Engine\OpenGL_Wpf\Engine\Space\Render\Source\lib\Vert_2DTexture.glsl"
#include "Vert_Light.glsl" //! #include "D:\Revit_API\Projects\Simple_Engine\OpenGL_Wpf\Engine\Space\Render\Source\lib\Vert_Light.glsl"
#include "Vert_Fog.glsl" //! #include "D:\Revit_API\Projects\Simple_Engine\OpenGL_Wpf\Engine\Space\Render\Source\lib\Vert_Fog.glsl"

void main()
{
    mat4 modelTransform = GetLocalMatrix();
	 
	vec4 worldPosition =  modelTransform  * vec4(aPosition, 1.0);
     gl_ClipDistance[1] = dot(ClipPlanY,worldPosition);
    
    ToCamera = normalize(vec3(vec4(CameraPosition,0) - worldPosition));

    vec4 positionFromCamera = ViewTransform * worldPosition;
    gl_Position = clipSpace = ProjectionTransform * positionFromCamera;
    textureCoor2 = aTextureCoor * tiling;
    
    LoadSurfaceNormal(modelTransform);
    compileFromLightSource(worldPosition);
}
