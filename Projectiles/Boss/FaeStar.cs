using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Boss
{
	public class FaeStar : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fae Star");
		}

		public override void SetDefaults()
		{
			projectile.hostile = true;
			projectile.magic = true;
			projectile.width = 20;
			projectile.height = 20;
			projectile.friendly = false;
			projectile.aiStyle = 1;
			aiType = ProjectileID.Bullet;
			projectile.tileCollide = false;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			projectile.penetrate--;
			if (projectile.penetrate <= 0)
			{
				projectile.Kill();
				Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 14);
			}
		}

		public override void AI()
		{
			if (Main.rand.Next(10) == 0)
			{
				Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height,
					62, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
			}
		}

	}
}