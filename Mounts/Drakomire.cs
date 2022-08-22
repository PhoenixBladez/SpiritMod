using Microsoft.Xna.Framework;
using SpiritMod.Buffs.Mount;
using SpiritMod.Projectiles;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Mounts
{
	public class Drakomire : ModMount
	{

		public override void SetStaticDefaults()
		{
			MountData.buff = ModContent.BuffType<DrakomireMountBuff>();
			MountData.heightBoost = 20;
			MountData.fallDamage = 0f;
			MountData.runSpeed = 8f;
			MountData.dashSpeed = 3f;
			MountData.flightTimeMax = 0;
			MountData.fatigueMax = 0;
			MountData.jumpHeight = 12;
			MountData.acceleration = 0.1f;
			MountData.jumpSpeed = 10f;
			MountData.blockExtraJumps = true;
			MountData.totalFrames = 8;
			MountData.constantJump = false;
			int[] array = new int[MountData.totalFrames];
			for (int i = 0; i < array.Length; i++) {
				if (i == 1) {
					array[i] = 24;
				}
				else if (i == 3 || i == 4 || i == 5) {
					array[i] = 18;
				}
				else {
					array[i] = 20;
				}
			}
			MountData.playerYOffsets = array;
			MountData.yOffset = 6;
			MountData.xOffset = -7;
			MountData.bodyFrame = 3;
			MountData.playerHeadOffset = 22;
			MountData.standingFrameCount = 1;
			MountData.standingFrameDelay = 12;
			MountData.standingFrameStart = 0;
			MountData.runningFrameCount = 6;
			MountData.runningFrameDelay = 20;
			MountData.runningFrameStart = 2;
			MountData.flyingFrameCount = 1;
			MountData.flyingFrameDelay = 0;
			MountData.flyingFrameStart = 1;
			MountData.inAirFrameCount = 1;
			MountData.inAirFrameDelay = 12;
			MountData.inAirFrameStart = 1;
			MountData.idleFrameCount = 1;
			MountData.idleFrameDelay = 12;
			MountData.idleFrameStart = 0;
			MountData.idleFrameLoop = true;
			MountData.swimFrameCount = MountData.inAirFrameCount;
			MountData.swimFrameDelay = MountData.inAirFrameDelay;
			MountData.swimFrameStart = MountData.inAirFrameStart;
			if (Main.netMode != NetmodeID.Server) {
				MountData.textureWidth = MountData.backTexture.Width();
				MountData.textureHeight = MountData.backTexture.Height();
			}
		}

		public override void UpdateEffects(Player player)
		{
			player.statDefense += 40;
			player.noKnockback = true;
			if (player.dashDelay > 0) {
				player.dashDelay--;
			}
			else {
				int num4 = 0;
				bool flag = false;
				if (player.dashTime > 0) {
					player.dashTime--;
				}
				else if (player.dashTime < 0) {
					player.dashTime++;
				}
				if (player.controlRight && player.releaseRight) {
					if (player.dashTime > 0) {
						num4 = 1;
						flag = true;
						player.dashTime = 0;
					}
					else {
						player.dashTime = 15;
					}
				}
				else if (player.controlLeft && player.releaseLeft) {
					if (player.dashTime < 0) {
						num4 = -1;
						flag = true;
						player.dashTime = 0;
					}
					else {
						player.dashTime = -15;
					}
				}
				if (flag) {
					player.velocity.X = 16.9f * (float)num4;
					Point point = Utils.ToTileCoordinates(player.Center + new Vector2((float)(num4 * player.width / 2 + 2), player.gravDir * -(float)player.height / 2f + player.gravDir * 2f));
					Point point2 = Utils.ToTileCoordinates(player.Center + new Vector2((float)(num4 * player.width / 2 + 2), 0f));
					if (WorldGen.SolidOrSlopedTile(point.X, point.Y) || WorldGen.SolidOrSlopedTile(point2.X, point2.Y)) {
						player.velocity.X = player.velocity.X / 2f;
					}
					player.dashDelay = 600;
				}
			}
			if (player.velocity.X != 0f && player.velocity.Y == 0f) {
				player.mount._abilityCooldown -= (int)Math.Abs(player.velocity.X);
				if (player.mount._abilityCooldown <= -15) {
					Vector2 vector = player.Center + new Vector2((26 * -(float)player.direction), 26f * player.gravDir);
					Projectile.NewProjectile(player.GetSource_Misc("Mount"), vector.X, vector.Y, 0f, 0f, ModContent.ProjectileType<DrakomireFlame>(), player.statDefense / 2, 0f, player.whoAmI, 0f, 0f);
					player.mount._abilityCooldown = 0;
				}
			}
			if (Main.rand.NextBool(10)) {
				Vector2 vector2 = player.Center + new Vector2((-48 * player.direction), -6f * player.gravDir);
				if (player.direction == -1) {
					vector2.X -= 20f;
				}
				Dust.NewDust(vector2, 16, 16, DustID.Torch, 0f, 0f, 0, default, 1f);
			}
		}
	}
}
