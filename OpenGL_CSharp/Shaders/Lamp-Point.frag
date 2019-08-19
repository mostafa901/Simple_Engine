#version 330 core
out vec4 FragColor;

in vec4 vcolor; //vertex Color
in vec2 texCoord;
in vec3 PixelNormal; //normal vector of pixel
in vec3 FragPos; //Pixel position due world coordinates

uniform vec3 objectColor; 
uniform vec3 LightPos; 
uniform sampler2D texture0;
uniform sampler2D texture1;
uniform vec3 ViewPos; //View (Camera) Position
uniform float specintens = 1.0f; //setupSpecular intenesty 

struct Material {
    vec3  ambient;
    vec3  diffuse;
    vec3  specular;     
}; 

uniform Material material;
 
void main(){
 
FragColor = vec4(material.ambient*material.diffuse*material.specular,1.0);
 
}