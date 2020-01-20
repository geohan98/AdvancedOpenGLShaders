#version 330 core
layout (triangles) in;
layout (triangle_strip, max_vertices = 3) out;


in vec2 vTexCoords[];
in vec3 vNormal[] ;

out vec2 gTexCoords; 
out vec3 gNormal; 

void main() {    

  // loop through each vertex (3 because we're reading a triangle)
  for(int i = 0 ; i < 3; i++){
    gl_Position = gl_in[i].gl_Position ;
    gTexCoords = vTexCoords[i]; 
    gNormal = vNormal[i]; 
    EmitVertex() ;
  }
    EndPrimitive();
}



