using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
    public class Zephyr : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Zephyr");
            Main.projFrames[projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

        public override void SetDefaults() {
            projectile.friendly = true;
            projectile.magic = true;
            projectile.width = 14;
            projectile.height = 26;
            projectile.penetrate = 1;
            projectile.timeLeft = 120;
            projectile.scale = 1.1f;
        }

        public override bool PreAI() {
            projectile.tileCollide = true;
            int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 16, 0f, 0f);
            Main.dust[dust].scale = 1.2f;
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity *= 0f;
            Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 16, 0f, 0f); //to make some with gravity to fly all over the place :P

            projectile.velocity.Y += projectile.ai[0];
            projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + 1.57f;

            projectile.frameCounter++;
            if(projectile.frameCounter >= 6) {
                projectile.frameCounter = 0;
                projectile.frame = (projectile.frame + 1) % 2;
            }
            return false;
        }

        public override bool OnTileCollide(Vector2 oldVelocity) {
            projectile.Kill();
            Dust.NewDust(projectile.position + projectile.velocity * 0, projectile.width, projectile.height, 16, projectile.oldVelocity.X * 0, projectile.oldVelocity.Y * 0);
            return false;
        }

        public override void Kill(int timeLeft) {
            Main.PlaySound(0, (int)projectile.position.X, (int)projectile.position.Y);
            Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 16, projectile.oldVelocity.X * 0.2f, projectile.oldVelocity.Y * 0.2f);
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor) {
            Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
            for(int k = 0; k < projectile.oldPos.Length; k++) {
                Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
                Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
                spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
            }
            return false;
        }
    }
}
