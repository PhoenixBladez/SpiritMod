using Microsoft.Xna.Framework;
using SpiritMod.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Dusts;

namespace SpiritMod.Projectiles.Bullet
{
	public class RubberBulletProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rubber Bullet");
		}

		public override void SetDefaults()
		{
			Projectile.width = 2;
			Projectile.height = 2;

			Projectile.aiStyle = 1;
			AIType = ProjectileID.Bullet;

			Projectile.DamageType = DamageClass.Ranged;
			Projectile.friendly = true;

			Projectile.penetrate = 1;
			Projectile.timeLeft = 600;
			Projectile.light = 0.5f;
		}


		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			//	knockback *= 1.8f;
			Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height,
				ModContent.DustType<RubberBulletDust>(), Main.rand.NextFloat(-3, 3), Main.rand.NextFloat(-3, 3));
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.Cyan;
		}
	}
}
