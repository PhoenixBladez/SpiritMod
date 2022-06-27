using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SpiritMod.Projectiles
{
	public class WhiteSoul : ModProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Soul Wisp");

		public override void SetDefaults()
		{
			Projectile.width = 4;       //projectile width
			Projectile.height = 4;  //projectile height
			Projectile.friendly = true;      //make that the projectile will not damage you
			Projectile.minion = true;         // 
			Projectile.tileCollide = false;   //make that the projectile will be destroed if it hits the terrain
			Projectile.penetrate = 2;      //how many npc will penetrate
			Projectile.timeLeft = 270;   //how many time projectile projectile has before disepire // projectile light
			Projectile.extraUpdates = 1;
			Projectile.minion = true;
			Projectile.ignoreWater = true;
			Projectile.alpha = 255;
			Projectile.aiStyle = -1;
		}

		public override void AI()
		{
			bool flag25 = false;
			int jim = 1;
			for (int index1 = 0; index1 < 200; index1++) {
				if (Main.npc[index1].CanBeChasedBy(Projectile, false) && Collision.CanHit(Projectile.Center, 1, 1, Main.npc[index1].Center, 1, 1)) {
					float num23 = Main.npc[index1].position.X + (Main.npc[index1].width / 2);
					float num24 = Main.npc[index1].position.Y + (Main.npc[index1].height / 2);
					float num25 = Math.Abs(Projectile.position.X + (Projectile.width / 2) - num23) + Math.Abs(Projectile.position.Y + (Projectile.height / 2) - num24);
					if (num25 < 300f) {
						flag25 = true;
						jim = index1;
					}
				}
			}
			if (flag25) {
				float num1 = 4f;
				Vector2 vector2 = new Vector2(Projectile.position.X + Projectile.width * 0.5f, Projectile.position.Y + Projectile.height * 0.5f);
				float num2 = Main.npc[jim].Center.X - vector2.X;
				float num3 = Main.npc[jim].Center.Y - vector2.Y;
				float num4 = (float)Math.Sqrt(num2 * num2 + num3 * num3);
				float num5 = num1 / num4;
				float num6 = num2 * num5;
				float num7 = num3 * num5;

				Projectile.velocity.X = (Projectile.velocity.X * (30 - 1) + num6) / 30;
				Projectile.velocity.Y = (Projectile.velocity.Y * (30 - 1) + num7) / 30;
			}

			for (int index1 = 0; index1 < 5; ++index1) {
				float num1 = Projectile.velocity.X * 0.2f * index1;
				float num2 = (float)-(Projectile.velocity.Y * 0.200000002980232) * index1;
				int index2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Rainbow, 0.0f, 0.0f, 100, new Color(), 1.3f);
				Main.dust[index2].noGravity = true;
				Main.dust[index2].velocity *= 0.0f;
				Main.dust[index2].scale = 1f;
				Main.dust[index2].position.X -= num1;
				Main.dust[index2].position.Y -= num2;
			}
		}
	}
}
