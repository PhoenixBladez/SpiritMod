using Microsoft.Xna.Framework;
using SpiritMod.Buffs;
using SpiritMod.Buffs.DoT;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	class CryoliteMage : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cryo Blast");
		}

		public override void SetDefaults()
		{
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.penetrate = 2;
			Projectile.timeLeft = 240;
			Projectile.height = 18;
			Projectile.width = 18;
			Projectile.alpha = 255;
			AIType = ProjectileID.Bullet;
			Projectile.extraUpdates = 1;
		}

		int counter;

		public override void AI()
		{
			Projectile.velocity *= 0.99f;

			int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.BlueCrystalShard, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
			int dust2 = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.DungeonSpirit, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
			Main.dust[dust].noGravity = true;
			Main.dust[dust2].noGravity = true;
			Main.dust[dust2].velocity *= 0.6f;
			Main.dust[dust2].velocity *= 0.1f;
			Main.dust[dust2].scale = 1.2f;
			Main.dust[dust].scale = .8f;

			counter++;
			if (counter >= 1440)
				counter = -1440;
			for (int i = 0; i < 4; i++)
			{
				int num2121 = Dust.NewDust(Projectile.Center + new Vector2(0, (float)Math.Cos(counter / 4.2f) * 9.2f).RotatedBy(Projectile.rotation), 6, 6, DustID.DungeonSpirit, 0f, 0f, 0, default, 1f);
				Main.dust[num2121].velocity *= 0f;
				Main.dust[num2121].scale *= .75f;
				Main.dust[num2121].noGravity = true;
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.NextBool(2))
				target.AddBuff(ModContent.BuffType<CryoCrush>(), 300);
		}

		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item27, Projectile.Center);
			for (int i = 0; i < 10; i++) {
				int num = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.BlueCrystalShard, 0f, -2f, 0, default, 1.1f);
				Main.dust[num].noGravity = true;
				Dust dust = Main.dust[num];
				dust.position.X += (Main.rand.Next(-30, 31) / 20) - 1.5f;
				dust.position.Y += (Main.rand.Next(-30, 31) / 20) - 1.5f;
				if (dust.position != Projectile.Center)
					dust.velocity = Projectile.DirectionTo(dust.position) * 6f;
			}
		}
	}
}
