#version 330 core

in vec3 fragPosGS;
in vec3 normalGS;
in vec2 texCoordsGS;
in float heightGS;
in vec4 fragPosLightSpaceGS;

out vec4 FragColor;

uniform float u_ambientStrength;
uniform float u_diffuseStrength;

uniform vec3 u_lightPos;
uniform vec3 u_lightColor;

uniform sampler2D u_shadowMap;

float calcAlphaBetween(float x, float a, float b);
float ShadowCalculation(vec4 fragPosLightSpace);

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
	vec3 lightDir = normalize(u_lightPos - fragPosGS);
	float bias = max(0.05 * (1.0 - dot(normalGS, lightDir)), 0.005); 
	float shadow = currentDepth - bias > closestDepth  ? 1.0 : 0.0;  
	return shadow;
}
	