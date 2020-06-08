
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.DonatorItems
{
    public class DemonIceProj : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Ice Razor");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;

        }
        public override void SetDefaults() {
            projectile.width = 46;
            projectile.height = 46;
            projectile.friendly = true;
            projectile.melee = true;
            projectile.timeLeft = 75;
            projectile.penetrate = 4;

        }

        public override bool PreAI() {
            projectile.rotation += .3f;
            int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 206, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
            int dust1 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 68, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
            Main.dust[dust].noGravity = true;
            Main.dust[dust1].noGravity = true;
            Main.dust[dust1].scale = .85f;
            Main.dust[dust].scale = .45f;
            projectile.velocity.Y += 0.4F;
            projectile.velocity.X *= 1.005F;
            projectile.velocity.X = MathHelper.Clamp(projectile.velocity.X, -10, 10);
            return true;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            if(Main.rand.Next(2) == 0) {
                target.AddBuff(BuffID.Frostburn, 120, true);
            }
        }
        public override Color? GetAlpha(Color lightColor) {
            return new Color(50, 180, 205, 100);
        }
        public override void Kill(int timeLeft) {
            Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 27));
            for(int num257 = 0; num257 < 20; num257++) {
                int newDust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 206, 0f, 0f, 0, default(Color), 1f);
                Main.dust[newDust].position = (Main.dust[newDust].position + projectile.Center) / 2f;
                Main.dust[newDust].velocity *= 0.5f;
                Main.dust[newDust].noGravity = true;
            }
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
        public override bool OnTileCollide(Vector2 oldVelocity) {
            return false;
        }
    }
}
