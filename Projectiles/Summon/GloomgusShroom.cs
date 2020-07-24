using Microsoft.Xna.Framework;
using SpiritMod.Buffs;
using SpiritMod.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Summon
{
	public class GloomgusShroom : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gloomgus Mushroom");
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.StickyGrenade);
			aiType = ProjectileID.StickyGrenade;
			projectile.friendly = true;
			projectile.minion = true;
			projectile.hostile = false;
			projectile.timeLeft = 200;
			projectile.width = 20;
			projectile.height = 30;
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 14);
			if (projectile.ai[0] == 1) {
				ProjectileExtras.Explode(projectile.whoAmI, 120, 120,
					delegate {
						for (int i = 0; i < 80; i++) {
							int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, ModContent.DustType<BlueMoonPinkDust>(), 0f, -2f, 0, default(Color), 2f);
							Main.dust[num].noGravity = true;
							Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
							Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
							if (Main.dust[num].position != projectile.Center)
								Main.dust[num].velocity = projectile.DirectionTo(Main.dust[num].position) * 3f;
						}
					});
			}
			else {
				ProjectileExtras.Explode(projectile.whoAmI, 120, 120,
				delegate {
					for (int i = 0; i < 80; i++) {
						int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, ModContent.DustType<BlueMoonBlueDust>(), 0f, -2f, 0, default(Color), 2f);
						Main.dust[num].noGravity = true;
						Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
						Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
						if (Main.dust[num].position != projectile.Center)
							Main.dust[num].velocity = projectile.DirectionTo(Main.dust[num].position) * 3f;
					}
				});
			}
		}


		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(3) == 0)
				target.AddBuff(ModContent.BuffType<StarFlame>(), 180);
			projectile.Kill();
		}

		public override void AI()
		{
			projectile.ai[1]++;
			if(projectile.ai[1] == 1 && Main.netMode != NetmodeID.MultiplayerClient) {
				projectile.ai[0] = Main.rand.Next(2);
				projectile.netUpdate = true;
			}
			if (projectile.ai[1] >= 7200f) {
				projectile.alpha += 5;
				if (projectile.alpha > 255) {
					projectile.alpha = 255;
					projectile.Kill();
				}
			}
			Lighting.AddLight((int)(projectile.position.X / 16f), (int)(projectile.position.Y / 16f), 0.196f, 0.870588235f, 0.964705882f);
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

					if (num416 > 15) {
						Main.projectile[num417].netUpdate = true;
						Main.projectile[num417].ai[1] = 36000f;
						return;
					}
				}
			}
		}

	}
}