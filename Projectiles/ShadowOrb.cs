using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class ShadowOrb : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Void Singularity");
			Main.projFrames[Projectile.type] = 4;
		}

		public override void SetDefaults()
		{
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.width = 50;
			Projectile.height = 50;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 220;
		}

		public override bool PreAI()
		{
			Projectile.tileCollide = false;
			int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.ShadowbeamStaff, 0f, 0f);
			Main.dust[dust].scale = 0.8f;
			Main.dust[dust].noGravity = true;
			return true;
		}

		int timer = 16;

		public override void AI()
		{
			Projectile.frameCounter++;
			if (Projectile.frameCounter >= 4) {
				Projectile.frame = (Projectile.frame + 1) % Main.projFrames[Projectile.type];
				Projectile.frameCounter = 0;
			}

			timer--;
			if (timer == 0) {
				SoundEngine.PlaySound(SoundID.Item, (int)Projectile.position.X, (int)Projectile.position.Y, 8);
				Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, Projectile.velocity.X + Main.rand.Next(-3, 5), Projectile.velocity.Y + Main.rand.Next(-3, 5), ModContent.ProjectileType<VoidStar>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
				timer = 21;
			}

			Projectile.ai[1] += 1f;
			if (Projectile.ai[1] >= 7200f) {
				Projectile.alpha += 5;
				if (Projectile.alpha > 255) {
					Projectile.alpha = 255;
					Projectile.Kill();
				}
			}

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
				}
				if (num416 > 1) {
					Main.projectile[num417].netUpdate = true;
					Main.projectile[num417].ai[1] = 36000f;
					return;
				}
			}
		}
	}
}
