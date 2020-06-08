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
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Arrow
{
    public class NegativeArrow : ModProjectile
    {
        bool stuck = false;
        float ai0 = 0;
        float ai1 = 0;
        float ai2 = 0;
        int damage = 0;
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Negative Arrow");
        }

        public override void SetDefaults() {
            projectile.width = 16;
            projectile.height = 16;

            projectile.aiStyle = 1;
            aiType = ProjectileID.WoodenArrowFriendly;

            projectile.ranged = true;
            projectile.friendly = true;
            projectile.penetrate = -1;
        }

        public override void Kill(int timeLeft) {
            for(int i = 0; i < 3; i++) {
                Dust.NewDust(projectile.position, projectile.width, projectile.height, 130);
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity) {
            if(!stuck) {
                projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
                projectile.position += projectile.velocity * 2;
                stuck = true;
                projectile.timeLeft = 270;
                projectile.velocity = Vector2.Zero;
                projectile.aiStyle = -1;
            }
            return false;
        }
        public override bool PreAI() {
            int num = 5;

            if(projectile.damage != 0) {
                damage = projectile.damage;
            }
            if(stuck) {
                //return false;
                projectile.velocity = Vector2.Zero;
            }
            if(ai0 != 0) {
                projectile.ignoreWater = true;
                projectile.tileCollide = false;
                int num996 = 15;
                bool flag52 = false;
                bool flag53 = false;
                ai2 += 1f;
                if(ai2 % 30f == 0f)
                    flag53 = true;

                int num997 = (int)ai1;
                if(ai2 >= (float)(60 * num996))
                    flag52 = true;
                else if(num997 < 0 || num997 >= 200)
                    flag52 = true;
                else if(Main.npc[num997].active && !Main.npc[num997].dontTakeDamage) {
                    projectile.Center = Main.npc[num997].Center - projectile.velocity * 2f;
                    projectile.gfxOffY = Main.npc[num997].gfxOffY;
                    if(flag53) {
                        Main.npc[num997].HitEffect(0, 1.0);
                    }
                } else
                    flag52 = true;

                if(flag52)
                    projectile.Kill();
            }
            return base.PreAI();
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            if(projectile.timeLeft > 10) {
                ai0 = 1f;
                ai1 = (float)target.whoAmI;
                projectile.velocity = (target.Center - projectile.Center) * 0.75f;
                projectile.netUpdate = true;
                projectile.damage = 0;

                int num31 = 1;
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
            }
        }
        public override void AI() {
            var list = Main.projectile.Where(x => x.Hitbox.Intersects(projectile.Hitbox));
            foreach(var proj in list) {
                if(proj.type == ModContent.ProjectileType<PositiveArrow>() && proj.active) {
                    projectile.damage = damage;
                    proj.active = false;
                    proj.timeLeft = 2;

                    Main.PlaySound(SoundID.Item93, projectile.position);
                    for(int num621 = 0; num621 < 40; num621++) {
                        int num622 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 226, 0f, 0f, 100, default(Color), 2f);
                        Main.dust[num622].velocity *= 3f;
                        Main.dust[num622].noGravity = true;
                        Main.dust[num622].scale = 0.5f;
                    }
                    Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 14);
                    projectile.damage = damage;
                    projectile.friendly = true;
                    //projectile.penetrate = 1;
                    projectile.timeLeft = 8;
                    ProjectileExtras.Explode(projectile.whoAmI, 220, 220,
                    delegate {
                        for(int i = 0; i < 20; i++) {
                            int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, 226, 0f, -2f, 0, default(Color), 1.1f);
                            Main.dust[num].noGravity = true;
                            Dust expr_62_cp_0 = Main.dust[num];
                            expr_62_cp_0.position.X = expr_62_cp_0.position.X + ((float)(Main.rand.Next(-30, 31) / 20) - 1.5f);
                            Dust expr_92_cp_0 = Main.dust[num];
                            expr_92_cp_0.position.Y = expr_92_cp_0.position.Y + ((float)(Main.rand.Next(-30, 31) / 20) - 1.5f);
                            if(Main.dust[num].position != projectile.Center) {
                                Main.dust[num].velocity = projectile.DirectionTo(Main.dust[num].position) * 6f;
                            }
                        }
                    });
                    projectile.active = false;
                }
            }
        }
    }
}