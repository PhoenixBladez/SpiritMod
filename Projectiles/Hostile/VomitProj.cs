
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Hostile
{
	public class VomitProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bloody Vomit");
		}

		public override void SetDefaults()
		{
			projectile.width = 6;
			projectile.height = 6;
			aiType = ProjectileID.Flames;
			projectile.alpha = 255;
			projectile.timeLeft = 18;
			projectile.penetrate = 4;
			projectile.hostile = true;
			projectile.friendly = false;
			projectile.extraUpdates = 3;
		}

		public override void AI()
		{
			projectile.rotation += 0.1f;
			for (int i = 0; i < 4; i++) {
				int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 5, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
				Main.dust[dust].scale = Main.rand.NextFloat(.8f, 1.5f);
				Main.dust[dust].noGravity = true;

			}
		}

	}
}
