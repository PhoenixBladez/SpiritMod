
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
			projectile.width = 4;
			projectile.height = 4;
			aiType = ProjectileID.Bullet;
			projectile.alpha = 255;
			projectile.penetrate = 2;
			projectile.friendly = true;
			projectile.ranged = true;
			projectile.hide = true;
		}

		public override void AI()
		{
			projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;

			// Don't emit dust until we've finished fading in
			if(projectile.alpha < 170) {
				for(int i = 0; i < 10; i++) {
					float x = projectile.position.X - 3 - projectile.velocity.X / 10f * i;
					float y = projectile.position.Y - 3 - projectile.velocity.Y / 10f * i;
					int num = Dust.NewDust(new Vector2(x, y), 2, 2, 107);
					Main.dust[num].velocity = Vector2.Zero;
					Main.dust[num].noGravity = true;
				}
			}

			projectile.alpha = Math.Max(0, projectile.alpha - 25);
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if(Main.rand.Next(3) == 0)
				target.AddBuff(BuffID.OnFire, 300, true);

			if(Main.rand.Next(3) == 0)
				target.AddBuff(BuffID.Frostburn, 300, true);

			if(Main.rand.Next(3) == 0)
				target.AddBuff(BuffID.CursedInferno, 300, true);

		}

	}
}
