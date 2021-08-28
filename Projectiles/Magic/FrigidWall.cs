using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class FrigidWall : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frigid Wall");
		}

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.width = 16;
			projectile.height = 140;
			projectile.aiStyle = -1;
			projectile.friendly = true;
			projectile.penetrate = 4;
			projectile.alpha = 255;
			projectile.timeLeft = 480;
			projectile.tileCollide = false; //Tells the game whether or not it can collide with a tile
		}

		public override bool PreAI()
		{
			var list = Main.projectile.Where(x => x.Hitbox.Intersects(projectile.Hitbox));
			foreach (var proj in list) {
				if (projectile != proj && proj.hostile) {
					proj.velocity.X = proj.velocity.X * -1;
					proj.hostile = false;
					proj.friendly = true;
				}
			}
			//Create particles
			for (int k = 0; k < 4; k++) {
				int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustID.Flare_Blue, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
				int dust1 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustID.Flare_Blue, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
				int dust2 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustID.Flare_Blue, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
				Main.dust[dust].noGravity = true;
				Main.dust[dust1].noGravity = true;
				Main.dust[dust2].noGravity = true;
				Main.dust[dust2].scale = .82f;
				Main.dust[dust1].scale = .75f;
				Main.dust[dust].scale = 1.5f;
			}
			return false;
		}
		public override void AI()
		{
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

				if (num416 > 1) {
					Main.projectile[num417].netUpdate = true;
					Main.projectile[num417].ai[1] = 36000f;
					return;
				}
			}

			++projectile.localAI[1];
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(BuffID.Frostburn, 180, true);
		}

	}
}
