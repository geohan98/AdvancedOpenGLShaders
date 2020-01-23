#version 330 core

out vec4 FragColor;

uniform sampler2D texture_diffuse1;
uniform vec3 light_Position;

in vec2 vTexCoords;
in vec3 vNormal;
in vec3 vFragPosition;

void main()
{   
    float ambientStrength = 0.1f;
    vec3 ambient = ambientStrength * vec3(1.0f,1.0f,1.0f);

    vec3 normal = normalize(vNormal);
    vec3 lightDir = normalize(light_Position - vFragPosition);

    float diffuseStrength = max(dot(normal,lightDir),0.0f);

    if(diffuseStrength >= 0.25f)
    {
        diffuseStrength = 1.0f;
    }
    else
    {  
        diffuseStrength = 0.5f;
    }


    FragColor = texture(texture_diffuse1, vTexCoords) * diffuseStrength;
    FragColor = vec4(1.0f,0.0f,0.0f,1.0f) * diffuseStrength;
}

