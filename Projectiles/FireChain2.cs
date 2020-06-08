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

namespace SpiritMod.Projectiles
{
    public class FireChain2 : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Fiery Essence");
        }

        public override void SetDefaults() {
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = -1;
            projectile.timeLeft = 240;
            projectile.height = 6;
            projectile.width = 6;
            projectile.alpha = 255;
            aiType = ProjectileID.Bullet;
            projectile.extraUpdates = 1;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.magic = true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            projectile.localAI[1] += 1f;
            target.AddBuff(BuffID.OnFire, 280);
            projectile.velocity *= 0f;
        }

        public override bool PreAI() {
            projectile.localAI[1] += 1f;
            int num = 1;
            int num2 = 1;
            if((double)projectile.localAI[1] <= 1.0) {
                int num3 = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)num, (float)num2, mod.ProjectileType("FireChain3"), projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
                Main.projectile[num3].localAI[0] = (float)projectile.whoAmI;
            }

            int num4 = (int)projectile.localAI[1];
            if(num4 <= 30) {
                if(num4 == 30 || num4 == 10)
                    num2--;
            } else if(num4 == 50 || num4 == 70) {
                num2--;
            }

            if((int)projectile.localAI[1] == 20) {
                int num5 = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, num, num2, ModContent.ProjectileType<FireChain3>(), projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
                Main.projectile[num5].localAI[0] = projectile.whoAmI;
            }
            return true;
        }

        public override void AI() {
            int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 6, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
            int dust2 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 6, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
            Main.dust[dust].noGravity = true;
            Main.dust[dust2].noGravity = true;
            Main.dust[dust2].velocity *= 0f;
            Main.dust[dust2].velocity *= 0f;
            Main.dust[dust2].scale = 0.9f;
            Main.dust[dust].scale = 0.9f;

            int num = (int)projectile.velocity.X * 10;
            int num2 = (int)projectile.velocity.Y + 1;
            projectile.frameCounter++;
            if(projectile.frameCounter > 120) {
                projectile.frameCounter = 0;
                if(projectile.frame == 5)
                    projectile.frame = 0;
                else
                    projectile.frame++;
            }

            int num3 = 80 - num;
            int num4 = 12 - num2;
            int num5 = 16;
            projectile.localAI[1] += 0.0104719754f * (float)num4;
            projectile.localAI[1] %= 6.28318548f;

            Vector2 center = Main.projectile[(int)projectile.localAI[0]].Center;
            center.X -= (float)num5;
            projectile.rotation = (float)Math.Atan2((double)center.Y, (double)center.X) - 2f;
            projectile.Center = center + (float)num3 * new Vector2((float)Math.Cos((double)projectile.localAI[1]), (float)Math.Sin((double)projectile.localAI[1]));
        }

        private static Vector2 GetVelocity(Projectile projectile) {
            float num = 400f;
            Vector2 velocity = projectile.velocity;
            Vector2 vector = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
            vector.SafeNormalize(Vector2.UnitY);
            vector *= Main.rand.Next(10, 41) * 0.1f;
            if(Main.rand.Next(3) == 0)
                vector *= 2f;

            Vector2 vector3 = velocity * 0.25f + vector;
            for(int i = 0; i < 200; i++) {
                if(Main.npc[i].CanBeChasedBy(projectile, false)) {
                    float num2 = Main.npc[i].position.X + (Main.npc[i].width / 2);
                    float num3 = Main.npc[i].position.Y + (Main.npc[i].height / 2);
                    float num4 = Math.Abs(projectile.position.X + (projectile.width / 2) - num2) + Math.Abs(projectile.position.Y + (projectile.height / 2) - num3);
                    if(num4 < num && Collision.CanHit(projectile.position, projectile.width, projectile.height, Main.npc[i].position, Main.npc[i].width, Main.npc[i].height)) {
                        num = num4;
                        vector3.X = num2;
                        vector3.Y = num3;
                        Vector2 vector4 = vector3 - projectile.Center;
                        vector4.Normalize();
                        vector3 = vector4 * 8f;
                    }
                }
            }
            return vector3 * 0.8f;
        }

        public override void PostAI() {
            projectile.rotation -= 10f;
            projectile.velocity *= 0.95f;
        }
    }
}
