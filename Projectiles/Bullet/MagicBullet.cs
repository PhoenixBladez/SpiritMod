using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Bullet
{
	public class MagicBullet : ModProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Magic Bullet");

		public override void SetDefaults()
		{
			Projectile.width = 2;       //projectile width
			Projectile.height = 2;  //projectile height
			Projectile.friendly = true;      //make that the projectile will not damage you
			Projectile.DamageType = DamageClass.Ranged;         // 
			Projectile.tileCollide = false;   //make that the projectile will be destroed if it hits the terrain
			Projectile.penetrate = 2;      //how many npc will penetrate
			Projectile.timeLeft = 210;   //how many time projectile projectile has before disepire // projectile light
			Projectile.extraUpdates = 0;
			Projectile.ignoreWater = true;
			Projectile.alpha = 255;
			Projectile.aiStyle = -1;
		}

		public override void AI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation() + 1.57f;

			for (int i = 0; i < 10; i++) {
				float x = Projectile.Center.X - Projectile.velocity.X / 10f * (float)i;
				float y = Projectile.Center.Y - Projectile.velocity.Y / 10f * (float)i;
				int num = Dust.NewDust(new Vector2(x, y), 2, 2, DustID.GoldCoin);
				Main.dust[num].alpha = Projectile.alpha;
				Main.dust[num].velocity = Vector2.Zero;
				Main.dust[num].noGravity = true;
			}

			bool flag25 = false;
			int jim = 1;
			for (int index1 = 0; index1 < 200; index1++) {
				if (Main.npc[index1].CanBeChasedBy(Projectile, false) && Collision.CanHit(Projectile.Center, 1, 1, Main.npc[index1].Center, 1, 1)) {
					float num23 = Main.npc[index1].position.X + (Main.npc[index1].width / 2);
					float num24 = Main.npc[index1].position.Y + (Main.npc[index1].height / 2);
					float num25 = Math.Abs(Projectile.position.X + (Projectile.width / 2) - num23) + Math.Abs(Projectile.position.Y + (float)(Projectile.height / 2) - num24);
					if (num25 < 500f) {
						flag25 = true;
						jim = index1;
					}
				}
			}

			if (flag25) {
				float num1 = 10f;
				Vector2 vector2 = new Vector2(Projectile.position.X + Projectile.width * 0.5f, Projectile.position.Y + Projectile.height * 0.5f);
				float num2 = Main.npc[jim].Center.X - vector2.X;
				float num3 = Main.npc[jim].Center.Y - vector2.Y;
				float num4 = (float)Math.Sqrt(num2 * num2 + num3 * num3);
				float num5 = num1 / num4;
				float num6 = num2 * num5;
				float num7 = num3 * num5;
				int num8 = 10;
				Projectile.velocity.X = (Projectile.velocity.X * (float)(num8 - 1) + num6) / (float)num8;
				Projectile.velocity.Y = (Projectile.velocity.Y * (float)(num8 - 1) + num7) / (float)num8;
			}
			else {
				Projectile.velocity *= 0.95f;
			}
		}

		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
			ProjectileExtras.Explode(Projectile.whoAmI, 120, 120,
				delegate {
					for (int i = 0; i < 40; i++) {
						int num = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height,
							DustID.GoldCoin, 0f, -2f, 0, default, 2f);
						Main.dust[num].noGravity = true;
						Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
						Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
						if (Main.dust[num].position != Projectile.Center)
							Main.dust[num].velocity = Projectile.DirectionTo(Main.dust[num].position) * 6f;
					}
				});
		}
	}
}
