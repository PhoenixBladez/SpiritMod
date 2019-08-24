using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria;

namespace SpiritMod.Projectiles.Yoyo
{
	public class Murk : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Murk");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(546);
			projectile.extraUpdates = 1;
			aiType = 546;
		}

		public override void PostAI()
		{
			projectile.rotation -= 10f;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(BuffID.Poisoned, 180, true);
		}

	}
}
