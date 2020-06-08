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
using Terraria.ID;

namespace SpiritMod.Projectiles.Summon
{
    public class SteamMinion : Minion
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Miniflayer");
            Main.projFrames[projectile.type] = 1;
            ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
            ProjectileID.Sets.Homing[projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
        }

        public override void SetDefaults() {
            projectile.width = 32;
            projectile.height = 32;
            Main.projPet[projectile.type] = true;
            projectile.friendly = true;
            projectile.minion = true;
            projectile.minionSlots = 1f;
            projectile.penetrate = -1;
            projectile.aiStyle = -1;
            projectile.timeLeft = 18000;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.netImportant = true;
        }

        public override void CheckActive() {
            MyPlayer mp = Main.player[projectile.owner].GetModPlayer<MyPlayer>();
            if(mp.player.dead)
                mp.Flayer = false;

            if(mp.Flayer)
                projectile.timeLeft = 2;

        }

        public override void Behavior() {
            Player player = Main.player[projectile.owner];
            float num = (float)projectile.width * 1.1f;
            for(int i = 0; i < 1000; i++) {
                Projectile current = Main.projectile[i];
                if(i != current.whoAmI && current.active && projectile.owner == current.owner && projectile.type == current.type && Math.Abs(projectile.position.X - current.position.X) + Math.Abs(projectile.position.Y - current.position.Y) < num) {
                    if(projectile.position.X < Main.projectile[i].position.X)
                        projectile.velocity.X -= 0.08f;
                    else
                        projectile.velocity.X += 0.08f;

                    if(projectile.position.Y < Main.projectile[i].position.Y)
                        projectile.velocity.Y -= 0.08f;
                    else
                        projectile.velocity.Y += 0.08f;

                }
            }

            Vector2 value = projectile.position;
            float num2 = 500f;
            bool flag = false;
            projectile.tileCollide = true;
            for(int j = 0; j < 200; j++) {
                NPC nPC = Main.npc[j];
                if(nPC.CanBeChasedBy(this, false)) {
                    float num3 = Vector2.Distance(nPC.Center, projectile.Center);
                    if((num3 < num2 || !flag) && Collision.CanHitLine(projectile.position, projectile.width, projectile.height, nPC.position, nPC.width, nPC.height)) {
                        num2 = num3;
                        value = nPC.Center;
                        flag = true;
                    }
                }
            }

            if(Vector2.Distance(player.Center, projectile.Center) > (flag ? 1000f : 500f)) {
                projectile.ai[0] = 1f;
                projectile.netUpdate = true;
            }

            if(projectile.ai[0] == 1f) {
                projectile.tileCollide = false;
            }
            if(flag && projectile.ai[0] == 0f) {
                Vector2 value2 = value - projectile.Center;
                if(value2.Length() > 200f) {
                    value2.Normalize();
                    projectile.velocity = (projectile.velocity * 20f + value2 * 6f) / 21f;
                } else {
                    projectile.velocity *= (float)Math.Pow(0.97, 2.0);
                }
            } else {
                if(!Collision.CanHitLine(projectile.Center, 1, 1, player.Center, 1, 1))
                    projectile.ai[0] = 1f;

                float num4 = 6f;
                if(projectile.ai[0] == 1f)
                    num4 = 15f;

                Vector2 center = projectile.Center;
                Vector2 vector = player.Center - center;
                projectile.ai[1] = 3600f;
                projectile.netUpdate = true;
                int num5 = 1;
                for(int k = 0; k < projectile.whoAmI; k++) {
                    if(Main.projectile[k].active && Main.projectile[k].owner == projectile.owner && Main.projectile[k].type == projectile.type) {
                        num5++;
                    }
                }
                vector.X -= (10 + num5 * 40) * player.direction;
                vector.Y -= 70f;

                float num6 = vector.Length();
                if(num6 > 200f && num4 < 9f) {
                    num4 = 9f;
                }
                if(num6 < 100f && projectile.ai[0] == 1f && !Collision.SolidCollision(projectile.position, projectile.width, projectile.height)) {
                    projectile.ai[0] = 0f;
                    projectile.netUpdate = true;
                }
                if(num6 > 2000f) {
                    projectile.Center = player.Center;
                }
                if(num6 > 48f) {
                    vector.Normalize();
                    vector *= num4;
                    float num7 = 10f;
                    projectile.velocity = (projectile.velocity * num7 + vector) / (num7 + 1f);
                } else {
                    projectile.direction = Main.player[projectile.owner].direction;
                    projectile.velocity *= (float)Math.Pow(0.9, 2.0);
                }
            }

            projectile.rotation = projectile.velocity.X * 0.05f;
            if(projectile.velocity.X > 0f) {
                projectile.spriteDirection = (projectile.direction = -1);
            } else if(projectile.velocity.X < 0f) {
                projectile.spriteDirection = (projectile.direction = 1);
            }

            if(projectile.ai[1] > 0f) {
                projectile.ai[1] += 1f;
            }
            if(projectile.ai[1] > 140f) {
                projectile.ai[1] = 0f;
                projectile.netUpdate = true;
            }

            if(projectile.ai[0] == 0f && flag) {
                if((value - projectile.Center).X > 0f) {
                    projectile.spriteDirection = (projectile.direction = -1);
                } else if((value - projectile.Center).X < 0f) {
                    projectile.spriteDirection = (projectile.direction = 1);
                }
                {
                    if(projectile.ai[1] == 0f) {
                        projectile.ai[1] = 1f;
                        if(Main.myPlayer == projectile.owner) {
                            Vector2 vector2 = value - projectile.Center;
                            if(vector2 == Vector2.Zero) {
                                vector2 = new Vector2(0f, 1f);
                            }
                            vector2.Normalize();
                            vector2 *= 12f;
                            int num8 = Terraria.Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vector2.X, vector2.Y, base.mod.ProjectileType("NovaBeam3"), projectile.damage, projectile.knockBack, Main.myPlayer, 0f, 0f);
                            Main.projectile[num8].timeLeft = 300;
                            Main.projectile[num8].netUpdate = true;
                            Main.projectile[num8].minion = true;
                            Main.projectile[num8].magic = false;
                            projectile.netUpdate = true;
                        }
                    }
                }
            }
            Lighting.AddLight((int)(projectile.Center.X / 16f), (int)(projectile.Center.Y / 16f), 0.2f, 0.4f, 0.6f);
        }
        public override void SelectFrame() {
        }
        public override void Kill(int timeLeft) {
            if(projectile.aiStyle == -3) {
                int n = 8;
                int deviation = Main.rand.Next(0, 300);
                for(int i = 0; i < n; i++) {
                    float rotation = MathHelper.ToRadians(270 / n * i + deviation);
                    Vector2 perturbedSpeed = new Vector2(projectile.velocity.X + 1, projectile.velocity.Y).RotatedBy(rotation);
                    perturbedSpeed.Normalize();
                    perturbedSpeed.X *= 2.5f;
                    perturbedSpeed.Y *= 2.5f;
                    Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, mod.ProjectileType("CorruptPortal_Star"), projectile.damage / 5 * 4, 2, projectile.owner);
                }
                Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 14);
                ProjectileExtras.Explode(projectile.whoAmI, 120, 120,
                    delegate {
                        for(int i = 0; i < 40; i++) {
                            int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, 173, 0f, -2f, 0, default(Color), 1.3f);
                            Main.dust[num].noGravity = true;
                            Dust expr_62_cp_0 = Main.dust[num];
                            expr_62_cp_0.position.X = expr_62_cp_0.position.X + ((float)(Main.rand.Next(-50, 51) / 10) - 1.5f);
                            Dust expr_92_cp_0 = Main.dust[num];
                            expr_92_cp_0.position.Y = expr_92_cp_0.position.Y + ((float)(Main.rand.Next(-50, 51) / 10) - 1.5f);
                            if(Main.dust[num].position != projectile.Center) {
                                Main.dust[num].velocity = projectile.DirectionTo(Main.dust[num].position) * 6f;
                            }
                        }
                    });
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor) {
            Texture2D texture2D = Main.projectileTexture[projectile.type];
            Vector2 origin = new Vector2((float)texture2D.Width * 0.5f, (float)(texture2D.Height / Main.projFrames[projectile.type]) * 0.5f);
            SpriteEffects effects = (projectile.direction == -1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Vector2 value = new Vector2(projectile.Center.X - 4f, projectile.Center.Y - 8f);
            Main.spriteBatch.Draw(texture2D, value - Main.screenPosition, new Rectangle?(Utils.Frame(texture2D, 1, Main.projFrames[projectile.type], 0, projectile.frame)), lightColor, projectile.rotation, origin, projectile.scale, effects, 0f);
            return false;
        }

    }
}
