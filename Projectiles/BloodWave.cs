using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SpiritMod.Projectiles
{
	public class BloodWave : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blood Wave");
		}

		public override void SetDefaults()
		{
			projectile.width = 50;
			projectile.height = 18;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.penetrate = 1;
			projectile.timeLeft = 500;
			projectile.ranged = true;
			projectile.tileCollide = true;
			projectile.aiStyle = 1;
			aiType = ProjectileID.Bullet;
		}

		public override bool PreAI()
		{
			projectile.rotation = projectile.velocity.ToRotation() + 1.57f;

			int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, 235, 0f, -2f, 0, default(Color), 2f);
			Main.dust[num].noGravity = true;
			Main.dust[num].velocity *= 0f;
			Main.dust[num].scale *= 0.5f;

			return true;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 1; i++)
			{
				int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, 235, 0f, -2f, 0, default(Color), 2f);
			}
		}

	}
}