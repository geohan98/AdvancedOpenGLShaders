#version 330 core
layout (triangles) in;
layout (line_strip, max_vertices = 2) out;


in vec2 vTexCoords[];
in vec3 vNormal[] ;

out vec2 gTexCoords; 
out vec3 gNormal; 

void main() {    

  // loop through each vertex (3 because we're reading a triangle)
  for(int i = 0 ; i < 3; i++){
    gl_Position = gl_in[i].gl_Position;
	EmitVertex();
	gl_Position = (gl_in[i].gl_Position + vec4(vNormal[i] * 1.0f,0.0f));
	EmitVertex();
    gTexCoords = vTexCoords[i]; 
    gNormal = vNormal[i]; 
    EmitVertex() ;
  }
    EndPrimitive();
}



