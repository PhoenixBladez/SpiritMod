using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
    public class CobaltStaffProj : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Cobalt Shard");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

        public override void SetDefaults() {
            projectile.width = 14;
            projectile.height = 14;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.magic = true;
            projectile.timeLeft = 160;
            projectile.penetrate = 2;
            projectile.extraUpdates = 1;
        }

        public override void AI() {
            projectile.rotation += .1f;
            float num1 = 8f;
            float num2 = 6f;
            float num3 = 25f;
            num1 = 6f;
            num2 = 3.5f;
            float num4 = 0.5f;
            if(projectile.timeLeft < 120)
                num4 = 1.1f;
            if(projectile.timeLeft < 60)
                num4 = 1.6f;

            ++projectile.ai[1];
            double num5 = (double)projectile.ai[1] / 180.0;


            int index1 = (int)projectile.ai[0];
            if(index1 >= 0 && Main.player[index1].active && !Main.player[index1].dead) {
                if(projectile.Distance(Main.player[index1].Center) <= num3)
                    return;
                Vector2 unitY = projectile.DirectionTo(Main.player[index1].Center);
                if(unitY.HasNaNs())
                    unitY = Vector2.UnitY;
                projectile.velocity = (projectile.velocity * (num1 - 1f) + unitY * num2) / num1;
            } else {
                if(projectile.timeLeft > 30)
                    projectile.timeLeft = 30;
                if(projectile.ai[0] == -1f)
                    return;
                projectile.ai[0] = -1f;
                projectile.netUpdate = true;
            }
            int num = 3;
            for(int k = 0; k < 3; k++) {
                int index2 = Dust.NewDust(projectile.position, 1, 1, 48, 0.0f, 0.0f, 0, new Color(), .5f);
                Main.dust[index2].position = projectile.Center - projectile.velocity / num * (float)k;
                Main.dust[index2].scale = .6f;
                Main.dust[index2].velocity *= 0f;
                Main.dust[index2].noGravity = true;
                Main.dust[index2].noLight = false;
            }
        }
        public override void Kill(int timeLeft) {

            Main.PlaySound(0, (int)projectile.position.X, (int)projectile.position.Y);
            for(int num623 = 0; num623 < 15; num623++) {
                int num624 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 48, 0f, 0f, 100, default(Color), .31f);
                Main.dust[num624].velocity *= .5f;
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor) {
            {
                Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
                for(int k = 0; k < projectile.oldPos.Length; k++) {
                    Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
                    Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
                    spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
                }
            }
            return false;
        }
    }
}