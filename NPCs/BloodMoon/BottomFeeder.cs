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

namespace SpiritMod.NPCs.BloodMoon
{
    public class BottomFeeder : ModNPC
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Bottom Feeder");
            Main.npcFrameCount[npc.type] = 11;
        }
        public override void SetDefaults() {
            npc.width = 60;
            npc.height = 68;
            npc.damage = 24;
            npc.defense = 9;
            npc.lifeMax = 275;
            npc.HitSound = SoundID.NPCHit18;
            npc.DeathSound = SoundID.NPCDeath5;
            npc.value = 160f;
            npc.knockBackResist = 0.34f;
            npc.aiStyle = 3;
            npc.noGravity = false;
            aiType = NPCID.WalkingAntlion;
        }
        int frame = 1;
        int timer = 0;
        int shoottimer = 0;
        public override void AI() {
            npc.spriteDirection = -npc.direction;

            Player target = Main.player[npc.target];
            {
                timer++;
                Player player = Main.player[npc.target];
                int distance = (int)Math.Sqrt((npc.Center.X - target.Center.X) * (npc.Center.X - target.Center.X) + (npc.Center.Y - target.Center.Y) * (npc.Center.Y - target.Center.Y));

                if(timer == 4) {
                    frame++;
                    timer = 0;
                }
                if(frame >= 8 && distance >= 180) {
                    frame = 1;
                } else if(frame == 11 && distance < 180) {
                    frame = 8;
                }
                if(distance < 178) {
                    shoottimer++;
                    if(!npc.wet) {
                        npc.velocity = Vector2.Zero;
                        if(npc.velocity == Vector2.Zero) {
                            npc.velocity.X = .01f * npc.spriteDirection;
                            npc.spriteDirection = -npc.direction;
                            npc.velocity.Y = 10f;
                        }
                    }
                    if(shoottimer >= 8) {
                        int bloodproj;
                        bloodproj = Main.rand.Next(new int[] { mod.ProjectileType("Feeder1"), mod.ProjectileType("Feeder2"), mod.ProjectileType("Feeder3") });
                        bool expertMode = Main.expertMode;
                        int damage = expertMode ? 10 : 15;
                        int p = Terraria.Projectile.NewProjectile(npc.Center.X + (7 * npc.direction), npc.Center.Y - 10, -(npc.position.X - target.position.X) / distance * 8, -(npc.position.Y - target.position.Y + Main.rand.Next(-50, 50)) / distance * 8, bloodproj, damage, 0);
                        shoottimer = 0;
                    }
                }

            }
        }
        public override void FindFrame(int frameHeight) {
            npc.frame.Y = frameHeight * frame;
        }
        public override void NPCLoot() {
            if(Main.rand.Next(17) == 1) {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<BottomFeederGun>());
            }
        }
        public override void HitEffect(int hitDirection, double damage) {
            if(npc.life <= 0 || npc.life >= 0) {
                int d = 5;
                for(int k = 0; k < 25; k++) {
                    Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.47f);
                    Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, Color.White, .97f);
                }
            }
            if(npc.life <= 0) {
                Gore.NewGore(npc.position, npc.velocity * 1.11f, mod.GetGoreSlot("Gores/FeederGore"), 1f);
                Gore.NewGore(npc.position, npc.velocity * 1.11f, mod.GetGoreSlot("Gores/FeederGore1"), 1f);
                Gore.NewGore(npc.position, npc.velocity * 1.11f, mod.GetGoreSlot("Gores/FeederGore1"), 1f);
                Gore.NewGore(npc.position, npc.velocity * 1.11f, mod.GetGoreSlot("Gores/FeederGore2"), 1f);
                Gore.NewGore(npc.position, npc.velocity * 1.11f, mod.GetGoreSlot("Gores/FeederGore2"), 1f);
                Gore.NewGore(npc.position, npc.velocity * 1.11f, mod.GetGoreSlot("Gores/FeederGore2"), 1f);
                Gore.NewGore(npc.position, npc.velocity * 1.11f, mod.GetGoreSlot("Gores/FeederGore3"), 1f);
            }
        }
    }
}
