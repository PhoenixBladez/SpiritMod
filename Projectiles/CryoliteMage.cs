using Microsoft.Xna.Framework;
using SpiritMod.Buffs;
using System;
using Terraria;
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
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.magic = true;
			projectile.penetrate = 2;
			projectile.timeLeft = 240;
			projectile.height = 18;
			projectile.width = 18;
			projectile.alpha = 255;
			aiType = ProjectileID.Bullet;
			projectile.extraUpdates = 1;
		}

		int counter;

		public override void AI()
		{
			projectile.velocity *= 0.99f;

			int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustID.BlueCrystalShard, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
			int dust2 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustID.DungeonSpirit, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
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
				int num2121 = Dust.NewDust(projectile.Center + new Vector2(0, (float)Math.Cos(counter / 4.2f) * 9.2f).RotatedBy(projectile.rotation), 6, 6, DustID.DungeonSpirit, 0f, 0f, 0, default, 1f);
				Main.dust[num2121].velocity *= 0f;
				Main.dust[num2121].scale *= .75f;
				Main.dust[num2121].noGravity = true;
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(2) == 0)
				target.AddBuff(ModContent.BuffType<CryoCrush>(), 300);
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 27);
			for (int i = 0; i < 10; i++) {
				int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.BlueCrystalShard, 0f, -2f, 0, default, 1.1f);
				Main.dust[num].noGravity = true;
				Dust dust = Main.dust[num];
				dust.position.X += (Main.rand.Next(-30, 31) / 20) - 1.5f;
				dust.position.Y += (Main.rand.Next(-30, 31) / 20) - 1.5f;
				if (dust.position != projectile.Center)
					dust.velocity = projectile.DirectionTo(dust.position) * 6f;
			}
		}
	}
}
