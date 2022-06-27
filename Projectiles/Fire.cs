using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class Fire : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Malevolent Wrath");
		}

		public override void SetDefaults()
		{
			Projectile.width = 80;
			Projectile.timeLeft = 20;
			Projectile.height = 80;
			Projectile.penetrate = 9;
			Projectile.ignoreWater = true;
			Projectile.alpha = 255;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.friendly = true;
		}

		public override void AI()
		{
			Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Torch, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.NextBool(3))
				target.AddBuff(BuffID.OnFire, 180);
		}
	}
}
