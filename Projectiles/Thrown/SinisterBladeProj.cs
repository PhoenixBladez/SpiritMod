using SpiritMod.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Thrown
{
	public class SinisterBladeProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sinister Blade");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 9;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.width = 7;
			projectile.height = 14;
			projectile.aiStyle = 113;
			projectile.friendly = true;
			projectile.melee = true;
			projectile.penetrate = 5;
			projectile.timeLeft = 600;
			projectile.alpha = 255;
			projectile.extraUpdates = 1;
			projectile.light = 0;
			aiType = ProjectileID.ThrowingKnife;
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.NPCKilled, (int)projectile.position.X, (int)projectile.position.Y, 6);
			for (int I = 0; I < 8; I++)
				Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 5, projectile.oldVelocity.X * 0.2f, projectile.oldVelocity.Y * 0.2f);
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(3) == 0)
				target.AddBuff(ModContent.BuffType<Wither>(), 160);
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