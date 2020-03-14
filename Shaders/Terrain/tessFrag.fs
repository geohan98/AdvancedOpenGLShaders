#version 330 core

in vec3 fragPosGS;
in vec3 normalGS;
out vec4 FragColor;

uniform vec3 lightPos;
uniform vec3 viewPos;
uniform mat4 model;

uniform float fogNear;
uniform float fogFar;
uniform vec3 fogColor;

float CalcAlpha(float x, float a, float b);

void main()
{	
	//Ambient
	vec3 ambient = vec3(0.01,0.01,0.01);
	//Diffuse
	vec3 fragPos = vec3(model * vec4(fragPosGS,1.0));	
	vec3 lightDir = normalize(lightPos - fragPos);
	float diffuseStrength = max(dot(normalGS,lightDir),0.0);
	
	vec3 diffuse;
	if(fragPos.y < 30)
	{
		diffuse = diffuseStrength * vec3(0.0,0.1,0.0);
	}
	else 
	{
		diffuse = diffuseStrength * vec3(0.5,0.5,0.5);
	}
	
	 

	float dist = abs(distance(fragPos,viewPos));
	if (dist > fogFar)
	{
	FragColor = vec4(fogColor,1.0);
	return;
	}
	else if (dist < fogNear)
	{
	FragColor = vec4(ambient + diffuse,1.0);
	return;
	}
	
	vec3 color = mix(ambient + diffuse, fogColor, CalcAlpha(dist,fogNear,fogFar));
	
	FragColor = vec4(color,1.0);
	
	
}

float CalcAlpha(float x, float a, float b)
{
	return (x - a) / (b - a);
}

//0