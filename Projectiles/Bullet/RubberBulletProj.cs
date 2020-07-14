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
			projectile.width = 2;
			projectile.height = 2;

			projectile.aiStyle = 1;
			aiType = ProjectileID.Bullet;

			projectile.ranged = true;
			projectile.friendly = true;

			projectile.penetrate = 1;
			projectile.timeLeft = 600;
			projectile.light = 0.5f;
		}


		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
		//	knockback *= 1.8f;
				Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height,
					ModContent.DustType<RubberBulletDust>(), Main.rand.NextFloat(-3,3), Main.rand.NextFloat(-3,3));
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.Cyan;
		}
	}
}
