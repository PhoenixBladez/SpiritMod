using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class ShadowEmber : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shadow Ember");
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(326);
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.friendly = true;
			Projectile.timeLeft = 60;
			Projectile.alpha = 255;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(5) == 0)
				target.AddBuff(BuffID.ShadowFlame, 180);
		}

		public override bool PreAI()
		{
			if (Main.rand.Next(2) == 1) {
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Shadowflame, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
				Main.dust[dust].scale = 2f;
				Main.dust[dust].noGravity = true;
			}

			return true;
		}

	}
}
