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
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic.Artifact
{
    public class Ember1 : ModProjectile
    {
        int timer = 0;
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Fiery Ember");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 9;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
            Main.projFrames[projectile.type] = 4;
        }

        public override void SetDefaults() {
            projectile.width = 6;
            projectile.height = 6;
            projectile.friendly = true;
            projectile.penetrate = 1;
            projectile.tileCollide = false;
            projectile.magic = true;
            projectile.timeLeft = 200;
            projectile.alpha = 255;
            projectile.light = 0;
            projectile.extraUpdates = 1;
        }

        Vector2 offset = new Vector2(50, 50);
        public override void AI() {
            projectile.frameCounter++;
            if(projectile.frameCounter >= 4) {
                projectile.frame = (projectile.frame + 1) % Main.projFrames[projectile.type];
                projectile.frameCounter = 0;
            }

            Player player = Main.player[projectile.owner];
            if(player.GetSpiritPlayer().SoulStone == true && player.active && !player.dead)
                projectile.timeLeft = 2;

            timer++;
            int range = 30;   //How many tiles away the projectile targets NPCs

            //TARGET NEAREST NPC WITHIN RANGE
            float lowestDist = float.MaxValue;
            foreach(NPC npc in Main.npc) {
                //if npc is a valid target (active, not friendly, and not a critter)
                if(npc.active && !npc.friendly && npc.catchItem == 0) {
                    //if npc is within 50 blocks
                    float dist = projectile.Distance(npc.Center);
                    if(dist / 16 < range) {
                        //if npc is closer than closest found npc
                        if(dist < lowestDist) {
                            lowestDist = dist;

                            //target this npc
                            projectile.ai[1] = npc.whoAmI;
                        }
                    }
                }
            }

            int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 6, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
            int dust2 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 6, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
            Main.dust[dust].noGravity = true;
            Main.dust[dust2].noGravity = true;
            Main.dust[dust].velocity *= 0f;
            Main.dust[dust2].velocity *= 0f;
            Main.dust[dust2].scale = 1.2f;
            Main.dust[dust].scale = 1.2f;
            projectile.rotation = projectile.velocity.ToRotation() + (float)(Math.PI / 2);

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
                float num1 = 10f;
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
            } else {
                var list = Main.projectile.Where(x => x.Hitbox.Intersects(projectile.Hitbox));
                foreach(var proj in list) {
                    if(projectile != proj && proj.hostile)
                        proj.Kill();

                    projectile.ai[0] += .02f;
                    projectile.Center = player.Center + offset.RotatedBy(projectile.ai[0] + projectile.ai[1] * (Math.PI * 10));
                }
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            if(Main.rand.Next(4) == 0)
                target.AddBuff(mod.BuffType("ShadowBurn1"), 180);
        }

        public override void Kill(int timeLeft) {
            for(int i = 0; i < 10; i++) {
                int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, 6, 0f, -2f, 0, default(Color), 2f);
                Main.dust[num].noGravity = true;
                Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0f, 0f, ModContent.ProjectileType<Fire>(), projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);

                Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
                Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
                if(Main.dust[num].position != projectile.Center) {
                    Main.dust[num].velocity = projectile.DirectionTo(Main.dust[num].position) * 6f;
                }
            }
        }


        //public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        //{
        //    Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
        //    for (int k = 0; k < projectile.oldPos.Length; k++)
        //    {
        //        Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
        //        Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
        //        spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
        //    }
        //    return true;
        //}

    }
}