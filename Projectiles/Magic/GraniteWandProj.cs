using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class GraniteWandProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Unstable Cascade");
		}

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.magic = true;
			projectile.width = 26;
			projectile.height = 115;
			projectile.penetrate = 5;
			projectile.alpha = 255;
			projectile.timeLeft = 240;
		}
		bool hitGround = false;
		public override bool? CanHitNPC(NPC target) => hitGround;
		public override bool PreAI()
		{
			float num1 = 6f;
			float num2 = (float)projectile.timeLeft / 60f;
			if ((double)num2 < 1.0)
				num1 *= num2;

			if (hitGround) {
				for (int index3 = 0; index3 < 2; ++index3) {
					Vector2 vector2 = new Vector2(0.0f, -num1);
					vector2 = (vector2 * (float)(0.850000023841858 + Main.rand.NextDouble() * 0.200000002980232)).RotatedBy((Main.rand.NextDouble() - 0.5) * 0.785398185253143, new Vector2());
					int index4 = Dust.NewDust(projectile.position, 4, projectile.height + 10, 221, 0.0f, 0.0f, 100, new Color(), 1f);
					Dust dust1 = Main.dust[index4];
					dust1.scale = (float)(1.0 + Main.rand.NextDouble() * 0.300000011920929);
					Dust dust2 = dust1;
					dust2.velocity = dust2.velocity * 0.1f;
					Dust dust3 = dust1;
					dust3.position = dust3.position - new Vector2((float)(2 + Main.rand.Next(-2, 3)), 0.0f);
					Dust dust4 = dust1;
					dust4.velocity = dust4.velocity + vector2;
					dust1.scale = 0.6f;
					dust1.fadeIn = dust1.scale + 0.2f;
				}
				if (projectile.timeLeft % 10 == 0) {
					float num3 = (float)(0.850000023841858 + Main.rand.NextDouble() * 0.200000002980232);
					for (int index3 = 0; index3 < 9; ++index3) {
						Vector2 vector2 = new Vector2((float)(index3 - 4) / 5f, -num1 * num3);
						int index4 = Dust.NewDust(projectile.position, 4, projectile.height + 10, 226, 0.0f, 0.0f, 100, new Color(), 1f);
						Dust dust1 = Main.dust[index4];
						dust1.scale = (float)(0.699999988079071 + Main.rand.NextDouble() * 0.300000011920929);
						Dust dust2 = dust1;
						dust2.velocity = dust2.velocity * 0.0f;
						Dust dust3 = dust1;
						dust3.position = dust3.position - new Vector2((float)(2 + Main.rand.Next(-2, 3)), 0.0f);
						Dust dust4 = dust1;
						dust4.velocity = dust4.velocity + vector2;
						dust1.scale = 0.6f;
						dust1.fadeIn = dust1.scale + 0.2f;
					}
				}
			}
			return false;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			hitGround = true;
			return false;
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
		{
			fallThrough = false;
			return true;
		}
		public override void Kill(int timeLeft)
		{
			Dust.NewDust(projectile.position + projectile.velocity,
				projectile.width, projectile.height,
				187, projectile.oldVelocity.X * 0.2f, projectile.oldVelocity.Y * 0.2f);
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (target.life <= 0) {
				if (projectile.friendly && !projectile.hostile) {
					ProjectileExtras.Explode(projectile.whoAmI, 30, 30,
					delegate {
					});

				}
				Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 109));
				{
					for (int i = 0; i < 20; i++) {
						int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, 226, 0f, -2f, 0, default(Color), 2f);
						Main.dust[num].noGravity = true;
						Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
						Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
						Main.dust[num].scale *= .25f;
						if (Main.dust[num].position != projectile.Center)
							Main.dust[num].velocity = projectile.DirectionTo(Main.dust[num].position) * 6f;
					}
				}
				int proj = Projectile.NewProjectile(target.Center.X, target.Center.Y,
					0, 0, ModContent.ProjectileType<GraniteSpike1>(), projectile.damage / 2, projectile.knockBack, projectile.owner);
				Main.projectile[proj].timeLeft = 2;
			}
		}
	}
}