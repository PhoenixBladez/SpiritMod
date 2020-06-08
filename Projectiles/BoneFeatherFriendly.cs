using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
    public class BoneFeatherFriendly : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Bone Feather");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 2;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

        public override void SetDefaults() {
            projectile.width = 20;
            projectile.height = 20;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = 1;
            projectile.timeLeft = 1000;
            projectile.tileCollide = true;
            projectile.aiStyle = 1;
            aiType = ProjectileID.Bullet;
        }
        public override void Kill(int timeLeft) {
            Main.PlaySound(0, (int)projectile.position.X, (int)projectile.position.Y);
            for(int i = 0; i < 10; i++) {
                int d = Dust.NewDust(projectile.Center, projectile.width, projectile.height, 0, (float)(Main.rand.Next(8) - 4), (float)(Main.rand.Next(8) - 4), 133);
                Main.dust[d].scale *= .75f;
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
        public override Color? GetAlpha(Color lightColor) {
            return new Color(160, 160, 160, 100);
        }
        public override void AI() {
            if(projectile.timeLeft >= 880) {
                projectile.tileCollide = false;
            } else {
                projectile.tileCollide = true;
            }
        }
    }
}