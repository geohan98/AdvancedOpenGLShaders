#version 330 core
layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aNormal;
layout (location = 2) in vec2 aTexCoords;
layout (location = 3) in vec3 aTangent;
layout (location = 4) in vec3 aBitangent;

out vec3 fragPosVS;
out vec2 texCoordsVS;
out mat3 TBNVS;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

void main()
{
	fragPosVS = vec3(model * vec4(aPos, 1.0));
	texCoordsVS = aTexCoords;
	
	vec3 t = normalize(vec3(model * vec4(aTangent,0.0)));
	vec3 b = normalize(vec3(model * vec4(aBitangent,0.0)));
	vec3 n = normalize(vec3(model * vec4(aNormal,0.0)));
	TBNVS = mat3(t,b,n);
	
    gl_Position = projection * view * model * vec4(aPos, 1.0);
}