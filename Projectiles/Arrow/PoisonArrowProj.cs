
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Arrow
{
	public class PoisonArrowProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Poisoned Arrow");
		}

		public override void SetDefaults()
		{
			Projectile.width = 9;
			Projectile.height = 17;

			Projectile.penetrate = 2;

			Projectile.aiStyle = 1;
			AIType = ProjectileID.WoodenArrowFriendly;

			Projectile.DamageType = DamageClass.Ranged;
			Projectile.friendly = true;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 2; i++) {
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Moss_Green);
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(5) == 0)
				target.AddBuff(BuffID.Poisoned, 180);
		}
	}
}
