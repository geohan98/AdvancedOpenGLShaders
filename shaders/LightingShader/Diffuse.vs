#version 330 core
layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aNormal;
layout (location = 2) in vec2 aTexCoords;

layout (std140) uniform u_VMMatrices
{
	mat4 u_projection;
	mat4 u_view;
};

uniform mat4 u_model;

out vec2 vTexCoords;
out vec3 vNormal;
out vec3 vFragPosition;

void main()
{
    gl_Position = u_projection * u_view * u_model * vec4(aPos, 1.0);
    vTexCoords = aTexCoords;    
    vNormal = aNormal ;
	vFragPosition = vec3(u_model * vec4(aPos,1.0f));
}