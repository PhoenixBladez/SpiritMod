using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.SteamRaider
{
	public class SteamBodyFallingProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starshock");
		}

		public override void SetDefaults()
		{
			Projectile.hostile = true;
			Projectile.height = 16;
			Projectile.width = 16;
			Projectile.friendly = false;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 900;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.penetrate--;
			if (Projectile.penetrate <= 0)
				Projectile.Kill();


			if (Projectile.velocity.X != oldVelocity.X)
				Projectile.velocity.X = oldVelocity.X * .5f;

			if (Projectile.velocity.Y != oldVelocity.Y)
				Projectile.velocity.Y = oldVelocity.Y * -1.3f;

			SoundEngine.PlaySound(SoundID.Item10, Projectile.Center);
			return false;
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(255, 255, 255, 120);
		}

		public float counter = -1440;
		public override void AI()
		{
			Projectile.velocity *= 0.97f;
			counter++;
			if (counter == 0) {
				counter = -1440;
			}
			for (int i = 0; i < 6; i++) {
				if (Projectile.velocity.X != 0) {
					float x = Projectile.Center.X - Projectile.velocity.X / 10f * (float)i;
					float y = Projectile.Center.Y - Projectile.velocity.Y / 10f * (float)i;

					int num = Dust.NewDust(Projectile.Center + new Vector2(0, (float)Math.Cos(counter / 8.2f) * 9.2f).RotatedBy(Projectile.rotation), 6, 6, DustID.DungeonSpirit, 0f, 0f, 0, default, 1f);
					Main.dust[num].velocity *= .1f;
					Main.dust[num].scale *= .7f;
					Main.dust[num].noGravity = true;
				}

			}
		}

	}
}