using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Summon
{
	public class LavaRockSummon : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Magmarock");
			ProjectileID.Sets.MinionSacrificable[base.projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.width = 36;
			projectile.height = 36;
			projectile.netImportant = true;
			projectile.friendly = true;
			projectile.ignoreWater = true;
			projectile.minionSlots = 1f;
			projectile.timeLeft = 18000;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.timeLeft *= 5;
			projectile.minion = true;
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
			Lighting.AddLight(projectile.position, 0.4f, .12f, .036f);
			//int num = 5;
			for (int k = 0; k < 3; k++) {
				int index2 = Dust.NewDust(new Vector2(projectile.Center.X - 6, projectile.Center.Y + 3), 1, 1, 127, 0.0f, 0.0f, 0, new Color(), 1f);
				Main.dust[index2].scale = .5f;
				Main.dust[index2].noGravity = false;
				Main.dust[index2].velocity.X *= 0f;
				Main.dust[index2].velocity.Y *= 1.7f;
				Main.dust[index2].noLight = false;
			}
			for (int j = 0; j < 3; j++) {
				int index2 = Dust.NewDust(new Vector2(projectile.Center.X - 12, projectile.Center.Y + 3), 1, 1, 127, 0.0f, 0.0f, 0, new Color(), 1f);
				Main.dust[index2].scale = .5f;
				Main.dust[index2].noGravity = false;
				Main.dust[index2].velocity.X *= 0f;
				Main.dust[index2].velocity.Y *= 1.7f;
				Main.dust[index2].noLight = false;
			}
			bool flag64 = projectile.type == ModContent.ProjectileType<LavaRockSummon>();
			Player player = Main.player[projectile.owner];
			MyPlayer modPlayer = player.GetSpiritPlayer();
			if (flag64) {
				if (player.dead)
					modPlayer.lavaRock = false;

				if (modPlayer.lavaRock)
					projectile.timeLeft = 2;

			}

			projectile.position.X = Main.player[projectile.owner].Center.X - (float)(projectile.width / 2);
			projectile.position.Y = Main.player[projectile.owner].Center.Y - (float)(projectile.height / 2) + Main.player[projectile.owner].gfxOffY - 60f;
			if (Main.player[projectile.owner].gravDir == -1f) {
				projectile.position.Y = projectile.position.Y + 120f;
				projectile.rotation = 3.14f;
			}
			else {
				projectile.rotation = 0f;
			}

			projectile.position.X = ((int)projectile.position.X);
			projectile.position.Y = ((int)projectile.position.Y);
			float num395 = Main.mouseTextColor / 200f - 0.35f;
			num395 *= 0.2f;
			projectile.scale = num395 + 0.95f;
			if (projectile.owner == Main.myPlayer) {
				if (projectile.ai[0] != 0f) {
					projectile.ai[0] -= 1f;
					return;
				}
				float num396 = projectile.position.X;
				float num397 = projectile.position.Y;
				float num398 = 700f;
				bool flag11 = false;
				for (int num399 = 0; num399 < 200; num399++) {
					if (Main.npc[num399].CanBeChasedBy(projectile, true)) {
						float num400 = Main.npc[num399].position.X + (float)(Main.npc[num399].width / 2);
						float num401 = Main.npc[num399].position.Y + (float)(Main.npc[num399].height / 2);
						float num402 = Math.Abs(projectile.position.X + (float)(projectile.width / 2) - num400) + Math.Abs(projectile.position.Y + (float)(projectile.height / 2) - num401);
						if (num402 < num398 && Collision.CanHit(projectile.position, projectile.width, projectile.height, Main.npc[num399].position, Main.npc[num399].width, Main.npc[num399].height)) {
							num398 = num402;
							num396 = num400;
							num397 = num401;
							flag11 = true;
						}
					}
				}

				if (flag11) {
					float num403 = 6f; //modify the speed the projectile are shot.  Lower number = slower projectile.
					Vector2 vector29 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
					float num404 = num396 - vector29.X;
					float num405 = num397 - vector29.Y;
					float num406 = (float)Math.Sqrt((double)(num404 * num404 + num405 * num405));
					num406 = num403 / num406;
					num404 *= num406;
					num405 *= num406;
					int p = Projectile.NewProjectile(projectile.Center.X - 4f, projectile.Center.Y, num404, num405, ModContent.ProjectileType<Blaze>(), projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
					Main.projectile[p].magic = false;
					Main.projectile[p].minion = true;
					projectile.ai[0] = 30f;
					return;
				}
			}
		}

	}
}