using Microsoft.Xna.Framework;
using SpiritMod.Buffs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Thrown
{
	public class InfernalJavelin : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Infernal Javelin");
		}

		public override void SetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.penetrate = 1;
			Projectile.alpha = 255;
		}

		public override bool PreAI()
		{
			if (Main.rand.Next(2) == 0) {
				int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Torch, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
				Main.dust[dust].scale *= 1.5f;
				Main.dust[dust].noGravity = true;
			}

			if (Projectile.alpha > 0)
				Projectile.alpha -= 25;

			if (Projectile.alpha < 0)
				Projectile.alpha = 0;


			Projectile.ai[1] += 1f;
			if (Projectile.ai[1] >= 45f) {
				Projectile.ai[1] = 45f;
				Projectile.velocity.X = Projectile.velocity.X * 0.98F;
				Projectile.velocity.Y = Projectile.velocity.Y + 0.35F;
			}
			Projectile.rotation = Projectile.velocity.ToRotation() + 1.57079637f;

			return false;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(ModContent.BuffType<StackingFireBuff>(), 300);
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if (targetHitbox.Width > 8 && targetHitbox.Height > 8)
				targetHitbox.Inflate(-targetHitbox.Width / 8, -targetHitbox.Height / 8);

			return projHitbox.Intersects(targetHitbox);
		}

		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Dig, (int)Projectile.position.X, (int)Projectile.position.Y, 1);
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
			for (int i = 0; i < 2; ++i) {
				int randFire = Main.rand.Next(3);
				int newProj = Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y,
					Main.rand.Next(-1000, 1000) / 100, Main.rand.Next(-8, 8),
					ProjectileID.GreekFire1 + randFire, 20, 0, Projectile.owner);
				Main.projectile[newProj].hostile = false;
				Main.projectile[newProj].friendly = true;
			}
		}

	}
}
