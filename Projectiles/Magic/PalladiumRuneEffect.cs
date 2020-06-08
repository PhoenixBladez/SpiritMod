using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
    public class PalladiumRuneEffect : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Palladium Rune");
            Main.projFrames[projectile.type] = 10;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 16;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

        public override void SetDefaults() {
            projectile.hostile = false;
            projectile.magic = true;
            projectile.width = 18;
            projectile.height = 18;
            projectile.friendly = true;
            projectile.timeLeft = 60;

        }
        public override Color? GetAlpha(Color lightColor) {
            return new Color(252 - (int)(timer / 3), 153 - (int)(timer / 3), 3 - (int)(timer / 3), 255 - timer * 2);
        }
        int timer;
        public override void AI() {
            Lighting.AddLight(projectile.position, 0.4f / 2, .12f / 2, .036f / 2);
            timer += 4;
            projectile.alpha += 6;
            projectile.velocity.X *= .98f;
            projectile.velocity.Y *= .98f;
            projectile.frameCounter++;
            if(projectile.frameCounter >= 10) {
                projectile.frameCounter = 0;
                projectile.frame = (projectile.frame + 1) % 10;
            }
        }
        public override void Kill(int timeLeft) {
            {
                for(int i = 0; i < 10; i++) {
                    int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, 158, 0f, -2f, 0, Color.White, 2f);
                    Main.dust[num].noLight = true;
                    Main.dust[num].noGravity = true;
                    Dust expr_62_cp_0 = Main.dust[num];
                    expr_62_cp_0.position.X = expr_62_cp_0.position.X + ((float)(Main.rand.Next(-10, 11) / 20) - 1.5f);
                    Dust expr_92_cp_0 = Main.dust[num];
                    expr_92_cp_0.position.Y = expr_92_cp_0.position.Y + ((float)(Main.rand.Next(-10, 11) / 20) - 1.5f);
                    if(Main.dust[num].position != projectile.Center) {
                        Main.dust[num].velocity = projectile.DirectionTo(Main.dust[num].position) * 2f;
                    }
                }
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor) {
            Texture2D texture = Main.projectileTexture[projectile.type];
            int frameHeight = texture.Height / Main.projFrames[projectile.type];
            Rectangle frameRect = new Rectangle(0, projectile.frame * frameHeight, texture.Width, frameHeight);
            Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
            for(int k = 0; k < projectile.oldPos.Length; k++) {
                Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
                Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
                spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, frameRect, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
            }
            return false;
        }
    }
}