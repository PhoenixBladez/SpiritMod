using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class TeslaSpikeProjectile : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tesla Spike");
			Main.projFrames[Projectile.type] = 4;
		}

		public override void SetDefaults()
		{
			Projectile.width = 32;
			Projectile.height = 14;
			Projectile.aiStyle = 1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			AIType = ProjectileID.MartianTurretBolt;
			Projectile.timeLeft = 150;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(Mod.Find<ModBuff>("ElectrifiedV2").Type, 540, true);
		}
		public override void AI()
		{
			if (Main.rand.NextBool(8))
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Electric);
		}
	}
}
