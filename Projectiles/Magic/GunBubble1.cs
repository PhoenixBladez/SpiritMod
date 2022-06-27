﻿using Terraria.Audio;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;

namespace SpiritMod.Projectiles.Magic
{
	public class GunBubble1 : ModProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Bubble");

		public override void SetDefaults()
		{
			Projectile.aiStyle = -1;
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 1;
			Projectile.timeLeft = 150;
			Projectile.alpha = 110;
		}

		public override void AI()
		{
			if (Projectile.timeLeft == 150)
				Projectile.scale = Main.rand.NextFloat(0.7f, 1.3f);
			Projectile.velocity.X *= 0.99f;
			Projectile.velocity.Y -= 0.015f;
		}

		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item, (int)Projectile.position.X, (int)Projectile.position.Y, 54);
			for (int i = 0; i < 20; i++) {
				int num = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.FungiHit, 0f, -2f, 0, default, 2f);
				Main.dust[num].noGravity = true;
				Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
				Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
				Main.dust[num].scale *= .3f;
				if (Main.dust[num].position != Projectile.Center)
					Main.dust[num].velocity = Projectile.DirectionTo(Main.dust[num].position) * 7f;
			}
		}
	}
}
