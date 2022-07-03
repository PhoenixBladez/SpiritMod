using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SpiritMod.Mounts.Minecarts.MarbleMinecart
{
	class MarbleMinecart : ModMount
	{
		public override void SetStaticDefaults()
		{
			int total_frames = 3;
			int[] player_y_offsets = new int[total_frames];
			for (int i = 0; i < player_y_offsets.Length; i++)
				player_y_offsets[i] = 10;

			MountData.Minecart = true;
			MountData.MinecartDirectional = true;

			MountData.runSpeed = 17;
			MountData.dashSpeed = 14;
			MountData.fallDamage = 1f;
			MountData.jumpHeight = 15;
			MountData.spawnDust = 174;
			MountData.jumpSpeed = 5.15f;
			MountData.flightTimeMax = 0;
			MountData.acceleration = 0.08f;
			MountData.blockExtraJumps = true;
			MountData.buff = ModContent.BuffType<MarbleMinecartBuff>();

			MountData.xOffset = 2;
			MountData.yOffset = 13;
			MountData.bodyFrame = 3;
			MountData.heightBoost = 12;
			MountData.playerHeadOffset = 20;
			MountData.totalFrames = total_frames;
			MountData.playerYOffsets = player_y_offsets;

			MountData.standingFrameCount = 1;
			MountData.standingFrameDelay = 12;
			MountData.standingFrameStart = 0;
			MountData.runningFrameCount = 3;
			MountData.runningFrameDelay = 12;
			MountData.runningFrameStart = 0;
			MountData.flyingFrameCount = 0;
			MountData.flyingFrameDelay = 0;
			MountData.flyingFrameStart = 0;
			MountData.inAirFrameCount = 0;
			MountData.inAirFrameDelay = 0;
			MountData.inAirFrameStart = 0;
			MountData.idleFrameCount = 1;
			MountData.idleFrameDelay = 10;
			MountData.idleFrameStart = 0;
			MountData.idleFrameLoop = false;

			if (Main.netMode == NetmodeID.Server)
				return;

			MountData.textureWidth = MountData.backTexture.Width();
			MountData.textureHeight = MountData.backTexture.Height();
		}
	}
}