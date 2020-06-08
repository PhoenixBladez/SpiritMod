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
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs
{
    public class SamuraiHostile : ModNPC
    {
        int chargeTime = 200; //how many frames between charges
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Phantom Samurai");
            Main.npcFrameCount[npc.type] = 9;
            NPCID.Sets.TownCritter[npc.type] = true;
            NPCID.Sets.TrailCacheLength[npc.type] = 3;
            NPCID.Sets.TrailingMode[npc.type] = 0;
        }

        public override void SetDefaults() {
            if(NPC.downedBoss2) {
                npc.width = 30;
                npc.height = 40;
                npc.damage = 35;
                npc.noGravity = true;
                npc.defense = 8;
                npc.lifeMax = 130;
            } else if(NPC.downedBoss3) {
                npc.width = 30;
                npc.height = 40;
                npc.damage = 38;
                npc.noGravity = true;
                npc.defense = 11;
                npc.lifeMax = 180;
            } else {
                npc.width = 30;
                npc.height = 40;
                npc.damage = 33;
                npc.noGravity = true;
                npc.defense = 4;
                npc.lifeMax = 90;
            }
            npc.HitSound = SoundID.NPCHit3;
            npc.DeathSound = SoundID.NPCDeath6;
            npc.value = 120f;
            npc.knockBackResist = .1f;
            npc.noTileCollide = true;
        }
        float frameCounter = 0;
        public override void FindFrame(int frameHeight) {
            frameCounter += 0.15f;
            frameCounter %= 3f;
            if(chargetimer == chargeTime) {
                frameCounter = 0;
            }
            if(charging) {
                npc.frameCounter = frameCounter + 6;
            } else if(chargetimer > chargeTime - 50) {
                npc.frameCounter = frameCounter + 3;
            } else {
                npc.frameCounter = frameCounter;
            }
            if(npc.frameCounter == 8) {
                charging = false;
            }
            int frame = (int)npc.frameCounter;
            npc.frame.Y = frame * frameHeight;
        }

        public override void HitEffect(int hitDirection, double damage) {
            int d1 = 54;
            for(int k = 0; k < 20; k++) {
                Dust.NewDust(npc.position, npc.width, npc.height, d1, 2.5f * hitDirection, -2.5f, 54, new Color(0, 255, 142), .6f);
            }
            if(npc.life <= 0) {
                Gore.NewGore(npc.position, npc.velocity, 99);
                Gore.NewGore(npc.position, npc.velocity, 99);
                Gore.NewGore(npc.position, npc.velocity, 99);
                for(int i = 0; i < 40; i++) {
                    int num = Dust.NewDust(npc.position, npc.width, npc.height, 54, 0f, -2f, 117, new Color(0, 255, 142), .6f);
                    Main.dust[num].noGravity = true;
                    Dust expr_62_cp_0 = Main.dust[num];
                    expr_62_cp_0.position.X = expr_62_cp_0.position.X + ((float)(Main.rand.Next(-50, 51) / 20) - 1.5f);
                    Dust expr_92_cp_0 = Main.dust[num];
                    expr_92_cp_0.position.Y = expr_92_cp_0.position.Y + ((float)(Main.rand.Next(-50, 51) / 20) - 1.5f);
                    if(Main.dust[num].position != npc.Center) {
                        Main.dust[num].velocity = npc.DirectionTo(Main.dust[num].position) * 6f;
                    }
                }
            }
        }

        private static int[] SpawnTiles = { };
        int chargetimer = 0;
        bool charging = false;
        Vector2 targetLocation = Vector2.Zero;
        float chargeRotation = 0;
        public override void AI() {
            float num395 = Main.mouseTextColor / 200f - 0.35f;
            num395 *= 0.14f;
            npc.scale = num395 + 0.95f;
            float velMax = 1f;
            float acceleration = 0.011f;
            npc.TargetClosest(true);
            Vector2 center = npc.Center;
            float deltaX = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) - center.X;
            float deltaY = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2) - center.Y;
            float distance = (float)Math.Sqrt((double)deltaX * (double)deltaX + (double)deltaY * (double)deltaY);
            npc.ai[1] += 1f;
            if(!charging) {
                if((double)npc.ai[1] > 600.0) {
                    acceleration *= 8f;
                    velMax = 4f;
                    if((double)npc.ai[1] > 650.0) {
                        npc.ai[1] = 0f;
                    }
                } else if((double)distance < 250.0) {
                    npc.ai[0] += 0.9f;
                    if(npc.ai[0] > 0f) {
                        npc.velocity.Y = npc.velocity.Y + 0.019f;
                    } else {
                        npc.velocity.Y = npc.velocity.Y - 0.019f;
                    }
                    if(npc.ai[0] < -100f || npc.ai[0] > 100f) {
                        npc.velocity.X = npc.velocity.X + 0.019f;
                    } else {
                        npc.velocity.X = npc.velocity.X - 0.019f;
                    }
                    if(npc.ai[0] > 200f) {
                        npc.ai[0] = -200f;
                    }
                }
                if((double)distance > 350.0) {
                    velMax = 5f;
                    acceleration = 0.3f;
                } else if((double)distance > 300.0) {
                    velMax = 3f;
                    acceleration = 0.2f;
                } else if((double)distance > 250.0) {
                    velMax = 1.5f;
                    acceleration = 0.1f;
                }
                float stepRatio = velMax / distance;
                float velLimitX = deltaX * stepRatio;
                float velLimitY = deltaY * stepRatio;
                if(Main.player[npc.target].dead) {
                    velLimitX = (float)((double)((float)npc.direction * velMax) / 2.0);
                    velLimitY = (float)((double)(-(double)velMax) / 2.0);
                }
                if(npc.velocity.X < velLimitX) {
                    npc.velocity.X = npc.velocity.X + acceleration;
                } else if(npc.velocity.X > velLimitX) {
                    npc.velocity.X = npc.velocity.X - acceleration;
                }
                if(npc.velocity.Y < velLimitY) {
                    npc.velocity.Y = npc.velocity.Y + acceleration;
                } else if(npc.velocity.Y > velLimitY) {
                    npc.velocity.Y = npc.velocity.Y - acceleration;
                }
                if((double)velLimitX > 0.0) {
                    npc.rotation = (float)Math.Atan2((double)velLimitY, (double)velLimitX);
                }
                if((double)velLimitX < 0.0) {
                    npc.rotation = (float)Math.Atan2((double)velLimitY, (double)velLimitX) + 3.14f;
                }
                if(chargetimer < chargeTime - 50) {
                    npc.rotation = 0;
                } else if(chargetimer > chargeTime - 40) {
                    npc.rotation = chargeRotation;
                }
                if(chargetimer > chargeTime - 50 && chargetimer < chargeTime - 40) {
                    targetLocation = Main.player[npc.target].Center;
                    chargeRotation = npc.rotation;
                }
            }
            chargetimer++;
            if(chargetimer == chargeTime) {
                Vector2 direction = targetLocation - npc.Center;
                direction.Normalize();
                direction.X = direction.X * Main.rand.Next(15, 19);
                direction.Y = direction.Y * Main.rand.Next(9, 10);
                npc.velocity.X = direction.X;
                npc.velocity.Y = direction.Y;
                npc.velocity.Y *= 0.95f;
                npc.velocity.X *= 0.95f;
                for(int i = 0; i < 20; i++) {
                    int num = Dust.NewDust(npc.position, npc.width, npc.height, 14, 0f, -2f, 0, default(Color), .8f);
                    Main.dust[num].noGravity = true;
                    Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
                    Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
                    if(Main.dust[num].position != npc.Center)
                        Main.dust[num].velocity = npc.DirectionTo(Main.dust[num].position) * 6f;
                }
                charging = true;
            }
            if(chargetimer >= chargeTime + 20) {
                chargetimer = 0;
                charging = false;
            }
            npc.spriteDirection = npc.direction;
        }
    }
}
