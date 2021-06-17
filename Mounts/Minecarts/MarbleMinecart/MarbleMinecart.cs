using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SpiritMod.Mounts.Minecarts.MarbleMinecart
{
	class MarbleMinecart : ModMountData
	{
		public override void SetDefaults()
		{
			int total_frames = 3;
			int[] player_y_offsets = new int[total_frames];
			for (int i = 0; i < player_y_offsets.Length; i++)
				player_y_offsets[i] = 10;

			mountData.Minecart = true;
			mountData.MinecartDirectional = true;
			mountData.MinecartDust = new Action<Vector2>(DelegateMethods.Minecart.Sparks);

			mountData.runSpeed = 17;
			mountData.dashSpeed = 14;
			mountData.fallDamage = 1f;
			mountData.jumpHeight = 15;
			mountData.spawnDust = 174;
			mountData.jumpSpeed = 5.15f;
			mountData.flightTimeMax = 0;
			mountData.acceleration = 0.08f;
			mountData.blockExtraJumps = true;
			mountData.buff = ModContent.BuffType<MarbleMinecartBuff>();

			mountData.xOffset = 2;
			mountData.yOffset = 13;
			mountData.bodyFrame = 3;
			mountData.heightBoost = 12;
			mountData.playerHeadOffset = 20;
			mountData.totalFrames = total_frames;
			mountData.playerYOffsets = player_y_offsets;

			mountData.standingFrameCount = 1;
			mountData.standingFrameDelay = 12;
			mountData.standingFrameStart = 0;
			mountData.runningFrameCount = 3;
			mountData.runningFrameDelay = 12;
			mountData.runningFrameStart = 0;
			mountData.flyingFrameCount = 0;
			mountData.flyingFrameDelay = 0;
			mountData.flyingFrameStart = 0;
			mountData.inAirFrameCount = 0;
			mountData.inAirFrameDelay = 0;
			mountData.inAirFrameStart = 0;
			mountData.idleFrameCount = 1;
			mountData.idleFrameDelay = 10;
			mountData.idleFrameStart = 0;
			mountData.idleFrameLoop = false;

			if (Main.netMode == NetmodeID.Server)
				return;

			mountData.textureWidth = mountData.backTexture.Width;
			mountData.textureHeight = mountData.backTexture.Height;
		}
	}
}