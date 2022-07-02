using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.FloatingItems.MessageBottle
{
	public class MessageBottleMount : ModMount
	{
		int wetCounter;
		public override void SetStaticDefaults()
		{
			MountData.buff = ModContent.BuffType<BottleMountBuff>();
			MountData.spawnDust = 7;
			MountData.spawnDustNoGravity = true;
			MountData.heightBoost = 16;
			MountData.fallDamage = 0f;
			MountData.runSpeed = 0;
			MountData.flightTimeMax = 0;
			MountData.fatigueMax = 0;
			MountData.jumpHeight = 0;
			MountData.acceleration = 0f;
			MountData.swimSpeed = 3;
			MountData.jumpSpeed = 0;
			MountData.blockExtraJumps = true;
			MountData.totalFrames = 1;
			MountData.constantJump = true;
			MountData.playerYOffsets = new int[] { 12 };
			MountData.yOffset = 5;
			MountData.xOffset = -15;
			MountData.playerHeadOffset = 22;
			MountData.standingFrameCount = 1;
			MountData.standingFrameDelay = 12;
			MountData.standingFrameStart = 0;
			MountData.inAirFrameCount = 1;
			MountData.inAirFrameDelay = 12;
			MountData.inAirFrameStart = 0;
			MountData.idleFrameCount = 1;
			MountData.idleFrameDelay = 12;
			MountData.idleFrameStart = 0;
			MountData.idleFrameLoop = true;

			if (Main.netMode != NetmodeID.Server)
			{
				MountData.textureWidth = MountData.frontTexture.Width();
				MountData.textureHeight = MountData.frontTexture.Height();
			}
		}
		public override void UpdateEffects(Player player)
		{
			const float MaxSpeed = 12f;

			if (Collision.WetCollision(player.position, player.width, player.height + 16) && !Collision.LavaCollision(player.position, player.width, player.height + 16))
			{
				MountData.runSpeed = 7;
				MountData.acceleration = 0.075f;

				if (Collision.WetCollision(player.position, player.width, player.height + 6))
				{
					player.velocity.Y -= player.gravity * 2.5f;
					if (player.velocity.Y <= -MaxSpeed)
						player.velocity.Y = -MaxSpeed;

					if (!Collision.WetCollision(player.position, player.width, (int)(player.height * 0.6f)))
						player.velocity.Y *= 0.92f;
				}
				else
				{
					if (player.velocity.Y > -MaxSpeed * 0.5f && player.velocity.Y < 0.5f)
						player.velocity.Y = -player.gravity;
				}

				player.fishingSkill += 10;
				wetCounter = 15;
			}
			else 
			{
				if (Collision.LavaCollision(player.position, player.width, player.height + 16))
					player.QuickMount();

				wetCounter--;
				MountData.acceleration = 0.05f;

				player.gravity *= 1.5f;
				if (wetCounter < 0)
				{
					if (Collision.SolidCollision(player.position, player.width, player.height + 16))
					{
						player.velocity.X *= 0.92f;
						MountData.runSpeed = 0.05f;
					}
					else
						MountData.runSpeed = 7;
				}
			}

		}
	}
}