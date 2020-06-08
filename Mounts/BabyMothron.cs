using Microsoft.Xna.Framework;
using SpiritMod.Tiles.Furniture.Reach;
using SpiritMod.NPCs.Critters;
using SpiritMod.Mounts;
using SpiritMod.NPCs.Boss.SpiritCore;
using SpiritMod.Boss.SpiritCore;
using SpiritMod.Buffs.Candy;
using SpiritMod.Buffs.Potion;
using SpiritMod.Projectiles.Pet;
using SpiritMod.Buffs.Pet;
using SpiritMod.Projectiles.Arrow.Artifact;
using SpiritMod.Projectiles.Bullet.Crimbine;
using SpiritMod.Projectiles.Bullet;
using SpiritMod.Projectiles.Magic.Artifact;
using SpiritMod.Projectiles.Summon.Artifact;
using SpiritMod.Projectiles.Summon.LaserGate;
using SpiritMod.Projectiles.Flail;
using SpiritMod.Projectiles.Arrow;
using SpiritMod.Projectiles.Magic;
using SpiritMod.Projectiles.Sword.Artifact;
using SpiritMod.Projectiles.Summon.Dragon;
using SpiritMod.Projectiles.Sword;
using SpiritMod.Projectiles.Thrown.Artifact;
using SpiritMod.Items.Boss;
using SpiritMod.Items.Armor.Masks;
using SpiritMod.Projectiles.Returning;
using SpiritMod.Projectiles.Held;
using SpiritMod.Projectiles.Thrown;
using SpiritMod.Items.Equipment;
using SpiritMod.Projectiles.DonatorItems;
using SpiritMod.Buffs.Mount;
using SpiritMod.Items.Weapon.Yoyo;
using SpiritMod.Projectiles.Yoyo;
using SpiritMod.Items.Weapon.Spear;
using SpiritMod.Items.Weapon.Swung;
using SpiritMod.NPCs.Boss;
using SpiritMod.Items.Material;
using SpiritMod.Items.Pets;
using SpiritMod.Items.Weapon.Summon;
using SpiritMod.Projectiles.Boss;
using SpiritMod.Items.BossBags;
using SpiritMod.Items.Consumable.Fish;
using SpiritMod.Buffs.Summon;
using SpiritMod.Projectiles.Summon;
using SpiritMod.NPCs.Spirit;
using SpiritMod.Items.Consumable;
using SpiritMod.Tiles.Block;
using SpiritMod.Items.Placeable.Furniture;
using SpiritMod.Items.Consumable.Quest;
using SpiritMod.Items.Consumable.Potion;
using SpiritMod.Items.Placeable.IceSculpture;
using SpiritMod.Items.Weapon.Bow;
using SpiritMod.Items.Weapon.Gun;
using SpiritMod.Buffs;
using SpiritMod.Items;
using SpiritMod.Items.Weapon;
using SpiritMod.Items.Weapon.Returning;
using SpiritMod.Items.Weapon.Thrown;
using SpiritMod.Items.Material;
using SpiritMod.Items.Weapon.Magic;
using SpiritMod.Items.Accessory;

using SpiritMod.Items.Accessory.Leather;
using SpiritMod.Items.Ammo;
using SpiritMod.Items.Armor;
using SpiritMod.Dusts;
using SpiritMod.Buffs;
using SpiritMod.Buffs.Artifact;
using SpiritMod.NPCs;
using SpiritMod.NPCs.Asteroid;
using SpiritMod.Projectiles;
using SpiritMod.Projectiles.Hostile;
using SpiritMod.Tiles;
using SpiritMod.Tiles.Ambient;
using SpiritMod.Tiles.Ambient.IceSculpture;
using SpiritMod.Tiles.Ambient.ReachGrass;
using SpiritMod.Tiles.Ambient.ReachMicros;
using System;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Mounts
{
    public class BabyMothron : ModMountData
    {
        public const float groundSlowdown = 0.3f;

        public int fatigue;
        public const int maxFatigue = 1200;
        public const float verticalSpeed = 0.34f;
        private const float maxTilt = (float)Math.PI * 0.08f;

        public override void SetDefaults() {
            mountData.spawnDust = 226;
            mountData.spawnDustNoGravity = true;
            mountData.buff = ModContent.BuffType<BabyMothronBuff>();
            mountData.heightBoost = 2;
            mountData.flightTimeMax = 320;
            mountData.fatigueMax = 320;
            mountData.fallDamage = 0f;
            mountData.runSpeed = 6;
            mountData.dashSpeed = 2f;
            mountData.acceleration = 0.2F;
            mountData.blockExtraJumps = true;
            mountData.totalFrames = 12;
            mountData.usesHover = true;

            this.fatigue = maxFatigue;

            int[] offsets = new int[mountData.totalFrames];
            for(int i = 0; i < offsets.Length; i++) {
                offsets[i] = 0;
            }
            mountData.playerYOffsets = offsets;

            mountData.xOffset = 6;
            mountData.yOffset = -12;

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

            if(Main.netMode != 2) {
                mountData.textureWidth = mountData.backTexture.Width + 20;
                mountData.textureHeight = mountData.backTexture.Height;
            }
        }

        public override void UpdateEffects(Player player) {
            MyPlayer modPlayer = player.GetSpiritPlayer();
            float tilt = player.fullRotation;

            // Do not allow the mount to be ridden in water, honey or lava.
            if(player.wet || player.honeyWet || player.lavaWet) {
                player.mount.Dismount(player);
                return;
            }

            // Only keep flying, when the player is holding up
            if((player.controlUp || player.controlJump) && this.fatigue > 0) {
                player.mount._abilityCharging = true;
                player.mount._flyTime = 0;
                player.mount._fatigue = -verticalSpeed;
                --this.fatigue;
            } else {
                player.mount._abilityCharging = false;
            }

            tilt = player.velocity.X * (MathHelper.PiOver2 * 0.125f);
            tilt = (float)Math.Sin(tilt) * maxTilt;
            player.fullRotation = tilt;
            player.fullRotationOrigin = new Vector2(10f, 14f);

            // If the player is on the ground, regain fatigue.
            if(modPlayer.onGround) {
                if(player.controlUp || player.controlJump) {
                    player.position.Y -= mountData.acceleration;
                }

                this.fatigue += 6;
                if(this.fatigue > maxFatigue)
                    this.fatigue = maxFatigue;
            }

            player.velocity.Y = MathHelper.Clamp(player.velocity.Y, -4, 4);
        }

        public override bool UpdateFrame(Player mountedPlayer, int state, Vector2 velocity) {
            MyPlayer modPlayer = mountedPlayer.GetSpiritPlayer();
            Mount mount = mountedPlayer.mount;
            // Part of vanilla code, mount will glitch out
            // if this is not executed.
            if(mount._frameState != state) {
                mount._frameState = state;
            }
            //End of required vanilla code

            // Idle animation
            if(state == 0) {
                if(this.fatigue != 0f) {
                    if(mount._idleTime == 0) {
                        mount._idleTimeNext = mount._idleTime + 1;
                    }
                } else {
                    mount._idleTime = 0;
                    mount._idleTimeNext = 2;
                }

                mount._frameCounter += 1f;
                if(mount._data.idleFrameCount != 0 && mount._idleTime >= mount._idleTimeNext) {
                    float num11 = mount._data.idleFrameDelay;
                    num11 *= 2f - 1f * mount._fatigue / mount._fatigueMax;
                    int num12 = (int)((mount._idleTime - mount._idleTimeNext) / num11);
                    if(num12 >= mount._data.idleFrameCount) {
                        if(mount._data.idleFrameLoop) {
                            mount._idleTime = mount._idleTimeNext;
                            mount._frame = mount._data.idleFrameStart;
                        } else {
                            mount._frameCounter = 0f;
                            mount._frame = mount._data.standingFrameStart;
                            mount._idleTime = 0;
                        }
                    } else {
                        mount._frame = mount._data.idleFrameStart + num12;
                    }
                    mount._frameExtra = mount._frame;
                } else {
                    if(mount._frameCounter > mount._data.standingFrameDelay) {
                        mount._frameCounter -= mount._data.standingFrameDelay;
                        mount._frame++;
                    }
                    if(mount._frame < mount._data.standingFrameStart || mount._frame >= mount._data.standingFrameStart + mount._data.standingFrameCount) {
                        mount._frame = mount._data.standingFrameStart;
                    }
                }
            } else if(state == 1) // Running
              {

            } else if(state == 2) {

            } else if(state == 3) {

            }
            // State 4 is for when the player is wet, so we don't need to update any frames for that.

            return false;
        }
    }
}
