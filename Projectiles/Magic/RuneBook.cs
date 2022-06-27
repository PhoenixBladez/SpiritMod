using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class RuneBook : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rune Book");
			Main.projFrames[Projectile.type] = 4;
		}

		public override void SetDefaults()
		{
			Projectile.width = 28;
			Projectile.height = 32;
			Projectile.timeLeft = 100;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.ignoreWater = true;
			Projectile.extraUpdates = 1;
		}

		public override void AI()
		{
			Projectile.frameCounter++;
			if (Projectile.frameCounter >= 6) {
				Projectile.frame++;
				Projectile.frameCounter = 0;
				if (Projectile.frame >= 4)
					Projectile.frame = 0;
			}
		}

		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item, (int)Projectile.position.X, (int)Projectile.position.Y, 27);
			for (int k = 0; k < 15; k++) {
				Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.UnusedWhiteBluePurple, Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f);
			}
			for (int h = 0; h < 7; h++) {
				Vector2 vel = new Vector2(0, -1);
				float rand = Main.rand.NextFloat() * (float)Math.PI;
				vel = vel.RotatedBy(rand);
				vel *= 6f;
				Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y + 20, vel.X, vel.Y, ModContent.ProjectileType<Rune>(), Projectile.damage, 0, Projectile.owner);
			}
		}

	}
}
