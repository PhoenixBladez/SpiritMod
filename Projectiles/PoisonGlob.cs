
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class PoisonGlob : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Poison Glob");
		}

		public override void SetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 16;

			Projectile.penetrate = 1;

			Projectile.aiStyle = 1;
			AIType = ProjectileID.WoodenArrowHostile;

			Projectile.DamageType = DamageClass.Ranged;
			Projectile.friendly = false;
			Projectile.hostile = true;
		}

		public override void AI()
		{
			for (int i = 0; i < 2; i++) {
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.TerraBlade);
			}
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 2; i++) {
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.TerraBlade);
			}
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			target.AddBuff(BuffID.Poisoned, 180);
		}
	}
}
