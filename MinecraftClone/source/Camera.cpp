#include "Camera.h"

#include <iostream>

Camera::Camera(float Ratio, int FOV)
{
	fov = FOV;
	ratio = Ratio;

	position = glm::vec3(0, 0, 0);

	//rightdir = glm::vec3(-1, 0, 0);
	//topdir = glm::vec3(0, 1, 0);
	//frontdir = glm::vec3(0, 0, 1);

	yaw = 90;
	pitch = 0;

	UpdateVectors();
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
	return glm::perspective(glm::radians((float)fov), ratio, 0.1f, 100.0f) * glm::scale(glm::mat4(1), glm::vec3(-1, 1, 1));
}

void Camera::LocalMoveCamera(float MoveX, float MoveY, float MoveZ)
{
	position -= rightdir * MoveX;
	position += topdir * MoveY;
	position += frontdir * MoveZ;

	UpdateVectors();
}

void Camera::GlobalMoveCamera(float MoveX, float MoveY, float MoveZ)
{
	position -= MoveX;
	position += MoveY;
	position += MoveZ;

	UpdateVectors();
}

void Camera::LocalRotateCamera(float RotateYaw, float RotatePitch)
{
	yaw -= RotateYaw;
	pitch += RotatePitch;

	UpdateVectors();
}

void Camera::UpdateVectors()
{
	// calculate the new Front vector
	glm::vec3 front;
	front.x = cos(glm::radians(yaw)) * cos(glm::radians(pitch));
	front.y = sin(glm::radians(pitch));
	front.z = sin(glm::radians(yaw)) * cos(glm::radians(pitch));
	frontdir = glm::normalize(front);

	// also re-calculate the Right and Up vector
	rightdir = glm::normalize(glm::cross(frontdir, glm::vec3(0.0f, 1.0f, 0.0f)));  // normalize the vectors, because their length gets closer to 0 the more you look up or down which results in slower movement.
	topdir = glm::normalize(glm::cross(rightdir, frontdir));

	std::cout << "X:" << (int)(position.x * 100) / 100.0f << ", Y:" << (int)(position.y * 100) / 100.0f << ", Z:" << (int)(position.z * 100) / 100.0f << std::endl;
}