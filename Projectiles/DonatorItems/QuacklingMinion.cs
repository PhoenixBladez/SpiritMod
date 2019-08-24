using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.DonatorItems
{
	public class QuacklingMinion : ModProjectile
	{
		int timer = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Quackling Minion");
			Main.projFrames[base.projectile.type] = 4;
			ProjectileID.Sets.MinionSacrificable[base.projectile.type] = true;
			ProjectileID.Sets.Homing[base.projectile.type] = true;
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.netImportant = true;
			projectile.width = 52;
			projectile.height = 40;
			projectile.friendly = true;
			Main.projPet[projectile.type] = true;
			projectile.minion = true;
			projectile.minionSlots = 1;
			projectile.penetrate = -1;
			projectile.timeLeft = 18000;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			aiType = ProjectileID.Raven;
		}

		public override void AI()
		{
			projectile.frameCounter++;
			if (projectile.frameCounter >= 6f)
			{
				projectile.frame = (projectile.frame + 1) % Main.projFrames[projectile.type];
				projectile.frameCounter = 0;
			}

			bool flag64 = projectile.type == mod.ProjectileType("QuacklingMinion");
			Player player = Main.player[projectile.owner];
			MyPlayer modPlayer = player.GetModPlayer<MyPlayer>(mod);
			if (flag64)
			{
				if (player.dead)
					modPlayer.QuacklingMinion = false;

				if (modPlayer.QuacklingMinion)
					projectile.timeLeft =2;

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

			if (projectile.ai[0] == 0f)
			{
				for (int num531 = 0; num531 < 200; num531++)
				{
					if (Main.npc[num531].CanBeChasedBy(projectile, false))
					{
						float num532 = Main.npc[num531].position.X + (float)(Main.npc[num531].width / 2);
						float num533 = Main.npc[num531].position.Y - 250 + (float)(Main.npc[num531].height / 2);
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
					projectile.position.X = Main.player[projectile.owner].Center.X - (projectile.width * .5f);
					projectile.position.Y = Main.player[projectile.owner].Center.Y - (projectile.width * .5f);
				}

				if (num538 > 70f)
				{
					num538 = num535 / num538;
					num536 *= num538;
					num537 *= num538;
					projectile.velocity.X = (projectile.velocity.X * 20f + num536) * (1f / 21f);
					projectile.velocity.Y = (projectile.velocity.Y * 20f + num537) * (1f / 21f);
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
				timer++;
				if (timer % 60 == 1)
					Projectile.NewProjectile(projectile.Center, new Vector2(0, 3),
						mod.ProjectileType("AquaBall"), projectile.damage, 0, Main.myPlayer);

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

				if (Math.Abs(projectile.velocity.X) > 0.2)
				{
					projectile.spriteDirection = -projectile.direction;
					return;
				}
			}
		}

		public override bool MinionContactDamage()
		{
			return true;
		}

	}
}