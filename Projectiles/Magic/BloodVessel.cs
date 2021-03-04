using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class BloodVessel : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blood Vessel");
		}

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.magic = true;
			projectile.width = 30;
			projectile.height = 30;
			projectile.friendly = true;
			projectile.penetrate = 2;
			projectile.alpha = 255;
			projectile.extraUpdates = 2;
			projectile.timeLeft = 50;
			projectile.tileCollide = true;
		}

		int num2475;
		Vector2 desiredvel = Vector2.Zero;

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(num2475);
			writer.WriteVector2(desiredvel);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			num2475 = reader.Read();
			desiredvel = reader.ReadVector2();
		}

		public override void AI()
		{
			for (int num1438 = 0; num1438 < 2; num1438 = num2475 + 1) {
				Vector2 center22 = projectile.Center;
				projectile.scale = 1f - projectile.localAI[0];
				projectile.width = (int)(20f * projectile.scale);
				projectile.height = projectile.width;
				projectile.position.X = center22.X - (float)(projectile.width / 2);
				projectile.position.Y = center22.Y - (float)(projectile.height / 2);
				if ((double)projectile.localAI[0] < 0.1) {
					projectile.localAI[0] += 0.01f;
				}
				else {
					projectile.localAI[0] += 0.025f;
				}
				if (projectile.localAI[0] >= 0.8f) {
					projectile.Kill();
				}

				if (projectile.scale < 1f) {
					int num1448 = 0;
					while ((float)num1448 < projectile.scale * 10f) {
						Vector2 position177 = new Vector2(projectile.position.X, projectile.position.Y);
						int width138 = projectile.width;
						int height138 = projectile.height;
						float x38 = projectile.velocity.X;
						float y36 = projectile.velocity.Y;
						Color newColor5 = default(Color);
						int num1447 = Dust.NewDust(position177, width138, height138, 5, x38, y36, 100, newColor5, 1.1f);
						Main.dust[num1447].position = (Main.dust[num1447].position + projectile.Center) / 2f;
						Main.dust[num1447].noGravity = true;
						Dust dust81 = Main.dust[num1447];
						dust81.velocity *= 0.1f;
						dust81 = Main.dust[num1447];
						dust81.velocity -= projectile.velocity * (1.3f - projectile.scale);
						Main.dust[num1447].fadeIn = (float)(100 + projectile.owner);
						dust81 = Main.dust[num1447];
						dust81.scale += projectile.scale * 0.45f;
						num2475 = num1448;
						num1448 = num2475 + 1;
					}
				}
			}

			if (++projectile.ai[1] == 1) {
				float rotation = (MathHelper.Pi / 6) * (Main.rand.NextBool() ? -1 : 1) * Main.rand.NextFloat(0.5f, 1.2f);
				desiredvel = projectile.velocity.RotatedBy(rotation);
				projectile.velocity = projectile.velocity.RotatedBy(-rotation);
				projectile.netUpdate = true;
			}
			projectile.velocity = Vector2.Lerp(projectile.velocity, desiredvel, 0.05f);
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 10; i++) {
				Dust.NewDust(projectile.position, projectile.width, projectile.height, 5);
			}
			Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 103);
			int n = 4;
			for (int i = 0; i < n; i++) {
				Vector2 perturbedSpeed = Main.rand.NextVector2CircularEdge(2.5f, 2.5f);
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, mod.ProjectileType("Blood3"), projectile.damage / 5 * 4, 2, projectile.owner);
			}
		}
	}
}
