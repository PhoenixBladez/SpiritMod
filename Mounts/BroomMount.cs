using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Mounts
{
	public class BroomMount : ModMountData
	{
		public static int _type;

		public const float groundSlowdown = 0.3f;

		public int fatigue;
		public const int maxFatigue = 1400;
		public const float verticalSpeed = 0.64f;
		private const float maxTilt = (float)Math.PI * 0.04f;

		public override void SetDefaults()
		{
			mountData.spawnDust = 173;
			mountData.spawnDustNoGravity = true;
			mountData.buff = Buffs.Mount.BroomBuff._type;
			mountData.heightBoost = 2;
			mountData.flightTimeMax = 60;
			mountData.fatigueMax = 120;
			mountData.fallDamage = 0f;
			mountData.runSpeed = 6;
			mountData.dashSpeed = 2f;
			mountData.acceleration = 0.2F;
			mountData.blockExtraJumps = true;
			mountData.totalFrames = 12;
			mountData.usesHover = true;

			this.fatigue = maxFatigue;

			int[] offsets = new int[mountData.totalFrames];
			for (int i = 0; i < offsets.Length; i++)
			{
				offsets[i] = 0;
			}
			mountData.playerYOffsets = offsets;

			mountData.xOffset = -5;
			mountData.yOffset = +15;

			mountData.idleFrameLoop = true;
			mountData.standingFrameCount = 1;
			mountData.standingFrameDelay = 12;
			mountData.standingFrameStart = 0;

			mountData.runningFrameCount = 4;
			mountData.runningFrameDelay = 12;
			mountData.runningFrameStart = 0;

			mountData.flyingFrameCount = 4;
			mountData.flyingFrameDelay = 12;
			mountData.flyingFrameStart = 4;

			mountData.inAirFrameCount = 4;
			mountData.inAirFrameDelay = 12;
			mountData.inAirFrameStart = 4;

			mountData.idleFrameCount = 4;
			mountData.idleFrameDelay = 12;
			mountData.idleFrameStart = 8;

			mountData.swimFrameCount = 0;
			mountData.swimFrameDelay = 12;
			mountData.swimFrameStart = 0;

			if (Main.netMode != 2)
			{
				mountData.textureWidth = mountData.backTexture.Width;
				mountData.textureHeight = mountData.backTexture.Height;
			}
		}

		public override void UpdateEffects(Player player)
		{
			MyPlayer modPlayer = player.GetModPlayer<MyPlayer>(mod);
			float tilt = player.fullRotation;

			// Do not allow the mount to be ridden in water, honey or lava.

			// Only keep flying, when the player is holding up
			if ((player.controlUp || player.controlJump) && this.fatigue > 0)
			{
				player.mount._abilityCharging = true;
				player.mount._flyTime = 0;
				player.mount._fatigue = -verticalSpeed;
				--this.fatigue;
			}
			else
			{
				player.mount._abilityCharging = false;
			}

			tilt = player.velocity.X * (MathHelper.PiOver2 * 0.125f);
			tilt = (float)Math.Sin(tilt) * maxTilt;
			player.fullRotation = tilt;
			player.fullRotationOrigin = new Vector2(10f, 14f);

			// If the player is on the ground, regain fatigue.
			if (modPlayer.onGround)
			{
				if (player.controlUp || player.controlJump)
				{
					player.position.Y -= mountData.acceleration;
				}

				this.fatigue += 6;
				if (this.fatigue > maxFatigue)
					this.fatigue = maxFatigue;
			}

			player.velocity.Y = MathHelper.Clamp(player.velocity.Y, -4, 4);
		}

		public override bool UpdateFrame(Player mountedPlayer, int state, Vector2 velocity)
		{
			MyPlayer modPlayer = mountedPlayer.GetModPlayer<MyPlayer>(mod);
			Mount mount = mountedPlayer.mount;
			// Part of vanilla code, mount will glitch out
			// if this is not executed.
			if (mount._frameState != state)
			{
				mount._frameState = state;
			}
			//End of required vanilla code

			// Idle animation
			if (state == 0)
			{
				if (this.fatigue != 0f)
				{
					if (mount._idleTime == 0)
					{
						mount._idleTimeNext = mount._idleTime + 1;
					}
				}
				else
				{
					mount._idleTime = 0;
					mount._idleTimeNext = 2;
				}

				mount._frameCounter += 1f;
				if (mount._data.idleFrameCount != 0 && mount._idleTime >= mount._idleTimeNext)
				{
					float num11 = mount._data.idleFrameDelay;
					num11 *= 2f - 1f * mount._fatigue / mount._fatigueMax;
					int num12 = (int)((mount._idleTime - mount._idleTimeNext) / num11);
					if (num12 >= mount._data.idleFrameCount)
					{
						if (mount._data.idleFrameLoop)
						{
							mount._idleTime = mount._idleTimeNext;
							mount._frame = mount._data.idleFrameStart;
						}
						else
						{
							mount._frameCounter = 0f;
							mount._frame = mount._data.standingFrameStart;
							mount._idleTime = 0;
						}
					}
					else
					{
						mount._frame = mount._data.idleFrameStart + num12;
					}
					mount._frameExtra = mount._frame;
				}
				else
				{
					if (mount._frameCounter > mount._data.standingFrameDelay)
					{
						mount._frameCounter -= mount._data.standingFrameDelay;
						mount._frame++;
					}
					if (mount._frame < mount._data.standingFrameStart || mount._frame >= mount._data.standingFrameStart + mount._data.standingFrameCount)
					{
						mount._frame = mount._data.standingFrameStart;
					}
				}
			}
			else if (state == 1) // Running
			{

			}
			else if (state == 2)
			{

			}
			else if (state == 3)
			{

			}
			// State 4 is for when the player is wet, so we don't need to update any frames for that.

			return false;
		}
	}
}
