using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Thrown
{
	public class ChaosPearl : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chaos Pearl");
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.Shuriken);
			projectile.width = 16;
			projectile.height = 16;
			projectile.penetrate = 2;
			projectile.damage = 0;
			//projectile.thrown = false;
			projectile.friendly = false;
			projectile.hostile = false;
		}

		public override bool PreAI()
		{
			projectile.rotation += 0.1f;

			return true;
		}

		public override void Kill(int timeLeft)
		{
			Player player = Main.player[projectile.owner];
			Main.PlaySound((int)projectile.position.X, (int)projectile.position.Y, 27);
			//Main.PlaySound(0, (int)projectile.position.X, (int)projectile.position.Y, 1);
			for (int num424 = 0; num424 < 10; num424++)
			{
				Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 62, projectile.velocity.X * 0.1f, projectile.velocity.Y * 0.1f, 0, default(Color), 0.75f);
			}
			Main.player[projectile.owner].Teleport(new Vector2(projectile.position.X, projectile.position.Y - 32), 2, 0);
			Main.player[projectile.owner].AddBuff(88, 180);
			if (Main.player[projectile.owner].FindBuffIndex(88)>=0)
			{
				player.statLife -= (player.statLifeMax2 / 7);
				if (player.statLife <= 0)
				{
					player.statLife = 1;
					player.AddBuff(BuffID.OnFire, 120);
					//    player.KillMe(9999, 1, true, "'s head appeared where their legs should be.");
				}
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			ProjectileExtras.DrawAroundOrigin(projectile.whoAmI, lightColor);
			return false;
		}

	}
}
