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
uniform float ambientcoff = 0.07f; //ambient coffecient intenesty 
uniform int TotalLightNumber = 1; //ambient coffecient intenesty 
uniform int IsBlin = 0; //consider Blinne shading calclations

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
	vec3 Direction;

	float Constant;
	float Linear;
	float Quaderic;
	
	float InnerAngle;
	float OuterAngle;
	
	int LightType; //0=>Point, 1=>Direction 2=>Spot Light
};
#define NumberofLights 5
uniform Light Lights[NumberofLights];

  vec3 CalculatePointLight(Light light);
  vec3 CalculateDirectionLight(Light light);
  float GetFadOut(Light light);
  bool isLightWithinRange(Light light);
  float GetAttenuation(Light light);


	void main()
	{
		vec3 result= vec3(0);
		for	(int i=0;i<NumberofLights;i++)
		{
			Light light = Lights[i];
			if(TotalLightNumber>i)
			{
			if(isLightWithinRange(light))
			{
				//Light is Point light
				if(light.LightType==0)
				{			
					result += CalculatePointLight(light);			
				}

				if(light.LightType==1)
				{			 
					result += CalculateDirectionLight(light);			
				}
			}
			}
			
			if(result==vec3(0)) result= vec3(ambientcoff * vec3(texture(material.diffuse, texCoord)));	
				
			//Finally combine the results
			FragColor = vec4(result,1.0f); //multiply the sum of the ambient and diffuse by the object color to get the approiate color result.
		} 
	 }

	 float GetFadOut(Light light)
	 {
		if(light.InnerAngle==0 || light.OuterAngle==0) return 1;

		float theta = cos(light.InnerAngle); //the hot area
		float theta2 = cos(light.OuterAngle); // the gradiant area + hot area
		float angle=dot(normalize(FragPos-light.position),light.Direction); //the actual angle of this pixel
		float gammadiff= theta-theta2; //get the gradient area
		return clamp((angle-theta2)/gammadiff, ambientcoff ,1); //calculate intenisty

	 }

	 bool isLightWithinRange(Light light)
	 {
			 if(dot((FragPos-light.position),PixelNormal)>=0 )
			{
				return false;
			}
			return true;
	 }

	  float GetAttenuation(Light light)
	  {
		float d=length(light.position-FragPos); //get the distance between light source and fragment position
		return 1.0/(light.Constant+light.Linear*d+light.Quaderic * d * d); //Calculate Attenuation
	  }


	 vec3 CalculatePointLight(Light light)
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

			vec3 specular ;
			if(IsBlin==1)
			{
			vec3 halfwayDir = normalize(lightDir + viewDir);
			float spec = pow(max(dot(normal, halfwayDir), 0.0),material.shininess);
		      specular = light.specular * spec * vec3(texture(material.specular,texCoord));
			}
			else
			{
			float spec = pow(max(dot(viewDir, reflDir),0),material.shininess); //pow here is for the radius of the specular the more the narrower
			  specular = light.specular * spec * vec3(texture(material.specular,texCoord)); //multiply all to get the actual specular intenisty and color and radius
			}

			float atten = GetAttenuation(light);
			float intens =  GetFadOut(light);

			ambient*=atten*intens;
			specular*=atten*intens;
			diffuse*=atten*intens;

			//Finally combine the results
			return vec3((ambient + diffuse + specular)); //multiply the sum of the ambient and diffuse by the object color to get the approiate color result.
 
}

	vec3 CalculateDirectionLight(Light light)
	 {
		 //Calculate the light Direction
		vec3 lightDir = normalize(-light.Direction); //get the Vectore Ray between light position and target (Pixel) Position,

		 //get ambient color
		vec3 ambient = light.ambient * vec3(texture(material.ambient, texCoord));

		//get diffuse
		vec3 normal = normalize(PixelNormal); //notmalize Pixel normal, we only need Direction
		float reflectionAngle = dot(lightDir,normal); //calculate the angle to use it as a degree of diffuse effeciency
		float diffuseamount = max(reflectionAngle, 0); //if the angle is less tha 0 then the ray is not hitting the pixel

		vec3 diffuse =light.diffuse * diffuseamount * vec3(texture(material.diffuse, texCoord)); //multiply the diffuse value by the light color to get the amount of light required to lighten up object

		//Create Specular
		vec3 viewDir = normalize(ViewPos-FragPos); //get the direction from the pixel to the camera
		vec3 reflDir = reflect(-lightDir,PixelNormal); //get the reflection vector of the vector from source to pixel on Normal vector of pixel
		float spec = pow(max(dot(viewDir, reflDir),0),material.shininess); //pow here is for the radius of the specular the more the narrower
		vec3 specular = light.specular * spec * vec3(texture(material.specular,texCoord)); //multiply all to get the actual specular intenisty and color and radius
 
		float atten = 1;
		float intens =  1;
		ambient*=atten*intens;
		specular*=atten*intens;
		diffuse*=atten*intens; 

		//Finally combine the results
		return vec3(ambient + diffuse + specular); //multiply the sum of the ambient and diffuse by the object color to get the approiate color result.
}
