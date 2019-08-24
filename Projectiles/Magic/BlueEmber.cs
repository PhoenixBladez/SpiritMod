using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class BlueEmber : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blue Ember");
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(326);
			projectile.hostile = false;
			projectile.thrown = true;
			projectile.width = 14;
			projectile.height = 14;
			projectile.friendly = true;
			projectile.timeLeft = 30;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(5) == 0)
				target.AddBuff(mod.BuffType("StarFlame"), 180);
		}

		public override bool PreAI()
		{
			if (Main.rand.Next(2) == 1)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 206, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
				Main.dust[dust].scale = 2f;
				Main.dust[dust].noGravity = true;
			}

			return true;
		}

	}
}
