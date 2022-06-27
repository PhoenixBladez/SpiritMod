using Microsoft.Xna.Framework;
using SpiritMod.Buffs.Mount;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Mounts
{
	public class DiabolicPlatform : ModMount
	{
		public const float groundSlowdown = 0.3f;

		public int fatigue;
		public const int maxFatigue = 9999999;
		public const float verticalSpeed = 0.34f;
		private const float maxTilt = (float)Math.PI * 0.08f;

		public override void SetStaticDefaults()
		{
			MountData.spawnDust = 6;
			MountData.spawnDustNoGravity = true;
			MountData.buff = ModContent.BuffType<DiabolicPlatformBuff>();
			MountData.heightBoost = 2;
			MountData.flightTimeMax = 60;
			MountData.fatigueMax = 120;
			MountData.fallDamage = 0f;
			MountData.runSpeed = 6;
			MountData.dashSpeed = 2f;
			MountData.acceleration = 0.2F;
			MountData.blockExtraJumps = true;
			MountData.totalFrames = 12;
			MountData.usesHover = true;

			this.fatigue = maxFatigue;

			int[] offsets = new int[MountData.totalFrames];
			for (int i = 0; i < offsets.Length; i++) {
				offsets[i] = 0;
			}
			MountData.playerYOffsets = offsets;

			MountData.xOffset = 1;
			MountData.yOffset = +32;

			MountData.idleFrameLoop = true;
			MountData.standingFrameCount = 1;
			MountData.standingFrameDelay = 12;
			MountData.standingFrameStart = 0;

			MountData.runningFrameCount = 4;
			MountData.runningFrameDelay = 12;
			MountData.runningFrameStart = 0;

			MountData.flyingFrameCount = 4;
			MountData.flyingFrameDelay = 12;
			MountData.flyingFrameStart = 4;

			MountData.inAirFrameCount = 4;
			MountData.inAirFrameDelay = 12;
			MountData.inAirFrameStart = 4;

			MountData.idleFrameCount = 4;
			MountData.idleFrameDelay = 12;
			MountData.idleFrameStart = 8;

			MountData.swimFrameCount = 0;
			MountData.swimFrameDelay = 12;
			MountData.swimFrameStart = 0;

			if (Main.netMode != NetmodeID.Server) {
				MountData.textureWidth = MountData.backTexture.Width();
				MountData.textureHeight = MountData.backTexture.Height();
			}
		}

		public override void UpdateEffects(Player player)
		{
			if (Main.rand.Next(10) == 0) {
				Vector2 vector2 = player.Center + new Vector2((float)(-0f * player.direction), 3f * player.gravDir);
				if (player.direction == -1) {
					vector2.X -= 0f;
				}
				int d = Dust.NewDust(vector2, 0, 20, DustID.Torch, 0f, 0f, 0, default, 1f);
				Main.dust[d].scale *= 1.2f;
				int d1 = Dust.NewDust(vector2, 0, 20, DustID.Torch, 0f, 0f, 0, default, 1f);
				Main.dust[d1].scale *= 1.2f;
				int d2 = Dust.NewDust(vector2, 0, 20, DustID.Torch, 0f, 0f, 0, default, 1f);
				Main.dust[d2].scale *= 1.2f;
			}
			MyPlayer modPlayer = player.GetSpiritPlayer();
			float tilt = player.fullRotation;

			// Do not allow the mount to be ridden in water, honey or lava.
			if (player.wet || player.honeyWet || player.lavaWet) {
				player.mount.Dismount(player);
				return;
			}

			// Only keep flying, when the player is holding up
			if ((player.controlUp || player.controlJump) && this.fatigue > 0) {
				player.mount._abilityCharging = true;
				player.mount._flyTime = 0;
				player.mount._fatigue = -verticalSpeed;
				--this.fatigue;
			}
			else {
				player.mount._abilityCharging = false;
			}

			tilt = player.velocity.X * (MathHelper.PiOver2 * 0.125f);
			tilt = (float)Math.Sin(tilt) * maxTilt;
			player.fullRotation = tilt;
			player.fullRotationOrigin = new Vector2(10f, 14f);

			// If the player is on the ground, regain fatigue.
			if (modPlayer.onGround) {
				if (player.controlUp || player.controlJump) {
					player.position.Y -= MountData.acceleration;
				}

				this.fatigue += 6;
				if (this.fatigue > maxFatigue)
					this.fatigue = maxFatigue;
			}

			player.velocity.Y = MathHelper.Clamp(player.velocity.Y, -4, 4);
		}

		public override bool UpdateFrame(Player mountedPlayer, int state, Vector2 velocity)
		{
			MyPlayer modPlayer = mountedPlayer.GetSpiritPlayer();
			Mount mount = mountedPlayer.mount;
			// Part of vanilla code, mount will glitch out
			// if this is not executed.
			if (mount._frameState != state) {
				mount._frameState = state;
			}
			//End of required vanilla code

			// Idle animation
			if (state == 0) {
				if (this.fatigue != 0f) {
					if (mount._idleTime == 0) {
						mount._idleTimeNext = mount._idleTime + 1;
					}
				}
				else {
					mount._idleTime = 0;
					mount._idleTimeNext = 2;
				}

				mount._frameCounter += 1f;
				if (mount._data.idleFrameCount != 0 && mount._idleTime >= mount._idleTimeNext) {
					float num11 = mount._data.idleFrameDelay;
					num11 *= 2f - 1f * mount._fatigue / mount._fatigueMax;
					int num12 = (int)((mount._idleTime - mount._idleTimeNext) / num11);
					if (num12 >= mount._data.idleFrameCount) {
						if (mount._data.idleFrameLoop) {
							mount._idleTime = mount._idleTimeNext;
							mount._frame = mount._data.idleFrameStart;
						}
						else {
							mount._frameCounter = 0f;
							mount._frame = mount._data.standingFrameStart;
							mount._idleTime = 0;
						}
					}
					else {
						mount._frame = mount._data.idleFrameStart + num12;
					}
					mount._frameExtra = mount._frame;
				}
				else {
					if (mount._frameCounter > mount._data.standingFrameDelay) {
						mount._frameCounter -= mount._data.standingFrameDelay;
						mount._frame++;
					}
					if (mount._frame < mount._data.standingFrameStart || mount._frame >= mount._data.standingFrameStart + mount._data.standingFrameCount) {
						mount._frame = mount._data.standingFrameStart;
					}
				}
			}
			else if (state == 1) // Running
			{

			}
			else if (state == 2) {

			}
			else if (state == 3) {

			}
			// State 4 is for when the player is wet, so we don't need to update any frames for that.

			return false;
		}
	}
}
