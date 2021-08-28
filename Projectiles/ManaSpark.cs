using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SpiritMod.Projectiles
{
	public class ManaSpark : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Angelic Spark");
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(326);
			projectile.hostile = false;
			projectile.width = 5;
			projectile.height = 14;
			projectile.magic = true;
			projectile.friendly = true;
			projectile.timeLeft = 180;
			projectile.alpha = 255;
			projectile.penetrate = 2;
		}

		public override bool PreAI()
		{
			if (Main.rand.Next(2) == 1) {
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.CopperCoin, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
				Main.dust[dust].scale = 2f;
				Main.dust[dust].noGravity = true;
			}
			return true;
		}
	}
}
