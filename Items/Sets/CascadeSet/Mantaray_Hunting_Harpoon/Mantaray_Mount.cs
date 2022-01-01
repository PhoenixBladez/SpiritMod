using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace SpiritMod.Items.Sets.CascadeSet.Mantaray_Hunting_Harpoon
{
    public class Mantaray_Mount : ModMountData
    {
		protected int circularGlide = 0;

        public override void SetDefaults()
        {
			mountData.spawnDust = 103;
			mountData.buff = ModContent.BuffType<Mantaray_Buff>();
			mountData.heightBoost = 14;
			mountData.flightTimeMax = 0;
			mountData.fatigueMax = 0;
			mountData.fallDamage = 0.0f;
			mountData.usesHover = true;
			mountData.runSpeed = 8f;
			mountData.dashSpeed = 3f;
			mountData.acceleration = 0.35f;
			mountData.constantJump = false;
			mountData.jumpHeight = 10;
			mountData.jumpSpeed = 3f;
			mountData.swimSpeed = 95f;
			mountData.blockExtraJumps = true;
			mountData.totalFrames = 7;

			int[] yOffsets = new int[mountData.totalFrames];
			for (int index = 0; index < yOffsets.Length; ++index)
				yOffsets[index] = 12;
			mountData.playerYOffsets = yOffsets;

			mountData.xOffset = -10;
			mountData.bodyFrame = 3;
			mountData.yOffset = 20;
			mountData.playerHeadOffset = 31;
			mountData.standingFrameCount = 7;
			mountData.standingFrameDelay = 4;
			mountData.standingFrameStart = 0;
			mountData.runningFrameCount = 7;
			mountData.runningFrameDelay = 4;
			mountData.runningFrameStart = 0;
			mountData.flyingFrameCount = 7;
			mountData.flyingFrameDelay = 4;
			mountData.flyingFrameStart = 0;
			mountData.inAirFrameCount = 7;
			mountData.inAirFrameDelay = 4;
			mountData.inAirFrameStart = 0;
			mountData.idleFrameCount = 0;
			mountData.idleFrameDelay = 0;
			mountData.idleFrameStart = 0;
			mountData.idleFrameLoop = false;
			mountData.swimFrameCount = 7;
			mountData.swimFrameDelay = 12;
			mountData.swimFrameStart = 0;

			if (Main.netMode != NetmodeID.Server)
			{
				mountData.textureWidth = mountData.backTexture.Width;
				mountData.textureHeight = mountData.backTexture.Height;
			}
        }
 
        public override void UpdateEffects(Player player)
		{
			const float MaxSpeed = 16.5f;
			if (player.velocity.Y <= -MaxSpeed)
				player.velocity.Y = -MaxSpeed;

			if (!player.wet)
			{
				mountData.flightTimeMax = 0;
				mountData.usesHover = false;
				mountData.acceleration = 0.05f;
				mountData.dashSpeed = 0f;
				mountData.runSpeed = 0.05f;

				if (player.velocity.Y != 0 || player.oldVelocity.Y != 0)
				{
					int direction = (Math.Abs(player.velocity.X) == 0) ? 0 :
						(player.direction == Math.Sign(player.velocity.X)) ? 1 : -1;
					player.fullRotation = player.velocity.Y * 0.05f * player.direction * direction * mountData.jumpHeight / 14f;
					player.fullRotationOrigin = (player.Hitbox.Size() + new Vector2(0, 42)) / 2;
				}
			}
			else
			{
				mountData.flightTimeMax = 9999;
				mountData.fatigueMax = 9999;
				mountData.acceleration = 0.2f;
				mountData.dashSpeed = 3f;
				mountData.runSpeed = 12f;
				mountData.usesHover = true;

				player.gravity = 0f;
				player.fullRotation = 0f;
				player.velocity.Y *= 0.99f;
			}
			player.gills = true;
        }
    }
}