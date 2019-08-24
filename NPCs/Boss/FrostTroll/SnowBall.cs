using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.FrostTroll
{
	public class SnowBall : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Snowball");
		}

		public override void SetDefaults()
		{
			projectile.width = 26;
			projectile.height = 34;
			projectile.friendly = false;
			projectile.hostile = true;
			projectile.penetrate = -1;
			projectile.timeLeft = 120;
			projectile.tileCollide = true;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 2; i++)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 76);
			}
			Main.PlaySound(0, (int)projectile.position.X, (int)projectile.position.Y);

			Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 6, -2, 128, projectile.damage, projectile.knockBack, Main.myPlayer);
			Projectile.NewProjectile(projectile.position.X, projectile.position.Y, -6, -2, 128, projectile.damage, projectile.knockBack, Main.myPlayer);
		}

		public override void AI()
		{
			int timer = 0;

			projectile.rotation += 0.3f;


			for (int i = 1; i <= 3; i++)
			{
				if (Main.rand.Next(4) == 0)
					Dust.NewDust(projectile.position, projectile.width, projectile.height, 76);
			}
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.Next(4) == 0)
				target.AddBuff(BuffID.Frostburn, 180, true);
		}
	}
}