#version 330 core
out vec4 FragColor;

in vec4 vcolor;

struct Material {
    vec3  ambient;
    vec3  diffuse;
    vec3  specular;
}; 

uniform Material material;
 
void main(){
 
FragColor = vec4(vcolor);
 
 
}