#version 330 core

layout (location = 0) in vec3 vertexPosition;
layout (location = 1) in vec3 normal;
layout (location = 2) in vec4 color;
uniform mat4 vpMatrix;
uniform mat4 mMatrix;

out vec3 vertexNormal;
out vec3 vertexPos;
out vec4 vertexColor;

void main()
{
	vertexNormal = normalize(mat3(mMatrix) * normal);
	vertexPos = (mMatrix * vec4( vertexPosition,1.0)).xyz;
    vertexColor = vec4(color.rgb,0.5);
	
    gl_Position = vpMatrix*mMatrix * vec4(vertexPosition, 1.0);
}