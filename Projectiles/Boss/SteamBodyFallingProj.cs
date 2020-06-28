using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Boss
{
	public class SteamBodyFallingProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starshock");
		}

		public override void SetDefaults()
		{
			projectile.hostile = true;
			projectile.height = 16;
			projectile.width = 16;
			projectile.friendly = false;
			projectile.aiStyle = -1;
			projectile.timeLeft = 900;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			projectile.penetrate--;
			if(projectile.penetrate <= 0)
				projectile.Kill();


			if(projectile.velocity.X != oldVelocity.X)
				projectile.velocity.X = oldVelocity.X * .5f;

			if(projectile.velocity.Y != oldVelocity.Y)
				projectile.velocity.Y = oldVelocity.Y * -1.3f;

			Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 10);
			return false;
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(255, 255, 255, 120);
		}

		public float counter = -1440;
		public override void AI()
		{
			projectile.velocity *= 0.97f;
			counter++;
			if(counter == 0) {
				counter = -1440;
			}
			for(int i = 0; i < 6; i++) {
				if(projectile.velocity.X != 0) {
					float x = projectile.Center.X - projectile.velocity.X / 10f * (float)i;
					float y = projectile.Center.Y - projectile.velocity.Y / 10f * (float)i;

					int num = Dust.NewDust(projectile.Center + new Vector2(0, (float)Math.Cos(counter / 8.2f) * 9.2f).RotatedBy(projectile.rotation), 6, 6, 180, 0f, 0f, 0, default(Color), 1f);
					Main.dust[num].velocity *= .1f;
					Main.dust[num].scale *= .7f;
					Main.dust[num].noGravity = true;
				}

			}
		}

	}
}