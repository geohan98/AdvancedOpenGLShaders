#version 330 core

//IN
in vec3 texCoords_VS;
//OUT
out vec4 fragColor;
//UNIFORMS
uniform samplerCube u_skybox;

//MAIN
void main()
{    
    fragColor = texture(u_skybox, texCoords_VS);
}