using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Bullet.Crimbine
{
    public class CrimbineHeart : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bloody Heart");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }


        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);
            projectile.width = 12;
            projectile.height = 20;
            projectile.friendly = true;
            projectile.ranged = true;
            projectile.penetrate = 3;
            projectile.timeLeft = 240;
        }


        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Player player = Main.player[projectile.owner];
            int lifeToHeal = 0;

            if (player.statLife + 3 <= player.statLifeMax2)
                lifeToHeal = 3;
            else
                lifeToHeal = player.statLifeMax2 - player.statLife;

            player.statLife += lifeToHeal;
            player.HealEffect(lifeToHeal);
        }
        int bounces = 3;
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            bounces--;
            if (bounces <= 0)
                projectile.Kill();
            else
            {
                projectile.ai[0] += 0.1f;
                if (projectile.velocity.X != oldVelocity.X)
                    projectile.velocity.X = -oldVelocity.X;

                if (projectile.velocity.Y != oldVelocity.Y)
                    projectile.velocity.Y = -oldVelocity.Y;

                projectile.velocity *= 0.75f;
            }
            int d = 5;
            for (int k = 0; k < 6; k++)
            {
                Dust.NewDust(projectile.position, projectile.width, projectile.height, d, 2.5f * 1, -2.5f, 0, default(Color), 0.7f);
                Dust.NewDust(projectile.position, projectile.width, projectile.height, d, 2.5f * 1, -2.5f, 0, default(Color), 0.7f);
            }

            Dust.NewDust(projectile.position, projectile.width, projectile.height, d, 2.5f * 1, -2.5f, 0, default(Color), 0.7f);
            Dust.NewDust(projectile.position, projectile.width, projectile.height, d, 2.5f * 1, -2.5f, 0, default(Color), 0.7f);
            Dust.NewDust(projectile.position, projectile.width, projectile.height, d, 2.5f * 1, -2.5f, 0, default(Color), 0.7f);
            Dust.NewDust(projectile.position, projectile.width, projectile.height, d, 2.5f * 1, -2.5f, 0, default(Color), 0.7f);
            return false;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
            for (int k = 0; k < projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
                Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
                spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
            }
            return true;
        }
        public override bool PreAI()
        {
            int num = 5;
            for (int k = 0; k < 3; k++)
            {
                int index2 = Dust.NewDust(projectile.position, 1, 1, 5, 0.0f, 0.0f, 0, new Color(), 1f);
                Main.dust[index2].position = projectile.Center - projectile.velocity / num * (float)k;
                Main.dust[index2].scale = .5f;
                Main.dust[index2].velocity *= 0f;
                Main.dust[index2].noGravity = true;
                Main.dust[index2].noLight = false;
            }
            return true;
        }
        public override void Kill(int timeLeft)
        {
            Main.PlaySound(0, (int)projectile.position.X, (int)projectile.position.Y);
        }
    }
}
