#version 330 core

layout (points) in;
layout (points,max_vertices=1) out;

uniform vec3 SelectedVertex0;
uniform vec4 DefaultColor;
  
in vec3[] vid;

out vec3 gvid0;
out vec4 Color;
void main()
{ 

    gvid0 = vid[0];
  
    gl_Position = gl_in[0].gl_Position;
    Color = DefaultColor;
    if(SelectedVertex0 == gvid0)
    {
        Color = vec4(1,0,0,1);
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


