using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework;

namespace SpiritMod.Projectiles.Arrow
{
	public class EyeArrow : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Eye Arrow");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.BoneArrow);
			AIType = ProjectileID.BoneArrow;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Blood);
			SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
		}

		int num = 3;

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			for (int k = 0; k < 6; k++)
			{
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Blood, 2.5f * 1, -2.5f, 0, Color.White, 0.7f);
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Blood, 2.5f * 1, -2.5f, 0, Color.White, 0.7f);
			}

			SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
			num--;
			if (num <= 0)
				Projectile.Kill();
			else
			{
				Projectile.ai[0] += 0.1f;
				if (Projectile.velocity.X != oldVelocity.X)
					Projectile.velocity.X = -oldVelocity.X;

				if (Projectile.velocity.Y != oldVelocity.Y)
					Projectile.velocity.Y = -oldVelocity.Y;

				Projectile.velocity *= 0.75f;
			}
			return false;
		}
	}
}
