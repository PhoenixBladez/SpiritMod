using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

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
			projectile.width = 16;
			projectile.height = 20;
			projectile.friendly = true;
			projectile.melee = true;
			projectile.tileCollide = true;
			projectile.penetrate = 1;
			projectile.timeLeft = 500;
			projectile.extraUpdates = 1;
			projectile.ignoreWater = true;
		}

		public override void AI()
		{
			projectile.rotation = projectile.velocity.ToRotation() + 1.57f;

			int num = 5;
			for(int k = 0; k < 6; k++) {
				int index2 = Dust.NewDust(projectile.position, 4, 4, 187, 0.0f, 0.0f, 0, new Color(), .6f);
				Main.dust[index2].position = projectile.Center - projectile.velocity / num * (float)k;
				Main.dust[index2].scale = .8f;
				Main.dust[index2].velocity *= 0f;
				Main.dust[index2].noGravity = true;
				Main.dust[index2].noLight = false;
			}
		}

		public override void Kill(int timeLeft)
		{
			for(int i = 0; i < 10; i++) {
				int d = Dust.NewDust(projectile.position, projectile.width, projectile.height, 187);
				Main.dust[d].noGravity = true;
			}
		}

	}
}