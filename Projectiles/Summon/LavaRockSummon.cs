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
			Main.projPet[projectile.type] = true;
			Main.projFrames[projectile.type] = 1;
			ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
			ProjectileID.Sets.Homing[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.width = 36;
			projectile.height = 36;
			projectile.netImportant = true;
			projectile.friendly = true;
			projectile.ignoreWater = true;
			projectile.minionSlots = 1f;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.minion = true;
		}

		public override void AI()
		{
			projectile.localAI[0]++;
			if (projectile.localAI[0] >= 10f) {
				projectile.localAI[0] = 0f;
				int num416 = 0;
				int num417 = 0;
				float currentCap = 0f;
				int num419 = projectile.type;
				for (int i = 0; i < 1000; i++) {
					if (Main.projectile[i].active && Main.projectile[i].owner == projectile.owner && Main.projectile[i].type == num419 && Main.projectile[i].ai[1] < 3600f) {
						num416++;
						if (Main.projectile[i].ai[1] > currentCap) {
							num417 = i;
							currentCap = Main.projectile[i].ai[1];
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

			Player player = Main.player[projectile.owner];
			SpawnDust(player);

			MyPlayer modPlayer = player.GetSpiritPlayer();
			if (projectile.type == ModContent.ProjectileType<LavaRockSummon>()) {
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
			else
				projectile.rotation = 0f;

			float adjScale = Main.mouseTextColor / 200f - 0.35f;
			adjScale *= 0.2f;
			projectile.scale = adjScale + 0.95f;

			if (projectile.owner == Main.myPlayer) {
				if (projectile.ai[0] != 0f) {
					projectile.ai[0] -= 1f;
					return;
				}

				float shootPosX = projectile.position.X;
				float shootPosY = projectile.position.Y;
				float maxDist = 700f;
				bool validTarget = false;

				for (int i = 0; i < Main.maxNPCs; i++) {
					if (Main.npc[i].CanBeChasedBy()) {
						float dist = Math.Abs(projectile.Center.X - Main.npc[i].Center.X) + Math.Abs(projectile.Center.Y - Main.npc[i].Center.Y);
						if (dist < maxDist && Collision.CanHit(projectile.position, projectile.width, projectile.height, Main.npc[i].position, Main.npc[i].width, Main.npc[i].height)) {
							maxDist = dist;
							shootPosX = Main.npc[i].Center.X;
							shootPosY = Main.npc[i].Center.Y;
							validTarget = true;
						}
					}
				}

				if (validTarget) {
					const float Magnitude = 12f; //modify the speed the projectile are shot.  Lower number = slower projectile.
					float velX = shootPosX - projectile.Center.X;
					float velY = shootPosY - projectile.Center.Y;
					float str = (float)Math.Sqrt((velX * velX + velY * velY));
					str = Magnitude / str;
					velX *= str;
					velY *= str;
					Projectile.NewProjectile(projectile.Center.X - 4f, projectile.Center.Y, velX, velY, ModContent.ProjectileType<Blaze>(), projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
					projectile.ai[0] = 30f;
					return;
				}
			}
		}

		private void SpawnDust(Player player)
		{
			for (int k = 0; k < 3; k++) {
				int index2 = Dust.NewDust(new Vector2(projectile.Center.X - 6, projectile.Center.Y + 3), 1, 1, 127, 0.0f, 0.0f, 0, new Color(), 1f);
				Main.dust[index2].scale = .5f;
				Main.dust[index2].noGravity = false;
				Main.dust[index2].velocity.X = player.velocity.X;
				Main.dust[index2].velocity.Y *= 1.7f;
				Main.dust[index2].noLight = false;
			}
			for (int j = 0; j < 3; j++) {
				int index2 = Dust.NewDust(new Vector2(projectile.Center.X - 12, projectile.Center.Y + 3), 1, 1, 127, 0.0f, 0.0f, 0, new Color(), 1f);
				Main.dust[index2].scale = .5f;
				Main.dust[index2].noGravity = false;
				Main.dust[index2].velocity.X = player.velocity.X;
				Main.dust[index2].velocity.Y *= 1.7f;
				Main.dust[index2].noLight = false;
			}
		}
	}
}