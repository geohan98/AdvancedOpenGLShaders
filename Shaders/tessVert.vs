#version 450 core
layout (location = 0) in vec3 aPos;

out vec3 posVS; 

void main()
{
		posVS = aPos;
}

