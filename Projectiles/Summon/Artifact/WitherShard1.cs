using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Summon.Artifact
{
	public class WitherShard1 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wither Shard");
		}

		public override void SetDefaults()
		{
			projectile.width = 10;
			projectile.height = 16;
			projectile.hide = true;
			projectile.aiStyle = 1;
			aiType = ProjectileID.Bullet;
			projectile.alpha = 255;

			projectile.ranged = true;
			projectile.friendly = true;

			projectile.penetrate = 1;
			projectile.timeLeft = 600;
		}

		public override bool PreAI()
		{
			projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
			for (int i = 0; i < 10; i++)
			{
				float x = projectile.Center.X - projectile.velocity.X / 10f * (float)i;
				float y = projectile.Center.Y - projectile.velocity.Y / 10f * (float)i;
				int num = Dust.NewDust(new Vector2(x, y), 1, 1, 60, 0f, 0f, 0, default(Color), 1f);
				Main.dust[num].alpha = projectile.alpha;
				Main.dust[num].position.X = x;
				Main.dust[num].position.Y = y;
				Main.dust[num].velocity *= 0f;
				Main.dust[num].noGravity = true;
			}
			return true;
		}


		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(2) == 0)
				target.AddBuff(mod.BuffType("BCorrupt"), 300, true);
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(4, (int)projectile.position.X, (int)projectile.position.Y, 6);
			for (int i = 0; i < 40; i++)
			{
				int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, 60, 0f, -2f, 0, default(Color), 2f);
				Main.dust[num].noGravity = true;
				Dust expr_62_cp_0 = Main.dust[num];
				expr_62_cp_0.position.X = expr_62_cp_0.position.X + ((float)(Main.rand.Next(-50, 51) / 20) - 1.5f);
				Dust expr_92_cp_0 = Main.dust[num];
				expr_92_cp_0.position.Y = expr_92_cp_0.position.Y + ((float)(Main.rand.Next(-50, 51) / 20) - 1.5f);
				if (Main.dust[num].position != projectile.Center)
					Main.dust[num].velocity = projectile.DirectionTo(Main.dust[num].position) * 6f;

			}
		}

	}
}
