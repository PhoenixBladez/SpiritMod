using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class BrimstoneEmber : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Brimstone Ember");
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(326);
			projectile.hostile = false;
			projectile.magic = true;
			projectile.width = 14;
			projectile.height = 14;
			projectile.penetrate = 2;
			projectile.friendly = true;
			projectile.timeLeft = 100;
			projectile.alpha = 255;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(8) == 0)
				target.AddBuff(mod.BuffType("StackingFireBuff"), 120, true);
		}

		public override bool PreAI()
		{
			if (Main.rand.Next(2) == 1)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 235, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
				Main.dust[dust].scale = 2f;
				Main.dust[dust].noGravity = true;
			}

			return true;
		}

	}
}
