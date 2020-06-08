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
using SpiritMod.Items.Material;
using SpiritMod.Items.Weapon.Bow;
using SpiritMod.Items.Weapon.Magic;
using SpiritMod.Projectiles;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Tide.NPCs
{
    public class MurkTerror : ModNPC
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Murky Terror");
            Main.npcFrameCount[npc.type] = 6;
        }

        public override void SetDefaults() {

            npc.damage = 34;
            npc.width = 66; //324
            npc.height = 54; //216
            npc.defense = 15;
            npc.lifeMax = 160;
            npc.knockBackResist = 0.45f;
            npc.noGravity = true;
            npc.HitSound = SoundID.NPCHit25;
            npc.DeathSound = SoundID.NPCDeath5;
        }

        public override void AI() {
            npc.TargetClosest(true);
            float num1164 = 4f;
            float num1165 = 0.75f;
            Vector2 vector133 = new Vector2(npc.Center.X, npc.Center.Y);
            float num1166 = Main.player[npc.target].Center.X - vector133.X;
            float num1167 = Main.player[npc.target].Center.Y - vector133.Y - 200f;
            float num1168 = (float)Math.Sqrt((double)(num1166 * num1166 + num1167 * num1167));
            if(num1168 < 20f) {
                num1166 = npc.velocity.X;
                num1167 = npc.velocity.Y;
            } else {
                num1168 = num1164 / num1168;
                num1166 *= num1168;
                num1167 *= num1168;
            }
            if(npc.velocity.X < num1166) {
                npc.velocity.X = npc.velocity.X + num1165;
                if(npc.velocity.X < 0f && num1166 > 0f) {
                    npc.velocity.X = npc.velocity.X + num1165 * 2f;
                }
            } else if(npc.velocity.X > num1166) {
                npc.velocity.X = npc.velocity.X - num1165;
                if(npc.velocity.X > 0f && num1166 < 0f) {
                    npc.velocity.X = npc.velocity.X - num1165 * 2f;
                }
            }
            if(npc.velocity.Y < num1167) {
                npc.velocity.Y = npc.velocity.Y + num1165;
                if(npc.velocity.Y < 0f && num1167 > 0f) {
                    npc.velocity.Y = npc.velocity.Y + num1165 * 2f;
                }
            } else if(npc.velocity.Y > num1167) {
                npc.velocity.Y = npc.velocity.Y - num1165;
                if(npc.velocity.Y > 0f && num1167 < 0f) {
                    npc.velocity.Y = npc.velocity.Y - num1165 * 2f;
                }
            }
            if(npc.position.X + (float)npc.width > Main.player[npc.target].position.X && npc.position.X < Main.player[npc.target].position.X + (float)Main.player[npc.target].width && npc.position.Y + (float)npc.height < Main.player[npc.target].position.Y && Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height) && Main.netMode != 1) {
                npc.ai[0] += 4f;
                if(npc.ai[0] > 256f) {
                    npc.ai[0] = 0f;
                    int num1169 = (int)(npc.position.X + 10f + (float)Main.rand.Next(npc.width - 20));
                    int num1170 = (int)(npc.position.Y + (float)npc.height + 4f);
                    int num184 = 15;
                    if(Main.expertMode) {
                        num184 = 20;
                    }
                    Projectile.NewProjectile((float)num1169, (float)num1170, 0f, 5f, ModContent.ProjectileType<WitherBolt>(), num184, 0f, Main.myPlayer, 0f, 0f);
                    return;
                }
            }
        }

        public override void FindFrame(int frameHeight) {
            npc.frameCounter += 0.15f;
            npc.frameCounter %= Main.npcFrameCount[npc.type];
            int frame = (int)npc.frameCounter;
            npc.frame.Y = frame * frameHeight;
        }

        public override void NPCLoot() {
            if(Main.rand.Next(2) == 0 && !NPC.downedMechBossAny) {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<PearlFragment>(), 1);
            }
            if(Main.rand.Next(25) == 0) {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<DreadWater>(), 1);
            }
            if(Main.rand.Next(25) == 0) {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<BlackTide>(), 1);
            }
        }

        public override void HitEffect(int hitDirection, double damage) {
            if(npc.life <= 0) {
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Murklegs"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Murklegs"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Murklegs"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Murklegs"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Murkhead"), 1f);
                if(TideWorld.TheTide) {
                    TideWorld.TidePoints2 += 1;
                }
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo) {
            if(TideWorld.TheTide && TideWorld.InBeach)
                return 2f;
            else if(TideWorld.TheTide && TideWorld.InBeach && NPC.downedMechBossAny)
                return 1f;
            return 0;
        }
    }
}