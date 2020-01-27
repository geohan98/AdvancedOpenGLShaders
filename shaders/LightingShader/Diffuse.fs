#version 330 core

out vec4 FragColor;

const int numberOfLights = 2;
uniform vec3 light_Position[numberOfLights];
uniform vec4 light_Colour[numberOfLights];

uniform sampler2D texture_diffuse1;
uniform vec3 camera_Position;

in vec2 vTexCoords;
in vec3 vNormal;
in vec3 vFragPosition;



void main()
{   
	vec4 colour;
	vec3 normal = normalize(vNormal);
	vec3 viewDir = normalize(camera_Position - vFragPosition);
	
	for (int i = 0; i < numberOfLights; i++)
	{
		vec3 lightDir = normalize(light_Position[i] - vFragPosition);
		
		//Diffuse
		float diffuseStrength = max(dot(normal,lightDir),0.0);

		if(diffuseStrength > 0.25)
		{
		diffuseStrength = 1.0;
		}
		else
		{
		diffuseStrength = 0.15;
		}

		vec4 diffuse = texture2D(texture_diffuse1,vTexCoords) * light_Colour[i] * diffuseStrength;
		
		//Specular
		vec3 lightReflectDir = reflect(-lightDir, vNormal);
		float specStrength = pow(max(dot(viewDir, lightReflectDir), 0.0), 32);
		
		vec4 specular = light_Colour[i] * specStrength;
		
		colour += diffuse + specular;
	}
	
    FragColor = colour;
}

