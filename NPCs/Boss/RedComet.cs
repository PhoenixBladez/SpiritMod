using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SpiritMod.NPCs.Boss
{
	public class RedComet : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Red Comet");
			Main.projFrames[Projectile.type] = 5;
		}

		public override void SetDefaults()
		{
			Projectile.width = 34;
			Projectile.height = 34;
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.penetrate = 10;
			Projectile.timeLeft = 900;
			Projectile.tileCollide = false;
			Projectile.aiStyle = -1;
		}

		public override bool PreAI()
		{
			Projectile.alpha -= 40;
			if (Projectile.alpha < 0)
				Projectile.alpha = 0;

			Projectile.spriteDirection = Projectile.direction;
			Projectile.frameCounter++;
			if (Projectile.frameCounter >= 4) {
				Projectile.frame++;
				Projectile.frameCounter = 0;
				if (Projectile.frame >= 5)
					Projectile.frame = 0;

			}
			return true;
		}
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 20; i++) {
				Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.Dirt, 0, 60, 133);
			}
		}
		public override Color? GetAlpha(Color lightColor) => Color.White;

		public override void AI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation() + 1.57f + 3.14f;
			Projectile.localAI[0] += 1f;
			if (Projectile.localAI[0] == 16f) {
				Projectile.localAI[0] = 0f;
				for (int j = 0; j < 10; j++) {
					Vector2 vector2 = Vector2.UnitX * -Projectile.width / 2f;
					vector2 += -Utils.RotatedBy(Vector2.UnitY, ((float)j * 3.141591734f / 6f), default) * new Vector2(8f, 16f);
					vector2 = Utils.RotatedBy(vector2, (Projectile.rotation - 1.57079637f), default);
					int num8 = Dust.NewDust(Projectile.Center, 0, 0, DustID.RedTorch, 0f, 0f, 160, new Color(), 1f);
					Main.dust[num8].scale = 1.3f;
					Main.dust[num8].noGravity = true;
					Main.dust[num8].position = Projectile.Center + vector2;
					Main.dust[num8].velocity = Projectile.velocity * 0.1f;
					Main.dust[num8].noLight = true;
					Main.dust[num8].velocity = Vector2.Normalize(Projectile.Center - Projectile.velocity * 3f - Main.dust[num8].position) * 1.25f;
				}
			}
			int num1222 = 5;
			for (int k = 0; k < 2; k++) {
				int index2 = Dust.NewDust(Projectile.position, 1, 1, DustID.RedTorch, 0.0f, 0.0f, 0, new Color(), 1f);
				Main.dust[index2].position = Projectile.Center - Projectile.velocity / num1222 * (float)k;
				Main.dust[index2].scale = .95f;
				Main.dust[index2].velocity *= 0f;
				Main.dust[index2].noGravity = true;
				Main.dust[index2].noLight = true;
			}
		}

	}
}