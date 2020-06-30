using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class GunBubble : ModProjectile
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bubble");
		}

		public override void SetDefaults()
		{
			projectile.aiStyle = -1;
			projectile.width = 16;
			projectile.height = 16;
			projectile.friendly = true;
			projectile.tileCollide = true;
			projectile.hostile = false;
			projectile.penetrate = 1;
			projectile.timeLeft = 150;
			projectile.alpha = 75;
		}

		public override void AI()
		{
			projectile.velocity.X *= 0.99f;
			projectile.velocity.Y -= 0.015f;
		}
	}
}
