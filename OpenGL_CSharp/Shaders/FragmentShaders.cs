using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace OpenGL_CSharp.Shaders
{
    class FragmentShaders
    {
        public static string Frag()
        {
            return @"
#version 330 core
//out to screen
//-------------
out vec4 FragColor;

//in from shaders, ensure same name in vertex shaders
//----------------------------------------------------
in vec4 vcolor; //vertex Color 

//fragment additional properties
uniform vec3 objectColor;
unifom vec3 lightColor;
 
void main(){
FragColor=vec4(lightColor*objectColor,1.0f);
}
";
        }

        public static string TexFrag()
        {
            return @"
#version 330 core

out vec4 outputColor;
in vec2 texCoord;
uniform sampler2D texture0;

void main(){
outputColor = texture(texture0,texCoord);
}
";
        }

        public static string TexFrag2Tex()
        {
            return @"
#version 330 core

out vec4 outputColor;
in vec2 texCoord;
in vec4 vcolor;
uniform sampler2D texture0;
uniform sampler2D texture1;

void main(){
outputColor = mix(texture(texture0,texCoord)*vec4(vcolor),texture(texture1,texCoord),0.5);
outputColor = mix   (outputColor , texture(texture0,texCoord)*vec4(vcolor),.5);
}
";
        }

        public static string LightFrag()
        {
            return @"
#version 330 core
out vec4 FragColor;

in vec4 vcolor; //vertex Color
in vec2 texCoord;
in vec3 PixelNormal; //normal vector of pixel
in vec3 FragPos; //Pixel position due world coordinates

uniform vec3 objectColor;
uniform vec3 lightColor;
uniform vec3 LightPos; 
uniform sampler2D texture0;
uniform sampler2D texture1;
uniform vec3 ViewPos; //View (Camera) Position
uniform float specintens = 1f; //setupSpecular intenesty 


void main(){

float ambientstrength=.1f;

if(lightColor == vec3(0,0,0))
{
FragColor = vec4(ambientstrength * objectColor,1);
}
else FragColor=vec4(lightColor*objectColor,1);

vec3 normal = normalize(PixelNormal); //notmalize Pixel normal, we only need Direction
vec3 lightDir= normalize(LightPos-FragPos); //get the Vectore Ray between light position and target (Pixel) Position,
float reflectionAngle = dot(lightDir,normal); //calculate the angle to use it as a degree of diffuse effeciency
float diffuseamount = max(reflectionAngle,0); //if the angle is less tha 0 then the ray is not hitting the pixel
vec3 diffusecolor = diffuseamount*lightColor; //multiply the diffuse value by the light color to get the amount of light required to lighten up object

//Create Specular

vec3 viewDir = normalize(ViewPos-FragPos); //get the direction from the pixel to the camera
vec3 reflDir = reflect(-lightDir,PixelNormal); //get the reflection vector of the vector from source to pixel on Normal vector of pixel
float spec = pow(max(dot(viewDir, reflDir),0),256); //pow here is for the radius of the specular the more the narrower
vec3 specularV= specintens * spec * lightColor; //multiply all to get the actual specular intenisty and color and radius

//Finally combine the results
FragColor = vec4((ambientstrength + diffusecolor + specularV) *objectColor,1.0f); //multiply the sum of the ambient and diffuse by the object color to get the approiate color result.


//now mix the result with the textures
//FragColor = mix(texture(texture0,texCoord),FragColor,0.5);
 
}
";
        }

        public static void SetUniformV3(int programId, string name, Vector3 value)
        {
            var loc = GL.GetUniformLocation(programId, name);

            GL.Uniform3(loc, value);
        }

        internal static void SetFloat(int programId, string name, float value)
        {
            var loc = GL.GetUniformLocation(programId, name);

            GL.Uniform1(loc, value);
        }
    }
}
