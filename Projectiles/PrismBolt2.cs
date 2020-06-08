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
    public class PrismBolt2 : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Prismatic Energy");
            ProjectileID.Sets.Homing[projectile.type] = true;
        }

        //Warning : it's not my code. It's SpiritMod code. so i donnt fully understand it
        public override void SetDefaults() {
            projectile.width = 12;
            projectile.height = 36;
            projectile.aiStyle = 1;
            projectile.friendly = true;
            projectile.ranged = true;
            projectile.penetrate = 1;
            projectile.timeLeft = 440;
            aiType = ProjectileID.Bullet;
        }

        public override bool PreAI() {
            projectile.ai[1] += 1f;
            bool chasing = false;
            if(projectile.ai[1] >= 30f) {
                chasing = true;

                projectile.friendly = true;
                NPC target = null;
                if(projectile.ai[0] == -1f) {
                    target = ProjectileExtras.FindRandomNPC(projectile.Center, 960f, false);
                } else {
                    target = Main.npc[(int)projectile.ai[0]];
                    if(!target.active || !target.CanBeChasedBy())
                        target = ProjectileExtras.FindRandomNPC(projectile.Center, 960f, false);
                }

                if(target == null) {
                    chasing = false;
                    projectile.ai[0] = -1f;
                } else {
                    projectile.ai[0] = (float)target.whoAmI;
                    ProjectileExtras.HomingAI(this, target, 10f, 5f);
                }
            }

            ProjectileExtras.LookAlongVelocity(this);
            if(!chasing) {
                Vector2 dir = projectile.velocity;
                float vel = projectile.velocity.Length();
                if(vel != 0f) {
                    if(vel < 4f) {
                        dir *= 1 / vel;
                        projectile.velocity += dir * 0.0625f;
                    }
                } else {
                    //Stops the projectiles from spazzing out
                    projectile.velocity.X += Main.rand.Next(2) == 0 ? 0.1f : -0.1f;
                }
            }
            int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 206, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
            Main.dust[dust].scale = 2f;
            Main.dust[dust].noGravity = true;
            Main.dust[dust].noLight = true;

            return false;
        }

        public override void Kill(int timeLeft) {
            Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 14);
            ProjectileExtras.Explode(projectile.whoAmI, 120, 120,
                delegate {
                    for(int num621 = 0; num621 < 40; num621++) {
                        int num622 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 206, 0f, 0f, 100, default(Color), 2f);
                        Main.dust[num622].velocity *= 3f;
                        if(Main.rand.Next(2) == 0) {
                            Main.dust[num622].scale = 0.5f;
                            Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                        }
                    }
                    for(int num623 = 0; num623 < 70; num623++) {
                        int num624 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 257, 0f, 0f, 100, default(Color), 1f);
                        Main.dust[num624].noGravity = true;
                        Main.dust[num624].velocity *= 1.5f;
                        num624 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 206, 0f, 0f, 100, default(Color), 1f);
                        Main.dust[num624].velocity *= 2f;
                    }
                });
        }

        private void AdjustMagnitude(ref Vector2 vector) {
            float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if(magnitude > 6f)
                vector *= 6f / magnitude;
        }


    }
}
