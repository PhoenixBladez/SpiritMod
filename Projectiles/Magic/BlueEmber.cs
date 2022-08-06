using SpiritMod.Buffs;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SpiritMod.Projectiles.Magic
{
	public class BlueEmber : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blue Ember");
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(326);
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Throwing;
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.friendly = true;
			Projectile.timeLeft = 30;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(5) == 0)
				target.AddBuff(ModContent.BuffType<StarFlame>(), 180);
		}

		public override bool PreAI()
		{
			if (Main.rand.NextBool(2)) {
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.UnusedWhiteBluePurple, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
				Main.dust[dust].scale = 2f;
				Main.dust[dust].noGravity = true;
			}

			return true;
		}

	}
}
