#include "Camera.h"

Camera::Camera(float Ratio, int FOV)
{
	fov = FOV;
	ratio = Ratio;

	position = glm::vec3(0, 0, 0);

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
	return glm::perspective(glm::radians((float)fov), ratio, 0.01f, 100.0f) * glm::scale(glm::mat4(1), glm::vec3(-1, 1, 1));
}

void Camera::MoveCamera(float MoveX, float MoveY, float MoveZ)
{
	position -= rightdir * MoveX;
	position += topdir * MoveY;
	position += frontdir * MoveZ;

	UpdateVectors();
}

void Camera::RotateCamera(float RotateYaw, float RotatePitch)
{
	yaw -= RotateYaw;
	pitch = std::clamp(pitch + RotatePitch, -89.0f, 89.0f);

	UpdateVectors();
}

void Camera::SetCameraPosition(float MoveX, float MoveY, float MoveZ)
{
	position = glm::vec3(MoveX, MoveY, MoveZ);

	UpdateVectors();
}

void Camera::SetCameraRotation(float RotateYaw, float RotatePitch)
{
	yaw = RotateYaw;
	pitch = std::clamp(RotatePitch, -89.0f, 89.0f);

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

	//std::cout << "X:" << (int)(position.x * 100) / 100.0f << ", Y:" << (int)(position.y * 100) / 100.0f << ", Z:" << (int)(position.z * 100) / 100.0f << std::endl;
}