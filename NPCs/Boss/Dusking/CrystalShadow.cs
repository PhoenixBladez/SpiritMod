using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.Dusking
{
	public class CrystalShadow : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crystal Shadow");
			Main.projFrames[projectile.type] = 5;
		}

		public override void SetDefaults()
		{
			projectile.width = projectile.height = 12;

			projectile.hostile = true;

			projectile.penetrate = -1;
		}

		public override bool PreAI()
		{
			if (projectile.ai[0] == 0)
			{
				projectile.frame = Main.rand.Next(5);
				projectile.ai[0] = 1;
			}
			else if (projectile.ai[0] == 1)
			{
				projectile.ai[1]++;
				if (projectile.ai[1] >= 60)
				{
					projectile.velocity *= 4;
					projectile.ai[0] = 2;
				}
			}

			int newDust = Dust.NewDust(new Vector2(projectile.position.X - projectile.velocity.X * 4f + 2f, projectile.position.Y + 2f - projectile.velocity.Y * 4f), 8, 8, DustID.Shadowflame, projectile.oldVelocity.X, projectile.oldVelocity.Y, 100, default(Color), 1.25f);
			Main.dust[newDust].velocity *= -0.25f;
			Main.dust[newDust].noGravity = true;
			newDust = Dust.NewDust(new Vector2(projectile.position.X - projectile.velocity.X * 4f + 2f, projectile.position.Y + 2f - projectile.velocity.Y * 4f), 8, 8, DustID.Shadowflame, projectile.oldVelocity.X, projectile.oldVelocity.Y, 100, default(Color), 1.25f);
			Main.dust[newDust].velocity *= -0.25f;
			Main.dust[newDust].position -= projectile.velocity * 0.5f;
			Main.dust[newDust].noGravity = true;

			return false;
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			target.AddBuff(mod.BuffType("Shadowflame"), 150);
		}
	}
}
