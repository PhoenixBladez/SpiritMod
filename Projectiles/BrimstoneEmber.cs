using SpiritMod.Buffs;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SpiritMod.Projectiles
{
	public class BrimstoneEmber : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Brimstone Ember");
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(326);
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.penetrate = 2;
			Projectile.friendly = true;
			Projectile.timeLeft = 100;
			Projectile.alpha = 255;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(8) == 0)
				target.AddBuff(ModContent.BuffType<StackingFireBuff>(), 120, true);
		}

		public override bool PreAI()
		{
			if (Main.rand.NextBool(2)) {
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.LifeDrain, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
				Main.dust[dust].scale = 2f;
				Main.dust[dust].noGravity = true;
			}

			return true;
		}

	}
}
