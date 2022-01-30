#pragma once

#include <vector>
#include <string>

enum BlockRenderMode
{
	Opaque, Translucent, Transparent
};

struct BlockInfo
{
	BlockInfo()
	{
		RenderMode = BlockRenderMode::Transparent;

		Right_TextureID = 0;
		Left_TextureID = 0;
		Top_TextureID = 0;
		Bottom_TextureID = 0;
		Front_TextureID = 0;
		Back_TextureID = 0;
	}

	BlockInfo(BlockRenderMode NewRenderMode, unsigned int NewRightTextureID, unsigned int NewLeftTextureID, unsigned int NewTopTextureID, unsigned int NewBottomTextureID, unsigned int NewFrontTextureID, unsigned int NewBackTextureID)
	{
		RenderMode = NewRenderMode;

		Right_TextureID = NewRightTextureID;
		Left_TextureID = NewLeftTextureID;
		Top_TextureID = NewTopTextureID;
		Bottom_TextureID = NewBottomTextureID;
		Front_TextureID = NewFrontTextureID;
		Back_TextureID = NewBackTextureID;
	}

	BlockRenderMode RenderMode;

	unsigned int Right_TextureID;
	unsigned int Left_TextureID;
	unsigned int Top_TextureID;
	unsigned int Bottom_TextureID;
	unsigned int Front_TextureID;
	unsigned int Back_TextureID;
};

class BlockList
{
public:
	inline static const std::vector<std::string> Textures =
	{
		// Debug
		"textures/engine/normal/Right.png",
		"textures/engine/normal/Left.png",
		"textures/engine/normal/Top.png",
		"textures/engine/normal/Bottom.png",
		"textures/engine/normal/Front.png",
		"textures/engine/normal/Back.png",

		"textures/blocks/Grass/Right.png",
		"textures/blocks/Grass/Top.png",

		"textures/blocks/Dirt/Right.png",

		"textures/blocks/Stone/Right.png",

		"textures/blocks/Diamond/Right.png",

		"textures/blocks/Bedrock/Right.png",
	};

	inline static const std::vector<BlockInfo> Blocks =
	{
		// Air
		BlockInfo(),

		// Debug
		BlockInfo(BlockRenderMode::Opaque, 0, 1, 2, 3, 4, 5),

		// Grass
		BlockInfo(BlockRenderMode::Opaque, 6, 6, 7, 8, 6, 6),

		// Dirt
		BlockInfo(BlockRenderMode::Opaque, 8, 8, 8, 8, 8, 8),

		// Stone
		BlockInfo(BlockRenderMode::Opaque, 9, 9, 9, 9, 9, 9),

		// Diamond
		BlockInfo(BlockRenderMode::Opaque, 10, 10, 10, 10, 10, 10),

		// Bedrock
		BlockInfo(BlockRenderMode::Opaque, 11, 11, 11, 11, 11, 11),
	};
};