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

namespace SpiritMod.Projectiles.Magic
{
    public class SummonPortal : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Spiritguard Portal");
        }

        public override void SetDefaults() {
            projectile.hostile = false;
            projectile.minion = true;
            projectile.width = 40;
            projectile.height = 40;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.sentry = true;
            projectile.alpha = 255;
            projectile.timeLeft = 3600;
        }

        public override bool PreAI() {
            projectile.tileCollide = false;
            int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 173, 0f, 0f);
            Main.dust[dust].scale = 1.5f;
            Main.dust[dust].noGravity = true;

            int dust1 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 187, 0f, 0f);
            Main.dust[dust1].scale = 1.5f;
            Main.dust[dust1].noGravity = true;
            return true;
        }

        int timer = 50;
        int timer2 = 0;
        public override void AI() {
            timer--;

            if(timer == 0) {

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
                        float num4 = 5f;
                        float num5 = (float)Math.Sqrt((double)(value.X * value.X + value.Y * value.Y));
                        if(num5 > num4) {
                            num5 = num4 / num5;
                        }
                        value *= num5;
                        int p = Terraria.Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, value.X, value.Y, ModContent.ProjectileType<StarSoul>(), projectile.damage, projectile.knockBack / 2f, projectile.owner, 0f, 0f);
                        Main.projectile[p].friendly = true;
                        Main.projectile[p].hostile = false;
                    }
                }
                timer = 25;
            }
            timer2++;
            if(timer2 == 100) {
                for(int i = 0; i < 6; ++i) {
                    Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 92);
                    Vector2 targetDir = ((((float)Math.PI * 2) / 6) * i).ToRotationVector2();
                    targetDir.Normalize();
                    targetDir *= 15;
                    int y = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, targetDir.X, targetDir.Y, mod.ProjectileType("ShadowPulse1"), 55, projectile.knockBack / 2f, projectile.owner, 0f, 0f);
                    Main.projectile[y].friendly = true;
                    Main.projectile[y].hostile = false;
                    timer2 = 0;
                }
            }
        }

    }
}
