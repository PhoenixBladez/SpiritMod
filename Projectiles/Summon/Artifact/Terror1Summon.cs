using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using SpiritMod.Projectiles.Summon;

namespace SpiritMod.Projectiles.Summon.Artifact
{
	public class Terror1Summon : Minion
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Terror Fiend");
			ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
			ProjectileID.Sets.Homing[projectile.type] = true;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 9;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
			Main.projFrames[projectile.type] = 1;
		}

		public override void SetDefaults()
		{
			projectile.width = 52;
			projectile.height = 68;
			Main.projPet[projectile.type] = true;
			projectile.friendly = true;
			projectile.minion = true;
			projectile.minionSlots = 2f;
			projectile.penetrate = -1;
			projectile.timeLeft = 18000;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.netImportant = true;
		}

		public override void CheckActive()
		{
			MyPlayer mp = Main.player[projectile.owner].GetModPlayer<MyPlayer>(base.mod);
			if (mp.player.dead)
				mp.terror1Summon = false;

			if (mp.terror1Summon)
				projectile.timeLeft = 2;

		}

		public override void Behavior()
		{
			Player player = Main.player[projectile.owner];
			float num = (float)projectile.width * 1.1f;
			for (int i = 0; i < 1000; i++)
			{
				Projectile current = Main.projectile[i];
				if (i != projectile.whoAmI && current.active && current.owner == projectile.owner && current.type == projectile.type && Math.Abs(projectile.position.X - current.position.X) + Math.Abs(projectile.position.Y - current.position.Y) < num)
				{
					if (projectile.position.X < Main.projectile[i].position.X)
						projectile.velocity.X -= 0.08f;
					else
						projectile.velocity.X += 0.08f;

					if (projectile.position.Y < Main.projectile[i].position.Y)
						projectile.velocity.Y -= 0.08f;
					else
						projectile.velocity.Y += 0.08f;

				}
			}

			Vector2 value = projectile.position;
			float num2 = 500f;
			bool flag = false;
			projectile.tileCollide = true;
			for (int j = 0; j < 200; j++)
			{
				NPC nPC = Main.npc[j];
				if (nPC.CanBeChasedBy(this, false))
				{
					float num3 = Vector2.Distance(nPC.Center, projectile.Center);
					if ((num3 < num2 || !flag) && Collision.CanHitLine(projectile.position, projectile.width, projectile.height, nPC.position, nPC.width, nPC.height))
					{
						num2 = num3;
						value = nPC.Center;
						flag = true;
					}
				}
			}

			if (Vector2.Distance(player.Center, projectile.Center) > (flag ? 1000f : 500f))
			{
				projectile.ai[0] = 1f;
				projectile.netUpdate = true;
			}
			if (projectile.ai[0] == 1f)
			{
				projectile.tileCollide = false;
			}
			if (flag && projectile.ai[0] == 0f)
			{
				Vector2 value2 = value - projectile.Center;
				if (value2.Length() > 200f)
				{
					value2.Normalize();
					projectile.velocity = (projectile.velocity * 20f + value2 * 6f) / 21f;
				}
				else
				{
					projectile.velocity *= (float)Math.Pow(0.97, 2.0);
				}
			}
			else
			{
				if (!Collision.CanHitLine(projectile.Center, 1, 1, player.Center, 1, 1))
					projectile.ai[0] = 1f;

				float num4 = 6f;
				if (projectile.ai[0] == 1f)
					num4 = 15f;

				Vector2 center = projectile.Center;
				Vector2 vector = player.Center - center;
				projectile.ai[1] = 3600f;
				projectile.netUpdate = true;
				int num5 = 1;
				for (int k = 0; k < projectile.whoAmI; k++)
				{
					if (Main.projectile[k].active && Main.projectile[k].owner == projectile.owner && Main.projectile[k].type == projectile.type)
						num5++;
				}
				vector.X -= (float)((10 + num5 * 40) * player.direction);
				vector.Y -= 70f;

				float num6 = vector.Length();
				if (num6 > 200f && num4 < 9f)
					num4 = 9f;
				else if (num6 < 100f && projectile.ai[0] == 1f && !Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
				{
					projectile.ai[0] = 0f;
					projectile.netUpdate = true;
				}

				if (num6 > 2000f)
					projectile.Center = player.Center;

				if (num6 > 48f)
				{
					vector.Normalize();
					vector *= num4;
					float num7 = 10f;
					projectile.velocity = (projectile.velocity * num7 + vector) / (num7 + 1f);
				}
				else
				{
					projectile.direction = Main.player[projectile.owner].direction;
					projectile.velocity *= (float)Math.Pow(0.9, 2.0);
				}
			}

			projectile.rotation = projectile.velocity.X * 0.05f;
			if (projectile.velocity.X > 0f)
				projectile.spriteDirection = (projectile.direction = -1);
			else if (projectile.velocity.X < 0f)
				projectile.spriteDirection = (projectile.direction = 1);

			if (projectile.ai[1] > 0f)
				projectile.ai[1] += 1f;

			if (projectile.ai[1] > 140f)
			{
				projectile.ai[1] = 0f;
				projectile.netUpdate = true;
			}

			if (projectile.ai[0] == 0f && flag)
			{
				if ((value - projectile.Center).X > 0f)
					projectile.spriteDirection = (projectile.direction = -1);
				else if ((value - projectile.Center).X < 0f)
					projectile.spriteDirection = (projectile.direction = 1);

				if (projectile.ai[1] == 0f)
				{
					projectile.ai[1] = 1f;
					if (Main.myPlayer == projectile.owner)
					{
						Vector2 vector2 = value - projectile.Center;
						if (vector2 == Vector2.Zero)
							vector2 = Vector2.UnitY;

						vector2.Normalize();
						vector2 *= 9f;
						int num8 = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vector2.X, vector2.Y, base.mod.ProjectileType("WitherShard1"), 15, projectile.knockBack, Main.myPlayer, 0f, 0f);
						Main.projectile[num8].timeLeft = 300;
						Main.projectile[num8].netUpdate = true;
						projectile.netUpdate = true;
					}
				}
			}

			if (projectile.ai[0] == 0f)
			{
				if (Main.rand.Next(5) == 0)
				{
					int num9 = Dust.NewDust(projectile.position, projectile.width, projectile.height / 2, 60, 0f, 0f, 0, default(Color), 1f);
					Main.dust[num9].velocity.Y -= 1.2f;
				}
			}
			else if (Main.rand.Next(3) == 0)
			{
				Vector2 velocity = projectile.velocity;
				if (velocity != Vector2.Zero)
					velocity.Normalize();

				int num10 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 60, 0f, 0f, 0, default(Color), 1f);
				Main.dust[num10].velocity -= 1.2f * velocity;
			}

			Lighting.AddLight((int)(projectile.Center.X / 16f), (int)(projectile.Center.Y / 16f), 0.2f, 0.2f, 0.9f);
			if (Main.rand.Next(60) == 0)
			{
				float dum = 8000f;
				int dum2 = -1;
				for (int i = 0; i < 200; i++)
				{
					float dum3 = Vector2.Distance(projectile.Center, Main.npc[i].Center);
					if (dum3 < dum && dum3 < 640f && Main.npc[i].CanBeChasedBy(projectile, false))
					{
						dum2 = i;
						dum = dum3;
					}
				}
				if (dum2 != -1)
				{
					bool flags = Collision.CanHit(projectile.position, projectile.width, projectile.height, Main.npc[dum2].position, Main.npc[dum2].width, Main.npc[dum2].height);
					if (flags)
					{
						Vector2 values = Main.npc[dum2].Center - projectile.Center;
						float dum4 = 5f;
						float dum5 = (float)Math.Sqrt((double)(values.X * values.X + values.Y * values.Y));
						if (dum5 > dum4)
							dum5 = dum4 / dum5;

						values *= dum5;
						int p = Terraria.Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, values.X, values.Y, mod.ProjectileType("WitherShard1"), 15, projectile.knockBack / 2f, projectile.owner, 0f, 0f);
						Main.projectile[p].friendly = true;
						Main.projectile[p].hostile = false;
					}
				}
			}
		}

		public override void SelectFrame()
		{
			projectile.frameCounter++;
			if (projectile.frameCounter >= 10)
			{
				projectile.frameCounter = 0;
				projectile.frame = (projectile.frame + 1) % Main.projFrames[projectile.type];
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture2D = Main.projectileTexture[projectile.type];
			Vector2 origin = new Vector2((float)texture2D.Width * 0.5f, (float)(texture2D.Height / Main.projFrames[projectile.type]) * 0.5f);
			SpriteEffects effects = (projectile.direction == -1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			Vector2 value = new Vector2(projectile.Center.X - 4f, projectile.Center.Y - 8f);
			Main.spriteBatch.Draw(texture2D, value - Main.screenPosition, new Rectangle?(Utils.Frame(texture2D, 1, Main.projFrames[projectile.type], 0, projectile.frame)), lightColor, projectile.rotation, origin, projectile.scale, effects, 0f);
			return false;
		}

	}
}