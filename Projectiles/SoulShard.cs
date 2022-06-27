using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class SoulShard : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soul Shard");
		}

		public override void SetDefaults()
		{
			Projectile.width = 10;
			Projectile.height = 20;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 300;
			Projectile.tileCollide = true;
			Projectile.aiStyle = 1;
			AIType = ProjectileID.Bullet;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 20; i++) {
				Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.Flare_Blue, (float)(Main.rand.Next(8) - 4), (float)(Main.rand.Next(8) - 4), 187);
			}
		}

	}
}