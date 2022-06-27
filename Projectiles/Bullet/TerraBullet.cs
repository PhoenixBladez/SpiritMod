
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Bullet
{
	public class TerraBullet : ModProjectile
	{
		public override void SetStaticDefaults()
			=> DisplayName.SetDefault("Terra Bullet");

		public override void SetDefaults()
		{
			Projectile.width = 4;
			Projectile.height = 4;
			AIType = ProjectileID.Bullet;
			Projectile.alpha = 255;
			Projectile.penetrate = 2;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.hide = true;
		}

		public override void AI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

			// Don't emit dust until we've finished fading in
			if (Projectile.alpha < 170) {
				for (int i = 0; i < 10; i++) {
					float x = Projectile.position.X - 3 - Projectile.velocity.X / 10f * i;
					float y = Projectile.position.Y - 3 - Projectile.velocity.Y / 10f * i;
					int num = Dust.NewDust(new Vector2(x, y), 2, 2, DustID.TerraBlade);
					Main.dust[num].velocity = Vector2.Zero;
					Main.dust[num].noGravity = true;
				}
			}

			Projectile.alpha = Math.Max(0, Projectile.alpha - 25);
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(3) == 0)
				target.AddBuff(BuffID.OnFire, 300, true);

			if (Main.rand.Next(3) == 0)
				target.AddBuff(BuffID.Frostburn, 300, true);

			if (Main.rand.Next(3) == 0)
				target.AddBuff(BuffID.CursedInferno, 300, true);

		}

	}
}
