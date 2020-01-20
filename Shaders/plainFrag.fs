#version 410 core

out vec4 FragColor;

uniform sampler2D texture_diffuse1;

in vec2 gTexCoords;
in vec3 gNormal ;

void main()
{    
	//FragColor = vec4(0.0,1.0,0.0,1.0);
	FragColor = texture(texture_diffuse1, gTexCoords);
}

