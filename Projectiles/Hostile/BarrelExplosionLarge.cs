using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Hostile
{
	public class BarrelExplosionLarge : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Explosion");
			Main.projFrames[base.projectile.type] = 7;
		}

		public override void SetDefaults()
		{
			projectile.hostile = true;
			projectile.width = 196;
			projectile.height = 200;
			projectile.timeLeft = 225;
			projectile.tileCollide = false;
			projectile.friendly = true;
			projectile.penetrate = -1;
		}

		public override void AI()
		{
			projectile.frameCounter++;
			if (projectile.frameCounter >= 4) {
				projectile.frame = (projectile.frame + 1);
				projectile.frameCounter = 0;
				if (projectile.frame > 6)
				{
					projectile.active = false;
				}
			}
		}

		//public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		//{
		//    Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
		//    for (int k = 0; k < projectile.oldPos.Length; k++)
		//    {
		//        Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
		//        Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
		//        spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
		//    }
		//    return true;
		//}

	}
}