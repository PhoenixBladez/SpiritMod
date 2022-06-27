
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.Audio;
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
			Projectile.width = 9;
			Projectile.height = 18;

			Projectile.penetrate = 2;

			Projectile.aiStyle = 1;
			AIType = ProjectileID.WoodenArrowFriendly;

			Projectile.DamageType = DamageClass.Ranged;
			Projectile.friendly = true;
		}

		public override void AI()
		{
			int num384 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.CursedTorch);
			Main.dust[num384].velocity *= 0f;
			Main.dust[num384].noGravity = true;
		}

		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
			for (int i = 0; i < 2; i++) {
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.CursedTorch);
			}
		}
	}
}
