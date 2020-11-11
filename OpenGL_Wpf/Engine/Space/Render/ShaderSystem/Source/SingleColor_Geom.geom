#version 330 core

layout (points) in;
layout (points,max_vertices=1) out;
  
in int[] vid;
out float gvid;

void main()
{ 
    
   	gl_PrimitiveID = gl_PrimitiveIDIn;
    
    gvid = vid[0];
    
    gl_Position = gl_in[0].gl_Position;
    EmitVertex();
//
//
//    gl_Position = gl_in[1].gl_Position;
//    EmitVertex();
//    
//
//    gl_Position = gl_in[2].gl_Position;
//    EmitVertex();

}


