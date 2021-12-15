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
			mountData.buff = ModContent.BuffType<BottleMountBuff>();
			mountData.spawnDust = 7;
			mountData.spawnDustNoGravity = true;
			mountData.heightBoost = 16;
			mountData.fallDamage = 0f;
			mountData.runSpeed = 0;
			mountData.flightTimeMax = 0;
			mountData.fatigueMax = 0;
			mountData.jumpHeight = 0;
			mountData.acceleration = 0f;
			mountData.swimSpeed = 3;
			mountData.jumpSpeed = 0;
			mountData.blockExtraJumps = true;
			mountData.totalFrames = 1;
			mountData.constantJump = true;
			mountData.playerYOffsets = new int[] { 12 };
			mountData.yOffset = 5;
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
			if (Main.netMode != NetmodeID.Server)
			{
				mountData.textureWidth = mountData.frontTexture.Width;
				mountData.textureHeight = mountData.frontTexture.Height;
			}
		}
		public override void UpdateEffects(Player player)
		{
			if (Framing.GetTileSafely(player.MountedCenter + new Vector2(0, 36)).liquid > 125)
			{
				mountData.runSpeed = 6;
				mountData.acceleration = 0.15f;

				if (Framing.GetTileSafely(player.MountedCenter + new Vector2(0, 34)).liquid > 125)
				{
					player.velocity.Y -= player.gravity * 2.5f;
					if (player.velocity.Y <= -12f)
						player.velocity.Y = -12f;
				}
				else
					player.velocity.Y = -player.gravity;

				player.fishingSkill += 10;

				wetCounter = 15;
			}
			else
			{
				wetCounter--;
				mountData.acceleration = 0.05f;

				player.gravity *= 1.5f;
				if (wetCounter < 0)
				{
					player.velocity.X *= 0.9f;
					mountData.runSpeed = 0.05f;
				}
			}
		}
	}
}