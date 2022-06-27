
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.Infernon
{
	public class FireSpike : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fire Spike");
			Main.projFrames[Projectile.type] = 4;
		}

		public override void SetDefaults()
		{
			Projectile.width = Projectile.height = 34;

			Projectile.hostile = true;
			Projectile.friendly = false;

			Projectile.penetrate = -1;
		}

		public override bool PreAI()
		{
			Projectile.alpha -= 40;
			if (Projectile.alpha < 0) {
				Projectile.alpha = 0;
			}
			Projectile.spriteDirection = Projectile.direction;
			Projectile.frameCounter++;
			if (Projectile.frameCounter >= 3) {
				Projectile.frame++;
				Projectile.frameCounter = 0;
				if (Projectile.frame >= 4)
					Projectile.frame = 0;
			}
			Projectile.rotation = Projectile.velocity.ToRotation() + 1.57F;

			return true;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(200, 200, 200, Projectile.alpha);
		}

		public override void AI()
		{
			int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Torch, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
			int dust2 = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Torch, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
			Main.dust[dust].noGravity = true;
			Main.dust[dust2].noGravity = true;
			Main.dust[dust2].velocity *= 0f;
			Main.dust[dust2].velocity *= 0f;
			Main.dust[dust2].scale = 0.9f;
			Main.dust[dust].scale = 0.9f;
		}

		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
			Projectile.position.X = Projectile.position.X + (float)(Projectile.width / 4);
			Projectile.position.Y = Projectile.position.Y + (float)(Projectile.height / 4);
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 4);
			Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 4);

			for (int num621 = 0; num621 < 20; num621++) {
				int num622 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 60, default, 2f);
				Main.dust[num622].velocity *= 3f;
				if (Main.rand.Next(2) == 0) {
					Main.dust[num622].scale = 0.5f;
					Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
				}
			}

			for (int num625 = 0; num625 < 3; num625++) {
				float scaleFactor10 = 0.2f;
				if (num625 == 1) {
					scaleFactor10 = 0.5f;
				}
				if (num625 == 2) {
					scaleFactor10 = 1f;
				}
				int num626 = Gore.NewGore(new Vector2(Projectile.position.X + (float)(Projectile.width / 2) - 24f, Projectile.position.Y + (float)(Projectile.height / 2) - 24f), default, Main.rand.Next(61, 64), 1f);
				Main.gore[num626].velocity *= scaleFactor10;
				Gore expr_13AB6_cp_0 = Main.gore[num626];
				expr_13AB6_cp_0.velocity.X = expr_13AB6_cp_0.velocity.X + 1f;
				Gore expr_13AD6_cp_0 = Main.gore[num626];
				expr_13AD6_cp_0.velocity.Y = expr_13AD6_cp_0.velocity.Y + 1f;
				num626 = Gore.NewGore(new Vector2(Projectile.position.X + (float)(Projectile.width / 2) - 24f, Projectile.position.Y + (float)(Projectile.height / 2) - 24f), default, Main.rand.Next(61, 64), 1f);
				Main.gore[num626].velocity *= scaleFactor10;
				Gore expr_13B79_cp_0 = Main.gore[num626];
				expr_13B79_cp_0.velocity.X = expr_13B79_cp_0.velocity.X - 1f;
				Gore expr_13B99_cp_0 = Main.gore[num626];
				expr_13B99_cp_0.velocity.Y = expr_13B99_cp_0.velocity.Y + 1f;
			}

			Projectile.position.X = Projectile.position.X + (float)(Projectile.width / 4);
			Projectile.position.Y = Projectile.position.Y + (float)(Projectile.height / 4);
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 4);
			Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 4);

			SoundEngine.PlaySound(SoundID.Item, (int)Projectile.position.X, (int)Projectile.position.Y, 27);
			for (int num273 = 0; num273 < 3; num273++) {
				int num274 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 0, default, 1f);
				Main.dust[num274].noGravity = true;
				Main.dust[num274].noLight = true;
				Main.dust[num274].scale = 0.7f;
			}
		}
	}
}
