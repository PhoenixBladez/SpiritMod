using Microsoft.Xna.Framework;
using SpiritMod.Buffs;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class CryoFire : ModProjectile
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cryolite Explosion");
		}

		public override void SetDefaults()
		{
			projectile.width = 60;
			projectile.timeLeft = 20;
			projectile.height = 60;
			projectile.penetrate = 999;
			projectile.ignoreWater = true;
			projectile.alpha = 255;
			projectile.tileCollide = false;
			projectile.ranged = true;
			projectile.hostile = true; // Explosions deal self-damage
			projectile.friendly = true;
		}

		public override void AI()
		{
			Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 180, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if (projectile.timeLeft > 17) return null;
			return false;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(ModContent.BuffType<CryoCrush>(), 300);
		}

	}
}
