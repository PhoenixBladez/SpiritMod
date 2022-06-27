using Microsoft.Xna.Framework;
using SpiritMod.Utilities;
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
			Main.projPet[Projectile.type] = true;
			Main.projFrames[Projectile.type] = 1;
			ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.width = 36;
			Projectile.height = 36;
			Projectile.netImportant = true;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.ignoreWater = true;
			Projectile.minionSlots = 1f;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.minion = true;
		}

		public override void AI()
		{
			Projectile.localAI[0]++;
			if (Projectile.localAI[0] >= 10f) {
				Projectile.localAI[0] = 0f;
				int num416 = 0;
				int num417 = 0;
				float currentCap = 0f;
				for (int i = 0; i < 1000; i++) {
					if (Main.projectile[i].active && Main.projectile[i].owner == Projectile.owner && Main.projectile[i].type == Projectile.type && Main.projectile[i].ai[1] < 3600f) {
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
			Lighting.AddLight(Projectile.position, 0.4f, .12f, .036f);

			Player player = Main.player[Projectile.owner];
			SpawnDust(player);

			MyPlayer modPlayer = player.GetSpiritPlayer();
			if (Projectile.type == ModContent.ProjectileType<LavaRockSummon>()) {
				if (player.dead)
					modPlayer.lavaRock = false;
				if (modPlayer.lavaRock)
					Projectile.timeLeft = 2;
			}

			Projectile.position.X = Main.player[Projectile.owner].Center.X - (Projectile.width / 2f);
			Projectile.position.Y = Main.player[Projectile.owner].Center.Y - (Projectile.height / 2f) + Main.player[Projectile.owner].gfxOffY - 60f;

			if (Main.player[Projectile.owner].gravDir == -1f) {
				Projectile.position.Y = Projectile.position.Y + 120f;
				Projectile.rotation = 3.14f;
			}
			else
				Projectile.rotation = 0f;

			float adjScale = Main.mouseTextColor / 200f - 0.35f;
			adjScale *= 0.2f;
			Projectile.scale = adjScale + 0.95f;

			if (Projectile.owner == Main.myPlayer) {
				if (Projectile.ai[0] != 0f) {
					Projectile.ai[0] -= 1f;
					return;
				}

				float shootPosX = Projectile.position.X;
				float shootPosY = Projectile.position.Y;
				float maxDist = 700f;
				bool validTarget = false;

				for (int i = 0; i < Main.maxNPCs; i++) {
					if (Main.npc[i].CanBeChasedBy()) {
						float dist = Math.Abs(Projectile.Center.X - Main.npc[i].Center.X) + Math.Abs(Projectile.Center.Y - Main.npc[i].Center.Y);
						if (dist < maxDist && Collision.CanHit(Projectile.position, Projectile.width, Projectile.height, Main.npc[i].position, Main.npc[i].width, Main.npc[i].height)) {
							maxDist = dist;
							shootPosX = Main.npc[i].Center.X;
							shootPosY = Main.npc[i].Center.Y;
							validTarget = true;
						}
					}
				}

				if (validTarget) {
					Projectile.NewProjectile(Projectile.Center, Projectile.GetArcVel(new Vector2(shootPosX, shootPosY), 0.15f, heightabovetarget: 150), ModContent.ProjectileType<Blaze>(), 
						Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
					Projectile.ai[0] = 30f;
					return;
				}
			}
		}

		private void SpawnDust(Player player)
		{
			for (int k = 0; k < 3; k++) {
				int index2 = Dust.NewDust(new Vector2(Projectile.Center.X - 6, Projectile.Center.Y + 3), 1, 1, DustID.Flare, 0.0f, 0.0f, 0, new Color(), 1f);
				Main.dust[index2].scale = .5f;
				Main.dust[index2].noGravity = false;
				Main.dust[index2].velocity.X = player.velocity.X;
				Main.dust[index2].velocity.Y *= 1.7f;
				Main.dust[index2].noLight = false;
			}

			for (int j = 0; j < 3; j++) {
				int index2 = Dust.NewDust(new Vector2(Projectile.Center.X - 12, Projectile.Center.Y + 3), 1, 1, DustID.Flare, 0.0f, 0.0f, 0, new Color(), 1f);
				Main.dust[index2].scale = .5f;
				Main.dust[index2].noGravity = false;
				Main.dust[index2].velocity.X = player.velocity.X;
				Main.dust[index2].velocity.Y *= 1.7f;
				Main.dust[index2].noLight = false;
			}
		}
	}
}