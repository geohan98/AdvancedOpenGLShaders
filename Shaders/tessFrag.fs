#version 330 core

in vec3 fragPosES ;
in vec2 texCoordsES;
in vec3 normalES;

uniform vec3 lightPos;

out vec4 FragColor ;

void main()
{
	vec3 ambient = vec3(1.0,1.0,1.0) * 0.25;
	
	vec3 lightDir = normalize(lightPos - fragPosES);
	
	float diff = max(dot(normalES,lightDir),0.0);
	vec3 diffuse = diff * vec3(1.0,1.0,1.0);

    FragColor = vec4(ambient + diffuse,1.0);
}
	