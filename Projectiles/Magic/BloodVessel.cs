using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.Audio;
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
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.width = 30;
			Projectile.height = 30;
			Projectile.friendly = true;
			Projectile.penetrate = 2;
			Projectile.alpha = 255;
			Projectile.extraUpdates = 2;
			Projectile.timeLeft = 50;
			Projectile.tileCollide = true;
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
				Vector2 center22 = Projectile.Center;
				Projectile.scale = 1f - Projectile.localAI[0];
				Projectile.width = (int)(20f * Projectile.scale);
				Projectile.height = Projectile.width;
				Projectile.position.X = center22.X - (float)(Projectile.width / 2);
				Projectile.position.Y = center22.Y - (float)(Projectile.height / 2);
				if ((double)Projectile.localAI[0] < 0.1) {
					Projectile.localAI[0] += 0.01f;
				}
				else {
					Projectile.localAI[0] += 0.025f;
				}
				if (Projectile.localAI[0] >= 0.8f) {
					Projectile.Kill();
				}

				if (Projectile.scale < 1f) {
					int num1448 = 0;
					while ((float)num1448 < Projectile.scale * 10f) {
						Vector2 position177 = new Vector2(Projectile.position.X, Projectile.position.Y);
						int width138 = Projectile.width;
						int height138 = Projectile.height;
						float x38 = Projectile.velocity.X;
						float y36 = Projectile.velocity.Y;
						Color newColor5 = default;
						int num1447 = Dust.NewDust(position177, width138, height138, DustID.Blood, x38, y36, 100, newColor5, 1.1f);
						Main.dust[num1447].position = (Main.dust[num1447].position + Projectile.Center) / 2f;
						Main.dust[num1447].noGravity = true;
						Dust dust81 = Main.dust[num1447];
						dust81.velocity *= 0.1f;
						dust81 = Main.dust[num1447];
						dust81.velocity -= Projectile.velocity * (1.3f - Projectile.scale);
						Main.dust[num1447].fadeIn = (float)(100 + Projectile.owner);
						dust81 = Main.dust[num1447];
						dust81.scale += Projectile.scale * 0.45f;
						num2475 = num1448;
						num1448 = num2475 + 1;
					}
				}
			}

			if (++Projectile.ai[1] == 1) {
				float rotation = (MathHelper.Pi / 6) * (Main.rand.NextBool() ? -1 : 1) * Main.rand.NextFloat(0.5f, 1.2f);
				desiredvel = Projectile.velocity.RotatedBy(rotation);
				Projectile.velocity = Projectile.velocity.RotatedBy(-rotation);
				Projectile.netUpdate = true;
			}
			Projectile.velocity = Vector2.Lerp(Projectile.velocity, desiredvel, 0.05f);
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Blood);

			SoundEngine.PlaySound(SoundID.Item103, Projectile.Center);

			for (int i = 0; i < 4; i++)
			{
				Vector2 perturbedSpeed = Main.rand.NextVector2CircularEdge(2.5f, 2.5f);
				Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center.X, Projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, ModContent.ProjectileType<Blood3>(), Projectile.damage / 5 * 4, 2, Projectile.owner);
			}
		}
	}
}