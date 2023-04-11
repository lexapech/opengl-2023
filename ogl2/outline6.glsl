#version 330 core

layout (location = 0) out vec4 Fragment0;
layout (location = 1) out int Fragment1; 
in vec3 vertexNormal; 
in vec3 vertexPos; 
in vec4 vertexColor;
uniform vec4 color;

void main()
{
    Fragment0 = color;
	Fragment1 = 0;
}