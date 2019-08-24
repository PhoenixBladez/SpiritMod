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
			DisplayName.SetDefault("Frost Boomerang");
		}

		public override void SetDefaults()
		{
			projectile.width = 38;
			projectile.height = 38;
			projectile.aiStyle = 3;
			projectile.friendly = true;
			projectile.melee = true;
			projectile.magic = false;
			projectile.penetrate = -1;
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
			Dust.NewDust(projectile.position, projectile.width, projectile.height, 68);
		}

	}
}
