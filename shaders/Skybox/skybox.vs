#version 330 core
layout (location = 0) in vec3 aPos;

out vec3 texCoords_VS;

uniform mat4 u_projection;
uniform mat4 u_view;

void main()
{
    texCoords_VS = aPos;
    gl_Position = u_projection * u_view * vec4(aPos, 1.0);
}  