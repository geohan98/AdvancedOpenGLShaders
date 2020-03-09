#version 450 core
layout (location = 0) in vec3 aPos;
layout (location = 1) in vec2 aTexCorrds;

vec3 calcNormal();

uniform sampler2D heightMap;
uniform float heightMapScale;

out vec3 vNormal;

void main()
{
	vec3 pos = aPos;
	pos.y = texture(heightMap,aTexCorrds).r * heightMapScale;
	gl_Position = vec4(pos, 1.0);
	vNormal = calcNormal();
}

vec3 calcNormal()
{
	float right;
	float left;
	float up;  
	float down;
	
	right = (textureOffset(heightMap,aTexCorrds,ivec2(1,0)).r * heightMapScale);
	left = (textureOffset(heightMap,aTexCorrds,ivec2(-1,0)).r) * heightMapScale;
	up = (textureOffset(heightMap,aTexCorrds,ivec2(0,1)).r * heightMapScale);
	down = (textureOffset(heightMap,aTexCorrds,ivec2(0,-1)).r * heightMapScale);
	
	
	vec3 value = vec3((left - right),2.0,(up - down));
	value = normalize(value);
	return value;
}