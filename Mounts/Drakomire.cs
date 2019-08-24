using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Mounts
{
	public class Drakomire : ModMountData
	{
		public static ModMountData _ref;

		public override void SetDefaults()
		{
			mountData.buff = mod.BuffType("DrakomireMountBuff");
			mountData.heightBoost = 20;
			mountData.fallDamage = 0f;
			mountData.runSpeed = 8f;
			mountData.dashSpeed = 3f;
			mountData.flightTimeMax = 0;
			mountData.fatigueMax = 0;
			mountData.jumpHeight = 12;
			mountData.acceleration = 0.1f;
			mountData.jumpSpeed = 10f;
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
			mountData.yOffset = 6;
			mountData.xOffset = -7;
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
			player.statDefense += 40;
			player.noKnockback = true;
			if (player.dashDelay > 0)
			{
				player.dashDelay--;
			}
			else
			{
				int num4 = 0;
				bool flag = false;
				if (player.dashTime > 0)
				{
					player.dashTime--;
				}
				else if (player.dashTime < 0)
				{
					player.dashTime++;
				}
				if (player.controlRight && player.releaseRight)
				{
					if (player.dashTime > 0)
					{
						num4 = 1;
						flag = true;
						player.dashTime = 0;
					}
					else
					{
						player.dashTime = 15;
					}
				}
				else if (player.controlLeft && player.releaseLeft)
				{
					if (player.dashTime < 0)
					{
						num4 = -1;
						flag = true;
						player.dashTime = 0;
					}
					else
					{
						player.dashTime = -15;
					}
				}
				if (flag)
				{
					player.velocity.X = 16.9f * (float)num4;
					Point point = Utils.ToTileCoordinates(player.Center + new Vector2((float)(num4 * player.width / 2 + 2), player.gravDir * -(float)player.height / 2f + player.gravDir * 2f));
					Point point2 = Utils.ToTileCoordinates(player.Center + new Vector2((float)(num4 * player.width / 2 + 2), 0f));
					if (WorldGen.SolidOrSlopedTile(point.X, point.Y) || WorldGen.SolidOrSlopedTile(point2.X, point2.Y))
					{
						player.velocity.X = player.velocity.X / 2f;
					}
					player.dashDelay = 600;
				}
			}
			if (player.velocity.X != 0f && player.velocity.Y == 0f)
			{
				player.mount._abilityCooldown -= (int)Math.Abs(player.velocity.X);
				if (player.mount._abilityCooldown <= -15)
				{
					Vector2 vector = player.Center + new Vector2((float)(26 * -(float)player.direction), 26f * player.gravDir);
					Terraria.Projectile.NewProjectile(vector.X, vector.Y, 0f, 0f, mod.ProjectileType("DrakomireFlame"), player.statDefense / 2, 0f, player.whoAmI, 0f, 0f);
					player.mount._abilityCooldown = 0;
				}
			}
			if (Main.rand.Next(10) == 0)
			{
				Vector2 vector2 = player.Center + new Vector2((float)(-48 * player.direction), -6f * player.gravDir);
				if (player.direction == -1)
				{
					vector2.X -= 20f;
				}
				Dust.NewDust(vector2, 16, 16, 6, 0f, 0f, 0, default(Color), 1f);
			}
		}
	}
}
