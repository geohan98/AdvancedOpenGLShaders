#version 330 core
layout(triangles) in ;
layout(triangle_strip, max_vertices = 3) out ;

in vec3 fragPosES[];
in vec3 normalES[];

out vec3 fragPosGS;
out vec3 normalGS;

void main()
{
   for(int i = 0 ; i < 3; i++)
   {
      gl_Position = gl_in[i].gl_Position ;
	  fragPosGS = fragPosES[i];
	  normalGS = normalES[0] + normalES[1] + normalES[2];
      EmitVertex() ;
  }
     EndPrimitive() ;

}