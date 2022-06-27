using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class Wheeze : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wheeze Gas");
			Main.projFrames[Projectile.type] = 8;
		}

		public override void SetDefaults()
		{
			Projectile.hostile = false;
			Projectile.width = 22;
			Projectile.height = 22;
			Projectile.aiStyle = 1;
			Projectile.DamageType = DamageClass.Melee;
			AIType = ProjectileID.Bullet;
			Projectile.friendly = true;
			Projectile.penetrate = 5;
			Projectile.alpha = 60;
			Projectile.timeLeft = 180;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(20, 150);
		}

		public override void OnHitPvp(Player target, int damage, bool crit)
		{
			target.AddBuff(20, 150);
		}

		public override void AI()
		{
			Projectile.alpha += 3;
			Projectile.velocity *= 0.92f;
			Projectile.spriteDirection = Projectile.direction;
			Projectile.frameCounter++;
			if (Projectile.frameCounter >= 6) {
				Projectile.frame++;
				Projectile.frameCounter = 0;
				if (Projectile.frame >= 8)
					Projectile.frame = 0;
			}
		}
	}
}
