using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.DonatorItems
{
	class HarpyPetFeather : ModProjectile
	{
		public static readonly int _type;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tiny Feather");
			Main.projFrames[projectile.type] = 3;
		}

		public override void SetDefaults()
		{
			projectile.width = 4;
			projectile.height = 4;
			projectile.aiStyle = 0;
			projectile.friendly = true;
			projectile.penetrate = 1;
			projectile.extraUpdates = 1;
			projectile.timeLeft = 600;
			projectile.frame = Main.rand.Next(Main.projFrames[_type]);
		}

		public override void AI()
		{
			if (projectile.localAI[0] == 0)
			{
				projectile.localAI[0] = 1;
				ProjectileExtras.LookAlongVelocity(this);
			}
		}

		public override void Kill(int timeLeft)
		{
			var dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 135);
			dust.noGravity = true;
			dust.noLight = true;
		}
	}
}
