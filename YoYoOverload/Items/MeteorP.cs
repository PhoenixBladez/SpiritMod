using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.YoYoOverload.Items
{
	public class MeteorP : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[base.projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[base.projectile.type] = 1;

		}

		public override void SetDefaults()
		{
			base.projectile.CloneDefaults(544);
			base.projectile.damage = 20;
			base.projectile.extraUpdates = 5;
			this.aiType = 544;
		}

		public override void PostAI()
		{
			base.projectile.rotation -= 6f;
		}

		public override void AI()
		{
			base.projectile.frameCounter++;
			if (base.projectile.frameCounter >= 240)
			{
				base.projectile.frameCounter = 0;
				float num = (float)((double)Main.rand.Next(0, 361) * 0.017453292519943295);
				Vector2 vector = new Vector2((float)Math.Cos((double)num), (float)Math.Sin((double)num));
				int num2 = Projectile.NewProjectile(base.projectile.Center.X, base.projectile.Center.Y, vector.X, vector.Y, 328, base.projectile.damage, (float)base.projectile.owner, 0, 0f, 0f);
				Main.projectile[num2].friendly = true;
				Main.projectile[num2].hostile = false;
				Main.projectile[num2].velocity *= 7f;
				Dust.NewDust(base.projectile.position, base.projectile.width, base.projectile.height, 6, 0f, 0f, 0, default(Color), 1f);
			}
		}

		public override bool PreAI()
		{
			if (Main.rand.Next(2) == 0)
			{
				int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 6, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
				Main.dust[dust].scale = 0.6f;
				Main.dust[dust].noGravity = true;

			}
			return true;
		}
	}
}
