using Terraria.Audio;
using Terraria.ModLoader;
using Terraria;
using System;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace SpiritMod.NPCs.Tides
{
	public class LobsterBubbleSmall : ModProjectile
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lobster Bubble");
		}

		public override void SetDefaults()
		{
			Projectile.aiStyle = -1;
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.friendly = false;
			Projectile.tileCollide = true;
			Projectile.hostile = true;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 150;
			Projectile.alpha = 75;
		}

		public override void AI()
		{
			Projectile.velocity.X *= 0.99f;
			Projectile.velocity.Y -= 0.015f;
		}
		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item54, Projectile.Center);
			for (int i = 0; i < 20; i++) {
				int num = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.FungiHit, 0f, -2f, 0, default, 2f);
				Main.dust[num].noGravity = true;
				Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
				Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
				Main.dust[num].scale *= .2825f;
				if (Main.dust[num].position != Projectile.Center)
					Main.dust[num].velocity = Projectile.DirectionTo(Main.dust[num].position) * 5f;
			}
		}
	}
}
