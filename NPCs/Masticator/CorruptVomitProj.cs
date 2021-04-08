
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
			projectile.width = 6;
			projectile.height = 6;
			aiType = ProjectileID.Flames;
			projectile.alpha = 255;
			projectile.timeLeft = 30;
			projectile.penetrate = 4;
			projectile.hostile = true;
			projectile.friendly = false;
			projectile.extraUpdates = 3;
		}

		public override void AI()
		{
			projectile.rotation += 0.1f;
			for (int i = 0; i < 4; i++) {
				int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 184, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
				Main.dust[dust].scale = Main.rand.NextFloat(.4f, .9f);
				Main.dust[dust].noGravity = true;

			}
			Vector2 currentSpeed = new Vector2(projectile.velocity.X, projectile.velocity.Y);
			if (Main.rand.Next(2) == 0) {
				projectile.velocity = currentSpeed.RotatedBy(Main.rand.Next(-1, 2) * (Math.PI / 40));
			}
			else {
				projectile.velocity = currentSpeed.RotatedBy(Main.rand.Next(-1, 2) * (Math.PI / -40));
			}
		}

	}
}
