using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class AquaSphere : ModProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Aqua Sphere");

		public override void SetDefaults()
		{
			Projectile.width = 34;
			Projectile.height = 40;
			Projectile.friendly = true;
			Projectile.penetrate = 3;
			Projectile.timeLeft = 120;
			Projectile.alpha = 0;
			Projectile.tileCollide = false;
		}

		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.NPCDeath6, Projectile.position);
			for (int i = 0; i < 2; i++)
			{
				int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Flare_Blue);
				Main.dust[d].noGravity = true;
			}

			SoundEngine.PlaySound(SoundID.Dig, Projectile.position);

			Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.position.X, Projectile.position.Y, 6, -4, ModContent.ProjectileType<AquaSphere2>(), Projectile.damage / 2, Projectile.knockBack, Main.myPlayer);
			Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.position.X, Projectile.position.Y, -6, -4, ModContent.ProjectileType<AquaSphere2>(), Projectile.damage / 2, Projectile.knockBack, Main.myPlayer);
			Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.position.X, Projectile.position.Y, 6, 4, ModContent.ProjectileType<AquaSphere2>(), Projectile.damage / 2, Projectile.knockBack, Main.myPlayer);
			Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.position.X, Projectile.position.Y, -6, 4, ModContent.ProjectileType<AquaSphere2>(), Projectile.damage / 2, Projectile.knockBack, Main.myPlayer);
		}

		public override void AI()
		{
			Projectile.alpha += 3;
			Projectile.scale += .011f;

			if (Projectile.alpha >= 160)
				Projectile.Kill();

			for (int i = 0; i < 5; i++)
			{
				Vector2 vector2 = Vector2.UnitY.RotatedByRandom(MathHelper.TwoPi) * new Vector2(Projectile.height, Projectile.height) * Projectile.scale * 1.45f / 2f;
				int index = Dust.NewDust(Projectile.Center + vector2, 0, 0, DustID.Flare_Blue, 0.0f, 0.0f, 0, new Color(), 1f);
				Main.dust[index].position = Projectile.Center + vector2;
				Main.dust[index].velocity = Vector2.Zero;
				Main.dust[index].noGravity = true;
			}
			Lighting.AddLight(Projectile.position, 0.1f, 0.2f, 0.3f);
		}
	}
}