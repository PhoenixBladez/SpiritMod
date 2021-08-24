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
			Main.projFrames[projectile.type] = 5;
		}

		public override void SetDefaults()
		{
			projectile.width = 34;
			projectile.height = 34;
			projectile.friendly = false;
			projectile.hostile = true;
			projectile.penetrate = 10;
			projectile.timeLeft = 900;
			projectile.tileCollide = false;
			projectile.aiStyle = -1;
		}

		public override bool PreAI()
		{
			projectile.alpha -= 40;
			if (projectile.alpha < 0)
				projectile.alpha = 0;

			projectile.spriteDirection = projectile.direction;
			projectile.frameCounter++;
			if (projectile.frameCounter >= 4) {
				projectile.frame++;
				projectile.frameCounter = 0;
				if (projectile.frame >= 5)
					projectile.frame = 0;

			}
			return true;
		}
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 20; i++) {
				Dust.NewDust(projectile.Center, projectile.width, projectile.height, DustID.Dirt, 0, 60, 133);
			}
		}
		public override Color? GetAlpha(Color lightColor) => Color.White;

		public override void AI()
		{
			projectile.rotation = projectile.velocity.ToRotation() + 1.57f + 3.14f;
			projectile.localAI[0] += 1f;
			if (projectile.localAI[0] == 16f) {
				projectile.localAI[0] = 0f;
				for (int j = 0; j < 10; j++) {
					Vector2 vector2 = Vector2.UnitX * -projectile.width / 2f;
					vector2 += -Utils.RotatedBy(Vector2.UnitY, ((float)j * 3.141591734f / 6f), default(Vector2)) * new Vector2(8f, 16f);
					vector2 = Utils.RotatedBy(vector2, (projectile.rotation - 1.57079637f), default(Vector2));
					int num8 = Dust.NewDust(projectile.Center, 0, 0, DustID.RedTorch, 0f, 0f, 160, new Color(), 1f);
					Main.dust[num8].scale = 1.3f;
					Main.dust[num8].noGravity = true;
					Main.dust[num8].position = projectile.Center + vector2;
					Main.dust[num8].velocity = projectile.velocity * 0.1f;
					Main.dust[num8].noLight = true;
					Main.dust[num8].velocity = Vector2.Normalize(projectile.Center - projectile.velocity * 3f - Main.dust[num8].position) * 1.25f;
				}
			}
			int num1222 = 5;
			for (int k = 0; k < 2; k++) {
				int index2 = Dust.NewDust(projectile.position, 1, 1, DustID.RedTorch, 0.0f, 0.0f, 0, new Color(), 1f);
				Main.dust[index2].position = projectile.Center - projectile.velocity / num1222 * (float)k;
				Main.dust[index2].scale = .95f;
				Main.dust[index2].velocity *= 0f;
				Main.dust[index2].noGravity = true;
				Main.dust[index2].noLight = true;
			}
		}

	}
}