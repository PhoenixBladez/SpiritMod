using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
    class BismiteShard : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Bismite Energy");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

        public override void SetDefaults() {
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = -1;
            projectile.tileCollide = true;
            projectile.timeLeft = 220;
            projectile.height = 14;
            projectile.width = 10;
            projectile.magic = true;
            aiType = ProjectileID.Bullet;
            projectile.extraUpdates = 1;
        }
        int num;
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor) {
            Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
            for(int k = 0; k < projectile.oldPos.Length; k++) {
                Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
                Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
                spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
            }
            return false;
        }
        public override void AI() {
            projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
            num += 2;
            projectile.velocity *= 0.98f;
            int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 167, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
            int dust2 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 167, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
            Main.dust[dust].noGravity = true;
            Main.dust[dust2].noGravity = true;
            Main.dust[dust2].velocity *= 0f;
            Main.dust[dust2].velocity *= 0f;
            Main.dust[dust2].scale = 0.9f;
            Main.dust[dust].scale = 0.9f;
        }
        public override Color? GetAlpha(Color lightColor) {
            return new Color(155 - (int)(num / 3 * 2), 204 - (int)(num / 3 * 2), 92 - (int)(num / 3 * 2), 255 - num);
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            if(Main.rand.Next(5) == 0)
                target.AddBuff(ModContent.BuffType<FesteringWounds>(), 180);
        }

    }
}
