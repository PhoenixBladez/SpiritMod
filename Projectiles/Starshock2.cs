using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class Starshock2 : ModProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Starlux");

		public override void SetDefaults()
		{
			projectile.width = 6;
			projectile.height = 6;
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.alpha = 255;
			projectile.timeLeft = 60;
			projectile.penetrate = 1;
			projectile.extraUpdates = 1;
		}

		public override void AI()
		{
			for (int k = 0; k < 6; k++) {
				int index2 = Dust.NewDust(projectile.position, 4, 4, DustID.Electric, 0.0f, 0.0f, 0, new Color(), 1f);
				Main.dust[index2].position = projectile.Center - projectile.velocity / 5 * (float)k;
				Main.dust[index2].scale = .8f;
				Main.dust[index2].velocity *= 0f;
				Main.dust[index2].noGravity = true;
				Main.dust[index2].noLight = false;
			}
		}

		public override void Kill(int timeLeft)
		{
			const int Repeats = 2;

			int deviation = Main.rand.Next(0, 300);
			for (int i = 0; i < Repeats; i++) {
				float rotation = MathHelper.ToRadians(270 / Repeats * i + deviation);
				Vector2 perturbedSpeed = Vector2.Normalize(new Vector2(projectile.velocity.X, projectile.velocity.Y).RotatedBy(rotation)) * 2.5f;
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, ModContent.ProjectileType<StarTrail1>(), 12, 2, projectile.owner);
			}

			Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 9);
			for (int i = 0; i < 6; i++) {
				int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Electric, 0f, -2f, 0, default, 2f);
				Main.dust[num].noGravity = true;
				Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
				Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
				Main.dust[num].scale *= .25f;
				if (Main.dust[num].position != projectile.Center)
					Main.dust[num].velocity = projectile.DirectionTo(Main.dust[num].position) * 6f;
			}
		}
	}
}