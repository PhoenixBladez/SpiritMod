using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
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
			Main.projFrames[base.Projectile.type] = 1;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 1;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.MinionSacrificable[base.Projectile.type] = true;
			ProjectileID.Sets.CultistIsResistantTo[base.Projectile.type] = true;
			ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.netImportant = true;
			Projectile.width = 20;
			Projectile.height = 52;
			Projectile.friendly = true;
			Main.projPet[Projectile.type] = true;
			Projectile.minion = true;
			Projectile.minionSlots = 0;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 18000;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			AIType = ProjectileID.Raven;
		}

        Color colorVer;
        float alphaCounter;

		public override void AI()
		{
			alphaCounter += 0.03f;
			Player player = Main.player[Projectile.owner];

			if (player.HasAccessory<CimmerianScepter>())
				Projectile.timeLeft = 2;

			for (int num526 = 0; num526 < 1000; num526++)
			{
				if (num526 != Projectile.whoAmI && Main.projectile[num526].active && Main.projectile[num526].owner == Projectile.owner && Main.projectile[num526].type == Projectile.type && Math.Abs(Projectile.position.X - Main.projectile[num526].position.X) + Math.Abs(Projectile.position.Y - Main.projectile[num526].position.Y) < (float)Projectile.width)
				{
					if (Projectile.position.X < Main.projectile[num526].position.X)
						Projectile.velocity.X = Projectile.velocity.X - 0.05f;
					else
						Projectile.velocity.X = Projectile.velocity.X + 0.05f;

					if (Projectile.position.Y < Main.projectile[num526].position.Y)
						Projectile.velocity.Y = Projectile.velocity.Y - 0.05f;
					else
						Projectile.velocity.Y = Projectile.velocity.Y + 0.05f;
				}
			}

			float num529 = 900f;
			bool flag19 = false;

			if (Projectile.ai[0] == 0f)
			{
				for (int num531 = 0; num531 < 200; num531++)
				{
					if (Main.npc[num531].CanBeChasedBy(Projectile, false))
					{
						float num532 = Main.npc[num531].position.X + 40 + (float)(Main.npc[num531].width / 2);
						float num533 = Main.npc[num531].position.Y - 90 + (float)(Main.npc[num531].height / 2);
						float num534 = Math.Abs(Projectile.position.X + (float)(Projectile.width / 2) - num532) + Math.Abs(Projectile.position.Y + (float)(Projectile.height / 2) - num533);
						if (num534 < num529 && Collision.CanHit(Projectile.position, Projectile.width, Projectile.height, Main.npc[num531].position, Main.npc[num531].width, Main.npc[num531].height))
						{
							num529 = num534;
							flag19 = true;
						}
					}
				}
			}
			else
				Projectile.tileCollide = false;

			if (!flag19)
			{
				Projectile.friendly = true;
				float num535 = 8f;
				if (Projectile.ai[0] == 1f)
					num535 = 12f;

				Vector2 vector38 = new Vector2(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
				float num536 = Main.player[Projectile.owner].Center.X - vector38.X;
				float num537 = Main.player[Projectile.owner].Center.Y - vector38.Y - 60f;
				float num538 = (float)Math.Sqrt((double)(num536 * num536 + num537 * num537));
				if (num538 < 100f && Projectile.ai[0] == 1f && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
				{
					Projectile.ai[0] = 0f;
				}
				if (num538 > 2000f)
				{
					Projectile.position.X = Main.player[Projectile.owner].Center.X - (Projectile.width * .5f);
					Projectile.position.Y = Main.player[Projectile.owner].Center.Y - (Projectile.width * .5f);
				}

				if (num538 > 70f)
				{
					num538 = num535 / num538;
					num536 *= num538;
					num537 *= num538;
					Projectile.velocity.X = (Projectile.velocity.X * 20f + num536) * (1f / 21f);
					Projectile.velocity.Y = (Projectile.velocity.Y * 20f + num537) * (1f / 21f);
				}
				else
				{
					if (Projectile.velocity.X == 0f && Projectile.velocity.Y == 0f)
					{
						Projectile.velocity.X = -0.05f;
						Projectile.velocity.Y = -0.025f;
					}
					Projectile.velocity *= 1.0035f;
				}
				Projectile.friendly = false;
				Projectile.rotation = Projectile.velocity.X * 0.15f;

				if (Math.Abs(Projectile.velocity.X) > 0.05)
				{
					Projectile.spriteDirection = -Projectile.direction;
					return;
				}
			}

			else
			{
				timer++;
				if (timer > 130 && timer < 170)
					Projectile.rotation += .3f;
				else
				{
					for (int num531 = 0; num531 < 200; num531++)
					{
						if (Main.npc[num531].CanBeChasedBy(Projectile, false))
							Projectile.rotation = Projectile.DirectionTo(Main.npc[num531].Center).ToRotation() + 1.57f;
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
						if (npc.active && npc.CanBeChasedBy(Projectile) && !npc.friendly)
						{
							//if npc is within 50 blocks
							float dist = Projectile.Distance(npc.Center);
							if (dist / 16 < range)
							{
								//if npc is closer than closest found npc
								if (dist < lowestDist)
								{
									lowestDist = dist;

									//target this npc
									Projectile.ai[1] = npc.whoAmI;
									Projectile.netUpdate = true;
								}
							}
						}
					}
					NPC target = (Main.npc[(int)Projectile.ai[1]] ?? new NPC());
					timer = 0;
					Vector2 ShootArea = new Vector2(Projectile.Center.X, Projectile.Center.Y - 13);
					Vector2 direction = Vector2.Normalize(target.Center - ShootArea) * shootVelocity;
					switch (Main.rand.Next(3))
					{
						case 0: //star attack
							colorVer = new Color(126, 61, 255);
							SoundEngine.PlaySound(SoundID.Item9, Projectile.Center);
							for (int z = 0; z < 4; z++)
							{
								Vector2 pos = new Vector2(Projectile.Center.X + Main.rand.Next(-30, 30), Projectile.Center.Y + Main.rand.Next(-30, 30));
								DustHelper.DrawStar(pos, 272, pointAmount: 5, mainSize: 1.425f, dustDensity: 2, dustSize: .5f, pointDepthMult: 0.3f, noGravity: true);
								Projectile.NewProjectile(Projectile.GetSource_FromAI(), pos.X, pos.Y, direction.X + Main.rand.Next(-2, 2), direction.Y + Main.rand.Next(-2, 2), ModContent.ProjectileType<CimmerianStaffStar>(), Projectile.damage, 0, Main.myPlayer);
							}
							break;
						case 1: //explosion attack
							colorVer = new Color(255, 20, 52);
							SoundEngine.PlaySound(SoundID.DD2_GhastlyGlaiveImpactGhost, Projectile.Center);
							DustHelper.DrawCircle(Projectile.Center, 130, 1, 1f, 1f, .85f, .85f);
							Projectile.NewProjectile(Projectile.GetSource_FromAI(), target.Center.X, target.Center.Y, 0f, 0f, ModContent.ProjectileType<CimmerianRedGlyph>(), Projectile.damage, 0, Main.myPlayer);
							break;
						case 2: //lightning attack
							colorVer = new Color(61, 184, 255);
							SoundEngine.PlaySound(SoundID.Item12, Projectile.Center);
							for (int k = 0; k < 15; k++)
							{
								Dust d = Dust.NewDustPerfect(Projectile.Center, 226, Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(3), 0, default, Main.rand.NextFloat(.4f, .8f));
								d.noGravity = true;
							}
							for (int i = 0; i < 3; i++)
								DustHelper.DrawElectricity(Projectile.Center, target.Center, 226, 0.3f);
							target.StrikeNPC((int)(Projectile.damage * 1.5f), 1f, 0, false);
							for (int k = 0; k < 10; k++)
							{
								Dust d = Dust.NewDustPerfect(target.Center, 226, Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(2), 0, default, Main.rand.NextFloat(.2f, .4f));
								d.noGravity = true;
							}
							break;
					}
				}
				if (Projectile.ai[1] == -1f)
					Projectile.ai[1] = 17f;

				if (Projectile.ai[1] > 0f)
					Projectile.ai[1] -= 1f;

				if (Projectile.ai[1] == 0f)
				{
					float num535 = 8f;
					if (Projectile.ai[0] == 1f)
						num535 = 12f;
					Vector2 vector38 = new Vector2(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
					float num536 = Main.player[Projectile.owner].Center.X - vector38.X;
					float num537 = Main.player[Projectile.owner].Center.Y - vector38.Y - 60f;
					float num538 = (float)Math.Sqrt((double)(num536 * num536 + num537 * num537));
					if (num538 < 100f && Projectile.ai[0] == 1f && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
						Projectile.ai[0] = 0f;
					if (num538 > 2000f)
					{
						Projectile.position.X = Main.player[Projectile.owner].Center.X - (Projectile.width * .5f);
						Projectile.position.Y = Main.player[Projectile.owner].Center.Y - (Projectile.width * .5f);
					}

					if (num538 > 70f)
					{
						num538 = num535 / num538;
						num536 *= num538;
						num537 *= num538;
						Projectile.velocity.X = (Projectile.velocity.X * 20f + num536) * (1f / 21f);
						Projectile.velocity.Y = (Projectile.velocity.Y * 20f + num537) * (1f / 21f);
					}
					else
					{
						if (Projectile.velocity.X == 0f && Projectile.velocity.Y == 0f)
						{
							Projectile.velocity.X = -0.05f;
							Projectile.velocity.Y = -0.025f;
						}
						Projectile.velocity *= 1.0035f;
					}
					Projectile.friendly = false;

					if (Math.Abs(Projectile.velocity.X) > 0.05)
					{
						Projectile.spriteDirection = -Projectile.direction;
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
                    Color color = colorVer * sineAdd * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);

                    float scale = Projectile.scale;
                    Texture2D tex = ModContent.Request<Texture2D>("SpiritMod/Projectiles/Summon/CimmerianStaff/CimmerianScepterProjectile_Glow", ReLogic.Content.AssetRequestMode.ImmediateLoad);

                    spriteBatch.Draw(tex, Projectile.oldPos[k] + Projectile.Size / 2 - Main.screenPosition, null, color, Projectile.rotation, tex.Size() / 2, scale, default, default);
                    //spriteBatch.Draw(tex, projectile.oldPos[k] + projectile.Size / 2 - Main.screenPosition, null, color, projectile.rotation, tex.Size() / 2, scale, default, default);
                }
            }
        }

		public override bool MinionContactDamage() => true;
	}
}