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
using SpiritMod.Buffs;
using System;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
    public class SoulSpirit : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Fiery Soul");
        }

        public override void SetDefaults() {
            projectile.width = 8;
            projectile.height = 8;
            projectile.timeLeft = 30;
            projectile.alpha = 255;
            projectile.maxPenetrate = -1;
            projectile.hostile = false;
            projectile.magic = true;
            projectile.friendly = true;
        }

        public override bool PreAI() {
            Lighting.AddLight((int)(projectile.Center.X / 16f), (int)(projectile.Center.Y / 16f), 0.5f, 0.5f, 0.9f);

            for(int i = 0; i < 10; i++) {
                float x = projectile.Center.X - projectile.velocity.X / 10f * (float)i;
                float y = projectile.Center.Y - projectile.velocity.Y / 10f * (float)i;
                int num = Dust.NewDust(new Vector2(x, y), 1, 1, 68, 0f, 0f, 0, default(Color), 1f);
                Main.dust[num].alpha = projectile.alpha;
                Main.dust[num].position.X = x;
                Main.dust[num].position.Y = y;
                Main.dust[num].velocity *= 0f;
                Main.dust[num].noGravity = true;
            }

            float num2 = (float)Math.Sqrt((double)(projectile.velocity.X * projectile.velocity.X + projectile.velocity.Y * projectile.velocity.Y));
            float num3 = projectile.localAI[0];
            if(num3 == 0f) {
                projectile.localAI[0] = num2;
                num3 = num2;
            }
            float num4 = projectile.position.X;
            float num5 = projectile.position.Y;
            float num6 = 300f;
            bool flag = false;
            int num7 = 0;
            if(projectile.ai[1] == 0f) {
                for(int j = 0; j < 200; j++) {
                    if(Main.npc[j].CanBeChasedBy(this, false) && (projectile.ai[1] == 0f || projectile.ai[1] == (float)(j + 1))) {
                        float num8 = Main.npc[j].position.X + (float)(Main.npc[j].width / 2);
                        float num9 = Main.npc[j].position.Y + (float)(Main.npc[j].height / 2);
                        float num10 = Math.Abs(projectile.position.X + (float)(projectile.width / 2) - num8) + Math.Abs(projectile.position.Y + (float)(projectile.height / 2) - num9);
                        if(num10 < num6 && Collision.CanHit(new Vector2(projectile.position.X + (float)(projectile.width / 2), projectile.position.Y + (float)(projectile.height / 2)), 1, 1, Main.npc[j].position, Main.npc[j].width, Main.npc[j].height)) {
                            num6 = num10;
                            num4 = num8;
                            num5 = num9;
                            flag = true;
                            num7 = j;
                        }
                    }
                }
                if(flag) {
                    projectile.ai[1] = (float)(num7 + 1);
                }
                flag = false;
            }
            if(projectile.ai[1] > 0f) {
                int num11 = (int)(projectile.ai[1] - 1f);
                if(Main.npc[num11].active && Main.npc[num11].CanBeChasedBy(this, true) && !Main.npc[num11].dontTakeDamage) {
                    float num12 = Main.npc[num11].position.X + (float)(Main.npc[num11].width / 2);
                    float num13 = Main.npc[num11].position.Y + (float)(Main.npc[num11].height / 2);
                    float num14 = Math.Abs(projectile.position.X + (float)(projectile.width / 2) - num12) + Math.Abs(projectile.position.Y + (float)(projectile.height / 2) - num13);
                    if(num14 < 1000f) {
                        flag = true;
                        num4 = Main.npc[num11].position.X + (float)(Main.npc[num11].width / 2);
                        num5 = Main.npc[num11].position.Y + (float)(Main.npc[num11].height / 2);
                    }
                } else {
                    projectile.ai[1] = 0f;
                }
            }
            if(!projectile.friendly)
                flag = false;

            if(flag) {
                float num15 = num3;
                Vector2 vector = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
                float num16 = num4 - vector.X;
                float num17 = num5 - vector.Y;
                float num18 = (float)Math.Sqrt((double)(num16 * num16 + num17 * num17));
                num18 = num15 / num18;
                num16 *= num18;
                num17 *= num18;
                int num19 = 8;
                projectile.velocity.X = (projectile.velocity.X * (float)(num19 - 1) + num16) / (float)num19;
                projectile.velocity.Y = (projectile.velocity.Y * (float)(num19 - 1) + num17) / (float)num19;
            }
            projectile.rotation = 0f;
            return false;
        }

        public override void Kill(int timeLeft) {
            Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 14);
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

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            target.AddBuff(ModContent.BuffType<SoulBurn>(), 180, true);
        }

    }
}
