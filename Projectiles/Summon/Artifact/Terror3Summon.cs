using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Summon.Artifact
{
	public class Terror3Summon : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Terror Fiend");
			Main.projFrames[base.projectile.type] = 1;
			ProjectileID.Sets.MinionSacrificable[base.projectile.type] = true;
			ProjectileID.Sets.Homing[base.projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.netImportant = true;
			projectile.width = 48;
			projectile.height = 56;
			projectile.friendly = true;
			Main.projPet[projectile.type] = true;
			projectile.minion = true;
			projectile.minionSlots = 2;
			projectile.penetrate = -1;
			projectile.timeLeft = 18000;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			aiType = ProjectileID.Raven;
		}

		public override void AI()
		{
			int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 60, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
			int dust2 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 60, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
			Main.dust[dust].noGravity = true;
			Main.dust[dust2].noGravity = true;
			Main.dust[dust2].velocity *= 0f;
			Main.dust[dust2].velocity *= 0f;
			Main.dust[dust2].scale = 0.9f;
			Main.dust[dust].scale = 0.9f;

			bool flag64 = projectile.type == mod.ProjectileType("Terror3Summon");
			Player player = Main.player[projectile.owner];
			MyPlayer modPlayer = player.GetModPlayer<MyPlayer>(mod);
			if (flag64)
			{
				if (player.dead)
					modPlayer.terror3Summon = false;

				if (modPlayer.terror3Summon)
					projectile.timeLeft = 2;

			}
			for (int num526 = 0; num526 < 1000; num526++)
			{
				if (num526 != projectile.whoAmI && Main.projectile[num526].active && Main.projectile[num526].owner == projectile.owner && Main.projectile[num526].type == projectile.type && Math.Abs(projectile.position.X - Main.projectile[num526].position.X) + Math.Abs(projectile.position.Y - Main.projectile[num526].position.Y) < (float)projectile.width)
				{
					if (projectile.position.X < Main.projectile[num526].position.X)
						projectile.velocity.X = projectile.velocity.X - 0.05f;
					else
						projectile.velocity.X = projectile.velocity.X + 0.05f;

					if (projectile.position.Y < Main.projectile[num526].position.Y)
						projectile.velocity.Y = projectile.velocity.Y - 0.05f;
					else
						projectile.velocity.Y = projectile.velocity.Y + 0.05f;

				}
			}

			float num527 = projectile.position.X;
			float num528 = projectile.position.Y;
			float num529 = 900f;
			bool flag19 = false;
			int num530 = 500;
			if (projectile.ai[1] != 0f || projectile.friendly)
				num530 = 1400;

			//if (Math.Abs(Projectile.Center.X - Main.player[projectile.owner].Center.X) + Math.Abs(Projectile.Center.Y - Main.player[projectile.owner].Center.Y) > (float)num530)
			//{
			//	projectile.ai[0] = 1f;
			//}
			if (projectile.ai[0] == 0f)
			{
				for (int num531 = 0; num531 < 200; num531++)
				{
					if (Main.npc[num531].CanBeChasedBy(projectile, false))
					{
						float num532 = Main.npc[num531].position.X + (float)(Main.npc[num531].width / 2);
						float num533 = Main.npc[num531].position.Y + (float)(Main.npc[num531].height / 2);
						float num534 = Math.Abs(projectile.position.X + (float)(projectile.width / 2) - num532) + Math.Abs(projectile.position.Y + (float)(projectile.height / 2) - num533);
						if (num534 < num529 && Collision.CanHit(projectile.position, projectile.width, projectile.height, Main.npc[num531].position, Main.npc[num531].width, Main.npc[num531].height))
						{
							num529 = num534;
							num527 = num532;
							num528 = num533;
							flag19 = true;
						}
					}
				}
			}
			else
			{
				projectile.tileCollide = false;
			}

			if (!flag19)
			{
				projectile.friendly = true;
				float num535 = 8f;
				if (projectile.ai[0] == 1f)
					num535 = 12f;

				Vector2 vector38 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
				float num536 = Main.player[projectile.owner].Center.X - vector38.X;
				float num537 = Main.player[projectile.owner].Center.Y - vector38.Y - 60f;
				float num538 = (float)Math.Sqrt((double)(num536 * num536 + num537 * num537));
				if (num538 < 100f && projectile.ai[0] == 1f && !Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
				{
					projectile.ai[0] = 0f;
				}
				if (num538 > 2000f)
				{
					projectile.position.X = Main.player[projectile.owner].Center.X - (float)(projectile.width / 2);
					projectile.position.Y = Main.player[projectile.owner].Center.Y - (float)(projectile.width / 2);
				}
				if (num538 > 70f)
				{
					num538 = num535 / num538;
					num536 *= num538;
					num537 *= num538;
					projectile.velocity.X = (projectile.velocity.X * 20f + num536) / 21f;
					projectile.velocity.Y = (projectile.velocity.Y * 20f + num537) / 21f;
				}
				else
				{
					if (projectile.velocity.X == 0f && projectile.velocity.Y == 0f)
					{
						projectile.velocity.X = -0.15f;
						projectile.velocity.Y = -0.05f;
					}
					projectile.velocity *= 1.01f;
				}

				projectile.friendly = false;
				projectile.rotation = projectile.velocity.X * 0.05f;
				if (Math.Abs(projectile.velocity.X) > 0.2)
				{
					projectile.spriteDirection = -projectile.direction;
					return;
				}
			}
			else
			{
				if (projectile.ai[1] == -1f)
					projectile.ai[1] = 17f;

				if (projectile.ai[1] > 0f)
					projectile.ai[1] -= 1f;

				if (projectile.ai[1] == 0f)
				{
					projectile.friendly = true;
					float num539 = 8f;
					Vector2 vector39 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
					float num540 = num527 - vector39.X;
					float num541 = num528 - vector39.Y;
					float num542 = (float)Math.Sqrt((double)(num540 * num540 + num541 * num541));
					if (num542 < 100f)
						num539 = 10f;

					num542 = num539 / num542;
					num540 *= num542;
					num541 *= num542;
					projectile.velocity.X = (projectile.velocity.X * 14f + num540) / 15f;
					projectile.velocity.Y = (projectile.velocity.Y * 14f + num541) / 15f;
				}
				else
				{
					projectile.friendly = false;
					if (Math.Abs(projectile.velocity.X) + Math.Abs(projectile.velocity.Y) < 10f)
						projectile.velocity *= 1.05f;

				}

				projectile.rotation = projectile.velocity.X * 0.05f;
				projectile.frameCounter++;
				if (projectile.frameCounter >= 4)
				{
					projectile.frameCounter = 0;
					projectile.frame++;
				}

				if (projectile.frame < 4)
					projectile.frame = 4;
				else if (projectile.frame > 7)
					projectile.frame = 4;

				if (Math.Abs(projectile.velocity.X) > 0.2)
				{
					projectile.spriteDirection = -projectile.direction;
					return;
				}
			}

			if (projectile.frameCounter < 8)
				projectile.frame = 0;
			else if (projectile.frameCounter >= 8 && projectile.frameCounter < 10)
				projectile.frame = 1;
			else
				projectile.frameCounter = 0;
			projectile.frameCounter++;

			if (Main.rand.Next(50) == 0)
			{
				float num = 8000f;
				int num2 = -1;
				for (int i = 0; i < 200; i++)
				{
					float num3 = Vector2.Distance(projectile.Center, Main.npc[i].Center);
					if (num3 < num && num3 < 640f && Main.npc[i].CanBeChasedBy(projectile, false))
					{
						num2 = i;
						num = num3;
					}
				}
				if (num2 != -1)
				{
					bool flag = Collision.CanHit(projectile.position, projectile.width, projectile.height, Main.npc[num2].position, Main.npc[num2].width, Main.npc[num2].height);
					if (flag)
					{
						Vector2 value = Main.npc[num2].Center - projectile.Center;
						float num4 = 5f;
						float num5 = (float)Math.Sqrt((double)(value.X * value.X + value.Y * value.Y));
						if (num5 > num4)
							num5 = num4 / num5;

						value *= num5;
						int p = Terraria.Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, value.X, value.Y, mod.ProjectileType("WitherShard2"), 34, projectile.knockBack / 2f, projectile.owner, 0f, 0f);
						Main.projectile[p].friendly = true;
						Main.projectile[p].hostile = false;
					}
				}
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(19) == 0)
			{
				Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 14);
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0f, 0f, mod.ProjectileType("NightmareExplosion"), 60, projectile.knockBack, projectile.owner, 0f, 0f);
			}
		}

		public override bool MinionContactDamage()
		{
			return true;
		}

	}
}