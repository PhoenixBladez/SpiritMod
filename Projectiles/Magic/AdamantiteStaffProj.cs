using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class AdamantiteStaffProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Adamantite Blast");
		}

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.magic = true;
			projectile.width = 10;
			projectile.height = 10;
			projectile.aiStyle = -1;
			projectile.friendly = true;
			projectile.penetrate = 1;
			projectile.alpha = 255;
			projectile.timeLeft = 50;
		}

		int counter = -720;
		public override bool PreAI()
		{
			int num = 5;
			for(int k = 0; k < 10; k++) {
				int index2 = Dust.NewDust(projectile.position, 1, 1, 60, 0.0f, 0.0f, 0, new Color(), 1.3f);
				Main.dust[index2].position = projectile.Center - projectile.velocity / num * (float)k;
				Main.dust[index2].scale = .5f;
				Main.dust[index2].velocity *= 0f;
				Main.dust[index2].noGravity = true;
				Main.dust[index2].noLight = true;
			}
			projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
			return true;
		}

		public override void Kill(int timeLeft)
		{
			Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 3.75f, 3.75f, mod.ProjectileType("AdamantiteStaffProj2"), projectile.damage, 0f, projectile.owner, 0f, 0f);
			Projectile.NewProjectile(projectile.position.X, projectile.position.Y, -3.75f, -3.75f, mod.ProjectileType("AdamantiteStaffProj2"), projectile.damage, 0f, projectile.owner, 0f, 0f);

			Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 7.5f, 0f, mod.ProjectileType("AdamantiteStaffProj2"), projectile.damage, 0f, projectile.owner, 0f, 0f);
			Projectile.NewProjectile(projectile.position.X, projectile.position.Y, -7.5f, 0f, mod.ProjectileType("AdamantiteStaffProj2"), projectile.damage, 0f, projectile.owner, 0f, 0f);
			Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 0f, 7.5f, mod.ProjectileType("AdamantiteStaffProj2"), projectile.damage, 0f, projectile.owner, 0f, 0f);
			Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 0f, -7.5f, mod.ProjectileType("AdamantiteStaffProj2"), projectile.damage, 0f, projectile.owner, 0f, 0f);
		}

	}
}
