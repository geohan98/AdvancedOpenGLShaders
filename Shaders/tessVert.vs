#version 450 core
layout (location = 0) in vec3 aPos;
layout (location = 1) in vec2 aTexCorrds;

out vec3 posVS;
out vec2 texCorrdsVS;

void main()
{
		posVS = aPos;
		texCorrdsVS = aTexCorrds;
}

