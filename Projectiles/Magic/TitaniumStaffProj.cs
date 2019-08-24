using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class TitaniumStaffProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Titanium Bolt");
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
		}

		public override bool PreAI()
		{
			for (int i = 0; i < 4; i++)
			{
				float x = projectile.Center.X - projectile.velocity.X / 10f * (float)i;
				float y = projectile.Center.Y - projectile.velocity.Y / 10f * (float)i;
				int num = Dust.NewDust(new Vector2(x, y), 26, 26, DustID.SilverCoin, 0f, 0f, 0, default(Color), 1f);
				Main.dust[num].alpha = projectile.alpha;
				Main.dust[num].position.X = x;
				Main.dust[num].position.Y = y;
				Main.dust[num].velocity *= 0f;
			}
			return true;
		}

		public override void Kill(int timeLeft)
		{
			Projectile.NewProjectile(projectile.position.X - 50, projectile.position.Y - 1000, 0f, 30f, mod.ProjectileType("TitaniumStaffProj2"), projectile.damage, 0f, projectile.owner, 0f, 0f);
			Projectile.NewProjectile(projectile.position.X, projectile.position.Y - 1000, 0f, 30f, mod.ProjectileType("TitaniumStaffProj2"), projectile.damage, 0f, projectile.owner, 0f, 0f);
			Projectile.NewProjectile(projectile.position.X + 50, projectile.position.Y - 1000, 0f, 30f, mod.ProjectileType("TitaniumStaffProj2"), projectile.damage, 0f, projectile.owner, 0f, 0f);
			Projectile.NewProjectile(projectile.position.X - 1000, projectile.position.Y - 50, 30f, 0f, mod.ProjectileType("TitaniumStaffProj2"), projectile.damage, 0f, projectile.owner, 0f, 0f);
			Projectile.NewProjectile(projectile.position.X - 1000, projectile.position.Y + 50, 30f, 0f, mod.ProjectileType("TitaniumStaffProj2"), projectile.damage, 0f, projectile.owner, 0f, 0f);
			Projectile.NewProjectile(projectile.position.X - 1000, projectile.position.Y, 30f, 0f, mod.ProjectileType("TitaniumStaffProj2"), projectile.damage, 0f, projectile.owner, 0f, 0f);
		}

	}
}
