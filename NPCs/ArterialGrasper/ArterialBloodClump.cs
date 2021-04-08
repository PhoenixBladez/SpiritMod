using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.ArterialGrasper
{
	public class ArterialBloodClump : ModProjectile
	{

		private int DamageAdditive;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Clumped Blood");
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.WoodenArrowHostile);
			projectile.width = 10;
			projectile.height = 10;
			projectile.friendly = false;
			projectile.hostile = true;
			projectile.penetrate = 3;
		}

		public override void AI()
		{
			projectile.rotation += .1f;
			projectile.ai[1] += 1f;
			if (projectile.ai[1] >= 7200f) {
				projectile.alpha += 5;
				if (projectile.alpha > 255) {
					projectile.alpha = 255;
					projectile.Kill();
				}
			}

			projectile.localAI[0] += 1f;
			if (projectile.localAI[0] >= 10f) {
				projectile.localAI[0] = 0f;
				int num416 = 0;
				int num417 = 0;
				float num418 = 0f;
				int num419 = projectile.type;
				for (int num420 = 0; num420 < 1000; num420++) {
					if (Main.projectile[num420].active && Main.projectile[num420].owner == projectile.owner && Main.projectile[num420].type == num419 && Main.projectile[num420].ai[1] < 3600f) {
						num416++;
						if (Main.projectile[num420].ai[1] > num418) {
							num417 = num420;
							num418 = Main.projectile[num420].ai[1];
						}
					}
				}
				if (num416 > 16) {
					Main.projectile[num417].netUpdate = true;
					Main.projectile[num417].ai[1] = 36000f;
					return;
				}
			}
			projectile.localAI[0] += 1f;
			if (projectile.localAI[0] == 16f) {
				projectile.localAI[0] = 0f;
				for (int j = 0; j < 10; j++) {
					Vector2 vector2 = Vector2.UnitX * -projectile.width / 2f;
					vector2 += -Utils.RotatedBy(Vector2.UnitY, ((float)j * 3.141591734f / 6f), default(Vector2)) * new Vector2(8f, 16f);
					vector2 = Utils.RotatedBy(vector2, (projectile.rotation - 1.57079637f), default(Vector2));
					int num8 = Dust.NewDust(projectile.Center, 0, 0, 5, 0f, 0f, 160, new Color(), 1f);
					Main.dust[num8].scale = 1.3f;
					Main.dust[num8].noGravity = true;
					Main.dust[num8].position = projectile.Center + vector2;
					Main.dust[num8].velocity = projectile.velocity * 0.1f;
					Main.dust[num8].noLight = true;
					Main.dust[num8].velocity = Vector2.Normalize(projectile.Center - projectile.velocity * 3f - Main.dust[num8].position) * 1.25f;
				}
			}
			int num = 5;
			for (int k = 0; k < 5; k++) {
				int index2 = Dust.NewDust(projectile.position, 1, 1, 5, 0.0f, 0.0f, 0, new Color(), 1f);
				Main.dust[index2].position = projectile.Center - projectile.velocity / num * (float)k;
				Main.dust[index2].scale = .5f;
				Main.dust[index2].velocity *= 0f;
				Main.dust[index2].noGravity = true;
				Main.dust[index2].noLight = false;
			}
			for (int k = 0; k < 12; k++) {
				int index2 = Dust.NewDust(projectile.position, 8, 8, 5, 0.0f, 0.0f, 0, new Color(), 1f);
				Main.dust[index2].position = projectile.Center - projectile.velocity / num * (float)k;
				Main.dust[index2].scale = 1f;
				Main.dust[index2].velocity *= .5f;
				Main.dust[index2].noGravity = true;
				Main.dust[index2].noLight = false;
			}

		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			{
				int d = 5;
				for (int k = 0; k < 6; k++) {
					Dust.NewDust(projectile.position, projectile.width, projectile.height, d, 2.5f * 1, -2.5f, 0, Color.White, 0.7f);
					Dust.NewDust(projectile.position, projectile.width, projectile.height, d, 2.5f * 1, -2.5f, 0, Color.White, 0.7f);
				}

				Dust.NewDust(projectile.position, projectile.width, projectile.height, d, 2.5f * 1, -2.5f, 0, Color.White, 0.7f);
				Dust.NewDust(projectile.position, projectile.width, projectile.height, d, 2.5f * 1, -2.5f, 0, Color.White, 0.7f);
				Dust.NewDust(projectile.position, projectile.width, projectile.height, d, 2.5f * 1, -2.5f, 0, Color.White, 0.7f);
				Dust.NewDust(projectile.position, projectile.width, projectile.height, d, 2.5f * 1, -2.5f, 0, Color.White, 0.7f);
			}
			return true;
		}
	}
}
