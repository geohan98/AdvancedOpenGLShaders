#version 330 core

in vec2 TexCoords;

out vec4 fragColor;

uniform sampler2D scene;

void main ()
{
	vec3 col = texture(scene, TexCoords).rgb;
    fragColor = vec4(col, 1.0);
}
