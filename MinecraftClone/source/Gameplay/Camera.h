#pragma once

#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include <glm/gtc/type_ptr.hpp>

#include <algorithm>

class Camera
{
public:
	Camera();
	Camera(float, int);

	void ChangeFOV(float, int);
	void ChangeFOV(int);

	glm::mat4 GetViewMatrix();
	glm::mat4 GetProjectionMatrix();

	void LocalMoveCamera(float, float, float);
	void LocalRotateCamera(float, float);

	void GlobalMoveCamera(float, float, float);

private:
	glm::vec3 position;
	float yaw;
	float pitch;

	glm::vec3 rightdir;
	glm::vec3 topdir;
	glm::vec3 frontdir;

	int fov;
	float ratio;

	void UpdateVectors();
};

