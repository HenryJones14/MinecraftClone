#include "Camera.h"

#include <iostream>

Camera::Camera()
{
	fov = 90;
	ratio = 16.0f / 9.0f;

	position = glm::vec3(0, 0, 0);

	rightdir = glm::vec3(1, 0, 0);
	topdir = glm::vec3(0, 1, 0);
	frontdir = glm::vec3(0, 0, 1);

	yaw = 90;
	pitch = 0;
}

Camera::Camera(float Ratio, int FOV)
{
	fov = FOV;
	ratio = Ratio;

	position = glm::vec3(0, 0, 0);

	rightdir = glm::vec3(1, 0, 0);
	topdir = glm::vec3(0, 1, 0);
	frontdir = glm::vec3(0, 0, 1);

	yaw = 90;
	pitch = 0;
}

void Camera::ChangeFOV(float Ratio, int FOV)
{
	ratio = Ratio;
	fov = FOV;
}

void Camera::ChangeFOV(int FOV)
{
	fov = FOV;
}

glm::mat4 Camera::GetViewMatrix()
{
	return glm::lookAt(position, position + frontdir, topdir);
}

glm::mat4 Camera::GetProjectionMatrix()
{
	return glm::perspective(glm::radians((float)fov), ratio, 0.1f, 100.0f);
}

void Camera::MoveCamera(float MoveX, float MoveY, float MoveZ)
{
	position += rightdir * MoveX;
	position += topdir * MoveY;
	position += frontdir * MoveZ;

	UpdateVectors();
}

void Camera::RotateCamera(float RotateYaw, float RotatePitch)
{
	yaw += glm::radians(RotateYaw);
	pitch += glm::radians(RotatePitch);

	UpdateVectors();
}

void Camera::UpdateVectors()
{
	std::cout << position.x << ", " << position.y << ", " << position.z << std::endl;

	// calculate the new Front vector
	glm::vec3 front;
	front.x = cos(glm::radians(yaw)) * cos(glm::radians(pitch));
	front.y = sin(glm::radians(pitch));
	front.z = sin(glm::radians(yaw)) * cos(glm::radians(pitch));
	frontdir = glm::normalize(front);

	// also re-calculate the Right and Up vector
	rightdir = glm::normalize(glm::cross(frontdir, glm::vec3(0.0f, 1.0f, 0.0f)));  // normalize the vectors, because their length gets closer to 0 the more you look up or down which results in slower movement.
	topdir = glm::normalize(glm::cross(rightdir, frontdir));
}