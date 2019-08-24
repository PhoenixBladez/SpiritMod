using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class AcidGlob : ModProjectile
	{
		private int DamageAdditive;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Acid Glob");
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);
			projectile.width = 25;
			projectile.height = 25;
			projectile.friendly = true;
			projectile.ranged = true;
			projectile.penetrate = 1;
		}

		public override void AI()
		{
			DamageAdditive++;
			if (DamageAdditive % 10 == 0)
				projectile.damage++;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(mod.BuffType("Toxify"), 150);
		}

		public override void OnHitPvp(Player target, int damage, bool crit)
		{
			target.AddBuff(mod.BuffType("Toxify"), 150);
		}

	}
}
