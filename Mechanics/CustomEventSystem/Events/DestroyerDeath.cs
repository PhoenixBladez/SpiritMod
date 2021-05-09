using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

using SpiritMod.Utilities;
using SpiritMod.Mechanics.EventSystem.Controllers;

using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SpiritMod.Mechanics.EventSystem.Cutscenes
{
	public class DestroyerDeath : Event
	{
		private NPC _head;
		private List<NPC> _destroyerInOrder;

		public DestroyerDeath(NPC headNPC)
		{
			// get the destroyer in order
			_destroyerInOrder = new List<NPC>();
			_head = headNPC;
			NPC current = _head;
			while (current.ai[0] != 0)
			{
				_destroyerInOrder.Add(current);
				current = Main.npc[(int)current.ai[0]];
			}
			_destroyerInOrder.Add(current);

			// try update all of the ai[2] values
			foreach (NPC npc in _destroyerInOrder)
			{
				npc.ai[2] = 1200f;
				npc.netUpdate = true;
			}

			// create camera controller object
			var cameraMoves = new CameraController(0f, new CameraController.EntityRelativePoint(Main.LocalPlayer, Vector2.Zero))
				.AddPoint(2f, new CameraController.EntityRelativePoint(_head, Vector2.Zero), EaseFunction.EaseCubicInOut)
				.RepeatPoint(3f);

			// add it to the controllers
			AddToQueue(cameraMoves);

			float time = 3f;
			float timeBetween = 0.4f;
			float timeBetweenSubtractor = 0.06f;
			for (int i = 0; i < _destroyerInOrder.Count; i++)
			{
				// get function
				EaseFunction function = EaseFunction.Linear;
				if (i == _destroyerInOrder.Count - 1) function = EaseFunction.EaseQuadOut;

				// add move
				cameraMoves.AddPoint(time, new CameraController.EntityRelativePoint(_destroyerInOrder[i], Vector2.Zero), function);
				NPC npc = _destroyerInOrder[i];

				// play an explosion
				AddToQueue(new ExpressionController(time, () =>
				{
					PlayExplosionAt(npc.Center);
				}));

				// then change the ai for the bodies at the right time to make them disappear
				AddToQueue(new ExpressionController(time + timeBetween * 0.04f, () =>
				{
					npc.ai[2] = 2000f;
				}));

				// change the timing
				time += timeBetween;
				if (timeBetween > 0.05f)
				{
					timeBetween -= timeBetweenSubtractor;
					if (timeBetweenSubtractor > 0.1f)
						timeBetweenSubtractor *= 0.5f;
				}
				else
				{
					timeBetween = 0.05f;
				}
			}

			// kill the head at the end
			AddToQueue(new ExpressionController(time + 2.98f, () =>
			{
				_head.StrikeNPC(1000, 0f, 1);
			}));

			// add the final camera moves
			cameraMoves.RepeatPoint(time + 1.5f)
				.AddPoint(time + 3f, new CameraController.EntityRelativePoint(Main.LocalPlayer, Vector2.Zero), EaseFunction.EaseQuadInOut);
		}

		private void PlayExplosionAt(Vector2 pos)
		{
			Main.PlaySound(SoundID.Item14, pos);

			int width = 100;
			int height = 100;
			Vector2 tl = new Vector2(pos.X - width * 0.5f, pos.Y - height * 0.5f);

			for (int d12 = 0; d12 < 50; d12++)
			{
				int num895 = Dust.NewDust(tl, width, height, 31, 0f, 0f, 100, Color.White, 2f);
				Main.dust[num895].velocity *= 1.4f;
			}
			for (int e12 = 0; e12 < 80; e12++)
			{
				int num898 = Dust.NewDust(tl, width, height, 6, 0f, 0f, 100, Color.White, 3f);
				Main.dust[num898].noGravity = true;
				Main.dust[num898].velocity *= 5f;
				num898 = Dust.NewDust(tl, width, height, 6, 0f, 0f, 100, Color.White, 2f);
				Main.dust[num898].velocity *= 3f;
			}
			Vector2 vector = pos;
			for (int f12 = 0; f12 < 2; f12++)
			{
				int num901 = Gore.NewGore(vector, Vector2.Zero, Main.rand.Next(61, 64), 1f);
				Main.gore[num901].scale = 1.5f;
				Main.gore[num901].velocity.X += 1.5f;
				Main.gore[num901].velocity.Y += 1.5f;
				num901 = Gore.NewGore(vector, Vector2.Zero, Main.rand.Next(61, 64), 1f);
				Main.gore[num901].scale = 1.5f;
				Main.gore[num901].velocity.X -= 1.5f;
				Main.gore[num901].velocity.Y += 1.5f;
				num901 = Gore.NewGore(vector, Vector2.Zero, Main.rand.Next(61, 64), 1f);
				Main.gore[num901].scale = 1.5f;
				Main.gore[num901].velocity.X += 1.5f;
				Main.gore[num901].velocity.Y -= 1.5f;
				num901 = Gore.NewGore(vector, Vector2.Zero, Main.rand.Next(61, 64), 1f);
				Main.gore[num901].scale = 1.5f;
				Main.gore[num901].velocity.X -= 1.5f;
				Main.gore[num901].velocity.Y -= 1.5f;
			}
		}
	}

	public class DestroyerDeathGlobalNPC : GlobalNPC
	{
		public override bool PreAI(NPC npc)
		{
			if (npc.type >= NPCID.TheDestroyer && npc.type <= NPCID.TheDestroyerTail)
			{
				if (npc.ai[2] > 1000f)
				{
					npc.dontTakeDamage = true;
					npc.damage = 0;
					CustomDestroyerAI(npc);
					return false;
				}
			}
			return true;
		}

		public override bool StrikeNPC(NPC npc, ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
		{
			if (npc.type >= NPCID.TheDestroyer && npc.type <= NPCID.TheDestroyerTail)
			{
				int life = npc.life;
				if (npc.type != NPCID.TheDestroyer)
				{
					life = Main.npc[npc.realLife].life;
				}
				if (npc.ai[2] < 1000f && life - damage <= 0)
				{
					damage = 0f;
					npc.ai[2] = 1200f;
					int headID = -1;
					for (int i = 0; i < Main.npc.Length; i++)
					{
						if (Main.npc[i].type == NPCID.Probe)
						{
							Main.npc[i].StrikeNPCNoInteraction(10000, 0f, 1);
						}
						if (Main.npc[i].type >= NPCID.TheDestroyer && Main.npc[i].type <= NPCID.TheDestroyerTail)
						{
							Main.npc[i].ai[2] = 1200f;
							Main.npc[i].dontTakeDamage = true;
							if (Main.npc[i].type == NPCID.TheDestroyer)
							{
								headID = i;
							}
						}
					}

					EventManager.PlayEvent(new DestroyerDeath(Main.npc[headID]));
					return false;
				}
			}
			return base.StrikeNPC(npc, ref damage, defense, ref knockback, hitDirection, ref crit);
		}

		private void CustomDestroyerAI(NPC npc)
		{
			if (npc.ai[2] > 1500f)
			{
				npc.velocity = Vector2.Zero;
				npc.Opacity = 0f;
				return;
			}

			Vector2 vector2 = new Vector2();
			if (npc.ai[3] > 0f)
			{
				npc.realLife = (int)npc.ai[3];
			}
			if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead)
			{
				npc.TargetClosest(true);
			}
			if (npc.type >= 134 && npc.type <= 136)
			{
				npc.velocity.Length();
				if (npc.type == 134 || npc.type != 134 && Main.npc[(int)npc.ai[1]].alpha < 128)
				{
					if (npc.alpha != 0)
					{
						for (int i = 0; i < 2; i++)
						{
							Vector2 vector21 = new Vector2(npc.position.X, npc.position.Y);
							int num = npc.width;
							int num1 = npc.height;
							Color color = new Color();
							int num2 = Dust.NewDust(vector21, num, num1, 182, 0f, 0f, 100, color, 2f);
							Main.dust[num2].noGravity = true;
							Main.dust[num2].noLight = true;
						}
					}
					npc.alpha -= 42;
					if (npc.alpha < 0)
					{
						npc.alpha = 0;
					}
				}
			}
			if (npc.type > 134)
			{
				bool flag = false;
				if (npc.ai[1] <= 0f)
				{
					flag = true;
				}
				else if (Main.npc[(int)npc.ai[1]].life <= 0)
				{
					flag = true;
				}
				if (flag)
				{
					npc.life = 0;
					npc.HitEffect(0, 10);
					npc.checkDead();
				}
			}
			if (Main.netMode != 1)
			{
				if (npc.ai[0] == 0f && npc.type == 134)
				{
					npc.ai[3] = (float)npc.whoAmI;
					npc.realLife = npc.whoAmI;
					int num3 = npc.whoAmI;
					int num4 = 80;
					for (int j = 0; j <= num4; j++)
					{
						int num5 = 135;
						if (j == num4)
						{
							num5 = 136;
						}
						int num6 = NPC.NewNPC((int)(npc.position.X + (float)(npc.width / 2)), (int)(npc.position.Y + (float)npc.height), num5, npc.whoAmI, 0f, 0f, 0f, 0f, 255);
						Main.npc[num6].ai[3] = (float)npc.whoAmI;
						Main.npc[num6].realLife = npc.whoAmI;
						Main.npc[num6].ai[1] = (float)num3;
						Main.npc[num3].ai[0] = (float)num6;
						NetMessage.SendData(23, -1, -1, null, num6, 0f, 0f, 0f, 0, 0, 0);
						num3 = num6;
					}
				}
				if (npc.type == 135)
				{
					npc.localAI[0] += (float)Main.rand.Next(4);
					if (npc.localAI[0] >= (float)Main.rand.Next(1400, 26000))
					{
						npc.localAI[0] = 0f;
						npc.TargetClosest(true);
						if (Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height))
						{
							/*
							Vector2 vector22 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)(npc.height / 2));
							float x = Main.player[npc.target].position.X + (float)Main.player[npc.target].width * 0.5f - vector22.X + (float)Main.rand.Next(-20, 21);
							float y = Main.player[npc.target].position.Y + (float)Main.player[npc.target].height * 0.5f - vector22.Y + (float)Main.rand.Next(-20, 21);
							float single = (float)Math.Sqrt((double)(x * x + y * y));
							single = 8f / single;
							x *= single;
							y *= single;
							x = x + (float)Main.rand.Next(-20, 21) * 0.05f;
							y = y + (float)Main.rand.Next(-20, 21) * 0.05f;
							int num7 = 22;
							if (Main.expertMode)
							{
								num7 = 18;
							}
							int num8 = 100;
							ref float singlePointer = ref vector22.X;
							singlePointer = singlePointer + x * 5f;
							ref float y1 = ref vector22.Y;
							y1 = y1 + y * 5f;
							int num9 = Projectile.NewProjectile(vector22.X, vector22.Y, x, y, num8, num7, 0f, Main.myPlayer, 0f, 0f);
							Main.projectile[num9].timeLeft = 300;
							*/
							npc.netUpdate = true;
						}
					}
				}
			}
			int x1 = (int)(npc.position.X / 16f) - 1;
			int x2 = (int)((npc.position.X + (float)npc.width) / 16f) + 2;
			int y2 = (int)(npc.position.Y / 16f) - 1;
			int y3 = (int)((npc.position.Y + (float)npc.height) / 16f) + 2;
			if (x1 < 0)
			{
				x1 = 0;
			}
			if (x2 > Main.maxTilesX)
			{
				x2 = Main.maxTilesX;
			}
			if (y2 < 0)
			{
				y2 = 0;
			}
			if (y3 > Main.maxTilesY)
			{
				y3 = Main.maxTilesY;
			}
			bool flag1 = false;
			if (!flag1)
			{
				for (int k = x1; k < x2; k++)
				{
					for (int l = y2; l < y3; l++)
					{
						if (Main.tile[k, l] != null && (Main.tile[k, l].nactive() && (Main.tileSolid[Main.tile[k, l].type] || Main.tileSolidTop[Main.tile[k, l].type] && Main.tile[k, l].frameY == 0) || Main.tile[k, l].liquid > 64))
						{
							vector2.X = (float)(k * 16);
							vector2.Y = (float)(l * 16);
							if (npc.position.X + (float)npc.width > vector2.X && npc.position.X < vector2.X + 16f && npc.position.Y + (float)npc.height > vector2.Y && npc.position.Y < vector2.Y + 16f)
							{
								flag1 = true;
								break;
							}
						}
					}
				}
			}
			if (flag1)
			{
				npc.localAI[1] = 0f;
			}
			else
			{
				if (npc.type != 135 || npc.ai[2] < 1f)
				{
					Lighting.AddLight((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.position.Y + (float)(npc.height / 2)) / 16f), 0.3f, 0.1f, 0.05f);
				}
				npc.localAI[1] = 1f;
				if (npc.type == 134)
				{
					Rectangle rectangle = new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height);
					int num10 = 1000;
					bool flag2 = true;
					if (npc.position.Y > Main.player[npc.target].position.Y)
					{
						for (int m = 0; m < 255; m++)
						{
							if (Main.player[m].active)
							{
								Rectangle rectangle1 = new Rectangle((int)Main.player[m].position.X - num10, (int)Main.player[m].position.Y - num10, num10 * 2, num10 * 2);
								if (rectangle.Intersects(rectangle1))
								{
									flag2 = false;
									break;
								}
							}
						}
						if (flag2)
						{
							flag1 = true;
						}
					}
				}
			}
			float single1 = 16f;
			if (Main.dayTime || Main.player[npc.target].dead)
			{
				flag1 = false;
				npc.velocity.Y += 1f;
				if ((double)npc.position.Y > Main.worldSurface * 16)
				{
					npc.velocity.Y += 1f;
					single1 = 32f;
				}
				if ((double)npc.position.Y > Main.rockLayer * 16)
				{
					for (int n = 0; n < 200; n++)
					{
						if (Main.npc[n].aiStyle == npc.aiStyle)
						{
							Main.npc[n].active = false;
						}
					}
				}
			}
			float single2 = 0.1f;
			float single3 = 0.15f;
			Vector2 vector23 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
			float x3 = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2);
			float y4 = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2);
			x3 = (float)((int)(x3 / 16f) * 16);
			y4 = (float)((int)(y4 / 16f) * 16);
			vector23.X = (float)((int)(vector23.X / 16f) * 16);
			vector23.Y = (float)((int)(vector23.Y / 16f) * 16);
			x3 -= vector23.X;
			y4 -= vector23.Y;
			float single4 = (float)Math.Sqrt((double)(x3 * x3 + y4 * y4));
			if (npc.ai[1] > 0f && npc.ai[1] < (float)((int)Main.npc.Length))
			{
				try
				{
					vector23 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
					x3 = Main.npc[(int)npc.ai[1]].position.X + (float)(Main.npc[(int)npc.ai[1]].width / 2) - vector23.X;
					y4 = Main.npc[(int)npc.ai[1]].position.Y + (float)(Main.npc[(int)npc.ai[1]].height / 2) - vector23.Y;
				}
				catch
				{
				}
				npc.rotation = (float)Math.Atan2((double)y4, (double)x3) + 1.57f;
				single4 = (float)Math.Sqrt((double)(x3 * x3 + y4 * y4));
				int num11 = (int)(44f * npc.scale);
				single4 = (single4 - (float)num11) / single4;
				x3 *= single4;
				y4 *= single4;
				npc.velocity = Vector2.Zero;
				npc.position.X += x3;
				npc.position.Y += y4;
				return;
			}
			if (flag1)
			{
				if (npc.soundDelay == 0)
				{
					float single5 = single4 / 40f;
					if (single5 < 10f)
					{
						single5 = 10f;
					}
					if (single5 > 20f)
					{
						single5 = 20f;
					}
					npc.soundDelay = (int)single5;
					//Main.PlaySound(15, (int)npc.position.X, (int)npc.position.Y, 1, 1f, 0f);
				}
				single4 = (float)Math.Sqrt((double)(x3 * x3 + y4 * y4));
				float single6 = Math.Abs(x3);
				float single7 = Math.Abs(y4);
				float single8 = single1 / single4;
				x3 *= single8;
				y4 *= single8;
				if ((npc.velocity.X > 0f && x3 > 0f || npc.velocity.X < 0f && x3 < 0f) && (npc.velocity.Y > 0f && y4 > 0f || npc.velocity.Y < 0f && y4 < 0f))
				{
					if (npc.velocity.X < x3)
					{
						npc.velocity.X += single3;
					}
					else if (npc.velocity.X > x3)
					{
						npc.velocity.X -= single3;
					}
					if (npc.velocity.Y < y4)
					{
						npc.velocity.Y += single3;
					}
					else if (npc.velocity.Y > y4)
					{
						npc.velocity.Y -= single3;
					}
				}
				if (npc.velocity.X > 0f && x3 > 0f || npc.velocity.X < 0f && x3 < 0f || npc.velocity.Y > 0f && y4 > 0f || npc.velocity.Y < 0f && y4 < 0f)
				{
					if (npc.velocity.X < x3)
					{
						npc.velocity.X += single2;
					}
					else if (npc.velocity.X > x3)
					{
						npc.velocity.X -= single2;
					}
					if (npc.velocity.Y < y4)
					{
						npc.velocity.Y += single2;
					}
					else if (npc.velocity.Y > y4)
					{
						npc.velocity.Y -= single2;
					}
					if ((double)Math.Abs(y4) < (double)single1 * 0.2 && (npc.velocity.X > 0f && x3 < 0f || npc.velocity.X < 0f && x3 > 0f))
					{
						if (npc.velocity.Y <= 0f)
						{
							npc.velocity.Y = npc.velocity.Y - single2 * 2f;
						}
						else
						{
							npc.velocity.Y = npc.velocity.Y + single2 * 2f;
						}
					}
					if ((double)Math.Abs(x3) < (double)single1 * 0.2 && (npc.velocity.Y > 0f && y4 < 0f || npc.velocity.Y < 0f && y4 > 0f))
					{
						if (npc.velocity.X <= 0f)
						{
							npc.velocity.X = npc.velocity.X - single2 * 2f;
						}
						else
						{
							npc.velocity.X = npc.velocity.X + single2 * 2f;
						}
					}
				}
				else if (single6 <= single7)
				{
					if (npc.velocity.Y < y4)
					{
						npc.velocity.Y = npc.velocity.Y + single2 * 1.1f;
					}
					else if (npc.velocity.Y > y4)
					{
						npc.velocity.Y = npc.velocity.Y - single2 * 1.1f;
					}
					if ((double)(Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y)) < (double)single1 * 0.5)
					{
						if (npc.velocity.X <= 0f)
						{
							npc.velocity.X -= single2;
						}
						else
						{
							npc.velocity.X += single2;
						}
					}
				}
				else
				{
					if (npc.velocity.X < x3)
					{
						npc.velocity.X = npc.velocity.X + single2 * 1.1f;
					}
					else if (npc.velocity.X > x3)
					{
						npc.velocity.X = npc.velocity.X - single2 * 1.1f;
					}
					if ((double)(Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y)) < (double)single1 * 0.5)
					{
						if (npc.velocity.Y <= 0f)
						{
							npc.velocity.Y -= single2;
						}
						else
						{
							npc.velocity.Y += single2;
						}
					}
				}
			}
			else
			{
				npc.TargetClosest(true);
				npc.velocity.Y += 0.15f;
				if (npc.velocity.Y > single1)
				{
					npc.velocity.Y = single1;
				}
				if ((double)(Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y)) < (double)single1 * 0.4)
				{
					if (npc.velocity.X >= 0f)
					{
						npc.velocity.X = npc.velocity.X + single2 * 1.1f;
					}
					else
					{
						npc.velocity.X = npc.velocity.X - single2 * 1.1f;
					}
				}
				else if (npc.velocity.Y != single1)
				{
					if (npc.velocity.Y > 4f)
					{
						if (npc.velocity.X >= 0f)
						{
							npc.velocity.X = npc.velocity.X - single2 * 0.9f;
						}
						else
						{
							npc.velocity.X = npc.velocity.X + single2 * 0.9f;
						}
					}
				}
				else if (npc.velocity.X < x3)
				{
					npc.velocity.X += single2;
				}
				else if (npc.velocity.X > x3)
				{
					npc.velocity.X -= single2;
				}
			}
			npc.rotation = (float)Math.Atan2((double)npc.velocity.Y, (double)npc.velocity.X) + 1.57f;
			if (npc.type == 134)
			{
				if (!flag1)
				{
					if (npc.localAI[0] != 0f)
					{
						npc.netUpdate = true;
					}
					npc.localAI[0] = 0f;
				}
				else
				{
					if (npc.localAI[0] != 1f)
					{
						npc.netUpdate = true;
					}
					npc.localAI[0] = 1f;
				}
				if ((npc.velocity.X > 0f && npc.oldVelocity.X < 0f || npc.velocity.X < 0f && npc.oldVelocity.X > 0f || npc.velocity.Y > 0f && npc.oldVelocity.Y < 0f || npc.velocity.Y < 0f && npc.oldVelocity.Y > 0f) && !npc.justHit)
				{
					npc.netUpdate = true;
				}
			}
		}
	}
}
