using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Gun.Basking_Shark
{
	public class Basking_Shark_Projectile : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Basking Shark");
		}
		public override void SetDefaults()
		{
			projectile.width = 16;
			projectile.height = 16;
			projectile.aiStyle = 1;
			aiType = ProjectileID.Bullet;
			projectile.hide = false;
			projectile.ranged = true;
			projectile.friendly = true;
			projectile.tileCollide = true;
			projectile.scale = 1f;
		}
		public override void AI()
		{
			projectile.velocity.Y += 0.2f;
			Player player = Main.player[projectile.owner];
			if ((double)projectile.ai[0] == 0.0)
			{
				projectile.ai[0] = 1f;
				Main.PlaySound(3, (int)projectile.position.X, (int)projectile.position.Y, 1, 1f, 0.4f);
				for (int index = 0; index < 8; ++index)
				{
					Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 5, 0.0f, 0.0f, 0, Color.Pink, 1f);
					dust.velocity *= 1f;
					dust.velocity += projectile.velocity * 0.65f;
					dust.scale = 0.8f;
					dust.fadeIn = 1.1f;
					dust.noGravity = true;
					dust.noLight = false;
					dust.position += dust.velocity * 3f;
				}
			}

			for (int index1 = 0; index1 < 4; ++index1)
			{
				int index2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 16, 16, 5, projectile.velocity.X, projectile.velocity.Y, 50, Color.Pink, 0.7f);
				Main.dust[index2].position = (Main.dust[index2].position + projectile.Center) / 2f;
				Main.dust[index2].noGravity = true;
				Main.dust[index2].velocity *= 0.3f;
				Main.dust[index2].scale *= 1f;
			}
			for (int index1 = 0; index1 < 2; ++index1)
			{
				int index2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 16, 16, 5, projectile.velocity.X, projectile.velocity.Y, 50, Color.Pink, 0.4f);
				Main.dust[index2].position = (Main.dust[index2].position + projectile.Center * 5f) / 6f;
				Main.dust[index2].velocity *= 0.1f;
				Main.dust[index2].noGravity = true;
				Main.dust[index2].fadeIn = 0.9f;
				Main.dust[index2].scale *= 1f;
			}
		}
		public override void Kill(int timeLeft)
		{
		}
	}
}