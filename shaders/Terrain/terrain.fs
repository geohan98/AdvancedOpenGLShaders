#version 330 core

in vec3 fragPosGS;
in vec3 normalGS;
in vec2 texCoordsGS;
in float heightGS;
in vec4 fragPosLightSpaceGS;

out vec4 FragColor;

uniform float u_ambientStrength;
uniform float u_diffuseStrength;
uniform float u_specularStrength;

uniform vec3 u_lightPos;
uniform vec3 u_lightColor;

uniform vec3 u_viewPos;

uniform sampler2D u_shadowMap;

float calcAlphaBetween(float x, float a, float b);
float ShadowCalculation(vec4 fragPosLightSpace);

void main()
{
	float a;
	vec3 colour;
	float v = max(dot(normalGS,vec3(0,1,0)),0.0);
	
	if(v < 0.5) 
	{
		colour = vec3(0.2,0.2,0.2);
	}
	else
	{
		if(heightGS > 0.75) 
		{
		a = clamp(calcAlphaBetween(heightGS,0.75,1),0,1);
		colour = mix(vec3(0.1,0.6,0.1),vec3(0.8,0.8,0.8),a);
		}
		else
		{
		a = clamp(calcAlphaBetween(heightGS,-1,0.75),0,1);
		colour = mix(vec3(1.0,0.0,0.0),vec3(0.1,0.6,0.1),a);
		}
	}
	
	//Ambient
	vec3 ambient = u_ambientStrength * colour;
	//Diffuse
	vec3 lightDir = normalize(u_lightPos - fragPosGS);
	float diffuseDot = max(dot(normalGS,lightDir),0.0);
	vec3 diffuse = u_diffuseStrength * u_lightColor * colour * diffuseDot;
	//Speccular
	vec3 viewDir = normalize(u_viewPos - fragPosGS);
	vec3 reflectDir = reflect(-lightDir, normalGS);
	float specDot = pow(max(dot(viewDir, reflectDir), 0.0), 16);
	vec3 specular = u_specularStrength * specDot * u_lightColor;
	//Shadow
	float shadow = ShadowCalculation(fragPosLightSpaceGS);
	//Result 
	FragColor = vec4(ambient + (1.0 - shadow) * diffuse,1.0);
}

float calcAlphaBetween(float x, float a, float b)
{
	return (x - a) / (b - a);
}

float ShadowCalculation(vec4 fragPosLightSpace)
{
	vec3 projCoords = fragPosLightSpace.xyz / fragPosLightSpace.w;
	projCoords = projCoords * 0.5 + 0.5;
	float closestDepth = texture(u_shadowMap, projCoords.xy).r;
	float currentDepth = projCoords.z;
	float shadow = currentDepth > closestDepth  ? 1.0 : 0.0;  
	return shadow;
}

	