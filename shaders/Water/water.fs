#version 330 core

in vec3 fragPosVS;
in vec2 texCoordsVS;
in mat3 TBNVS;

out vec4 FragColor;

uniform float u_ambientStrength;
uniform float u_diffuseStrength;
uniform float u_specularStrength;

uniform vec3 u_lightPos;
uniform vec3 u_lightColor;

uniform vec3 u_viewPos;

uniform vec3 u_colour;

uniform sampler2D u_normalMap;

uniform float u_Scale;

uniform float u_Time;
uniform float u_waterSpeedX;
uniform float u_waterSpeedY;

void main()
{
	//Ambient
	vec3 ambient = u_ambientStrength * u_colour;
	//Diffuse
	vec3 normal = texture2D(u_normalMap,vec2(texCoordsVS.x + u_Time * u_waterSpeedX,texCoordsVS.y + u_Time * u_waterSpeedY) * u_Scale).xyz;
	normal = normalize(normal * 2.0 - 1.0);
	normal = normalize(TBNVS * normal);
	vec3 lightDir = normalize(u_lightPos - fragPosVS);
	float diffuseDot = max(dot(normal,lightDir),0.0);
	vec3 diffuse = u_diffuseStrength * u_lightColor * u_colour * diffuseDot;
	//Speccular
	vec3 viewDir = normalize(u_viewPos - fragPosVS);
	vec3 reflectDir = reflect(-lightDir, normal);
	float specDot = pow(max(dot(viewDir, reflectDir), 0.0), 256);
	vec3 specular = u_specularStrength * specDot * u_lightColor;  
	//Result
    FragColor = vec4(ambient + diffuse + specular,1.0);
}