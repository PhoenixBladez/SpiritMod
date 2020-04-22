using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Returning
{
	public class FrostBoomerang : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frigid Boomerang");
		}

		public override void SetDefaults()
		{
			projectile.width = 34;
			projectile.height = 34;
			projectile.aiStyle = 3;
			projectile.friendly = true;
			projectile.melee = true;
			projectile.magic = false;
			projectile.penetrate = -1;
            projectile.scale *= .95f;
			projectile.timeLeft = 700;
			projectile.extraUpdates = 1;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(6) == 0)
				target.AddBuff(BuffID.Frostburn, 120, true);
		}

		public override void AI()
		{
			int d = Dust.NewDust(projectile.position, projectile.width, projectile.height, 68);
			Main.dust[d].noGravity = true;
			Main.dust[d].scale = Main.rand.NextFloat(.4f, .6f);
			Main.dust[d].velocity *= 0f;
		}

	}
}
