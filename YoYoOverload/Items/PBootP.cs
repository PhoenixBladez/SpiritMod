using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.YoYoOverload.Items
{
	public class PBootP : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[base.projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[base.projectile.type] = 1;

		}

		public override void SetDefaults()
		{
			base.projectile.CloneDefaults(552);
			base.projectile.damage = 42;
			base.projectile.extraUpdates = 1;
			this.aiType = 552;
		}

		public override void PostAI()
		{
			base.projectile.rotation -= 10f;
		}
		public override bool PreAI()
		{
			if (Main.rand.Next(2) == 0)
			{
				int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustID.GoldCoin, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
				Main.dust[dust].scale = 0.6f;
				Main.dust[dust].noGravity = true;

			}
			return true;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(4) == 0)
			{
				target.AddBuff(72, 180, true);
			}
		}
	}
}
