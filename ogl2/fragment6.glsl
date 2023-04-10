#version 330 core

out vec4 FragColor;
 
in vec3 vertexNormal; 
in vec3 vertexPos; 
in vec4 vertexColor;

void main()
{
	vec3 point = vec3(2,2,2);
	vec3 dir_to_point = normalize(point - vertexPos);
    float scalar_product = dot(vertexNormal, dir_to_point);
    FragColor = vertexColor * (0.5+scalar_product*0.5);
}