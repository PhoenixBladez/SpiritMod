using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Arrow
{
	public class MarbleArrowStone : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Marble Shard");
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);
			projectile.width = 8;
			projectile.height = 8;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.ranged = true;
			projectile.penetrate = 3;
			projectile.timeLeft = 7200;
		}

		public override void AI()
		{
			projectile.rotation += .1f;

			int num = 5;
			for(int k = 0; k < 2; k++) {
				Dust.NewDustPerfect(projectile.Center, 222, Vector2.One.RotatedByRandom(6.28f) * 0.4f, 0, default, 0.3f);
			}

		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.Dig, (int)projectile.position.X, (int)projectile.position.Y);

			for(int k = 0; k < 6; k++) {
				Dust.NewDust(projectile.position, projectile.width, projectile.height, 233, 2.5f * 1, -2.5f, 0, Color.White, 0.7f);
			}
		}
	}
}
