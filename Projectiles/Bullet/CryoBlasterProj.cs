using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using SpiritMod.Projectiles.Bullet;

namespace SpiritMod.Projectiles.Bullet
{
    public class CryoBlasterProj : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Cryo Blaster");
        }

        public override void SetDefaults() {
            projectile.hostile = false;
            projectile.magic = true;
            projectile.width = 16;
            projectile.height = 16;
            projectile.aiStyle = -1;
            projectile.friendly = false;
            projectile.penetrate = 1;
            projectile.tileCollide = false;
           // projectile.alpha = 255;
            projectile.timeLeft = 999999;
        }

        int counter = 0;
         Vector2 holdOffset = new Vector2(-15, -15);
        public override bool PreAI() {
            DoDustEffect(projectile.Center, 14f);
             Player player = Main.player[projectile.owner];
            Vector2 direction = Main.MouseWorld - (player.Center - new Vector2(4, 4));
            direction.Normalize();
            direction *= 12f;
            if(player.channel) {
                  if(direction.X > 0) {
                    player.direction = 1;
                    projectile.spriteDirection = 1;
                    holdOffset.X = -15;
                } else {
                    player.direction = 0;
                    projectile.spriteDirection = 0;
                     holdOffset.X = -30;
                }
                
                projectile.position = player.Center + holdOffset;
                player.velocity.X *= 0.97f;
                counter++;
                if(counter > 160) {
                    DoDustEffect(projectile.Center, 54f);
                }
            } else {
                if(counter > 160) {
                    Projectile.NewProjectile(player.Center - new Vector2(4, 4), direction, ModContent.ProjectileType<CryoliteBullet>(), projectile.damage, projectile.knockBack, projectile.owner);
                }
                projectile.active = false;
            }
            player.heldProj = projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
            projectile.rotation = direction.ToRotation();
            if (direction.ToRotation() > 3.14)
            {
                 projectile.spriteDirection = 1;
            }
            else
            {
                projectile.spriteDirection = 0;
            }
            return true;
        }
        private void DoDustEffect(Vector2 position, float distance, float minSpeed = 2f, float maxSpeed = 3f, object follow = null) {
            float angle = Main.rand.NextFloat(-MathHelper.Pi, MathHelper.Pi);
            Vector2 vec = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
            Vector2 vel = vec * Main.rand.NextFloat(minSpeed, maxSpeed);

            int dust = Dust.NewDust(position - vec * distance, 0, 0, 226);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].scale *= .3f;
            Main.dust[dust].velocity = vel;
            Main.dust[dust].customData = follow;
        }
    }
}
