using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss
{
	public class ExplodingFeather : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Exploding Feather");
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

		public override void AI()
		{
			int num = 5;
			for (int k = 0; k < 3; k++) {
				int index2 = Dust.NewDust(Projectile.position, 1, 1, DustID.RedTorch, 0.0f, 0.0f, 0, new Color(), 1f);
				Main.dust[index2].position = Projectile.Center - Projectile.velocity / num * (float)k;
				Main.dust[index2].scale = .5f;
				Main.dust[index2].velocity *= 0f;
				Main.dust[index2].noGravity = true;
				Main.dust[index2].noLight = false;
			}
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

			for (int num625 = 0; num625 < 2; num625++) {
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

			for (int num273 = 0; num273 < 3; num273++) {
				int num274 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Flare_Blue, 0f, 0f, 0, default, 1f);
				Main.dust[num274].noGravity = true;
				Main.dust[num274].noLight = true;
				Main.dust[num274].scale = 0.7f;

				int num278 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.DemonTorch, 0f, 0f, 0, default, 1f);
				Main.dust[num278].noGravity = true;
				Main.dust[num278].noLight = true;
				Main.dust[num278].scale = 0.7f;
			}
		}
	}
}
