using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class Terrorflame : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Terrorflame");
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(326);
			projectile.hostile = false;
			projectile.magic = true;
			projectile.width = 14;
			projectile.height = 14;
			projectile.friendly = true;
			projectile.timeLeft = 60;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(5) == 0)
				target.AddBuff(mod.BuffType("Wither"), 180);
		}

		public override bool PreAI()
		{
			if (Main.rand.Next(2) == 1)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 60, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
				Main.dust[dust].scale = 2f;
				Main.dust[dust].noGravity = true;
			}

			return true;
		}

	}
}
