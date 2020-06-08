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
using SpiritMod.Buffs;
using System;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
    public class FireSoul : ModProjectile
    {
        int target;
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Fiery Soul");
        }

        public override void SetDefaults() {
            projectile.width = 24;       //projectile width
            projectile.height = 46;  //projectile height
            projectile.friendly = true;      //make that the projectile will not damage you
            projectile.magic = true;         // 
            projectile.tileCollide = false;   //make that the projectile will be destroed if it hits the terrain
            projectile.penetrate = 1;      //how many npc will penetrate
            projectile.timeLeft = 300;   //how many time projectile projectile has before disepire // projectile light
            projectile.extraUpdates = 1;
            projectile.ignoreWater = true;
            projectile.aiStyle = -1;
        }

        public override void AI() {
            projectile.rotation = projectile.velocity.ToRotation() + 1.57F;

            bool flag25 = false;
            int jim = 1;
            for(int index1 = 0; index1 < 200; index1++) {
                if(Main.npc[index1].CanBeChasedBy(projectile, false) && Collision.CanHit(projectile.Center, 1, 1, Main.npc[index1].Center, 1, 1)) {
                    float num23 = Main.npc[index1].position.X + (float)(Main.npc[index1].width / 2);
                    float num24 = Main.npc[index1].position.Y + (float)(Main.npc[index1].height / 2);
                    float num25 = Math.Abs(projectile.position.X + (float)(projectile.width / 2) - num23) + Math.Abs(projectile.position.Y + (float)(projectile.height / 2) - num24);
                    if(num25 < 300f) {
                        flag25 = true;
                        jim = index1;
                    }

                }
            }

            if(flag25) {
                float num1 = 4f;
                Vector2 vector2 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
                float num2 = Main.npc[jim].Center.X - vector2.X;
                float num3 = Main.npc[jim].Center.Y - vector2.Y;
                float num4 = (float)Math.Sqrt((double)num2 * (double)num2 + (double)num3 * (double)num3);
                float num5 = num1 / num4;
                float num6 = num2 * num5;
                float num7 = num3 * num5;
                int num8 = 30;
                projectile.velocity.X = (projectile.velocity.X * (float)(num8 - 1) + num6) / (float)num8;
                projectile.velocity.Y = (projectile.velocity.Y * (float)(num8 - 1) + num7) / (float)num8;
            }

            for(int index1 = 0; index1 < 5; ++index1) {
                float num1 = projectile.velocity.X * 0.2f * (float)index1;
                float num2 = (float)-((double)projectile.velocity.Y * 0.200000002980232) * (float)index1;
                int index2 = Dust.NewDust(projectile.position, projectile.width, projectile.height,
                    6, 0.0f, 0.0f, 100, default(Color), 1.3f);
                Main.dust[index2].noGravity = true;
                Main.dust[index2].velocity *= 0.0f;
                Main.dust[index2].position.X -= num1;
                Main.dust[index2].position.Y -= num2;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            target.AddBuff(ModContent.BuffType<StackingFireBuff>(), 180);
        }

        public override void Kill(int timeLeft) {
            Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 14);
            ProjectileExtras.Explode(projectile.whoAmI, 120, 120,
                delegate {
                    Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 14);
                    projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
                    projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
                    projectile.width = 50;
                    projectile.height = 50;
                    projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
                    projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);

                    for(int num621 = 0; num621 < 20; num621++) {
                        int num622 = Dust.NewDust(projectile.position, projectile.width, projectile.height,
                            244, 0f, 0f, 100, default(Color), 2f);
                        Main.dust[num622].velocity *= 3f;
                        if(Main.rand.Next(2) == 0) {
                            Main.dust[num622].scale = 0.5f;
                            Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                        }
                    }

                    for(int num623 = 0; num623 < 35; num623++) {
                        int num624 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 244, 0f, 0f, 100, default(Color), 3f);
                        Main.dust[num624].noGravity = true;
                        Main.dust[num624].velocity *= 5f;
                        num624 = Dust.NewDust(projectile.position, projectile.width, projectile.height,
                            244, 0f, 0f, 100, default(Color), 2f);
                        Main.dust[num624].velocity *= 2f;
                    }

                    for(int num625 = 0; num625 < 3; num625++) {
                        float scaleFactor10 = 0.33f;
                        if(num625 == 1)
                            scaleFactor10 = 0.66f;
                        else if(num625 == 2)
                            scaleFactor10 = 1f;

                        int num626 = Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                        Main.gore[num626].velocity *= scaleFactor10;
                        Gore expr_13AB6_cp_0 = Main.gore[num626];
                        expr_13AB6_cp_0.velocity.X = expr_13AB6_cp_0.velocity.X + 1f;
                        Gore expr_13AD6_cp_0 = Main.gore[num626];
                        expr_13AD6_cp_0.velocity.Y = expr_13AD6_cp_0.velocity.Y + 1f;
                        num626 = Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                        Main.gore[num626].velocity *= scaleFactor10;
                        Gore expr_13B79_cp_0 = Main.gore[num626];
                        expr_13B79_cp_0.velocity.X = expr_13B79_cp_0.velocity.X - 1f;
                        Gore expr_13B99_cp_0 = Main.gore[num626];
                        expr_13B99_cp_0.velocity.Y = expr_13B99_cp_0.velocity.Y + 1f;
                        num626 = Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                        Main.gore[num626].velocity *= scaleFactor10;
                        Gore expr_13C3C_cp_0 = Main.gore[num626];
                        expr_13C3C_cp_0.velocity.X = expr_13C3C_cp_0.velocity.X + 1f;
                        Gore expr_13C5C_cp_0 = Main.gore[num626];
                        expr_13C5C_cp_0.velocity.Y = expr_13C5C_cp_0.velocity.Y - 1f;
                        num626 = Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                        Main.gore[num626].velocity *= scaleFactor10;
                        Gore expr_13CFF_cp_0 = Main.gore[num626];
                        expr_13CFF_cp_0.velocity.X = expr_13CFF_cp_0.velocity.X - 1f;
                        Gore expr_13D1F_cp_0 = Main.gore[num626];
                        expr_13D1F_cp_0.velocity.Y = expr_13D1F_cp_0.velocity.Y - 1f;
                    }

                    projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
                    projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
                    projectile.width = 10;
                    projectile.height = 10;
                    projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
                    projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
                });
        }

        public override bool OnTileCollide(Vector2 oldVelocity) {
            return true;
        }

    }
}
