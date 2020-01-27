#version 330 core

out vec4 FragColor;

const int numberOfLights = 2;
uniform vec3 light_Position[numberOfLights];
uniform vec4 light_Colour[numberOfLights];

uniform sampler2D texture_diffuse1;
uniform vec3 camera_Position;

in vec2 gTexCoords;
in vec3 gNormal;
in vec3 gFragPosition;



void main()
{   
	vec4 colour;
	vec3 normal = normalize(gNormal);
	vec3 viewDir = normalize(camera_Position - gFragPosition);
	
	for (int i = 0; i < numberOfLights; i++)
	{
		vec3 lightDir = normalize(light_Position[i] - gFragPosition);
		
		//Diffuse
		float diffuseStrength = max(dot(normal,lightDir),0.0);
		
		vec4 diffuse = texture2D(texture_diffuse1,gTexCoords) * light_Colour[i] * diffuseStrength;
		
		//Specular
		vec3 lightReflectDir = reflect(-lightDir, gNormal);
		float specStrength = pow(max(dot(viewDir, lightReflectDir), 0.0), 32);
		
		vec4 specular = light_Colour[i] * specStrength;
		
		colour += diffuse + specular;
	}
	
    FragColor = colour;
}

