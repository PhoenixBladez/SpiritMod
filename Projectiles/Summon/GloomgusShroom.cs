using Microsoft.Xna.Framework;
using SpiritMod.Buffs;
using SpiritMod.Dusts;
using Terraria;
using Terraria.Audio;
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
			Projectile.CloneDefaults(ProjectileID.StickyGrenade);
			AIType = ProjectileID.StickyGrenade;
			Projectile.friendly = true;
			Projectile.minion = true;
			Projectile.hostile = false;
			Projectile.timeLeft = 200;
			Projectile.width = 20;
			Projectile.height = 30;
		}

		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
			if (Projectile.ai[0] == 1) {
				ProjectileExtras.Explode(Projectile.whoAmI, 120, 120,
					delegate {
						for (int i = 0; i < 80; i++) {
							int num = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<BlueMoonPinkDust>(), 0f, -2f, 0, default, 2f);
							Main.dust[num].noGravity = true;
							Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
							Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
							if (Main.dust[num].position != Projectile.Center)
								Main.dust[num].velocity = Projectile.DirectionTo(Main.dust[num].position) * 3f;
						}
					});
			}
			else {
				ProjectileExtras.Explode(Projectile.whoAmI, 120, 120,
				delegate {
					for (int i = 0; i < 80; i++) {
						int num = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<BlueMoonBlueDust>(), 0f, -2f, 0, default, 2f);
						Main.dust[num].noGravity = true;
						Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
						Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
						if (Main.dust[num].position != Projectile.Center)
							Main.dust[num].velocity = Projectile.DirectionTo(Main.dust[num].position) * 3f;
					}
				});
			}
		}


		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(3) == 0)
				target.AddBuff(ModContent.BuffType<StarFlame>(), 180);
			Projectile.Kill();
		}

		public override void AI()
		{
			Projectile.ai[1]++;
			if(Projectile.ai[1] == 1 && Main.netMode != NetmodeID.MultiplayerClient) {
				Projectile.ai[0] = Main.rand.Next(2);
				Projectile.netUpdate = true;
			}
			if (Projectile.ai[1] >= 7200f) {
				Projectile.alpha += 5;
				if (Projectile.alpha > 255) {
					Projectile.alpha = 255;
					Projectile.Kill();
				}
			}
			Lighting.AddLight((int)(Projectile.position.X / 16f), (int)(Projectile.position.Y / 16f), 0.196f, 0.870588235f, 0.964705882f);
			Projectile.localAI[0] += 1f;
			if (Projectile.localAI[0] >= 10f) {
				Projectile.localAI[0] = 0f;
				int num416 = 0;
				int num417 = 0;
				float num418 = 0f;
				int num419 = Projectile.type;
				for (int num420 = 0; num420 < 1000; num420++) {
					if (Main.projectile[num420].active && Main.projectile[num420].owner == Projectile.owner && Main.projectile[num420].type == num419 && Main.projectile[num420].ai[1] < 3600f) {
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