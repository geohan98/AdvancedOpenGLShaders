#version 330 core

in vec3 fragPosGS;
in vec3 normalGS;
in vec2 texCoordsGS;
in float heightGS;

out vec4 FragColor;

uniform float u_ambientStrength;
uniform float u_diffuseStrength;
uniform float u_specularStrength;

uniform vec3 u_lightPos;
uniform vec3 u_lightColor;

uniform vec3 u_viewPos;

float calcAlphaBetween(float x, float a, float b);

void main()
{
	float a = calcAlphaBetween(heightGS,-1,1);
	vec3 colour = mix(vec3(0,0.5,1),vec3(0,1,0.75),a);
	
	//Ambient
	vec3 ambient = u_ambientStrength * colour;
	//Diffuse
	vec3 lightDir = normalize(u_lightPos - fragPosGS);
	float diffuseDot = max(dot(normalGS,lightDir),0.0);
	vec3 diffuse = u_diffuseStrength * u_lightColor * colour * diffuseDot;
	//Speccular
	vec3 viewDir = normalize(u_viewPos - fragPosGS);
	vec3 reflectDir = reflect(-lightDir, normalGS);
	float specDot = pow(max(dot(viewDir, reflectDir), 0.0), 512);
	vec3 specular = u_specularStrength * specDot * u_lightColor;
	//Result 
	FragColor = vec4(ambient + diffuse + specular,1.0);
}

float calcAlphaBetween(float x, float a, float b)
{
	return (x - a) / (b - a);
}
	