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

namespace SpiritMod.NPCs.Dungeon
{
    public class DungeonCubeGreen : ModNPC
    {
        bool xacc = true;
        bool yacc = false;
        bool xchase = true;
        int timer = 0;
        bool ychase = false;
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Dungeon Cube");
            Main.npcFrameCount[npc.type] = 8;
        }

        public override void SetDefaults() {
            npc.width = 36;
            npc.height = 32;
            npc.noGravity = true;
            npc.lifeMax = 150;
            npc.defense = 10;
            npc.damage = 32;

            npc.HitSound = SoundID.NPCHit2;
            npc.DeathSound = SoundID.NPCDeath6;

            npc.knockBackResist = 0f;
            npc.value = 500f;

            npc.netAlways = true;
            npc.chaseable = true;
            npc.lavaImmune = true;
        }
        public override void OnHitPlayer(Player target, int damage, bool crit) {
            target.AddBuff(BuffID.Cursed, 250, true);
        }
        public override bool PreAI() {
            timer++;
            npc.TargetClosest(true);
            Player player = Main.player[npc.target];
            if(npc.velocity == Vector2.Zero) {
                xchase = true;
                xacc = true;
                yacc = false;
                ychase = false;
                if(player.position.X > npc.position.X) {
                    npc.velocity.X = 0.1f;
                } else {
                    npc.velocity.X = -0.1f;
                }
            }
            if(xchase) {
                npc.velocity.Y = 0;
                if(Math.Abs(npc.position.X - player.position.X) > 48 && xacc && timer < 200) {
                    if(xacc && npc.velocity.X < 15) {
                        npc.velocity.X *= 1.06f;
                    }
                } else {
                    xacc = false;
                    timer = 0;
                    npc.velocity.X *= 0.94f;
                    if(Math.Abs(npc.velocity.X) < 0.1f) {
                        yacc = true;
                        ychase = true;

                        if(player.position.Y > npc.position.Y) {
                            npc.velocity.Y = 0.1f;
                        } else {
                            npc.velocity.Y = -0.1f;
                        }
                        xchase = false;
                    }
                }
                if(npc.velocity.X == 0) {
                    yacc = true;
                    ychase = true;

                    if(player.position.Y > npc.position.Y) {
                        npc.velocity.Y = 0.1f;
                    } else {
                        npc.velocity.Y = -0.1f;
                    }
                    timer = 0;
                    xchase = false;
                }
            }

            if(ychase) {
                npc.velocity.X = 0;
                if(Math.Abs(npc.position.Y - player.position.Y) > 48 && yacc && timer < 200) {
                    if(yacc && npc.velocity.Y < 15) {
                        npc.velocity.Y *= 1.06f;
                    }
                } else {
                    yacc = false;
                    timer = 0;
                    npc.velocity.Y *= 0.94f;
                    if(Math.Abs(npc.velocity.Y) < 0.1f) {
                        xacc = true;
                        xchase = true;

                        if(player.position.X > npc.position.X) {
                            npc.velocity.X = 0.1f;
                        } else {
                            npc.velocity.X = -0.1f;
                        }
                        ychase = false;
                    }
                }
                if(npc.velocity.Y == 0) {
                    xacc = true;
                    xchase = true;

                    if(player.position.X > npc.position.X) {
                        npc.velocity.X = 0.1f;
                    } else {
                        npc.velocity.X = -0.1f;
                    }
                    timer = 0;
                    ychase = false;
                }
            }
            return false;
        }



        public override void FindFrame(int frameHeight) {
            npc.frameCounter += 0.15f;
            npc.frameCounter %= 6;
            int frame = (int)npc.frameCounter;
            if(xchase) {
                npc.frame.Y = (int)(MathHelper.Clamp(Math.Abs(npc.velocity.X * 3), 0, 6)) * frameHeight;
            } else {
                npc.frame.Y = (int)(MathHelper.Clamp(Math.Abs(npc.velocity.Y * 3), 0, 6)) * frameHeight;
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo) {
            if(spawnInfo.spawnTileType == TileID.GreenDungeonBrick) {
                return spawnInfo.player.ZoneDungeon ? 0.04f : 0f;
            }
            return 0f;
        }

        public override void NPCLoot() {

            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, 327);
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, 137, Main.rand.Next(4));
        }
        public override void HitEffect(int hitDirection, double damage) {
            if(npc.life <= 0) {
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/DungeonCubeGreenGore1"));
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/DungeonCubeGreenGore2"));
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/DungeonCubeGreenGore3"));
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/DungeonCubeGreenGore4"));
            }
        }

    }
}