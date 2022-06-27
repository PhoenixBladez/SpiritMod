using SpiritMod.Buffs.Mount;
using System;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Mounts
{
	public class TideMount : ModMount
	{
		private const float damage = 15f;
		private const float knockback = 2f;

		public override void SetStaticDefaults()
		{
			MountData.spawnDust = 167;
			MountData.spawnDustNoGravity = true;
			MountData.buff = ModContent.BuffType<TideMountBuff>();
			MountData.heightBoost = 16;
			MountData.fallDamage = 0f;
			MountData.runSpeed = 9f;
			MountData.dashSpeed = 3f;
			MountData.flightTimeMax = 0;
			MountData.fatigueMax = 0;
			MountData.jumpHeight = 12;
			MountData.acceleration = 0.05f;
			MountData.jumpSpeed = 5f;
			MountData.blockExtraJumps = true;
			MountData.totalFrames = 11;
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
			MountData.yOffset = -4;
			MountData.xOffset = +15;
			MountData.bodyFrame = 3;
			MountData.playerHeadOffset = 22;
			MountData.standingFrameCount = 1;
			MountData.standingFrameDelay = 12;
			MountData.standingFrameStart = 0;
			MountData.runningFrameCount = 6;
			MountData.runningFrameDelay = 20;
			MountData.runningFrameStart = 0;
			MountData.flyingFrameCount = 1;
			MountData.flyingFrameDelay = 0;
			MountData.flyingFrameStart = 1;
			MountData.inAirFrameCount = 1;
			MountData.inAirFrameDelay = 12;
			MountData.inAirFrameStart = 3;
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
		public bool atkFrames;
		public bool attack;
		public override void UpdateEffects(Player player)
		{
			if (Math.Abs(player.velocity.X) > player.mount.DashSpeed - player.mount.RunSpeed / 2f) {
				player.noKnockback = true;
			}
			for (int i = 0; i < 200; i++) {
				if (Main.npc[i].active && !Main.npc[i].friendly && Main.npc[i].type != NPCID.TargetDummy) {
					int distance = (int)Main.npc[i].Distance(player.Center);
					if (player.velocity == Vector2.Zero) {
						if (distance < 80) {
							atkFrames = true;
							if (player.mount._frame == 10 && !attack) {
								attack = true;
								Main.npc[i].StrikeNPC(30, 2f, 0, false);
							}

						}
						/*if (((distance.X > 0 && player.direction == 1) || (distance.X < 0 && player.direction == -1)) && atkFrames)
                        {
                            Main.npc[i].StrikeNPC(30, 0f, 0, false);
                        }*/
					}
					if (distance > 85) {
						atkFrames = false;
					}
				}
			}
			if (player.velocity != Vector2.Zero) {
				atkFrames = false;
			}
		}
		public override bool UpdateFrame(Player mountedPlayer, int state, Vector2 velocity)
		{
			if (atkFrames) {
				mountedPlayer.mount._frameCounter++;
				if (mountedPlayer.mount._frameCounter >= 8) {
					mountedPlayer.mount._frameCounter = 0;
					mountedPlayer.mount._frame++;
				}
				if (mountedPlayer.mount._frame < 7 || mountedPlayer.mount._frame > 10) {
					mountedPlayer.mount._frame = 7;
					attack = false;
				}
				return false;
			}
			return true;
		}

	}
}

