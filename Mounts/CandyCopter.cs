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
    class CandyCopter : ModMountData
    {
        public static ModMountData _ref;

        private const float damage = 60f;
        private const float knockback = 1.5f;
        private const float velocity = 10f; //Changing this will also affect the accuracy. Higher velocity -> higher accuracy
                                            //Setting the velocity to 5 or lower will send bullets everywhere!
        private const int cooldown = 10;
        public const float verticalSpeed = 0.34f; //This is not linearly related to vertical speed.
                                                  //Keep in range -1f (no speed) to 1.0f (capped at acceleration)
        private const float fov = (float)Math.PI * 0.333333333f; //60 degrees
        private const float aimRange = 640; //40 blocks
        public const float groundSlowdown = 0.3f;

        private static readonly Vector3 lightColor = new Vector3(0.5294f, 0.7137f, 0.8745f);
        public const int revUpFrames = 60;
        public const int muzzleFrames = 3;
        private const float maxTilt = (float)Math.PI * 0.08f;
        private const float rangeSquare = aimRange * aimRange; //Just for performance reasons

        private FOVHelper helper = new FOVHelper();

        public override void SetDefaults() {
            mountData.spawnDust = 226;
            mountData.spawnDustNoGravity = true;
            mountData.buff = ModContent.BuffType<RidingCandyCopter>();
            mountData.flightTimeMax = 2;
            mountData.fatigueMax = 1;
            mountData.fallDamage = 0f;
            mountData.runSpeed = 8f;
            mountData.dashSpeed = 8f;
            mountData.acceleration = 0.16f;
            mountData.usesHover = true;
            mountData.jumpHeight = 0;
            mountData.jumpSpeed = 0f;
            mountData.blockExtraJumps = true;
            mountData.abilityChargeMax = 10;
            mountData.bodyFrame = 3;
            //Caution: changing these offsets will mess up the muzzle position!
            mountData.heightBoost = 0;
            mountData.xOffset = 3;
            mountData.yOffset = -5;
            mountData.playerHeadOffset = 16;
            mountData.totalFrames = 12;
            int[] offsets = new int[mountData.totalFrames];
            for(int i = 0; i < offsets.Length; i++) {
                offsets[i] = 0;
            }
            mountData.playerYOffsets = offsets;
            mountData.standingFrameCount = 1;
            mountData.standingFrameDelay = 12;
            mountData.standingFrameStart = 0;
            mountData.runningFrameCount = 0;
            mountData.runningFrameDelay = 12;
            mountData.runningFrameStart = 0;
            mountData.dashingFrameCount = 0;
            mountData.dashingFrameDelay = 12;
            mountData.dashingFrameStart = 0;
            mountData.flyingFrameCount = 4;
            mountData.flyingFrameDelay = 12;
            mountData.flyingFrameStart = 0;
            mountData.inAirFrameCount = 4;
            mountData.inAirFrameDelay = 4;
            mountData.inAirFrameStart = 0;
            mountData.idleFrameCount = 0;
            mountData.idleFrameDelay = 12;
            mountData.idleFrameStart = 0;
            mountData.idleFrameLoop = true;
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
            modPlayer.copterBrake = false;
            float tilt = player.fullRotation;

            if(player.wet || player.honeyWet || player.lavaWet) {
                player.mount.Dismount(player);
                return;
            }

            //Only keep flying, when the player is holding up
            if(player.controlUp || player.controlJump) {
                player.mount._abilityCharging = true;
                player.mount._flyTime = 0;
                player.mount._fatigue = -verticalSpeed;
            } else {
                player.mount._abilityCharging = false;
            }

            //Slow down the helicopter, if it's on the ground
            if(modPlayer.onGround) {
                if(player.controlUp || player.controlJump) {
                    player.position.Y -= mountData.acceleration;
                } else {
                    modPlayer.copterBrake = true;
                }
            }

            //tilt the helicopter
            tilt = player.velocity.X * (MathHelper.PiOver2 * 0.125f);
            tilt = (float)Math.Sin(tilt) * maxTilt;
            player.fullRotation = tilt;
            player.fullRotationOrigin = new Vector2(10f, 14f);//If you change this, also change the x and y values below. 


            //Scan for enemies and fire at them
            if(player.mount._abilityCooldown == 0) {
                //Don't change these values, unless you want to adjust the nozzle position.
                float x = 20f * player.direction;
                float y = 12f;
                float sin = (float)Math.Sin(tilt);
                float cos = (float)Math.Cos(tilt);
                Vector2 muzzle = new Vector2(x * cos - y * sin, x * sin + y * cos);
                muzzle = muzzle + player.fullRotationOrigin + player.position;

                //Readjust the scanning cone to the current position.
                float direction;
                if(player.direction == 1) {
                    direction = FOVHelper.POS_X_DIR + tilt;
                } else {
                    direction = FOVHelper.NEG_X_DIR - tilt;
                }
                helper.AdjustCone(muzzle, fov, direction);

                //Look for the nearest, unobscured enemy inside the cone
                NPC nearest = null;
                float distNearest = rangeSquare;
                for(int i = 0; i < 200; i++) {
                    NPC npc = Main.npc[i];
                    Vector2 npcCenter = npc.Center;
                    if(npc.CanBeChasedBy() && helper.IsInCone(npcCenter)) //first param of canBeChasedBy has no effect
                    {
                        float distCurrent = Vector2.DistanceSquared(muzzle, npcCenter);
                        if(distCurrent < distNearest && Collision.CanHitLine(muzzle, 0, 0, npc.position, npc.width, npc.height)) {
                            nearest = npc;
                            distNearest = distCurrent;
                        }
                    }
                }
                //Shoot 'em dead
                if(nearest != null) {
                    if(player.whoAmI == Main.myPlayer) {
                        Vector2 aim = nearest.Center - muzzle;
                        aim.Normalize();
                        aim *= velocity;
                        float vX = aim.X;
                        float vY = aim.Y;
                        //This precisely mimics the Gatligators spread
                        //Random rand = Main.rand; commenting this out because I changed the rand lines here and waiting on more experienced help- Jenosis
                        vX += Main.rand.Next(-50, 51) * 0.03f;
                        vY += Main.rand.Next(-50, 51) * 0.03f;
                        vX += Main.rand.Next(-40, 41) * 0.05f;
                        vY += Main.rand.Next(-40, 41) * 0.05f;
                        if(Main.rand.Next(3) == 0) {
                            vX += Main.rand.Next(-30, 31) * 0.02f;
                            vY += Main.rand.Next(-30, 31) * 0.02f;
                        }
                        Terraria.Projectile.NewProjectile(muzzle.X, muzzle.Y, vX, vY, ModContent.ProjectileType<CandyCopterBullet>(), (int)(damage * player.minionDamage), knockback * player.minionKB, player.whoAmI); //CandyCopterBullet
                    }
                    Point point = player.Center.ToTileCoordinates();
                    Lighting.AddLight(point.X, point.Y, lightColor.X, lightColor.Y, lightColor.Z);
                    modPlayer.copterFiring = true;
                    modPlayer.copterFireFrame = 0;
                    player.mount._abilityCooldown = cooldown - 1;
                } else {
                    modPlayer.copterFiring = false;
                }
                return;
            } else if(player.mount._abilityCooldown > cooldown) {
                player.mount._abilityCooldown = cooldown;
            }
        }

        public override bool UpdateFrame(Player mountedPlayer, int state, Vector2 velocity) {
            MyPlayer modPlayer = mountedPlayer.GetSpiritPlayer();
            Terraria.Mount mount = mountedPlayer.mount;
            //Part of vanilla code, mount will glitch out
            // if this is not executed.
            if(mount._frameState != state) {
                mount._frameState = state;
            }
            //End of required vanilla code

            //Adjust animation speed after landing/takeoff
            if(state == 0) {
                if(mount._idleTime > 0) {
                    mount._idleTime--;
                }
            } else {
                if(mount._idleTime < revUpFrames) {
                    mount._idleTime += 4;
                }
            }
            float throttle = 1;
            if(mount._idleTime < revUpFrames) {
                throttle = (mount._idleTime) / (float)revUpFrames;
            }

            //Choose next frame
            mount._frameCounter += throttle;
            if((int)mount._frameCounter >= mountData.inAirFrameDelay) {
                mount._frameCounter -= mountData.inAirFrameDelay;
                mount._frameExtra++;
                if(mount._frameExtra >= mountData.inAirFrameCount) {
                    mount._frameExtra = 0;
                }
            }

            mount._frame = mount._frameExtra;
            if(modPlayer.copterFireFrame >= muzzleFrames) {
                mount._frame = mount._frameExtra;
            } else if(modPlayer.copterFireFrame >= muzzleFrames >> 1) {
                mount._frame = mount._frameExtra + mountData.inAirFrameCount;
            } else {
                mount._frame = mount._frameExtra + 2 * mountData.inAirFrameCount;
            }

            return false;
        }
    }
}