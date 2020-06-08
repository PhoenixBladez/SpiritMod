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
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
    public class RunicRune : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Rune");
            Main.projFrames[projectile.type] = 4;
        }

        public override void SetDefaults() {
            projectile.width = 16;
            projectile.height = 16;
            projectile.friendly = true;
            projectile.magic = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 3600;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
        }

        public override bool PreAI() {
            float num = 1f - (float)projectile.alpha / 255f;
            num *= projectile.scale;
            Lighting.AddLight(projectile.Center, 0.5f * num, 0.5f * num, 0.9f * num);
            projectile.frameCounter++;
            if((float)projectile.frameCounter >= 8f) {
                projectile.frame = (projectile.frame + 1) % Main.projFrames[projectile.type];
                projectile.frameCounter = 0;
            }

            projectile.localAI[0]++;
            if(projectile.localAI[0] >= 90f) {
                projectile.localAI[0] *= -1f;
            }
            if(projectile.localAI[0] >= 0f) {
                projectile.scale += 0.003f;
            } else {
                projectile.scale -= 0.003f;
            }

            float num2 = 1f;
            float num3 = 1f;
            int identity = projectile.identity % 6;
            if(identity == 0) {
                num3 *= -1f;
            } else if(identity == 1) {
                num2 *= -1f;
            } else if(identity == 2) {
                num3 *= -1f;
                num2 *= -1f;
            } else if(identity == 3) {
                num3 = 0f;
            } else if(identity == 4) {
                num2 = 0f;
            }

            projectile.localAI[1]++;
            if(projectile.localAI[1] > 60f) {
                projectile.localAI[1] = -180f;
            }
            if(projectile.localAI[1] >= -60f) {
                projectile.velocity.X = projectile.velocity.X + 0.002f * num3;
                projectile.velocity.Y = projectile.velocity.Y + 0.002f * num2;
            } else {
                projectile.velocity.X = projectile.velocity.X - 0.002f * num3;
                projectile.velocity.Y = projectile.velocity.Y - 0.002f * num2;
            }

            projectile.ai[0]++;
            if(projectile.ai[0] > 5400f) {
                projectile.damage = 0;
                projectile.ai[1] = 1f;
                if(projectile.alpha < 255) {
                    projectile.alpha += 5;
                    if(projectile.alpha > 255)
                        projectile.alpha = 255;
                } else if(projectile.owner == Main.myPlayer)
                    projectile.Kill();
            } else {
                float num4 = (projectile.Center - Main.player[projectile.owner].Center).Length() / 100f;
                if(num4 > 4f) {
                    num4 *= 1.1f;
                }
                if(num4 > 5f) {
                    num4 *= 1.2f;
                }
                if(num4 > 6f) {
                    num4 *= 1.3f;
                }
                if(num4 > 7f) {
                    num4 *= 1.4f;
                }
                if(num4 > 8f) {
                    num4 *= 1.5f;
                }
                if(num4 > 9f) {
                    num4 *= 1.6f;
                }
                if(num4 > 10f) {
                    num4 *= 1.7f;
                }
                if(!Main.player[projectile.owner].GetSpiritPlayer().runicSet) {
                    num4 += 100f;
                }
                projectile.ai[0] += num4;

                if(projectile.alpha > 50) {
                    projectile.alpha -= 10;
                    if(projectile.alpha < 50) {
                        projectile.alpha = 50;
                    }
                }
            }

            bool flag = false;
            Vector2 value = Vector2.Zero;
            float num5 = 280f;
            for(int i = 0; i < 200; i++) {
                if(Main.npc[i].CanBeChasedBy(this, false)) {
                    float num6 = Main.npc[i].position.X + (float)(Main.npc[i].width / 2);
                    float num7 = Main.npc[i].position.Y + (float)(Main.npc[i].height / 2);
                    float num8 = Math.Abs(projectile.position.X + (float)(projectile.width / 2) - num6) + Math.Abs(projectile.position.Y + (float)(projectile.height / 2) - num7);
                    if(num8 < num5) {
                        num5 = num8;
                        value = Main.npc[i].Center;
                        flag = true;
                    }
                }
            }
            if(flag) {
                Vector2 vector = value - projectile.Center;
                vector.Normalize();
                vector *= 0.75f;
                projectile.velocity = (projectile.velocity * 10f + vector) / 11f;
                return false;
            }

            if(projectile.velocity.Length() > 0.2f) {
                projectile.velocity *= 0.98f;
            }
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            projectile.velocity *= 0f;
            projectile.alpha = 255;
            projectile.timeLeft = 3;
        }

        public override void Kill(int timeLeft) {
            ProjectileExtras.Explode(projectile.whoAmI, 120, 120,
                delegate {
                    for(int i = 0; i < 40; i++) {
                        int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, 68, 0f, -2f, 0, default(Color), 2f);
                        Main.dust[num].noGravity = true;
                        Dust expr_62_cp_0 = Main.dust[num];
                        expr_62_cp_0.position.X = expr_62_cp_0.position.X + ((float)(Main.rand.Next(-50, 51) / 20) - 1.5f);
                        Dust expr_92_cp_0 = Main.dust[num];
                        expr_92_cp_0.position.Y = expr_92_cp_0.position.Y + ((float)(Main.rand.Next(-50, 51) / 20) - 1.5f);
                        if(Main.dust[num].position != projectile.Center) {
                            Main.dust[num].velocity = projectile.DirectionTo(Main.dust[num].position) * 6f;
                        }
                    }
                });
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor) {
            ProjectileExtras.DrawAroundOrigin(projectile.whoAmI, lightColor);
            return false;
        }
    }
}
