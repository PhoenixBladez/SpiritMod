using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
    public class Polyshot : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Polyshot");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

        public override void SetDefaults() {
            projectile.hostile = false;
            projectile.magic = true;
            projectile.width = 32;
            projectile.light = .25f;
            projectile.height = 36;
            projectile.friendly = true;
            projectile.damage = 35;
        }

        public override void AI() {
            float num395 = Main.mouseTextColor / 200f - 0.35f;
            num395 *= 0.3f;
            projectile.scale = num395 + 0.95f;
            projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + MathHelper.PiOver2;
        }
        public override Color? GetAlpha(Color lightColor) {
            return new Color(89, 255, 161, 100);
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
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            target.life = 0;
            NPC.NewNPC((int)target.position.X, (int)target.position.Y, NPCID.Bunny);
            for(int i = 0; i < 10; i++) {
                int num = Dust.NewDust(target.position, target.width, target.height, 107, 0f, -2f, 0, Color.White, .9f);
                Main.dust[num].noLight = true;
                Main.dust[num].noGravity = true;
                Dust expr_62_cp_0 = Main.dust[num];
                expr_62_cp_0.position.X = expr_62_cp_0.position.X + ((float)(Main.rand.Next(-10, 11) / 20) - 1.5f);
                Dust expr_92_cp_0 = Main.dust[num];
                expr_92_cp_0.position.Y = expr_92_cp_0.position.Y + ((float)(Main.rand.Next(-10, 11) / 20) - 1.5f);
                if(Main.dust[num].position != target.Center) {
                    Main.dust[num].velocity = target.DirectionTo(Main.dust[num].position) * 4f;
                }
            }
        }

        public override void Kill(int timeLeft) {
            for(int i = 0; i < 10; i++) {
                int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, 107, 0f, -2f, 0, Color.White, .9f);
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
            Main.PlaySound(0, (int)projectile.position.X, (int)projectile.position.Y);
        }

    }
}
