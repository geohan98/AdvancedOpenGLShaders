#pragma once

#include <glad/glad.h> 
#include <vector>
#include <set>
#include <iostream>
#include <glm/glm.hpp>

using namespace std;
// Iscohederon creation
//class creates a 20 sided geometric shape with PER VERTEX normals
class Icso {

private:
	const float X = -0.525731112119133606;
    const float Z = -0.850650808352039932; 
	const int numVerts = 12;  
	vector<glm::vec3> positions; 
	vector<glm::vec3> normals;
	glm::vec3 getNormal(vector<int> face);
	void makePositions();

	
public:
	Icso();
	void getVertices(int* indices, int numInd,float* vertices, int numVerts);


};