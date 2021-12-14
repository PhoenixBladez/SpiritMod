using SpiritMod.Buffs.Mount;
using System;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.FloatingItems.MessageBottle
{
	public class MessageBottleMount : ModMountData
	{
		private const float damage = 15f;
		private const float knockback = 2f;

		int wetCounter;
		public override void SetDefaults()
		{
			mountData.spawnDust = 167;
			mountData.spawnDustNoGravity = true;
			mountData.buff = ModContent.BuffType<BottleMountBuff>();
			mountData.heightBoost = 16;
			mountData.fallDamage = 0f;
			mountData.runSpeed = 0;
			mountData.flightTimeMax = 0;
			mountData.fatigueMax = 0;
			mountData.jumpHeight = 0;
			mountData.acceleration = 0.05f;
			mountData.swimSpeed = 3;
			mountData.jumpSpeed =0;
			mountData.blockExtraJumps = true;
			mountData.totalFrames = 1;
			mountData.constantJump = true;
			int[] array = new int[1];
			array[0] = 12;
			mountData.playerYOffsets = array;
			mountData.yOffset = +5;
			mountData.xOffset = -15;
			mountData.playerHeadOffset = 22;
			mountData.standingFrameCount = 1;
			mountData.standingFrameDelay = 12;
			mountData.standingFrameStart = 0;
			mountData.inAirFrameCount = 1;
			mountData.inAirFrameDelay = 12;
			mountData.inAirFrameStart = 0;
			mountData.idleFrameCount = 1;
			mountData.idleFrameDelay = 12;
			mountData.idleFrameStart = 0;
			mountData.idleFrameLoop = true;
			if (Main.netMode != NetmodeID.Server) {
				mountData.textureWidth = mountData.frontTexture.Width;
				mountData.textureHeight = mountData.frontTexture.Height;
			}
		}
		public override void UpdateEffects(Player player)
		{
			if (player.wet)
			{
				mountData.runSpeed = 4;
				player.velocity.Y = -2;
				wetCounter = 15;
			}
			else
			{
				wetCounter--;
				if (wetCounter < 0)
				{
					player.velocity.X *= 0.9f;
					mountData.runSpeed = 0;
				}
			}
		}
	}
}

