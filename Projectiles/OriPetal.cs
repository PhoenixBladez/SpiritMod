using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class OriPetal : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Orichalcum Petal");
			Main.projFrames[Projectile.type] = 3;
		}

		public override void SetDefaults()
		{
			Projectile.width = 20;
			Projectile.tileCollide = false;
			Projectile.height = 20;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.penetrate = 10;
			Projectile.timeLeft = 60;
		}

		public override void AI()
		{
			Projectile.frameCounter++;
			if (Projectile.frameCounter >= 8) {
				Projectile.frameCounter = 0;
				Projectile.frame = (Projectile.frame + 1) % 3;
			}
		}
	}
}
