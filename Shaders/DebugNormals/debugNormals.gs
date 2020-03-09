#version 330 core
layout (triangles) in;
layout (line_strip, max_vertices = 2) out;

in vec3 vNormal[];

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

void main() {    
    gl_Position = projection * view * model * ((gl_in[0].gl_Position + gl_in[1].gl_Position + gl_in[2].gl_Position) / 3);
    EmitVertex() ;
	
	gl_Position = projection * view * model * (((gl_in[0].gl_Position + gl_in[1].gl_Position + gl_in[2].gl_Position) / 3) + vec4((vNormal[0] + vNormal[1] + vNormal[2]) / 3,0) * 5);
    EmitVertex() ;
	
    EndPrimitive();
}



