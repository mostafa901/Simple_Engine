#version 330 core
out vec4 FragColor;

struct Material {
    vec3  ambient;
    vec3  diffuse;
    vec3  specular;
}; 

uniform Material material;
 
void main(){
 
//FragColor = vec4(material.ambient*material.diffuse*material.specular,1.0);
FragColor = vec4(1.0);
 
}