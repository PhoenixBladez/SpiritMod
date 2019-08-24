using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Mounts
{
	public class BasiliskMount : ModMountData
	{
		public static ModMountData _ref;
		private const float damage = 15f;
		private const float knockback = 2f;

		public override void SetDefaults()
		{
			mountData.buff = mod.BuffType("RidingBasiisk");
			mountData.heightBoost = 16;
			mountData.fallDamage = 0f;
			mountData.runSpeed = 8f;
			mountData.dashSpeed = 3f;
			mountData.flightTimeMax = 0;
			mountData.fatigueMax = 0;
			mountData.jumpHeight = 8;
			mountData.acceleration = 0.1f;
			mountData.jumpSpeed = 5f;
			mountData.blockExtraJumps = true;
			mountData.totalFrames = 8;
			mountData.constantJump = false;
			int[] array = new int[mountData.totalFrames];
			for (int i = 0; i < array.Length; i++)
			{
				if (i == 1)
				{
					array[i] = 24;
				}
				else if (i == 3 || i == 4 || i == 5)
				{
					array[i] = 18;
				}
				else
				{
					array[i] = 20;
				}
			}
			mountData.playerYOffsets = array;
			mountData.yOffset = 2;
			mountData.xOffset = +7;
			mountData.bodyFrame = 3;
			mountData.playerHeadOffset = 22;
			mountData.standingFrameCount = 1;
			mountData.standingFrameDelay = 12;
			mountData.standingFrameStart = 0;
			mountData.runningFrameCount = 6;
			mountData.runningFrameDelay = 20;
			mountData.runningFrameStart = 2;
			mountData.flyingFrameCount = 1;
			mountData.flyingFrameDelay = 0;
			mountData.flyingFrameStart = 1;
			mountData.inAirFrameCount = 1;
			mountData.inAirFrameDelay = 12;
			mountData.inAirFrameStart = 1;
			mountData.idleFrameCount = 1;
			mountData.idleFrameDelay = 12;
			mountData.idleFrameStart = 0;
			mountData.idleFrameLoop = true;
			mountData.swimFrameCount = mountData.inAirFrameCount;
			mountData.swimFrameDelay = mountData.inAirFrameDelay;
			mountData.swimFrameStart = mountData.inAirFrameStart;
			if (Main.netMode != 2)
			{
				mountData.textureWidth = mountData.backTexture.Width;
				mountData.textureHeight = mountData.backTexture.Height;
			}
		}

		public override void UpdateEffects(Player player)
		{
			if (Main.rand.Next(10) == 0)
			{
				Vector2 vector2 = player.Center + new Vector2((float)(-48 * player.direction), -6f * player.gravDir);
				if (player.direction == -1)
				{
					vector2.X -= 20f;
				}
				Dust.NewDust(vector2, 16, 16, 176, 0f, 0f, 0, default(Color), 1f);
			}
			if (Math.Abs(player.velocity.X) > player.mount.DashSpeed - player.mount.RunSpeed / 2f)
			{
				player.noKnockback = true;


				Rectangle rect = player.getRect();
				//Tweak hitbox to be the Basilisks head
				rect.Width = 2;
				rect.Height = 2;
				if (player.direction == 1)
				{
					rect.Offset(player.width - 1, 0);
				}
				rect.Offset(0, player.height - 19);
				rect.Inflate(28, 16);

				for (int n = 0; n < Main.maxNPCs; n++)
				{
					NPC npc = Main.npc[n];
					if (npc.active && !npc.dontTakeDamage && !npc.friendly && npc.immune[player.whoAmI] == 0)
					{
						Rectangle rect2 = npc.getRect();
						if (rect.Intersects(rect2) && (npc.noTileCollide || Collision.CanHit(player.position, player.width, player.height, npc.position, npc.width, npc.height)))
						{
							float damage = BasiliskMount.damage * player.minionDamage;
							float knockback = BasiliskMount.knockback * player.minionKB;
							int direction = player.direction;
							if (player.velocity.X < 0f)
							{
								direction = -1;
							}
							if (player.velocity.X > 0f)
							{
								direction = 1;
							}
							if (player.whoAmI == Main.myPlayer)
							{
								player.ApplyDamageToNPC(npc, (int)damage, knockback, direction, false);
							}
							npc.immune[player.whoAmI] = 30;
							player.immune = true;
							player.immuneTime = 6;
							return;
						}
					}
				}
			}
		}
	}
}

