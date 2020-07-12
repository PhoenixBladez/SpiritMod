using SpiritMod.Buffs.Mount;
using System;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Mounts
{
	public class TideMount : ModMountData
	{
				private const float damage = 15f;
		private const float knockback = 2f;

		public override void SetDefaults()
		{
			mountData.spawnDust = 167;
			mountData.spawnDustNoGravity = true;
			mountData.buff = ModContent.BuffType<TideMountBuff>();
			mountData.heightBoost = 16;
			mountData.fallDamage = 0f;
			mountData.runSpeed = 9f;
			mountData.dashSpeed = 3f;
			mountData.flightTimeMax = 0;
			mountData.fatigueMax = 0;
			mountData.jumpHeight = 8;
			mountData.acceleration = 0.05f;
			mountData.jumpSpeed = 5f;
			mountData.blockExtraJumps = true;
			mountData.totalFrames = 11;
			mountData.constantJump = false;
			int[] array = new int[mountData.totalFrames];
			for(int i = 0; i < array.Length; i++) {
				if(i == 1) {
					array[i] = 24;
				} else if(i == 3 || i == 4 || i == 5) {
					array[i] = 18;
				} else {
					array[i] = 20;
				}
			}
			mountData.playerYOffsets = array;
			mountData.yOffset = -4;
			mountData.xOffset = +1;
			mountData.bodyFrame = 3;
			mountData.playerHeadOffset = 22;
			mountData.standingFrameCount = 1;
			mountData.standingFrameDelay = 12;
			mountData.standingFrameStart = 0;
			mountData.runningFrameCount = 6;
			mountData.runningFrameDelay = 20;
			mountData.runningFrameStart = 0;
			mountData.flyingFrameCount = 1;
			mountData.flyingFrameDelay = 0;
			mountData.flyingFrameStart = 1;
			mountData.inAirFrameCount = 1;
			mountData.inAirFrameDelay = 12;
			mountData.inAirFrameStart = 3;
			mountData.idleFrameCount = 1;
			mountData.idleFrameDelay = 12;
			mountData.idleFrameStart = 0;
			mountData.idleFrameLoop = true;
			mountData.swimFrameCount = mountData.inAirFrameCount;
			mountData.swimFrameDelay = mountData.inAirFrameDelay;
			mountData.swimFrameStart = mountData.inAirFrameStart;
			if(Main.netMode != NetmodeID.Server) {
				mountData.textureWidth = mountData.backTexture.Width;
				mountData.textureHeight = mountData.backTexture.Height;
			}
		}
        public bool atkFrames;
        public bool attack;
        public override void UpdateEffects(Player player)
        {
            if (Math.Abs(player.velocity.X) > player.mount.DashSpeed - player.mount.RunSpeed / 2f)
            {
                player.noKnockback = true;
            }
            for (int i = 0; i < 200; i++)
            {
                if (Main.npc[i].active && !Main.npc[i].friendly && Main.npc[i].type != NPCID.TargetDummy)
                {
                    int distance = (int)Main.npc[i].Distance(player.Center);
                    if (player.velocity == Vector2.Zero)
                    {
                        if (distance < 80)
                        {
                            atkFrames = true;
                            if (player.mount._frame == 10 && !attack)
                            {
                                attack = true;
                                Main.npc[i].StrikeNPC(30, 2f, 0, false);
                            }

                        }
						/*if (((distance.X > 0 && player.direction == 1) || (distance.X < 0 && player.direction == -1)) && atkFrames)
                        {
                            Main.npc[i].StrikeNPC(30, 0f, 0, false);
                        }*/
                    }
                    if (distance > 85)
                    {
                        atkFrames = false;
                    }
                }
            }
			if (player.velocity != Vector2.Zero)
            {
                atkFrames = false;
            }
        }
		public override bool UpdateFrame(Player mountedPlayer, int state, Vector2 velocity)
        {
			if (atkFrames)
            {
                mountedPlayer.mount._frameCounter++;
				if (mountedPlayer.mount._frameCounter >= 8)
                {
                    mountedPlayer.mount._frameCounter = 0;
                    mountedPlayer.mount._frame++;
                }
                if (mountedPlayer.mount._frame < 7 || mountedPlayer.mount._frame > 10)
                {
                    mountedPlayer.mount._frame = 7;
                    attack = false;
                }
                return false;
            }
            return true;
        }

    }
}

