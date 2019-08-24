using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Thrown
{
	public class ThornbloomKnifeProj : ModProjectile
	{
		int timer = 0;
		// USE THIS DUST: 261

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thornbloom Knife");
		}

		public override void SetDefaults()
		{
			projectile.width = 22;
			projectile.height = 42;

			projectile.hostile = false;
			projectile.friendly = true;
			projectile.ignoreWater = true;
			projectile.tileCollide = true;

			projectile.penetrate = 3;

			projectile.timeLeft = 160;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(BuffID.Poisoned, 200, true);
			projectile.Kill();
		}

		public override void Kill(int timeLeft)
		{
			if (Main.rand.Next(0, 4) == 0)
				Item.NewItem((int)projectile.position.X, (int)projectile.position.Y, projectile.width, projectile.height, mod.ItemType("ThornbloomKnife"), 1, false, 0, false, false);

			for (int i = 0; i < 3; i++)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 163);
			}
			Main.PlaySound(0, (int)projectile.position.X, (int)projectile.position.Y);
			int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, Main.rand.Next(-8, 8), Main.rand.Next(-8, 8), ProjectileID.SporeCloud, (int)(projectile.damage), 0, Main.myPlayer);
		}

		public override bool PreAI()
		{
			projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
			int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 163);
			Main.dust[dust].velocity *= 0.5f;
			Main.dust[dust].noGravity = true;

			timer++;
			if (timer == 20 || timer == 40 || timer == 80)
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, Main.rand.Next(-8, 8), Main.rand.Next(-8, 8), ProjectileID.SporeCloud, (int)(projectile.damage), 0, Main.myPlayer);
			else if (timer >= 100)
				timer = 0;

			return false;
		}

	}
}
