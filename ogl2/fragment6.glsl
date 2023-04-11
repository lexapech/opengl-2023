#version 330 core

layout (location = 0) out vec4 Fragment0;
layout (location = 1) out int Fragment1; 
in vec3 vertexNormal; 
in vec3 vertexPos; 
in vec4 vertexColor;
uniform int id;
uniform vec3 lightSource;
uniform vec3 cameraPos;

void main()
{
	vec3 dir_to_point = normalize(lightSource - vertexPos);
	vec3 dir_to_camera = normalize(cameraPos - vertexPos);
	vec3 reflectDir = reflect(-dir_to_point, vertexNormal);
	float specularAngle = max(dot(reflectDir, dir_to_camera), 0.0);
    float scalar_product = dot(vertexNormal, dir_to_point);
	vec3 diffuse = vertexColor.rgb * (0.5 + max(0.0,scalar_product) * 0.5);
	float specularPower = 100;
	vec3 specular = vec3(1,1,1) * specularAngle * pow(specularAngle, specularPower);
    Fragment0 = vec4(diffuse + specular,vertexColor.a);
	Fragment1 = id;
}