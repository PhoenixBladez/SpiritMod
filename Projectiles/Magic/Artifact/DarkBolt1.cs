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

namespace SpiritMod.Projectiles.Magic.Artifact
{

    public class DarkBolt1 : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Shadow Bolt");
            Main.projFrames[projectile.type] = 5;
        }

        public override void SetDefaults() {
            projectile.width = 20;
            projectile.height = 20;
            projectile.friendly = true;
            projectile.magic = true;
            projectile.tileCollide = true;
            projectile.penetrate = 2;
            projectile.timeLeft = 500;
            projectile.extraUpdates = 1;
            projectile.ignoreWater = true;
        }

        public override void AI() {
            projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
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
                float num1 = 6f;
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

            projectile.frameCounter++;
            if(projectile.frameCounter >= 5) {
                projectile.frame = (projectile.frame + 1) % Main.projFrames[projectile.type];
                projectile.frameCounter = 0;
            }

            projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
            if(Main.rand.Next(4) == 0) {
                for(int k = 0; k < 1; k++) {
                    Vector2 value = -Utils.RotatedBy(Utils.RotatedByRandom(Vector2.UnitX, (Math.PI / 16)), (double)Utils.ToRotation(projectile.velocity));
                    int num9 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 173, 0f, 0f, 173);
                    Main.dust[num9].velocity *= 0.1f;
                    Main.dust[num9].noGravity = true;
                    Main.dust[num9].position = projectile.Center + value * (float)projectile.width / 2f;
                    Main.dust[num9].fadeIn = 0.9f;
                }
            }
            if(Main.rand.Next(2) == 0) {
                for(int m = 0; m < 2; m++) {
                    Vector2 value3 = -Utils.RotatedBy(Utils.RotatedByRandom(Vector2.UnitX, (Math.PI / 4)), (double)Utils.ToRotation(projectile.velocity));
                    int num11 = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Shadowflame, 0f, 0f, 0, default(Color), 1.2f);
                    Main.dust[num11].velocity *= 0.3f;
                    Main.dust[num11].noGravity = true;
                    Main.dust[num11].position = projectile.Center + value3 * (float)projectile.width / 2f;
                    if(Main.rand.Next(2) == 0)
                        Main.dust[num11].fadeIn = 1.4f;
                }
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            if(Main.rand.Next(4) == 0)
                target.AddBuff(mod.BuffType("ShadowBurn1"), 300);
        }

        public override void Kill(int timeLeft) {
            Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 14);
            Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 74);
            Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0f, 0f, ModContent.ProjectileType<Fire>(), projectile.damage / 3 * 4, projectile.knockBack, projectile.owner);

            for(int i = 0; i < 40; i++) {
                int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Shadowflame, 0f, -2f, 0, default(Color), 2f);
                Main.dust[num].noGravity = true;
                Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
                Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
                if(Main.dust[num].position != projectile.Center)
                    Main.dust[num].velocity = projectile.DirectionTo(Main.dust[num].position) * 6f;
            }
        }

    }
}