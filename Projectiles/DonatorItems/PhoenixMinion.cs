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

namespace SpiritMod.Projectiles.DonatorItems
{
    public class PhoenixMinion : ModProjectile
    {
        int counter = 0;
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Fiery Phoenix");
            Main.projFrames[projectile.type] = 8;
        }

        public override void SetDefaults() {
            projectile.friendly = true;
            projectile.magic = true;
            projectile.width = 72;
            projectile.height = 64;
            projectile.penetrate = -1;
            projectile.timeLeft = 6000;
        }

        int timer = 20;

        public override void AI() {
            projectile.spriteDirection = projectile.direction;
            counter++;
            if(counter >= 60) {
                counter = 0;
                float num = 8000f;
                int num2 = -1;
                for(int i = 0; i < 200; i++) {
                    float num3 = Vector2.Distance(projectile.Center, Main.npc[i].Center);
                    if(num3 < num && num3 < 640f && Main.npc[i].CanBeChasedBy(projectile, false)) {
                        num2 = i;
                        num = num3;
                    }
                }

                if(num2 != -1) {
                    bool flag = Collision.CanHit(projectile.position, projectile.width, projectile.height, Main.npc[num2].position, Main.npc[num2].width, Main.npc[num2].height);
                    if(flag) {
                        Vector2 value = Main.npc[num2].Center - projectile.Center;
                        float num4 = 9f;
                        float num5 = (float)Math.Sqrt((double)(value.X * value.X + value.Y * value.Y));
                        if(num5 > num4)
                            num5 = num4 / num5;

                        value *= num5;
                        int p = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, value.X, value.Y, ModContent.ProjectileType<PhoenixProjectile>(), 41, projectile.knockBack / 2f, projectile.owner, 0f, 0f);
                        Main.projectile[p].friendly = true;
                        Main.projectile[p].hostile = false;
                    }
                }
            }

            projectile.tileCollide = true;
            Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 6, 0f, 0f);
            projectile.frameCounter++;
            if(projectile.frameCounter >= 4) {
                projectile.frame = (projectile.frame + 1) % Main.projFrames[projectile.type];
                projectile.frameCounter = 0;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            if(Main.rand.Next(2) == 0)
                target.AddBuff(BuffID.OnFire, 180);
        }

        public override void Kill(int timeLeft) {
            Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0f, 0f, ModContent.ProjectileType<Fire>(), projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
            Projectile.NewProjectile(projectile.Center.X + 10, projectile.Center.Y, 0f, 0f, 15, projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
            Projectile.NewProjectile(projectile.Center.X - 10, projectile.Center.Y, 0f, 0f, 15, projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
            Projectile.NewProjectile(projectile.Center.X + 30, projectile.Center.Y - 10, 0f, 0f, 15, projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
            Projectile.NewProjectile(projectile.Center.X - 30, projectile.Center.Y + 10, 0f, 0f, 15, projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);

            Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 14);
            projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
            projectile.width = 80;
            projectile.height = 80;
            projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);

            for(int num621 = 0; num621 < 20; num621++) {
                int num622 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 2f);
                Main.dust[num622].velocity *= 3f;
                if(Main.rand.Next(2) == 0) {
                    Main.dust[num622].scale = 0.5f;
                    Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                }
            }
            for(int num623 = 0; num623 < 35; num623++) {
                int num624 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 3f);
                Main.dust[num624].noGravity = true;
                Main.dust[num624].velocity *= 5f;
                num624 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 2f);
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
                Main.gore[num626].velocity.X += 1f;
                Main.gore[num626].velocity.Y += 1f;
                num626 = Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[num626].velocity *= scaleFactor10;
                Main.gore[num626].velocity.X -= 1f;
                Main.gore[num626].velocity.Y += 1f;
                num626 = Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[num626].velocity *= scaleFactor10;
                Main.gore[num626].velocity.X += 1f;
                Main.gore[num626].velocity.Y -= 1f;
                num626 = Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[num626].velocity *= scaleFactor10;
                Main.gore[num626].velocity.X -= 1f;
                Main.gore[num626].velocity.Y -= 1f;
            }

            projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
            projectile.width = 10;
            projectile.height = 10;
            projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
        }

    }
}

