using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.Dusking
{
	public class ShadowBall : ModNPC
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shadow Ball");
		}

		public override void SetDefaults()
		{
			npc.width = npc.height = 40;
			npc.alpha = 255;

			npc.lifeMax = 1;
			npc.damage = 50;
			Main.npcFrameCount[npc.type] = 4;
			npc.friendly = false;
			npc.noGravity = true;
			npc.noTileCollide = true;
		}

		public override bool PreAI()
		{
			if (npc.ai[1] == 0f)
			{
				npc.ai[1] = 1f;
				Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 34);
			}
			else if (npc.ai[1] == 1f && Main.netMode != 1)
			{
				int target = -1;
				float distance = 2000f;
				for (int k = 0; k < 255; k++)
				{
					if (Main.player[k].active && !Main.player[k].dead)
					{
						Vector2 center = Main.player[k].Center;
						float currentDistance = Vector2.Distance(center, npc.Center);
						if ((currentDistance < distance || target == -1) && Collision.CanHit(npc.Center, 1, 1, center, 1, 1))
						{
							distance = currentDistance;
							target = k;
						}
					}
				}
				if (target != -1)
				{
					npc.ai[1] = 21f;
					npc.ai[0] = target;
					npc.netUpdate = true;
				}
			}
			else if (npc.ai[1] > 20f && npc.ai[1] < 200f)
			{
				npc.ai[1] += 1f;
				int target = (int)npc.ai[0];
				if (!Main.player[target].active || Main.player[target].dead)
				{
					npc.ai[1] = 1f;
					npc.ai[0] = 0f;
					npc.netUpdate = true;
				}
				else
				{
					float num6 = npc.velocity.ToRotation();
					Vector2 vector2 = Main.player[target].Center - npc.Center;

					float targetAngle = vector2.ToRotation();
					if (vector2 == Vector2.Zero)
					{
						targetAngle = num6;
					}
					float num7 = num6.AngleLerp(targetAngle, 0.008f);
					npc.velocity = new Vector2(npc.velocity.Length(), 0f).RotatedBy((double)num7, default(Vector2));
				}
			}

			if (npc.ai[1] >= 1f && npc.ai[1] < 20f)
			{
				npc.ai[1] += 1f;
				if (npc.ai[1] == 20f)
					npc.ai[1] = 1f;
			}

			if (npc.alpha <= 0)
				npc.alpha = 0;
			else
				npc.alpha -= 40;

			npc.spriteDirection = npc.direction;

			npc.localAI[0] += 1f;
			if (npc.localAI[0] == 12f)
			{
				npc.localAI[0] = 0f;
				for (int j = 0; j < 12; j++)
				{
					Vector2 vector2 = Vector2.UnitX * -npc.width / 2f;
					vector2 += -Utils.RotatedBy(Vector2.UnitY, ((float)j * 3.14159274f / 6f), default(Vector2)) * new Vector2(8f, 16f);
					vector2 = Utils.RotatedBy(vector2, (npc.rotation - 1.57079637f), default(Vector2));
					int num8 = Dust.NewDust(npc.Center, 0, 0, 27, 0f, 0f, 160, default(Color), 1f);
					Main.dust[num8].scale = 1.1f;
					Main.dust[num8].noGravity = true;
					Main.dust[num8].position = npc.Center + vector2;
					Main.dust[num8].velocity = npc.velocity * 0.1f;
					Main.dust[num8].velocity = Vector2.Normalize(npc.Center - npc.velocity * 3f - Main.dust[num8].position) * 1.25f;
				}
			}
			if (Main.rand.Next(4) == 0)
			{
				for (int k = 0; k < 1; k++)
				{
					Vector2 value = -Utils.RotatedBy(Utils.RotatedByRandom(Vector2.UnitX, 0.19634954631328583), Utils.ToRotation(npc.velocity), default(Vector2));
					int num9 = Dust.NewDust(npc.position, npc.width, npc.height, 27, 0f, 0f, 100, default(Color), 1f);
					Main.dust[num9].velocity *= 0.1f;
					Main.dust[num9].position = npc.Center + value * npc.width / 2f;
					Main.dust[num9].fadeIn = 0.9f;
				}
			}
			if (Main.rand.Next(32) == 0)
			{
				for (int l = 0; l < 1; l++)
				{
					Vector2 value2 = -Utils.RotatedBy(Utils.RotatedByRandom(Vector2.UnitX, 0.39269909262657166), Utils.ToRotation(npc.velocity), default(Vector2));
					int num10 = Dust.NewDust(npc.position, npc.width, npc.height, 27, 0f, 0f, 155, default(Color), 0.8f);
					Main.dust[num10].velocity *= 0.3f;
					Main.dust[num10].position = npc.Center + value2 * npc.width / 2f;
					if (Main.rand.Next(2) == 0)
						Main.dust[num10].fadeIn = 1.4f;
				}
			}
			if (Main.rand.Next(2) == 0)
			{
				for (int m = 0; m < 2; m++)
				{
					Vector2 value3 = -Utils.RotatedBy(Utils.RotatedByRandom(Vector2.UnitX, 0.78539818525314331), Utils.ToRotation(npc.velocity), default(Vector2));
					int num11 = Dust.NewDust(npc.position, npc.width, npc.height, 27, 0f, 0f, 0, default(Color), 1.2f);
					Main.dust[num11].velocity *= 0.3f;
					Main.dust[num11].noGravity = true;
					Main.dust[num11].position = npc.Center + value3 * npc.width / 2f;
					if (Main.rand.Next(2) == 0)
						Main.dust[num11].fadeIn = 1.4f;
				}
			}

			Lighting.AddLight(npc.Center, 1.1f, 0.3f, 1.1f);
			return false;
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter++;
			if (npc.frameCounter >= 3)
			{
				npc.frame.Y = (npc.frame.Y + frameHeight) % (Main.projFrames[npc.type] * frameHeight);
				npc.frameCounter = 0;
			}
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			bool expertMode = Main.expertMode;
			int dam = expertMode ? 19 : 35;
			int p = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, 0, mod.ProjectileType("HostileWrath"), dam, 1, Main.myPlayer, 0, 0);
			Main.projectile[p].timeLeft = 30;

			Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 14);
			if (npc.life <= 0)
			{
				npc.position.X = npc.position.X + (float)(npc.width / 2);
				npc.position.Y = npc.position.Y + (float)(npc.height / 2);
				npc.width = 30;
				npc.height = 30;
				npc.position.X = npc.position.X - (float)(npc.width / 2);
				npc.position.Y = npc.position.Y - (float)(npc.height / 2);
				for (int num621 = 0; num621 < 20; num621++)
				{
					int num622 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 173, 0f, 0f, 100, default(Color), 2f);
					Main.dust[num622].velocity *= 3f;
					if (Main.rand.Next(2) == 0)
					{
						Main.dust[num622].scale = 0.5f;
						Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
					}
				}
				for (int num623 = 0; num623 < 40; num623++)
				{
					int num624 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 173, 0f, 0f, 100, default(Color), 3f);
					Main.dust[num624].noGravity = true;
					Main.dust[num624].velocity *= 5f;
					num624 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 173, 0f, 0f, 100, default(Color), 2f);
					Main.dust[num624].velocity *= 2f;
				}
			}
		}

		public override bool CheckDead()
		{
			for (int num383 = 0; num383 < 5; num383++)
			{
				int num384 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.Shadowflame, 0f, 0f, 0, default(Color), 1f);
				Main.dust[num384].noGravity = true;
				Main.dust[num384].velocity *= 1.5f;
				Main.dust[num384].scale *= 0.9f;
			}
			return true;
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			target.AddBuff(mod.BuffType("Shadowflame"), 150);
		}
	}
}
