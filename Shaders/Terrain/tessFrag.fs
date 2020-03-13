#version 330 core

in vec3 fragPosGS;
in vec3 normalGS;
out vec4 FragColor;

uniform vec3 lightPos;
uniform vec3 viewPos;
uniform mat4 model;

uniform float fogDistance;

void main()
{	
	vec3 fragPos = vec3(model * vec4(fragPosGS,1.0));

	float ambientStrength = 0.01;
	vec3 ambient = ambientStrength * vec3(1.0,1.0,1.0);
	
	vec3 lightDir = normalize(lightPos - fragPos);
	
	float diffuseStrngth = max(dot(normalGS,lightDir),0.0);
	vec3 diffuse = diffuseStrngth * vec3(0.5,0.8,0.5) * 0.5;

	float fragDistance = abs( distance(fragPos, viewPos));

	float a = clamp(fogDistance / fragDistance,0.0,1.0);
	vec3 color = mix(vec3(0.1,0.1,0.1),ambient + diffuse,a);

    FragColor = vec4(color,1.0);
}