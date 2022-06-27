using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SpiritMod.Projectiles.Sword
{

	public class HarpyFeather : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Harpy Feather");
		}

		public override void SetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 20;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.tileCollide = true;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 500;
			Projectile.extraUpdates = 1;
			Projectile.ignoreWater = true;
		}

		public override void AI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation() + 1.57f;

			int num = 5;
			for (int k = 0; k < 6; k++) {
				int index2 = Dust.NewDust(Projectile.position, 4, 4, DustID.Flare_Blue, 0.0f, 0.0f, 0, new Color(), .6f);
				Main.dust[index2].position = Projectile.Center - Projectile.velocity / num * (float)k;
				Main.dust[index2].scale = .8f;
				Main.dust[index2].velocity *= 0f;
				Main.dust[index2].noGravity = true;
				Main.dust[index2].noLight = false;
			}
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 10; i++) {
				int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Flare_Blue);
				Main.dust[d].noGravity = true;
			}
		}

	}
}