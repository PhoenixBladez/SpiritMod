using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

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
			projectile.width = 10;
			projectile.height = 20;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.penetrate = 1;
			projectile.timeLeft = 300;
			projectile.tileCollide = true;
			projectile.aiStyle = 1;
			aiType = ProjectileID.Bullet;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 20; i++)
			{
				Dust.NewDust(projectile.Center, projectile.width, projectile.height, 187, (float)(Main.rand.Next(8) - 4), (float)(Main.rand.Next(8) - 4), 187);
			}
		}

	}
}