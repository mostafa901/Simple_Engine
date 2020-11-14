#version 330 core

layout (lines) in;
layout (line_strip, max_vertices=2) out;

uniform vec3 SelectedVertex0;
uniform vec3 SelectedVertex1;
uniform vec4 DefaultColor;
  
in vec3[] vid;

out vec3 gvid0;
out vec3 gvid1;

out vec4 Color;
void main()
{ 
    gvid0 = vid[0];
    gvid1 = vid[1];
  

    Color = DefaultColor;    

    if(SelectedVertex0 == gvid0 && SelectedVertex1 == gvid1  )
    {
        Color = vec4(1,0,0,1);
    }

    gl_Position = gl_in[0].gl_Position;
    EmitVertex();

    gl_Position = gl_in[1].gl_Position;
    EmitVertex();
}


