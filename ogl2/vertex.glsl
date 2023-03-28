#version 330 core

layout (location = 0) in vec3 vertexPosition;
layout (location = 1) in vec3 normal;
uniform mat4 projectionMatrix;
uniform mat4 modelViewMatrix;
uniform vec3 color;
uniform vec3 point;

out vec4 vertexColor;

void main()
{
	vec3 dir_to_point = normalize(point - vertexPosition);
    float scalar_product = dot(normal, dir_to_point);
    float alpha = abs(scalar_product);

    vertexColor = vec4(color,alpha);
    gl_Position = projectionMatrix * modelViewMatrix * vec4(vertexPosition, 1.0);
}