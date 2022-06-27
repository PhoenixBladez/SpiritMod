using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Putroma
{
	public class Teratoma1 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cancerous Chunk");
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.WoodenArrowHostile);
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.penetrate = 3;
		}

		public override void AI()
		{
			int num = 5;
			for (int k = 0; k < 2; k++) {
				int index2 = Dust.NewDust(Projectile.position, 1, 1, DustID.ScourgeOfTheCorruptor, 0.0f, 0.0f, 0, new Color(), 1f);
				Main.dust[index2].position = Projectile.Center - Projectile.velocity / num * (float)k;
				Main.dust[index2].scale = .5f;
				Main.dust[index2].velocity *= 0f;
				Main.dust[index2].noGravity = true;
				Main.dust[index2].noLight = false;
			}

		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);

			for (int k = 0; k < 6; k++)
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.ScourgeOfTheCorruptor, 2.5f * 1, -2.5f, 0, Color.White, 0.7f);

			Projectile.penetrate--;
			if (Projectile.penetrate <= 0)
				Projectile.Kill();
			else {
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
