
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Masticator
{
	public class CorruptVomitProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Corrupt Vomit");
		}

		public override void SetDefaults()
		{
			Projectile.width = 6;
			Projectile.height = 6;
			AIType = ProjectileID.Flames;
			Projectile.alpha = 255;
			Projectile.timeLeft = 30;
			Projectile.penetrate = 4;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.extraUpdates = 3;
		}

		public override void AI()
		{
			Projectile.rotation += 0.1f;
			for (int i = 0; i < 4; i++) {
				int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.ScourgeOfTheCorruptor, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
				Main.dust[dust].scale = Main.rand.NextFloat(.4f, .9f);
				Main.dust[dust].noGravity = true;

			}
			Vector2 currentSpeed = new Vector2(Projectile.velocity.X, Projectile.velocity.Y);
			if (Main.rand.NextBool(2)) {
				Projectile.velocity = currentSpeed.RotatedBy(Main.rand.Next(-1, 2) * (Math.PI / 40));
			}
			else {
				Projectile.velocity = currentSpeed.RotatedBy(Main.rand.Next(-1, 2) * (Math.PI / -40));
			}
		}

	}
}
