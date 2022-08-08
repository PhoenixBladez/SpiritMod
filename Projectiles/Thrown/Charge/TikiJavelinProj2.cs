using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Thrown.Charge
{
	public class TikiJavelinProj2 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tiki Javelin");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 9;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			Projectile.width = 8;
			Projectile.height = 36;
			Projectile.aiStyle = 1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.penetrate = 3;
			Projectile.timeLeft = 600;
			Projectile.extraUpdates = 1;
			Projectile.light = 0;
			AIType = ProjectileID.ThrowingKnife;
		}
		public override void AI()
		{
			if (Main.rand.NextBool(8)) {
				int d = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Torch, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
				Main.dust[d].scale *= 1.85f;
				Main.dust[d].noGravity = true;
			}
		}

		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
			Vector2 vector9 = Projectile.position;
			Vector2 value19 = (Projectile.rotation - 1.57079637f).ToRotationVector2();
			vector9 += value19 * 16f;
			for (int num257 = 0; num257 < 20; num257++) {
				int newDust = Dust.NewDust(vector9, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 0, default, 1f);
				Main.dust[newDust].position = (Main.dust[newDust].position + Projectile.Center) / 2f;
				Main.dust[newDust].velocity += value19 * 2f;
				Main.dust[newDust].velocity *= 0.5f;
				Main.dust[newDust].noGravity = true;
				vector9 -= value19 * 8f;
			}

		}
	}
}