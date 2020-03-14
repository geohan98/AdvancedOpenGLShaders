#include <glad/glad.h>
#include <GLFW/glfw3.h>
#include "stb_image.h"

#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include <glm/gtc/type_ptr.hpp>
#include <glm/gtx/string_cast.hpp>

#include "Shader.h"
#include "Camera.h"

#include <Model.h>
#include "Terrain.h"

#include<string>
#include <iostream>
#include <numeric>



// settings
const unsigned int SCR_WIDTH = 1200;
const unsigned int SCR_HEIGHT = 900;

void framebuffer_size_callback(GLFWwindow* window, int width, int height);
void mouse_callback(GLFWwindow* window, double xpos, double ypos);
void scroll_callback(GLFWwindow* window, double xoffset, double yoffset);
void processInput(GLFWwindow* window);
unsigned int loadTexture(char const* path);
//unsigned int loadTexture2(char const * path);
void setVAO(vector <float> vertices);

// camera
Camera camera(glm::vec3(0, 0, 0));
float lastX = SCR_WIDTH / 2.0f;
float lastY = SCR_HEIGHT / 2.0f;
bool firstMouse = true;

//Light
glm::vec3 lightPos = glm::vec3(2.5f, 10.0f, 2.5f);

//Light Strength
float ambient = 0.01;
float diffuse = 1.0;
float specular = 1.0;

//ClearColor & Fog
glm::vec3 clearColor = glm::vec3(0.85f, 0.9f, 1.0f);
float fogNear = 100000;
float fogFar = 200000;

//Water
unsigned int waterTex;
glm::vec3 waterColour = glm::vec3(0.6, 0.8, 9.0);
float waterSpeedX = 0.0;
float waterSpeedY = 0.001;
float waterScale = 100.0;
std::string waterNormalPath = "..\\Resources\\Textures\\waterNormal.jpg";

//arrays
unsigned int VBO, VAO;

//Terrain
int sizeX = 101;
int sizeY = 101;
int stepSize = 1;
unsigned int heightMap;
std::string heightMapPath = "..\\resources\\heightmap.jpg";
float heightMapScale = 1;



// timing
float deltaTime = 0.0f;
float lastFrame = 0.0f;

int main()
{
	glfwInit();
	glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 3);
	glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 3);
	glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);
	GLFWwindow* window = glfwCreateWindow(SCR_WIDTH, SCR_HEIGHT, "IMAT3907", NULL, NULL);
	if (window == NULL)
	{
		std::cout << "Failed to create GLFW window" << std::endl;
		glfwTerminate();
		return -1;
	}
	glfwMakeContextCurrent(window);
	glfwSetFramebufferSizeCallback(window, framebuffer_size_callback);
	glfwSetCursorPosCallback(window, mouse_callback);
	glfwSetScrollCallback(window, scroll_callback);
	glfwSetInputMode(window, GLFW_CURSOR, GLFW_CURSOR_DISABLED);

	if (!gladLoadGLLoader((GLADloadproc)glfwGetProcAddress))
	{
		std::cout << "Failed to initialize GLAD" << std::endl;
		return -1;
	}
	glEnable(GL_DEPTH_TEST);
	glEnable(GL_CULL_FACE);
	glCullFace(GL_BACK);


	// simple vertex and fragment shader - add your own tess and geo shader
	Shader tess("..\\shaders\\Terrain\\tessVert.vs", "..\\shaders\\Terrain\\tessFrag.fs", "..\\shaders\\Terrain\\Norms.gs", "..\\shaders\\Terrain\\tessControlShader.tcs", "..\\shaders\\tessEvaluationShader.tes");
	tess.use();
	tess.setVec3("lightPos", lightPos);

	heightMap = loadTexture(heightMapPath.c_str());

	//Terrain Constructor ; number of grids in width, number of grids in height, gridSize
	Terrain terrain(sizeX, sizeY, stepSize);
	std::vector<float> vertices = terrain.getVertices();
	setVAO(vertices);



	//Water
	Model water = Model("..\\Resources\\plane.obj");
	Shader waterShader = Shader("..\\shaders\\Water\\water.vs", "..\\shaders\\Water\\water.fs");
	waterTex = loadTexture(waterNormalPath.c_str());



	while (!glfwWindowShouldClose(window))
	{
		glClearColor(clearColor.x, clearColor.y, clearColor.z, 1.0f);
		float currentFrame = glfwGetTime();
		deltaTime = currentFrame - lastFrame;
		lastFrame = currentFrame;

		processInput(window);

		glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

		glm::mat4 projection = glm::perspective(glm::radians(camera.Zoom), (float)SCR_WIDTH / (float)SCR_HEIGHT, 0.1f, 1200.0f);
		glm::mat4 view = camera.GetViewMatrix();
		glm::mat4 model;

		if (glfwGetKey(window, GLFW_KEY_P) == GLFW_PRESS)
			camera.printCameraCoords();

		//Water
		glBindTexture(GL_TEXTURE_2D, waterTex);
		model = glm::translate(glm::mat4(1.0f), glm::vec3(0, 0, 0));
		model = glm::scale(model, glm::vec3((sizeX - 1) * (stepSize / 2), 1, (sizeY - 1) * (stepSize / 2)));
		waterShader.use();
		waterShader.setMat4("projection", projection);
		waterShader.setMat4("view", view);
		waterShader.setMat4("model", model);

		waterShader.setVec3("u_viewPos", camera.Position);

		waterShader.setVec3("u_lightPos", lightPos);
		waterShader.setVec3("u_lightColor", clearColor);

		waterShader.setFloat("u_ambientStrength", ambient);
		waterShader.setFloat("u_diffuseStrength", diffuse);
		waterShader.setVec3("u_colour", waterColour);
		waterShader.setFloat("u_Time", glfwGetTime());
		waterShader.setFloat("u_waterSpeedX", waterSpeedX);
		waterShader.setFloat("u_waterSpeedY", waterSpeedY);
		waterShader.setFloat("u_Scale", waterScale);
		water.Draw(waterShader);

		//Terrain
		model = glm::mat4(1.0f);
		model = glm::translate(model, glm::vec3(-(((sizeX - 1) * stepSize) / 2), -1, -(((sizeY - 1) * stepSize) / 2)));
		glBindVertexArray(VAO);
		glBindTexture(GL_TEXTURE_2D, heightMap);
		tess.use();
		tess.setMat4("projection", projection);
		tess.setMat4("view", view);
		tess.setMat4("model", model);
		tess.setVec3("viewPos", camera.Position);
		tess.setFloat("heightMapScale", heightMapScale);
		tess.setVec3("fogColor", clearColor);
		tess.setFloat("fogNear", fogNear);
		tess.setFloat("fogFar", fogFar);
		glPolygonMode(GL_FRONT_AND_BACK, GL_FILL);
		glDrawArrays(GL_PATCHES, 0, vertices.size() / 3);

		glfwSwapBuffers(window);
		glfwPollEvents();
	}


	glfwTerminate();
	return 0;
}

// process all input: query GLFW whether relevant keys are pressed/released this frame and react accordingly
// ---------------------------------------------------------------------------------------------------------
void processInput(GLFWwindow* window)
{
	if (glfwGetKey(window, GLFW_KEY_ESCAPE) == GLFW_PRESS)
		glfwSetWindowShouldClose(window, true);

	if (glfwGetKey(window, GLFW_KEY_W) == GLFW_PRESS)
		camera.ProcessKeyboard(FORWARD, deltaTime);
	if (glfwGetKey(window, GLFW_KEY_S) == GLFW_PRESS)
		camera.ProcessKeyboard(BACKWARD, deltaTime);
	if (glfwGetKey(window, GLFW_KEY_A) == GLFW_PRESS)
		camera.ProcessKeyboard(LEFT, deltaTime);
	if (glfwGetKey(window, GLFW_KEY_D) == GLFW_PRESS)
		camera.ProcessKeyboard(RIGHT, deltaTime);
}

// glfw: whenever the window size changed (by OS or user resize) this callback function executes
// ---------------------------------------------------------------------------------------------
void framebuffer_size_callback(GLFWwindow* window, int width, int height)
{
	glViewport(0, 0, width, height);
}

// glfw: whenever the mouse moves, this callback is called
// -------------------------------------------------------
void mouse_callback(GLFWwindow* window, double xpos, double ypos)
{
	if (firstMouse)
	{
		lastX = xpos;
		lastY = ypos;
		firstMouse = false;
	}

	float xoffset = xpos - lastX;
	float yoffset = lastY - ypos; // reversed since y-coordinates go from bottom to top

	lastX = xpos;
	lastY = ypos;

	camera.ProcessMouseMovement(xoffset, yoffset);
}


// glfw: whenever the mouse scroll wheel scrolls, this callback is called
// ----------------------------------------------------------------------
void scroll_callback(GLFWwindow* window, double xoffset, double yoffset)
{
	camera.ProcessMouseScroll(yoffset);
}

unsigned int loadTexture(char const* path)
{
	unsigned int textureID;
	glGenTextures(1, &textureID);

	int width, height, nrComponents;
	unsigned char* data = stbi_load(path, &width, &height, &nrComponents, 0);
	if (data)
	{
		GLenum format;
		if (nrComponents == 1)
			format = GL_RED;
		else if (nrComponents == 3)
			format = GL_RGB;
		else if (nrComponents == 4)
			format = GL_RGBA;

		glBindTexture(GL_TEXTURE_2D, textureID);
		glTexImage2D(GL_TEXTURE_2D, 0, format, width, height, 0, format, GL_UNSIGNED_BYTE, data);
		glGenerateMipmap(GL_TEXTURE_2D);

		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR_MIPMAP_LINEAR);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
		stbi_image_free(data);
		std::cout << "Loaded texture at path: " << path << " width " << width << " id " << textureID << std::endl;

	}
	else
	{
		std::cout << "Texture failed to load at path: " << path << std::endl;
		stbi_image_free(data);

	}

	return textureID;
}


void setVAO(vector <float> vertices) {

	glGenVertexArrays(1, &VAO);
	glGenBuffers(1, &VBO);
	glBindVertexArray(VAO);
	glBindBuffer(GL_ARRAY_BUFFER, VBO);
	glBufferData(GL_ARRAY_BUFFER, (vertices.size() * sizeof(GLfloat)), vertices.data(), GL_STATIC_DRAW);

	//xyz
	glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 5 * sizeof(float), (void*)0);
	glEnableVertexAttribArray(0);
	//texture
	glVertexAttribPointer(1, 2, GL_FLOAT, GL_FALSE, 5 * sizeof(float), (void*)(3 * sizeof(float)));
	glEnableVertexAttribArray(1);


	glBindBuffer(GL_ARRAY_BUFFER, 0);
	glBindVertexArray(0);
}



