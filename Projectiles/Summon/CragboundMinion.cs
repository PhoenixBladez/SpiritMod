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

namespace SpiritMod.Projectiles.Summon
{
    public class CragboundMinion : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Cragbound");
            Main.projFrames[base.projectile.type] = 8;
        }

        public override void SetDefaults() {
            projectile.width = 100;
            projectile.height = 86;
            projectile.timeLeft = 3000;
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.penetrate = -1;
            projectile.ignoreWater = true;
            projectile.minion = true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity) {
            return false;
        }

        public override void AI() {
            bool flag64 = projectile.type == ModContent.ProjectileType<CragboundMinion>();
            Player player = Main.player[projectile.owner];
            MyPlayer modPlayer = player.GetSpiritPlayer();
            if(flag64) {
                if(player.dead)
                    modPlayer.cragboundMinion = false;

                if(modPlayer.cragboundMinion)
                    projectile.timeLeft = 2;

            }

            projectile.frameCounter++;
            if(projectile.frameCounter >= 8) {
                projectile.frame++;
                projectile.frameCounter = 0;
                if(projectile.frame >= 8)
                    projectile.frame = 0;

            }

            projectile.velocity.Y = 5;
            //CONFIG INFO
            int range = 80;   //How many tiles away the projectile targets NPCs
            float shootVelocity = 18f; //magnitude of the shoot vector (speed of arrows shot)
            int shootSpeed = 20;

            //TARGET NEAREST NPC WITHIN RANGE
            float lowestDist = float.MaxValue;
            for(int i = 0; i < 200; ++i) {
                NPC npc = Main.npc[i];
                //if npc is a valid target (active, not friendly, and not a critter)
                if(npc.active && npc.CanBeChasedBy(projectile)) {
                    //if npc is within 50 blocks
                    float dist = projectile.Distance(npc.Center);
                    if(dist / 16 < range) {
                        //if npc is closer than closest found npc
                        if(dist < lowestDist) {
                            lowestDist = dist;

                            //target this npc
                            projectile.ai[1] = npc.whoAmI;
                        }
                    }
                }
            }

            projectile.ai[0]++;
            if(projectile.ai[0] >= 60) {
                if(projectile.ai[0] % 5 == 0) {
                    for(int i = 0; i < 200; ++i) {
                        if(Main.npc[i].active && (projectile.position - Main.npc[i].position).Length() < 180 && Main.npc[i].CanBeChasedBy(projectile)) {
                            projectile.direction = Main.npc[i].position.X < projectile.position.X ? -1 : 1;

                            Vector2 position = new Vector2(projectile.position.X + projectile.width * 0.5f + Main.rand.Next(201) * -projectile.direction + (Main.npc[i].position.X - projectile.position.X), projectile.Center.Y - 600f);
                            position.X = (position.X * 10f + projectile.position.X) / 11f + (float)Main.rand.Next(-100, 101);
                            position.Y -= 150;
                            float speedX = (float)Main.npc[i].position.X - position.X;
                            float speedY = (float)Main.npc[i].position.Y - position.Y;
                            if(speedY < 0f)
                                speedY *= -1f;
                            if(speedY < 20f)
                                speedY = 20f;

                            float length = (float)Math.Sqrt((double)(speedX * speedX + speedY * speedY));
                            length = 12 / length;
                            speedX *= length;
                            speedY *= length;
                            speedX = speedX + (float)Main.rand.Next(-40, 41) * 0.03f;
                            speedY = speedY + (float)Main.rand.Next(-40, 41) * 0.03f;
                            speedX *= (float)Main.rand.Next(75, 150) * 0.01f;
                            position.X += (float)Main.rand.Next(-50, 51);
                            Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ModContent.ProjectileType<PrismaticBolt>(), projectile.damage, projectile.knockBack, projectile.owner);
                            break;
                        }
                    }
                }

                if(projectile.ai[0] >= 90)
                    projectile.ai[0] = 0;

            }
        }

    }
}
