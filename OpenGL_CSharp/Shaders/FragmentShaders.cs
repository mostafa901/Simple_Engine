using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGL_CSharp.Shaders
{
    class FragmentShaders
    {
        public static string Frag()
        {
            return @"
#version 330 core
out vec4 FragColor;
in vec4 vcolor; //vertex Color superceeded

uniform vec4 objectColor;
unifom vec4 lightColor;
 
void main(){
FragColor=vec4(lightColor*objectColor);
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
in vec4 vcolor; //vertex Color superceeded

uniform vec4 objectColor;
unifom vec4 lightColor;
 
void main(){
FragColor=vec4(lightColor*objectColor);
}
";
        }
    }
}
