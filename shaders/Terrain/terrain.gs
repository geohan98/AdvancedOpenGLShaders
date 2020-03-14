#version 330 core
layout(triangles) in ;
layout(triangle_strip, max_vertices = 3) out ;

in vec3 fragPosES[];
in vec3 normalES[];
in vec2 texCoordsES[];
in float heightES[];

out vec3 fragPosGS;
out vec3 normalGS;
out vec2 texCoordsGS;
out float heightGS;

uniform vec3 u_viewPos;

void main()
{
   for(int i = 0 ; i < 3; i++)
   {
	fragPosGS = fragPosES[i];
	//normalGS = normalES[i];
	normalGS = (normalES[0] + normalES[1] + normalES[2]) / 3;
	texCoordsGS = texCoordsES[i];
	heightGS = heightES[i];
	gl_Position = gl_in[i].gl_Position;
	EmitVertex() ;
  }
  EndPrimitive() ;
}