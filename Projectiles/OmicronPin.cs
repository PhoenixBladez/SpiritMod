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
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
    public class OmicronPin : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Star Pin");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 9;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

        public override void SetDefaults() {
            projectile.width = projectile.height = 26;
            projectile.thrown = true;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.light = 0;
            projectile.extraUpdates = 1;
        }

        public override bool PreAI() {
            if(projectile.ai[0] == 0)
                projectile.rotation = projectile.velocity.ToRotation() + 1.57F;
            else {
                projectile.ignoreWater = true;
                projectile.tileCollide = false;
                int num996 = 15;
                bool flag52 = false;
                bool flag53 = false;
                projectile.localAI[0] += 1f;
                if(projectile.localAI[0] % 30f == 0f) {
                    flag53 = true;
                }
                int num997 = (int)projectile.ai[1];
                if(projectile.localAI[0] >= (float)(60 * num996)) {
                    flag52 = true;
                } else if(num997 < 0 || num997 >= 200) {
                    flag52 = true;
                } else if(Main.npc[num997].active && !Main.npc[num997].dontTakeDamage) {
                    projectile.Center = Main.npc[num997].Center - projectile.velocity * 2f;
                    projectile.gfxOffY = Main.npc[num997].gfxOffY;
                    if(flag53)
                        Main.npc[num997].HitEffect(0, 1.0);
                } else
                    flag52 = true;

                if(flag52)
                    projectile.Kill();
            }
            return false;
        }

        public override void AI() {
            Vector2 targetPos = projectile.Center;
            float targetDist = 350f;
            bool targetAcquired = false;

            //loop through first 200 NPCs in Main.npc
            //this loop finds the closest valid target NPC within the range of targetDist pixels
            for(int i = 0; i < 200; i++) {
                if(Main.npc[i].CanBeChasedBy(projectile) && Collision.CanHit(projectile.Center, 1, 1, Main.npc[i].Center, 1, 1)) {
                    float dist = projectile.Distance(Main.npc[i].Center);
                    if(dist < targetDist) {
                        targetDist = dist;
                        targetPos = Main.npc[i].Center;
                        targetAcquired = true;
                    }
                }
            }

            //change trajectory to home in on target
            if(targetAcquired) {
                float homingSpeedFactor = 6f;
                Vector2 homingVect = targetPos - projectile.Center;
                float dist = projectile.Distance(targetPos);
                dist = homingSpeedFactor / dist;
                homingVect *= dist;

                projectile.velocity = (projectile.velocity * 20 + homingVect) / 21f;
            }

            //Spawn the dust
            if(Main.rand.Next(11) == 0) {
                Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 87, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
            }
            projectile.rotation = projectile.velocity.ToRotation() + (float)(Math.PI / 2);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            projectile.ai[0] = 1f;
            projectile.ai[1] = (float)target.whoAmI;
            target.AddBuff(ModContent.BuffType<StarDestiny>(), 9000, false);
            projectile.velocity = (target.Center - projectile.Center) * 0.75f;
            projectile.netUpdate = true;
            projectile.damage = 0;
            int num31 = 6;
            Point[] array2 = new Point[num31];
            int num32 = 0;

            for(int n = 0; n < 1000; n++) {
                if(n != projectile.whoAmI && Main.projectile[n].active && Main.projectile[n].owner == Main.myPlayer && Main.projectile[n].type == projectile.type && Main.projectile[n].ai[0] == 1f && Main.projectile[n].ai[1] == target.whoAmI) {
                    array2[num32++] = new Point(n, Main.projectile[n].timeLeft);
                    if(num32 >= array2.Length)
                        break;
                }
            }
            if(num32 >= array2.Length) {
                int num33 = 0;
                for(int num34 = 1; num34 < array2.Length; num34++) {
                    if(array2[num34].Y < array2[num33].Y)
                        num33 = num34;
                }
                Main.projectile[array2[num33].X].Kill();
            }

            projectile.ai[1] += 1f;
            if(projectile.ai[1] >= 7200f) {
                projectile.alpha += 5;
                if(projectile.alpha > 255) {
                    projectile.alpha = 255;
                    projectile.Kill();
                }
            }

            projectile.rotation += 0.3f;

            projectile.localAI[0] += 1f;
            if(projectile.localAI[0] >= 10f) {
                projectile.localAI[0] = 0f;
                int num416 = 0;
                int num417 = 0;
                float num418 = 0f;
                int num419 = projectile.type;
                for(int num420 = 0; num420 < 1000; num420++) {
                    if(Main.projectile[num420].active && Main.projectile[num420].owner == projectile.owner && Main.projectile[num420].type == num419 && Main.projectile[num420].ai[1] < 3600f) {
                        num416++;
                        if(Main.projectile[num420].ai[1] > num418) {
                            num417 = num420;
                            num418 = Main.projectile[num420].ai[1];
                        }
                    }
                    if(num416 > 3) {
                        Main.projectile[num417].netUpdate = true;
                        Main.projectile[num417].ai[1] = 36000f;
                        return;
                    }
                }
            }
        }

        public override void Kill(int timeLeft) {
            for(int i = 0; i < 5; i++) {
                Dust.NewDust(projectile.position, projectile.width, projectile.height, 187);
            }
            Main.PlaySound(0, (int)projectile.position.X, (int)projectile.position.Y);
        }
    }
}
