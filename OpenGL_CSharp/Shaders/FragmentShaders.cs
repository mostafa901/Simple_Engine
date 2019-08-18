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

void main(){

float ambientstrength=.1f;

if(lightColor == vec3(0,0,0))
{
FragColor = vec4(ambientstrength * objectColor,1);
}
else FragColor=vec4(lightColor*objectColor,1);

vec3 normal = normalize(PixelNormal);
vec3 lightDir= normalize(LightPos-FragPos);
float reflectionAngle = dot(lightDir,normal);
float diffuseamount = max(reflectionAngle,0);
vec3 diffusecolor = diffuseamount*lightColor;
FragColor = vec4((ambientstrength + diffusecolor) *objectColor,1.0f);

}
";
        }

        public static void SetUniformV3(int programId, string name, Vector3 value)
        {
            var loc = GL.GetUniformLocation(programId, name);

            GL.Uniform3(loc, value);
        }

    }
}
