using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Projectiles.Boss
{
	public class BoneWave : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Arcane Wave");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

		public override void SetDefaults()
		{
			projectile.width = 30;
			projectile.height = 48;
			projectile.friendly = false;
			projectile.hostile = true;
			projectile.penetrate = 10;
			projectile.timeLeft = 1000;
			projectile.tileCollide = false;
			projectile.aiStyle = 1;
			aiType = ProjectileID.Bullet;
		}
        int timer;
        int colortimer;
        public override bool PreAI()
        {
            timer++;
            if (timer <= 50)
            {
                colortimer++;
            }
            if (timer > 50)
            {
                colortimer--;
            }
            if (timer >= 100)
            {
                timer = 0;
            }
            return true;
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
            return false;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(150 + colortimer * 2, 150 + colortimer * 2, 150 + colortimer * 2, 100);
        }
    }
}