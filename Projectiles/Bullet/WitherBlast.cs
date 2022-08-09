using Microsoft.Xna.Framework;
using SpiritMod.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Bullet
{
	public class WitherBlast : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Coiled Bullet");
		}

		public override void SetDefaults()
		{
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 300;
			Projectile.height = 4;
			Projectile.width = 4;
			Projectile.alpha = 255;
			AIType = ProjectileID.Bullet;
		}

		public override void AI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

			for (int i = 0; i < 10; i++) {
				float x = Projectile.Center.X - Projectile.velocity.X / 10f * i;
				float y = Projectile.Center.Y - Projectile.velocity.Y / 10f * i;
				int num = Dust.NewDust(new Vector2(x, y), 26, 26, DustID.RedTorch);
				Main.dust[num].position.X = x;
				Main.dust[num].position.Y = y;
				Main.dust[num].velocity = Vector2.Zero;
				Main.dust[num].noGravity = true;
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.NextBool(4))
				target.AddBuff(ModContent.BuffType<Wither>(), 180);
		}
	}
}
