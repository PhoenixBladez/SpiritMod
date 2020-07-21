
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Arrow
{
	public class AccursedArrow : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Accursed Arrow");
		}

		public override void SetDefaults()
		{
			projectile.width = 9;
			projectile.height = 18;

			projectile.penetrate = 2;

			projectile.aiStyle = 1;
			aiType = ProjectileID.WoodenArrowFriendly;

			projectile.ranged = true;
			projectile.friendly = true;
		}
		public override void AI()
		{
			int num384 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 75);
			Main.dust[num384].velocity *= 0f;
			Main.dust[num384].noGravity = true;
		}
		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.Dig, (int)projectile.position.X, (int)projectile.position.Y);
			for (int i = 0; i < 2; i++) {
				Dust.NewDust(projectile.position, projectile.width, projectile.height, 75);
			}
		}

	}
}
