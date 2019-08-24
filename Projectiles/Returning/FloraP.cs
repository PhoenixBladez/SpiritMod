using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Returning
{
	public class FloraP : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Florarang");
		}

		public override void SetDefaults()
		{
			projectile.width = 40;
			projectile.height = 40;
			projectile.aiStyle = 3;
			projectile.friendly = true;
			projectile.ranged = true;
			projectile.magic = false;
			projectile.penetrate = 3;
			projectile.timeLeft = 600;
			projectile.extraUpdates = 1;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(5) == 0)
				target.AddBuff(mod.BuffType("VineTrap"), 180);
		}

		public override void AI()
		{
			if (Main.rand.Next(6) == 0)
			{
				int d = Dust.NewDust(projectile.position, projectile.width, projectile.height, 44);
				Main.dust[d].scale *= 0.2f;
				Main.dust[d].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
			}
		}

	}
}
