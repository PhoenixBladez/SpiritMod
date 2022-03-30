using Microsoft.Xna.Framework;
using SpiritMod.Items;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Summon.BowSummon
{
	public class BowSummon : ModProjectile
	{
		int timer = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jinxbow");

			Main.projPet[projectile.type] = true;
			Main.projFrames[projectile.type] = 4;

			ProjectileID.Sets.TrailCacheLength[projectile.type] = 1;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
			ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
			ProjectileID.Sets.Homing[projectile.type] = true;
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.netImportant = true;
			projectile.width = 20;
			projectile.height = 40;
			projectile.friendly = true;
			projectile.minion = true;
			projectile.penetrate = -1;
			projectile.timeLeft = 18000;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
		}

		public override void AI()
		{
			bool flag64 = projectile.type == ModContent.ProjectileType<BowSummon>();
			Player player = Main.player[projectile.owner];
			MyPlayer modPlayer = player.GetSpiritPlayer();

			if (flag64)
			{
				if (player.dead)
					modPlayer.bowSummon = false;

				if (modPlayer.bowSummon)
					projectile.timeLeft = 2;
			}

			for (int i = 0; i < Main.maxProjectiles; i++)
			{
				if (i != projectile.whoAmI && Main.projectile[i].active && Main.projectile[i].owner == projectile.owner && Main.projectile[i].type == projectile.type && Math.Abs(projectile.position.X - Main.projectile[i].position.X) + Math.Abs(projectile.position.Y - Main.projectile[i].position.Y) < (float)projectile.width)
				{
					if (projectile.position.X < Main.projectile[i].position.X)
						projectile.velocity.X = projectile.velocity.X - 0.05f;
					else
						projectile.velocity.X = projectile.velocity.X + 0.05f;

					if (projectile.position.Y < Main.projectile[i].position.Y)
						projectile.velocity.Y = projectile.velocity.Y - 0.05f;
					else
						projectile.velocity.Y = projectile.velocity.Y + 0.05f;
				}
			}

			float num529 = 900f;
			bool flag19 = false;

			if (projectile.ai[0] == 0f)
			{
				for (int num531 = 0; num531 < 200; num531++)
				{
					if (Main.npc[num531].CanBeChasedBy(projectile, false))
					{
						float num532 = Main.npc[num531].position.X + 40 + (Main.npc[num531].width / 2);
						float num533 = Main.npc[num531].position.Y - 90 + (Main.npc[num531].height / 2);
						float num534 = Math.Abs(projectile.position.X + (projectile.width / 2) - num532) + Math.Abs(projectile.position.Y + (projectile.height / 2) - num533);
						if (num534 < num529 && Collision.CanHit(projectile.position, projectile.width, projectile.height, Main.npc[num531].position, Main.npc[num531].width, Main.npc[num531].height))
						{
							num529 = num534;
							flag19 = true;
						}
					}
				}
			}
			else
				projectile.tileCollide = false;

			if (!flag19)
			{
				projectile.friendly = true;
				projectile.position.X = Main.player[projectile.owner].Center.X - (projectile.width * .5f);
				projectile.position.Y = Main.player[projectile.owner].Center.Y - (projectile.width * .5f) - 40;
				projectile.rotation = Main.player[projectile.owner].velocity.X * 0.085f;
				projectile.spriteDirection = Main.player[projectile.owner].direction;
				projectile.frame = 0;
			}

			else
			{
				projectile.spriteDirection = 1;
				timer++;
				if (timer >= 70)
				{
					int range = 30;   //How many tiles away the projectile targets NPCs
					float shootVelocity = 16f; //magnitude of the shoot vector (speed of arrows shot)

					//TARGET NEAREST NPC WITHIN RANGE
					float lowestDist = float.MaxValue;
					for (int i = 0; i < 200; ++i)
					{
						NPC npc = Main.npc[i];
						//if npc is a valid target (active, not friendly, and not a critter)
						if (npc.active && npc.CanBeChasedBy(projectile) && !npc.friendly)
						{
							//if npc is within 50 blocks
							float dist = projectile.Distance(npc.Center);
							if (dist / 16 < range)
							{
								//if npc is closer than closest found npc
								if (dist < lowestDist)
								{
									lowestDist = dist;

									//target this npc
									projectile.ai[1] = npc.whoAmI;
									projectile.netUpdate = true;
								}
							}
						}
					}

					NPC target = Main.npc[(int)projectile.ai[1]];

					if (target.CanBeChasedBy(projectile, false))
						projectile.rotation = projectile.DirectionTo(target.Center).ToRotation();

					if (++projectile.frameCounter >= 5)
					{
						projectile.frameCounter = 0;
						projectile.frame++;
					}

					if (projectile.frame > 3)
					{
						Vector2 ShootArea = new Vector2(projectile.Center.X, projectile.Center.Y - 13);
						Vector2 direction = Vector2.Normalize(target.Center - ShootArea) * shootVelocity;

						int selectedIndex = 0;
						for (int i = 0; i < player.inventory.Length; ++i)
						{
							Item item = player.inventory[i];
							Item selItem = player.inventory[selectedIndex];

							if (item.active && item.ammo == AmmoID.Arrow && (selItem.ammo == AmmoID.None || item.damage > selItem.damage))
								selectedIndex = i;
						}

						Item selectedItem = player.inventory[selectedIndex];
						int shootType = selectedItem.shoot;
						int proj2 = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, direction.X, direction.Y, shootType, projectile.damage, projectile.knockBack, Main.myPlayer);
						Main.projectile[proj2].minion = true;
						Main.projectile[proj2].ranged = false;
						Main.projectile[proj2].netUpdate = true;

						GItem.UseAmmoDirect(Main.player[projectile.owner], selectedIndex);

						projectile.frame = 0;
						timer = 0;
					}
				}

				projectile.position.X = Main.player[projectile.owner].Center.X - (projectile.width * .5f);
				projectile.position.Y = Main.player[projectile.owner].Center.Y - (projectile.width * .5f) - 40;
			}
		}

		public override bool MinionContactDamage() => false;
	}
}