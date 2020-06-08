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
using SpiritMod.Projectiles.DonatorItems;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
    public class ShadowBall_Friendly : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Shadow Ball");
            Main.projFrames[projectile.type] = 4;
        }

        public override void SetDefaults() {
            projectile.width = 20;
            projectile.height = 20;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.alpha = 255;
            projectile.ignoreWater = true;
            projectile.extraUpdates = 1;
        }

        public override void Kill(int timeLeft) {
            Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, ModContent.ProjectileType<Wrath>(), (int)(projectile.damage), 0, Main.myPlayer);

            Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 14);
            {
                for(int num621 = 0; num621 < 40; num621++) {
                    int num622 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 27, 0f, 0f, 100, default(Color), 2f);
                    Main.dust[num622].velocity *= 3f;
                    if(Main.rand.Next(2) == 0) {
                        Main.dust[num622].scale = 0.5f;
                        Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                    }
                }
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            if(Main.rand.Next(2) == 0)
                target.AddBuff(BuffID.ShadowFlame, 180);
        }

        public override void AI() {
            if(projectile.owner == Main.myPlayer && projectile.timeLeft <= 3) {
                projectile.tileCollide = false;
                projectile.ai[1] = 0f;
                projectile.alpha = 255;
                projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
                projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
                projectile.width = 14;
                projectile.height = 14;
                projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
                projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
                projectile.knockBack = 4f;
            }
        }

        public override bool PreAI() {
            bool flag25 = false;
            int jim = 1;
            for(int index1 = 0; index1 < 200; index1++) {
                if(Main.npc[index1].CanBeChasedBy(projectile, false) && Collision.CanHit(projectile.Center, 1, 1, Main.npc[index1].Center, 1, 1)) {
                    float num23 = Main.npc[index1].position.X + (float)(Main.npc[index1].width / 2);
                    float num24 = Main.npc[index1].position.Y + (float)(Main.npc[index1].height / 2);
                    float num25 = Math.Abs(projectile.position.X + (float)(projectile.width / 2) - num23) + Math.Abs(projectile.position.Y + (float)(projectile.height / 2) - num24);
                    if(num25 < 500f) {
                        flag25 = true;
                        jim = index1;
                    }

                }
            }

            if(flag25) {
                float num1 = 10f;
                Vector2 vector2 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
                float num2 = Main.npc[jim].Center.X - vector2.X;
                float num3 = Main.npc[jim].Center.Y - vector2.Y;
                float num4 = (float)Math.Sqrt((double)num2 * (double)num2 + (double)num3 * (double)num3);
                float num5 = num1 / num4;
                float num6 = num2 * num5;
                float num7 = num3 * num5;
                int num8 = 10;
                projectile.velocity.X = (projectile.velocity.X * (float)(num8 - 1) + num6) / (float)num8;
                projectile.velocity.Y = (projectile.velocity.Y * (float)(num8 - 1) + num7) / (float)num8;
            }

            if(projectile.ai[1] >= 1f && projectile.ai[1] < 20f) {
                projectile.ai[1] += 1f;
                if(projectile.ai[1] == 20f)
                    projectile.ai[1] = 1f;
            }

            projectile.alpha -= 40;
            if(projectile.alpha < 0) {
                projectile.alpha = 0;
            }
            projectile.spriteDirection = projectile.direction;
            projectile.frameCounter++;
            if(projectile.frameCounter >= 3) {
                projectile.frame++;
                projectile.frameCounter = 0;
                if(projectile.frame >= 4)
                    projectile.frame = 0;
            }

            projectile.localAI[0] += 1f;
            if(projectile.localAI[0] == 12f) {
                projectile.localAI[0] = 0f;
                for(int j = 0; j < 12; j++) {
                    Vector2 vector2 = Vector2.UnitX * -(float)projectile.width / 2f;
                    vector2 += -Utils.RotatedBy(Vector2.UnitY, (double)((float)j * 3.14159274f / 6f), default(Vector2)) * new Vector2(8f, 16f);
                    vector2 = Utils.RotatedBy(vector2, (double)(projectile.rotation - 1.57079637f), default(Vector2));
                    int num8 = Dust.NewDust(projectile.Center, 0, 0, 27, 0f, 0f, 160, default(Color), 1f);
                    Main.dust[num8].scale = 1.1f;
                    Main.dust[num8].noGravity = true;
                    Main.dust[num8].position = projectile.Center + vector2;
                    Main.dust[num8].velocity = projectile.velocity * 0.1f;
                    Main.dust[num8].velocity = Vector2.Normalize(projectile.Center - projectile.velocity * 3f - Main.dust[num8].position) * 1.25f;
                }
            }

            if(Main.rand.Next(4) == 0) {
                for(int k = 0; k < 1; k++) {
                    Vector2 value = -Utils.RotatedBy(Utils.RotatedByRandom(Vector2.UnitX, 0.19634954631328583), (double)Utils.ToRotation(projectile.velocity), default(Vector2));
                    int num9 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 27, 0f, 0f, 100, default(Color), 1f);
                    Main.dust[num9].velocity *= 0.1f;
                    Main.dust[num9].position = projectile.Center + value * (float)projectile.width / 2f;
                    Main.dust[num9].fadeIn = 0.9f;
                }
            }

            if(Main.rand.Next(32) == 0) {
                for(int l = 0; l < 1; l++) {
                    Vector2 value2 = -Utils.RotatedBy(Utils.RotatedByRandom(Vector2.UnitX, 0.39269909262657166), (double)Utils.ToRotation(projectile.velocity), default(Vector2));
                    int num10 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 27, 0f, 0f, 155, default(Color), 0.8f);
                    Main.dust[num10].velocity *= 0.3f;
                    Main.dust[num10].position = projectile.Center + value2 * (float)projectile.width / 2f;
                    if(Main.rand.Next(2) == 0) {
                        Main.dust[num10].fadeIn = 1.4f;
                    }
                }
            }

            if(Main.rand.Next(2) == 0) {
                for(int m = 0; m < 2; m++) {
                    Vector2 value3 = -Utils.RotatedBy(Utils.RotatedByRandom(Vector2.UnitX, 0.78539818525314331), (double)Utils.ToRotation(projectile.velocity), default(Vector2));
                    int num11 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 27, 0f, 0f, 0, default(Color), 1.2f);
                    Main.dust[num11].velocity *= 0.3f;
                    Main.dust[num11].noGravity = true;
                    Main.dust[num11].position = projectile.Center + value3 * (float)projectile.width / 2f;
                    if(Main.rand.Next(2) == 0) {
                        Main.dust[num11].fadeIn = 1.4f;
                    }
                }
            }
            return false;
        }

    }
}
