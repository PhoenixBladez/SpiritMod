using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class CorruptPortal : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dark Portal");
		}

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.magic = true;
			projectile.width = 40;
			projectile.height = 40;
			projectile.aiStyle = -1;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.alpha = 255;
			projectile.timeLeft = 600;
		}
		public bool specialKill;
		public override bool PreAI()
		{
			projectile.tileCollide = false;
			{
				int index = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.PurpleCrystalShard, 0.0f, 0.0f, 200, new Color(), 0.5f);
				Main.dust[index].noGravity = true;
				Main.dust[index].velocity *= 0.75f;
				Main.dust[index].fadeIn = 1.3f;
				Vector2 vector2_1 = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
				vector2_1.Normalize();
				Vector2 vector2_2 = vector2_1 * ((float)Main.rand.Next(50, 100) * 0.04f);
				Main.dust[index].velocity = vector2_2;
				vector2_2.Normalize();
				Vector2 vector2_3 = vector2_2 * 34f;
				Main.dust[index].position = projectile.Center - vector2_3;
			}
			return true;
		}

		int timer = 50;

		public override void AI()
		{
			timer--;
			if (timer <= 0) {
				Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 8);
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, projectile.velocity.X + Main.rand.Next(-3, 5), projectile.velocity.Y + Main.rand.Next(-3, 5), ModContent.ProjectileType<NightSpit>(), projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
				timer = 50;
			}
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
				if (num416 > 2) {
					Main.projectile[num417].netUpdate = true;
					Main.projectile[num417].ai[1] = 36000f;
					return;
				}
			}
		}
		public override void Kill(int timeLeft)
		{
			if (projectile.aiStyle == -3) {
				int n = 8;
				int deviation = Main.rand.Next(0, 300);
				for (int i = 0; i < n; i++) {
					float rotation = MathHelper.ToRadians(270 / n * i + deviation);
					Vector2 perturbedSpeed = new Vector2(projectile.velocity.X + 1, projectile.velocity.Y).RotatedBy(rotation);
					perturbedSpeed.Normalize();
					perturbedSpeed.X *= 2.5f;
					perturbedSpeed.Y *= 2.5f;
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, mod.ProjectileType("CorruptPortal_Star"), projectile.damage / 5 * 4, 2, projectile.owner);
				}
				Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 14);
				ProjectileExtras.Explode(projectile.whoAmI, 120, 120,
					delegate {
						for (int i = 0; i < 40; i++) {
							int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.ShadowbeamStaff, 0f, -2f, 0, default, 1.3f);
							Main.dust[num].noGravity = true;
							Dust dust = Main.dust[num];
							dust.position.X = dust.position.X + ((float)(Main.rand.Next(-50, 51) / 10) - 1.5f);
							Dust expr_92_cp_0 = Main.dust[num];
							expr_92_cp_0.position.Y = expr_92_cp_0.position.Y + ((float)(Main.rand.Next(-50, 51) / 10) - 1.5f);
							if (Main.dust[num].position != projectile.Center) {
								Main.dust[num].velocity = projectile.DirectionTo(Main.dust[num].position) * 6f;
							}
						}
					});
			}
		}
	}
}
