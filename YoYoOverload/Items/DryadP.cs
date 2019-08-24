using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.YoYoOverload.Items
{
	public class DryadP : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[base.projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[base.projectile.type] = 1;

		}

		public override void SetDefaults()
		{
			base.projectile.CloneDefaults(542);
			base.projectile.damage = 16;
			base.projectile.extraUpdates = 1;
			this.aiType = 542;
		}

		public override void PostAI()
		{
			base.projectile.rotation -= 10f;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(8) == 0)
			{
				target.AddBuff(186, 160, true);
			}
		}
	}
}
