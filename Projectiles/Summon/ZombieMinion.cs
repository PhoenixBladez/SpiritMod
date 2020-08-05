using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Summon
{
	public class ZombieMinion : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
			DisplayName.SetDefault("Blood Zombie");
			Main.projFrames[projectile.type] = 20;
			ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
			ProjectileID.Sets.Homing[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.OneEyedPirate);
			projectile.width = 32;
			projectile.height = 36;
			aiType = ProjectileID.OneEyedPirate;
			projectile.scale = Main.rand.NextFloat(.7f, 1.1f);
			projectile.minion = true;
			projectile.friendly = true;
			projectile.ignoreWater = true;
			projectile.tileCollide = true;
			projectile.netImportant = true;
			projectile.alpha = 0;
			projectile.timeLeft = 120;
			projectile.penetrate = 2;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (projectile.penetrate == 0)
				projectile.Kill();

			return false;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(8) == 0) {
				Player player = Main.player[projectile.owner];
				int lifeToHeal = 0;

				if (player.statLife + 3 <= player.statLifeMax2)
					lifeToHeal = 3;
				else {
					lifeToHeal = player.statLifeMax2 - player.statLife;
				}
				if (player.statLife < player.statLifeMax2) {
					player.statLife += lifeToHeal;
					player.HealEffect(lifeToHeal);
				}
			}
		}
		public override void AI()
		{
			projectile.spriteDirection = projectile.direction;
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

					if (num416 > 4) {
						Main.projectile[num417].netUpdate = true;
						Main.projectile[num417].ai[1] = 36000f;
						return;
					}
				}
			}

		}
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 10; i++) {
				Dust.NewDust(projectile.position, projectile.width, projectile.height, 5);
			}
			Main.PlaySound(SoundID.NPCKilled, (int)projectile.position.X, (int)projectile.position.Y, 6);
		}
		public override bool MinionContactDamage()
		{
			return true;
		}

	}
}