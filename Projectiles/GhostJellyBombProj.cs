using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class GhostJellyBombProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ghost Jellybomb");
		}

		public override void SetDefaults()
		{
			///for reasons, I have to put a comment here.
			AIType = ProjectileID.StickyGrenade;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.timeLeft = 180;
			Projectile.width = 20;
			Projectile.CloneDefaults(ProjectileID.StickyGrenade);
			Projectile.height = 20;
		}

		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
			int proj = Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center.X - 10, Projectile.Center.Y - 10, 0, 0, ModContent.ProjectileType<SpiritBoom>(), (int)(Projectile.damage), 0, Main.myPlayer);
			for (int i = 0; i < 5; i++) {
				int dust = Dust.NewDust(Projectile.position, Projectile.width + 40, Projectile.height + 40, DustID.Flare_Blue);
				Main.dust[dust].scale = 1.9f;
			}
			Main.projectile[proj].friendly = true;
			Main.projectile[proj].hostile = true;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Projectile.Kill();
		}

		public override void AI()
		{
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
					if (num416 > 2) {
						Main.projectile[num417].netUpdate = true;
						Main.projectile[num417].ai[1] = 36000f;
						return;
					}
				}
			}
		}

	}
}