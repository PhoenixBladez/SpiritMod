using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class CursedFlameTrail : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blighted Flame Trail");
		}

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.aiStyle = 27;
			projectile.width = 20;
			projectile.height = 20;
			projectile.penetrate = -1;
			projectile.alpha = 255;
			projectile.timeLeft = 30;
		}

		public override bool PreAI()
		{
			projectile.tileCollide = false;
			if (Main.rand.Next(15) == 0)
			{
				int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 61, 0f, 0f);
				Main.dust[dust].scale = 2f;
				Main.dust[dust].noGravity = true;
			}
			return false;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(mod.BuffType("BlightedFlames"), 260, false);
		}

	}
}
