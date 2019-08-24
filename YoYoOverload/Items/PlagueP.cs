using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.YoYoOverload.Items
{
	public class PlagueP : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[base.projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[base.projectile.type] = 0;

		}

		public override void SetDefaults()
		{
			base.projectile.CloneDefaults(542);
			projectile.ranged = false;
			projectile.magic = false;
			projectile.melee = true;
			base.projectile.damage = 16;
			base.projectile.extraUpdates = 1;
			this.aiType = 542;
		}

		public override void PostAI()
		{
			base.projectile.rotation -= 10f;
		}

		public override bool PreAI()
		{
			if (Main.rand.Next(2) == 0)
			{
				int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 5, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
				Main.dust[dust].scale = 0.9f;
			}
			return true;
		}
	}
}
