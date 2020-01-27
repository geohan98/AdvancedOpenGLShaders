#version 330 core
layout (triangles) in;
layout (triangle_strip, max_vertices = 3) out;


in vec2 vTexCoords[];
in vec3 vNormal[];
in vec3 vFragPosition[];

out vec2 gTexCoords; 
out vec3 gNormal; 
out vec3 gFragPosition;

void main() {    

	vec3 surfaceNormal = (vNormal[0] + vNormal[1] + vNormal[2]) / 3;

  // loop through each vertex (3 because we're reading a triangle)
  for(int i = 0 ; i < 3; i++){
    gl_Position = gl_in[i].gl_Position ;
    gTexCoords = vTexCoords[i]; 
    gNormal = surfaceNormal; 
	gFragPosition = vFragPosition[i];
    EmitVertex() ;
  }
    EndPrimitive();
}



