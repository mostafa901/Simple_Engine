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
    sampler2D  ambient;
    sampler2D diffuse;
    sampler2D specular;
    float shininess;
}; 

uniform Material material;

struct Light {
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;    
    vec3 position;

}; 


uniform Light light;

void main(){

if(light.ambient == vec3(0,0,0))
{
FragColor = vec4(light.ambient * vec3(texture(material.diffuse, texCoord)),1);
}
else 
{

//get ambient color
vec3 ambient = light.ambient * vec3(texture(material.ambient, texCoord));

//get diffuse
vec3 normal = normalize(PixelNormal); //notmalize Pixel normal, we only need Direction
vec3 lightDir = normalize(light.position - FragPos); //get the Vectore Ray between light position and target (Pixel) Position,
float reflectionAngle = dot(lightDir,normal); //calculate the angle to use it as a degree of diffuse effeciency
float diffuseamount = max(reflectionAngle, 0); //if the angle is less tha 0 then the ray is not hitting the pixel

vec3 diffuse =light.diffuse * diffuseamount * vec3(texture(material.diffuse, texCoord)); //multiply the diffuse value by the light color to get the amount of light required to lighten up object

//Create Specular
vec3 viewDir = normalize(ViewPos-FragPos); //get the direction from the pixel to the camera
vec3 reflDir = reflect(-lightDir,PixelNormal); //get the reflection vector of the vector from source to pixel on Normal vector of pixel
float spec = pow(max(dot(viewDir, reflDir),0),material.shininess); //pow here is for the radius of the specular the more the narrower
vec3 specular = light.specular * spec * vec3(texture(material.specular,texCoord)); //multiply all to get the actual specular intenisty and color and radius

//Finally combine the results
FragColor = vec4((ambient + diffuse + specular),1.0f); //multiply the sum of the ambient and diffuse by the object color to get the approiate color result.

 }
}