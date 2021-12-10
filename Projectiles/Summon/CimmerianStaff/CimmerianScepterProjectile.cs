using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;
using SpiritMod.Utilities;
using SpiritMod.Items.Accessory;

namespace SpiritMod.Projectiles.Summon.CimmerianStaff
{
	public class CimmerianScepterProjectile : ModProjectile, IDrawAdditive
    {
		int timer = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cimmerian Scepter");
			Main.projFrames[base.projectile.type] = 1;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 1;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
            ProjectileID.Sets.MinionSacrificable[base.projectile.type] = true;
			ProjectileID.Sets.Homing[base.projectile.type] = true;
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.netImportant = true;
			projectile.width = 20;
			projectile.height = 52;
			projectile.friendly = true;
			Main.projPet[projectile.type] = true;
			projectile.minion = true;
			projectile.minionSlots = 0;
			projectile.penetrate = -1;
			projectile.timeLeft = 18000;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			aiType = ProjectileID.Raven;
		}
        Color colorVer;
        float alphaCounter;
		public override void AI()
		{
			alphaCounter += 0.03f;
			Player player = Main.player[projectile.owner];

			if (player.AccessoryEquipped<CimmerianScepter>())
				projectile.timeLeft = 2;

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
						float num532 = Main.npc[num531].position.X + 40 + (float)(Main.npc[num531].width / 2);
						float num533 = Main.npc[num531].position.Y - 90 + (float)(Main.npc[num531].height / 2);
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
				projectile.tileCollide = false;

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
						projectile.velocity.X = -0.05f;
						projectile.velocity.Y = -0.025f;
					}
					projectile.velocity *= 1.0035f;
				}
				projectile.friendly = false;
				projectile.rotation = projectile.velocity.X * 0.15f;

				if (Math.Abs(projectile.velocity.X) > 0.05)
				{
					projectile.spriteDirection = -projectile.direction;
					return;
				}
			}

			else
			{
				timer++;
				if (timer > 130 && timer < 170)
					projectile.rotation += .3f;
				else
				{
					for (int num531 = 0; num531 < 200; num531++)
					{
						if (Main.npc[num531].CanBeChasedBy(projectile, false))
							projectile.rotation = projectile.DirectionTo(Main.npc[num531].Center).ToRotation() + 1.57f;
					}
				}
				if (timer >= Main.rand.Next(180, 210))
				{
					int range = 100;   //How many tiles away the projectile targets NPCs
					float shootVelocity = 9.5f; //magnitude of the shoot vector (speed of arrows shot)

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
					NPC target = (Main.npc[(int)projectile.ai[1]] ?? new NPC());
					timer = 0;
					Vector2 ShootArea = new Vector2(projectile.Center.X, projectile.Center.Y - 13);
					Vector2 direction = Vector2.Normalize(target.Center - ShootArea) * shootVelocity;
					switch (Main.rand.Next(3))
					{
						case 0: //star attack
							colorVer = new Color(126, 61, 255);
							Main.PlaySound(SoundID.Item, projectile.Center, 9);
							for (int z = 0; z < 4; z++)
							{
								Vector2 pos = new Vector2(projectile.Center.X + Main.rand.Next(-30, 30), projectile.Center.Y + Main.rand.Next(-30, 30));
								DustHelper.DrawStar(pos, 272, pointAmount: 5, mainSize: 1.425f, dustDensity: 2, dustSize: .5f, pointDepthMult: 0.3f, noGravity: true);
								int proj2 = Projectile.NewProjectile(pos.X, pos.Y, direction.X + Main.rand.Next(-2, 2), direction.Y + Main.rand.Next(-2, 2), mod.ProjectileType("CimmerianStaffStar"), projectile.damage, 0, Main.myPlayer);
							}

							break;
						case 1: //explosion attack
							colorVer = new Color(255, 20, 52);
							Main.PlaySound(SoundID.DD2_GhastlyGlaiveImpactGhost, projectile.Center);
							DustHelper.DrawCircle(projectile.Center, 130, 1, 1f, 1f, .85f, .85f);
							int proj23 = Projectile.NewProjectile(target.Center.X, target.Center.Y, 0f, 0f, mod.ProjectileType("CimmerianRedGlyph"), projectile.damage, 0, Main.myPlayer);

							break;
						case 2: //lightning attack
							colorVer = new Color(61, 184, 255);
							Main.PlaySound(SoundID.Item, projectile.Center, 12);
							for (int k = 0; k < 15; k++)
							{
								Dust d = Dust.NewDustPerfect(projectile.Center, 226, Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(3), 0, default, Main.rand.NextFloat(.4f, .8f));
								d.noGravity = true;
							}
							for (int i = 0; i < 3; i++)
								DustHelper.DrawElectricity(projectile.Center, target.Center, 226, 0.3f);
							target.StrikeNPC((int)(projectile.damage * 1.5f), 1f, 0, false);
							for (int k = 0; k < 10; k++)
							{
								Dust d = Dust.NewDustPerfect(target.Center, 226, Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(2), 0, default, Main.rand.NextFloat(.2f, .4f));
								d.noGravity = true;
							}
							break;
					}
				}
				if (projectile.ai[1] == -1f)
					projectile.ai[1] = 17f;

				if (projectile.ai[1] > 0f)
					projectile.ai[1] -= 1f;

				if (projectile.ai[1] == 0f)
				{
					float num535 = 8f;
					if (projectile.ai[0] == 1f)
						num535 = 12f;
					Vector2 vector38 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
					float num536 = Main.player[projectile.owner].Center.X - vector38.X;
					float num537 = Main.player[projectile.owner].Center.Y - vector38.Y - 60f;
					float num538 = (float)Math.Sqrt((double)(num536 * num536 + num537 * num537));
					if (num538 < 100f && projectile.ai[0] == 1f && !Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
						projectile.ai[0] = 0f;
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
							projectile.velocity.X = -0.05f;
							projectile.velocity.Y = -0.025f;
						}
						projectile.velocity *= 1.0035f;
					}
					projectile.friendly = false;

					if (Math.Abs(projectile.velocity.X) > 0.05)
					{
						projectile.spriteDirection = -projectile.direction;
						return;
					}
				}
			}
		}
        public void AdditiveCall(SpriteBatch spriteBatch)
        {
            float sineAdd = (float)Math.Sin(alphaCounter) + 2;
            {
                for (int k = 0; k < 1; k++)
                {
                    Color color = colorVer * sineAdd * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);

                    float scale = projectile.scale;
                    Texture2D tex = ModContent.GetTexture("SpiritMod/Projectiles/Summon/CimmerianStaff/CimmerianScepterProjectile_Glow");

                    spriteBatch.Draw(tex, projectile.oldPos[k] + projectile.Size / 2 - Main.screenPosition, null, color, projectile.rotation, tex.Size() / 2, scale, default, default);
                    //spriteBatch.Draw(tex, projectile.oldPos[k] + projectile.Size / 2 - Main.screenPosition, null, color, projectile.rotation, tex.Size() / 2, scale, default, default);
                }
            }
        }

		public override bool MinionContactDamage() => true;
	}
}