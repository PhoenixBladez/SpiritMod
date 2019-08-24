using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.YoYoOverload.Items
{
	public class Eye : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[base.projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[base.projectile.type] = 1;

		}

		public override void SetDefaults()
		{
			base.projectile.CloneDefaults(545);
			base.projectile.damage = 25;
			base.projectile.extraUpdates = 1;
			this.aiType = 544;
		}

		public override void PostAI()
		{
			base.projectile.rotation -= 10f;
		}

		public override void AI()
		{
			base.projectile.frameCounter++;
			if (base.projectile.frameCounter >= 220)
			{
				base.projectile.frameCounter = 0;
				float num = (float)((double)Main.rand.Next(0, 100) * 0.052359877559829883);
				Vector2 vector = new Vector2((float)Math.Cos((double)num), (float)Math.Sin((double)num));
				int num2 = Projectile.NewProjectile(base.projectile.Center.X, base.projectile.Center.Y, vector.X, vector.Y, base.mod.ProjectileType("Hungry"), base.projectile.damage, (float)base.projectile.owner, 0, 0f, 0f);
				Main.projectile[num2].friendly = true;
				Main.projectile[num2].hostile = false;
				Main.projectile[num2].velocity *= 7f;
			}
		}

		public override bool PreAI()
		{
			if (Main.rand.Next(4) == 0)
			{
				int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 5, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
				Main.dust[dust].scale = 1.2f;

			}
			return true;
		}
	}
}
