#version 330 core
layout (location = 0) in vec3 aPos;
layout (location = 2) in vec2 aTexCoords;

out vec3 fragPosVS;
out vec2 texCoordsVS;

uniform mat4 u_model;

void main()
{
	fragPosVS = vec3(u_model * vec4(aPos, 1.0));
	texCoordsVS = aTexCoords;
}