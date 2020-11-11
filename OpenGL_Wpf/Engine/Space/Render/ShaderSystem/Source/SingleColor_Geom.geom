#version 330 core

layout (points) in;
layout (points,max_vertices=1) out;

uniform int SelectedVertex;
uniform vec4 DefaultColor;
  
in int[] vid;

out float gvid;
out vec4 Color;
void main()
{ 
    
   	gl_PrimitiveID = gl_PrimitiveIDIn;
    
    gvid = vid[0];
  
    gl_Position = gl_in[0].gl_Position;
    Color = DefaultColor;
    if(SelectedVertex == gvid)
    {
        Color = vec4(1,0,0,1);
    }
    else
    {
        Color = DefaultColor;
    }

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


