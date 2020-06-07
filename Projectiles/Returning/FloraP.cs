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
				target.AddBuff(ModContent.BuffType<VineTrap>(), 180);
		}

		public override void AI()
		{
			if (Main.rand.Next(2) == 0)
			{
				int d = Dust.NewDust(projectile.position, projectile.width, projectile.height, 39);
				Main.dust[d].scale *= 0.8f;
				Main.dust[d].noGravity = true;
				Main.dust[d].velocity *= 0f;
			}
		}

	}
}
