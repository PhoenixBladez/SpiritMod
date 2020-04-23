
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public delegate void ExtraAction();

	public static class ProjectileExtras
	{
		public static void HomingAIVanilla(ModProjectile modProj, NPC target, float velocity = 4f, float weight = 0.0333f)
		{
			Projectile projectile = modProj.projectile;
			Vector2 pos = new Vector2(projectile.position.X + (float)(projectile.width >> 1), projectile.position.Y + (float)(projectile.height >> 1));
			Vector2 aim = new Vector2(target.position.X + (float)(target.width >> 1), target.position.Y + (float)(target.height >> 1));
			float num560 = aim.X - pos.X;
			float num561 = aim.Y - pos.Y;
			aim -= pos;
			aim *= velocity / aim.Length();
			projectile.velocity *= 1f - weight;
			projectile.velocity += aim * weight;
		}

		public static void HomingAI(ModProjectile modProj, NPC target, float velocity = 4f, float acceleration = 0.1f)
		{
			Projectile projectile = modProj.projectile;
			Vector2 aim = new Vector2(target.position.X + (float)(target.width >> 1), target.position.Y + (float)(target.height >> 1));
			Vector2 pos = new Vector2(projectile.position.X + (float)(projectile.width >> 1), projectile.position.Y + (float)(projectile.height >> 1));
			aim -= pos;
			aim *= velocity / aim.Length();
			Vector2 diff = aim - projectile.velocity;
			if (acceleration * acceleration >= diff.LengthSquared())
				projectile.velocity = aim;
			else
			{
				diff *= acceleration / diff.Length();
				projectile.velocity += diff;
			}
		}

		public static void HomingAIPredictive(ModProjectile modProj, NPC target, float velocity = 4f, float acceleration = 0.1f)
		{
			Projectile projectile = modProj.projectile;
			Vector2 aim = new Vector2(target.position.X + (float)(target.width >> 1), target.position.Y + (float)(target.height >> 1));
			Vector2 pos = new Vector2(projectile.position.X + (float)(projectile.width >> 1), projectile.position.Y + (float)(projectile.height >> 1));
			aim -= pos;
			aim += target.velocity * (aim.Length()/velocity);
			aim *= velocity / aim.Length();
			Vector2 diff = aim - projectile.velocity;
			if (acceleration * acceleration >= diff.LengthSquared())
				projectile.velocity = aim;
			else
			{
				diff *= acceleration / diff.Length();
				projectile.velocity += diff;
			}
		}

		public static NPC FindCheapestNPC(Vector2 position, Vector2 velocity, float acceleration, float maxAngle, float maxDist = 2000f, bool ignoreLineOfSight = false, bool ignoreFriendlies = true, bool ignoreDontTakeDamage = false, bool ignoreChaseable = false)
		{
			float invVel = 1 / velocity.LengthSquared();
			float invAcc = 1 / (acceleration * acceleration);
			maxDist *= maxDist;

			NPC best = null;
			Vector2 maxDeviation = velocity.RotatedBy(maxAngle);
			maxDeviation = maxDeviation * ((velocity.X*maxDeviation.X + velocity.Y*maxDeviation.Y) * invVel);
			if (maxAngle > Math.PI/2)
				maxDeviation = -maxDeviation;

			float costBest = Vector2.DistanceSquared(velocity, maxDeviation);
			for (int i = 0; i < 200; i++)
			{
				NPC npc = Main.npc[i];
				if (npc.active && (ignoreChaseable || npc.chaseable && npc.lifeMax > 5)
					&& (ignoreDontTakeDamage || !npc.dontTakeDamage) && (!ignoreFriendlies || !npc.friendly) && !npc.immortal)
				{
					Vector2 target = npc.Center;
					Vector2 aim = (target - position);
					float lenAim = aim.LengthSquared();
					if (lenAim > maxDist)
						continue;

					float scalar = (velocity.X*aim.X + velocity.Y*aim.Y);
					Vector2 projVel = aim * (scalar / lenAim);
					if (scalar < 0)
						projVel = -projVel;
					float cost = Vector2.DistanceSquared(velocity, projVel);
					if (cost*invAcc > lenAim*invVel)
						cost += acceleration;

					if (cost < costBest && (ignoreLineOfSight || Collision.CanHitLine(position, 0, 0, target, 0, 0)))
					{
						best = npc;
						costBest = cost;
					}
				}
			}
			return best;
		}

		public static NPC FindNearestNPC(Vector2 position, float maxDist, bool ignoreLineOfSight = true, bool ignoreFriendlies = true, bool ignoreDontTakeDamage = false, bool ignoreChaseable = false)
		{
			NPC nearest = null;
			float distNearest = maxDist * maxDist;
			for (int i = 0; i < 200; i++)
			{
				NPC npc = Main.npc[i];
				Vector2 npcCenter = npc.Center;
				if (npc.active && (ignoreChaseable || npc.chaseable && npc.lifeMax > 5)
					&& (ignoreDontTakeDamage || !npc.dontTakeDamage) && (!ignoreFriendlies || !npc.friendly) && !npc.immortal)
				{
					float distCurrent = Vector2.DistanceSquared(position, npcCenter);
					if (distCurrent < distNearest && (ignoreLineOfSight || Collision.CanHitLine(position, 0, 0, npcCenter, 0, 0)))
					{
						nearest = npc;
						distNearest = distCurrent;
					}
				}
			}
			return nearest;
		}

		public static NPC FindRandomNPC(Vector2 position, float maxDist, bool ignoreLineOfSight = true, bool ignoreFriendlies = true, bool ignoreDontTakeDamage = false, bool ignoreChaseable = false)
		{
			NPC[] targets = new NPC[Main.maxNPCs];
			maxDist *= maxDist;
			int next = 0;
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				NPC npc = Main.npc[i];
				Vector2 npcCenter = npc.Center;
				if (npc.active && (ignoreChaseable || npc.chaseable && npc.lifeMax > 5)
					&& (ignoreDontTakeDamage || !npc.dontTakeDamage) && (!ignoreFriendlies || !npc.friendly) && !npc.immortal)
				{
					float distCurrent = Vector2.DistanceSquared(position, npcCenter);
					if (distCurrent < maxDist && (ignoreLineOfSight || Collision.CanHitLine(position, 0, 0, npcCenter, 0, 0)))
					{
						targets[next] = npc;
						next++;
					}
				}
			}
			if (next == 0)
			{
				return null;
			}
			return targets[Main.rand.Next(next)];
		}

		public static void LookAlongVelocity(ModProjectile modProj)
		{
			Projectile projectile = modProj.projectile;
			projectile.rotation = (float)Math.Atan2(projectile.velocity.X, -projectile.velocity.Y);
		}

		public static void LookAt(ModProjectile modProj, Vector2 target)
		{
			Projectile projectile = modProj.projectile;
			Vector2 delta = target - projectile.position;
			projectile.rotation = (float)Math.Atan2(delta.X, -delta.Y);
		}

		public static void Bounce(ModProjectile modProj, Vector2 oldVelocity, float bouncyness = 1f)
		{
			Projectile projectile = modProj.projectile;
			if (projectile.velocity.X != oldVelocity.X)
				projectile.velocity.X = -oldVelocity.X * bouncyness;

			if (projectile.velocity.Y != oldVelocity.Y)
				projectile.velocity.Y = -oldVelocity.Y * bouncyness;
		}

		public static void YoyoAI(int index, float seconds, float length, float acceleration = 14f, float rotationSpeed = 0.45f, ExtraAction action = null, ExtraAction initialize = null)
		{
			Projectile projectile = Main.projectile[index];
			bool flag = false;
			if (initialize != null && projectile.localAI[1] == 0f)
			{
				projectile.localAI[1] = 1f;
				initialize();
			}

			for (int i = 0; i < projectile.whoAmI; i++)
			{
				if (Main.projectile[i].active && Main.projectile[i].owner == projectile.owner && Main.projectile[i].type == projectile.type)
					flag = true;
			}
			if (projectile.owner == Main.myPlayer)
			{
				projectile.localAI[0] += 1f;
				if (flag)
					projectile.localAI[0] += (float)Main.rand.Next(10, 31) * 0.1f;

				float num = projectile.localAI[0] / 60f;
				num /= (1f + Main.player[projectile.owner].meleeSpeed) / 2f;
				if (num > seconds)
					projectile.ai[0] = -1f;
			}

			if (Main.player[projectile.owner].dead)
			{
				projectile.Kill();
				return;
			}

			if (!flag)
			{
				Main.player[projectile.owner].heldProj = projectile.whoAmI;
				Main.player[projectile.owner].itemAnimation = 2;
				Main.player[projectile.owner].itemTime = 2;
				if (projectile.position.X + (float)(projectile.width / 2) > Main.player[projectile.owner].position.X + (float)(Main.player[projectile.owner].width / 2))
				{
					Main.player[projectile.owner].ChangeDir(1);
					projectile.direction = 1;
				}
				else
				{
					Main.player[projectile.owner].ChangeDir(-1);
					projectile.direction = -1;
				}
			}

			if (Utils.HasNaNs(projectile.velocity))
				projectile.Kill();

			projectile.timeLeft = 6;
			float num2 = length;
			if (Main.player[projectile.owner].yoyoString)
				num2 = num2 * 1.25f + 30f;

			num2 /= (1f + Main.player[projectile.owner].meleeSpeed * 3f) / 4f;
			float num3 = acceleration / ((1f + Main.player[projectile.owner].meleeSpeed * 3f) / 4f);
			float num4 = 14f - num3 / 2f;
			float num5 = 5f + num3 / 2f;
			if (flag)
				num5 += 20f;

			if (projectile.ai[0] >= 0f)
			{
				if (projectile.velocity.Length() > num3)
					projectile.velocity *= 0.98f;

				bool flag3 = false;
				bool flag4 = false;
				Vector2 vector = Main.player[projectile.owner].Center - projectile.Center;
				if (vector.Length() > num2)
				{
					flag3 = true;
					if (vector.Length() > num2 * 1.3f)
						flag4 = true;
				}

				if (projectile.owner == Main.myPlayer)
				{
					if (!Main.player[projectile.owner].channel || Main.player[projectile.owner].stoned || Main.player[projectile.owner].frozen)
					{
						projectile.ai[0] = -1f;
						projectile.ai[1] = 0f;
						projectile.netUpdate = true;
					}
					else
					{
						Vector2 vector2 = Main.ReverseGravitySupport(Main.MouseScreen, 0f) + Main.screenPosition;
						float x = vector2.X;
						float y = vector2.Y;
						Vector2 vector3 = new Vector2(x, y) - Main.player[projectile.owner].Center;
						if (vector3.Length() > num2)
						{
							vector3.Normalize();
							vector3 *= num2;
							vector3 = Main.player[projectile.owner].Center + vector3;
							x = vector3.X;
							y = vector3.Y;
						}

						if (projectile.ai[0] != x || projectile.ai[1] != y)
						{
							Vector2 value = new Vector2(x, y);
							Vector2 vector4 = value - Main.player[projectile.owner].Center;
							if (vector4.Length() > num2 - 1f)
							{
								vector4.Normalize();
								vector4 *= num2 - 1f;
								value = Main.player[projectile.owner].Center + vector4;
								x = value.X;
								y = value.Y;
							}
							projectile.ai[0] = x;
							projectile.ai[1] = y;
							projectile.netUpdate = true;
						}
					}
				}

				if (flag4 && projectile.owner == Main.myPlayer)
				{
					projectile.ai[0] = -1f;
					projectile.netUpdate = true;
				}

				if (projectile.ai[0] >= 0f)
				{
					if (flag3)
					{
						num4 /= 2f;
						num3 *= 2f;
						if (projectile.Center.X > Main.player[projectile.owner].Center.X && projectile.velocity.X > 0f)
						{
							projectile.velocity.X = projectile.velocity.X * 0.5f;
						}
						else if (projectile.Center.X < Main.player[projectile.owner].Center.X && projectile.velocity.X > 0f)
						{
							projectile.velocity.X = projectile.velocity.X * 0.5f;
						}
						if (projectile.Center.Y > Main.player[projectile.owner].Center.Y && projectile.velocity.Y > 0f)
						{
							projectile.velocity.Y = projectile.velocity.Y * 0.5f;
						}
						else if (projectile.Center.Y < Main.player[projectile.owner].Center.Y && projectile.velocity.Y > 0f)
						{
							projectile.velocity.Y = projectile.velocity.Y * 0.5f;
						}
					}

					Vector2 value2 = new Vector2(projectile.ai[0], projectile.ai[1]);
					Vector2 vector5 = value2 - projectile.Center;
					projectile.velocity.Length();
					if (vector5.Length() > num5)
					{
						vector5.Normalize();
						vector5 *= num3;
						projectile.velocity = (projectile.velocity * (num4 - 1f) + vector5) / num4;
					}
					else if (flag)
					{
						if ((double)projectile.velocity.Length() < (double)num3 * 0.6)
						{
							vector5 = projectile.velocity;
							vector5.Normalize();
							vector5 *= num3 * 0.6f;
							projectile.velocity = (projectile.velocity * (num4 - 1f) + vector5) / num4;
						}
					}
					else
					{
						projectile.velocity *= 0.8f;
					}

					if (flag && !flag3 && (double)projectile.velocity.Length() < (double)num3 * 0.6)
					{
						projectile.velocity.Normalize();
						projectile.velocity *= num3 * 0.6f;
					}
					if (action != null)
					{
						action();
					}
				}
			}
			else
			{
				num4 = (float)((int)((double)num4 * 0.8));
				num3 *= 1.5f;
				projectile.tileCollide = false;
				Vector2 vector6 = Main.player[projectile.owner].position - projectile.Center;
				float num6 = vector6.Length();
				if (num6 < num3 + 10f || num6 == 0f)
					projectile.Kill();
				else
				{
					vector6.Normalize();
					vector6 *= num3;
					projectile.velocity = (projectile.velocity * (num4 - 1f) + vector6) / num4;
				}
			}
			projectile.rotation += rotationSpeed;
		}

		public static void SpearAI(int index, float protractSpeed = 1.5f, float retractSpeed = 1.4f, ExtraAction action = null, ExtraAction initialize = null)
		{
			Projectile projectile = Main.projectile[index];
			if (initialize != null && projectile.localAI[1] == 0f)
			{
				projectile.localAI[1] = 1f;
				initialize();
			}

			Vector2 vector = Main.player[projectile.owner].RotatedRelativePoint(Main.player[projectile.owner].MountedCenter, true);
			projectile.direction = Main.player[projectile.owner].direction;
			Main.player[projectile.owner].heldProj = projectile.whoAmI;
			Main.player[projectile.owner].itemTime = Main.player[projectile.owner].itemAnimation;
			projectile.position.X = vector.X - (float)(projectile.width / 2);
			projectile.position.Y = vector.Y - (float)(projectile.height / 2);
			if (!Main.player[projectile.owner].frozen)
			{
				if (projectile.ai[0] == 0f)
				{
					projectile.ai[0] = 3f;
					projectile.netUpdate = true;
				}
				if (Main.player[projectile.owner].itemAnimation < Main.player[projectile.owner].itemAnimationMax / 3)
					projectile.ai[0] -= retractSpeed;
				else
					projectile.ai[0] += protractSpeed;
			}
			projectile.position += projectile.velocity * projectile.ai[0];
			if (Main.player[projectile.owner].itemAnimation == 0)
				projectile.Kill();

			projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 2.355f;
			if (projectile.spriteDirection == -1)
				projectile.rotation -= 1.57f;

			if (action != null)
				action();
		}

		public static void FlailAI(int index, float initialRange = 160f, float weaponOutRange = 300f, float retractRange = 100f, ExtraAction action = null, ExtraAction initialize = null)
		{
			Projectile projectile = Main.projectile[index];
			if (initialize != null && projectile.localAI[1] == 0f)
			{
				projectile.localAI[1] = 1f;
				initialize();
			}
			if (Main.player[projectile.owner].dead)
			{
				projectile.Kill();
				return;
			}
			Main.player[projectile.owner].itemAnimation = 10;
			Main.player[projectile.owner].itemTime = 10;

			if (projectile.position.X + (float)(projectile.width / 2) > Main.player[projectile.owner].position.X + (float)(Main.player[projectile.owner].width / 2))
			{
				Main.player[projectile.owner].ChangeDir(1);
				projectile.direction = 1;
			}
			else
			{
				Main.player[projectile.owner].ChangeDir(-1);
				projectile.direction = -1;
			}
			Vector2 mountedCenter = Main.player[projectile.owner].MountedCenter;
			Vector2 vector = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
			float num = mountedCenter.X - vector.X;
			float num2 = mountedCenter.Y - vector.Y;
			float num3 = (float)Math.Sqrt((double)(num * num + num2 * num2));
			if (projectile.ai[0] == 0f)
			{
				projectile.tileCollide = true;
				if (num3 > initialRange)
				{
					projectile.ai[0] = 1f;
					projectile.netUpdate = true;
				}
				else if (!Main.player[projectile.owner].channel)
				{
					if (projectile.velocity.Y < 0f)
					{
						projectile.velocity.Y = projectile.velocity.Y * 0.9f;
					}
					projectile.velocity.Y = projectile.velocity.Y + 1f;
					projectile.velocity.X = projectile.velocity.X * 0.9f;
				}
			}
			else if (projectile.ai[0] == 1f)
			{
				float num4 = 14f / Main.player[projectile.owner].meleeSpeed;
				float num5 = 0.9f / Main.player[projectile.owner].meleeSpeed;
				Math.Abs(num);
				Math.Abs(num2);
				if (projectile.ai[1] == 1f)
					projectile.tileCollide = false;

				if (!Main.player[projectile.owner].channel || num3 > weaponOutRange || !projectile.tileCollide)
				{
					projectile.ai[1] = 1f;
					if (projectile.tileCollide)
						projectile.netUpdate = true;

					projectile.tileCollide = false;
					if (num3 < 20f)
						projectile.Kill();
				}
				if (!projectile.tileCollide)
					num5 *= 2f;

				int num6 = (int)retractRange;
				if (num3 > (float)num6 || !projectile.tileCollide)
				{
					num3 = num4 / num3;
					num *= num3;
					num2 *= num3;
					new Vector2(projectile.velocity.X, projectile.velocity.Y);
					float num7 = num - projectile.velocity.X;
					float num8 = num2 - projectile.velocity.Y;
					float num9 = (float)Math.Sqrt((double)(num7 * num7 + num8 * num8));
					num9 = num5 / num9;
					num7 *= num9;
					num8 *= num9;
					projectile.velocity.X = projectile.velocity.X * 0.98f;
					projectile.velocity.Y = projectile.velocity.Y * 0.98f;
					projectile.velocity.X = projectile.velocity.X + num7;
					projectile.velocity.Y = projectile.velocity.Y + num8;
				}
				else
				{
					if (Math.Abs(projectile.velocity.X) + Math.Abs(projectile.velocity.Y) < 6f)
					{
						projectile.velocity.X = projectile.velocity.X * 0.96f;
						projectile.velocity.Y = projectile.velocity.Y + 0.2f;
					}
					if (Main.player[projectile.owner].velocity.X == 0f)
						projectile.velocity.X = projectile.velocity.X * 0.96f;
				}
			}

			if (projectile.velocity.X < 0f)
				projectile.rotation -= (Math.Abs(projectile.velocity.X) + Math.Abs(projectile.velocity.Y)) * 0.01f;
			else
				projectile.rotation += (Math.Abs(projectile.velocity.X) + Math.Abs(projectile.velocity.Y)) * 0.01f;

			if (action != null)
				action();
		}

		public static bool FlailTileCollide(int index, Vector2 oldVelocity)
		{
			Projectile projectile = Main.projectile[index];
			bool flag = false;
			if (oldVelocity.X != projectile.velocity.X)
			{
				if (Math.Abs(oldVelocity.X) > 4f)
					flag = true;
				projectile.velocity.X = -oldVelocity.X * 0.2f;
			}
			if (oldVelocity.Y != projectile.velocity.Y)
			{
				if (Math.Abs(oldVelocity.Y) > 4f)
					flag = true;
				projectile.velocity.Y = -oldVelocity.Y * 0.2f;
			}

			projectile.ai[0] = 1f;
			if (flag)
			{
				projectile.netUpdate = true;
				Collision.HitTiles(projectile.position, projectile.velocity, projectile.width, projectile.height);
				Main.PlaySound(0, (int)projectile.position.X, (int)projectile.position.Y, 1);
			}
			return false;
		}

		public static void BoomerangAI(int index, float retractTime = 30f, float speed = 9f, float speedAcceleration = 0.4f, ExtraAction action = null, ExtraAction initialize = null)
		{
			Terraria.Projectile projectile = Main.projectile[index];
			if (initialize != null && projectile.localAI[1] == 0f)
			{
				projectile.localAI[1] = 1f;
				initialize();
			}
			if (projectile.soundDelay == 0)
			{
				projectile.soundDelay = 8;
				Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 7);
			}

			if (projectile.ai[0] == 0f)
			{
				projectile.ai[1] += 1f;
				if (projectile.ai[1] >= retractTime)
				{
					projectile.ai[0] = 1f;
					projectile.ai[1] = 0f;
					projectile.netUpdate = true;
				}
			}
			else
			{
				projectile.tileCollide = false;
				Vector2 vector = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
				float num = Main.player[projectile.owner].position.X + (float)(Main.player[projectile.owner].width / 2) - vector.X;
				float num2 = Main.player[projectile.owner].position.Y + (float)(Main.player[projectile.owner].height / 2) - vector.Y;
				float num3 = (float)Math.Sqrt((double)(num * num + num2 * num2));
				if (num3 > 3000f)
					projectile.Kill();

				num3 = speed / num3;
				num *= num3;
				num2 *= num3;

				if (projectile.velocity.X < num)
				{
					projectile.velocity.X = projectile.velocity.X + speedAcceleration;
					if (projectile.velocity.X < 0f && num > 0f)
						projectile.velocity.X = projectile.velocity.X + speedAcceleration;
				}
				else if (projectile.velocity.X > num)
				{
					projectile.velocity.X = projectile.velocity.X - speedAcceleration;
					if (projectile.velocity.X > 0f && num < 0f)
						projectile.velocity.X = projectile.velocity.X - speedAcceleration;
				}

				if (projectile.velocity.Y < num2)
				{
					projectile.velocity.Y = projectile.velocity.Y + speedAcceleration;
					if (projectile.velocity.Y < 0f && num2 > 0f)
						projectile.velocity.Y = projectile.velocity.Y + speedAcceleration;
				}
				else if (projectile.velocity.Y > num2)
				{
					projectile.velocity.Y = projectile.velocity.Y - speedAcceleration;
					if (projectile.velocity.Y > 0f && num2 < 0f)
						projectile.velocity.Y = projectile.velocity.Y - speedAcceleration;
				}

				if (Main.myPlayer == projectile.owner)
				{
					Rectangle rectangle = new Rectangle((int)projectile.position.X, (int)projectile.position.Y, projectile.width, projectile.height);
					Rectangle value = new Rectangle((int)Main.player[projectile.owner].position.X, (int)Main.player[projectile.owner].position.Y, Main.player[projectile.owner].width, Main.player[projectile.owner].height);
					if (rectangle.Intersects(value))
						projectile.Kill();
				}
			}
			projectile.rotation += 0.4f * (float)projectile.direction;
			if (action != null)
				action();
		}

		public static bool BoomerangTileCollide(int index, Vector2 oldVelocity)
		{
			Projectile projectile = Main.projectile[index];
			Collision.HitTiles(projectile.position, projectile.velocity, projectile.width, projectile.height);
			projectile.ai[0] = 1f;
			projectile.velocity.X = -oldVelocity.X;
			projectile.velocity.Y = -oldVelocity.Y;
			projectile.netUpdate = true;
			Main.PlaySound(0, (int)projectile.position.X, (int)projectile.position.Y, 1);
			return false;
		}

		public static void BoomerangOnHitEntity(int index)
		{
			Projectile projectile = Main.projectile[index];
			if (projectile.ai[0] == 0f)
			{
				projectile.velocity.X = -projectile.velocity.X;
				projectile.velocity.Y = -projectile.velocity.Y;
				projectile.netUpdate = true;
			}
			projectile.ai[0] = 1f;
		}

		public static void ThrowingKnifeAI(int index, int airTime = 20, ExtraAction action = null, ExtraAction initialize = null)
		{
			Projectile projectile = Main.projectile[index];
			if (initialize != null && projectile.localAI[1] == 0f)
			{
				projectile.localAI[1] = 1f;
				initialize();
			}
			projectile.rotation += (Math.Abs(projectile.velocity.X) + Math.Abs(projectile.velocity.Y)) * 0.03f * (float)projectile.direction;
			projectile.localAI[0] += 1f;
			if (projectile.localAI[0] >= (float)airTime)
			{
				projectile.velocity.Y = projectile.velocity.Y + 0.4f;
				projectile.velocity.X = projectile.velocity.X * 0.98f;
			}
			else
				projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;

			if (projectile.velocity.Y > 16f)
				projectile.velocity.Y = 16f;

			if (action != null)
				action();
		}

		public static void Explode(int index, int sizeX, int sizeY, ExtraAction visualAction = null)
		{
			Projectile projectile = Main.projectile[index];
			if (!projectile.active)
				return;

			projectile.tileCollide = false;
			projectile.alpha = 255;
			projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
			projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
			projectile.width = sizeX;
			projectile.height = sizeY;
			projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
			projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
			projectile.Damage();
			Main.projectileIdentity[projectile.owner, projectile.identity] = -1;
			projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
			projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
			projectile.width = (int)((float)sizeX / 5.8f);
			projectile.height = (int)((float)sizeY / 5.8f);
			projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
			projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
			if (visualAction == null)
			{
				for (int i = 0; i < 30; i++)
				{
					int num = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 31, 0f, 0f, 100, default(Color), 1.5f);
					Main.dust[num].velocity *= 1.4f;
				}
				for (int j = 0; j < 20; j++)
				{
					int num2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 3.5f);
					Main.dust[num2].noGravity = true;
					Main.dust[num2].velocity *= 7f;
					num2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 1.5f);
					Main.dust[num2].velocity *= 3f;
				}
				for (int k = 0; k < 2; k++)
				{
					float scaleFactor = 0.4f;
					if (k == 1)
					{
						scaleFactor = 0.8f;
					}
					int num3 = Gore.NewGore(new Vector2(projectile.position.X, projectile.position.Y), default(Vector2), Main.rand.Next(61, 64), 1f);
					Main.gore[num3].velocity *= scaleFactor;
					Gore gore = Main.gore[num3];
					gore.velocity.X = gore.velocity.X + 1f;
					Gore gore2 = Main.gore[num3];
					gore2.velocity.Y = gore2.velocity.Y + 1f;
					num3 = Gore.NewGore(new Vector2(projectile.position.X, projectile.position.Y), default(Vector2), Main.rand.Next(61, 64), 1f);
					Main.gore[num3].velocity *= scaleFactor;
					Gore gore3 = Main.gore[num3];
					gore3.velocity.X = gore3.velocity.X - 1f;
					Gore gore4 = Main.gore[num3];
					gore4.velocity.Y = gore4.velocity.Y + 1f;
					num3 = Gore.NewGore(new Vector2(projectile.position.X, projectile.position.Y), default(Vector2), Main.rand.Next(61, 64), 1f);
					Main.gore[num3].velocity *= scaleFactor;
					Gore gore5 = Main.gore[num3];
					gore5.velocity.X = gore5.velocity.X + 1f;
					Gore gore6 = Main.gore[num3];
					gore6.velocity.Y = gore6.velocity.Y - 1f;
					num3 = Gore.NewGore(new Vector2(projectile.position.X, projectile.position.Y), default(Vector2), Main.rand.Next(61, 64), 1f);
					Main.gore[num3].velocity *= scaleFactor;
					Gore gore7 = Main.gore[num3];
					gore7.velocity.X = gore7.velocity.X - 1f;
					Gore gore8 = Main.gore[num3];
					gore8.velocity.Y = gore8.velocity.Y - 1f;
				}
				return;
			}
			visualAction();
		}

		public static void DrawString(int index, Vector2 to = default(Vector2))
		{
			Projectile projectile = Main.projectile[index];
			Vector2 mountedCenter = Main.player[projectile.owner].MountedCenter;
			Vector2 vector = mountedCenter;
			vector.Y += Main.player[projectile.owner].gfxOffY;
			if (to != default(Vector2))
				vector = to;

			float num = projectile.Center.X - vector.X;
			float num2 = projectile.Center.Y - vector.Y;
			Math.Sqrt((double)(num * num + num2 * num2));
			float rotation = (float)Math.Atan2((double)num2, (double)num) - 1.57f;
			if (!projectile.counterweight)
			{
				int num3 = -1;
				if (projectile.position.X + (float)(projectile.width / 2) < Main.player[projectile.owner].position.X + (float)(Main.player[projectile.owner].width / 2))
				{
					num3 = 1;
				}
				num3 *= -1;
				Main.player[projectile.owner].itemRotation = (float)Math.Atan2((double)(num2 * (float)num3), (double)(num * (float)num3));
			}

			bool flag = true;
			if (num == 0f && num2 == 0f)
			{
				flag = false;
			}
			else
			{
				float num4 = (float)Math.Sqrt((double)(num * num + num2 * num2));
				num4 = 12f / num4;
				num *= num4;
				num2 *= num4;
				vector.X -= num * 0.1f;
				vector.Y -= num2 * 0.1f;
				num = projectile.position.X + (float)projectile.width * 0.5f - vector.X;
				num2 = projectile.position.Y + (float)projectile.height * 0.5f - vector.Y;
			}

			while (flag)
			{
				float num5 = 12f;
				float num6 = (float)Math.Sqrt((double)(num * num + num2 * num2));
				float num7 = num6;
				if (float.IsNaN(num6) || float.IsNaN(num7))
				{
					flag = false;
				}
				else
				{
					if (num6 < 20f)
					{
						num5 = num6 - 8f;
						flag = false;
					}
					num6 = 12f / num6;
					num *= num6;
					num2 *= num6;
					vector.X += num;
					vector.Y += num2;
					num = projectile.position.X + (float)projectile.width * 0.5f - vector.X;
					num2 = projectile.position.Y + (float)projectile.height * 0.1f - vector.Y;
					if (num7 > 12f)
					{
						float num8 = 0.3f;
						float num9 = Math.Abs(projectile.velocity.X) + Math.Abs(projectile.velocity.Y);
						if (num9 > 16f)
						{
							num9 = 16f;
						}
						num9 = 1f - num9 / 16f;
						num8 *= num9;
						num9 = num7 / 80f;
						if (num9 > 1f)
						{
							num9 = 1f;
						}
						num8 *= num9;
						if (num8 < 0f)
						{
							num8 = 0f;
						}
						num8 *= num9;
						num8 *= 0.5f;
						if (num2 > 0f)
						{
							num2 *= 1f + num8;
							num *= 1f - num8;
						}
						else
						{
							num9 = Math.Abs(projectile.velocity.X) / 3f;
							if (num9 > 1f)
							{
								num9 = 1f;
							}
							num9 -= 0.5f;
							num8 *= num9;
							if (num8 > 0f)
							{
								num8 *= 2f;
							}
							num2 *= 1f + num8;
							num *= 1f - num8;
						}
					}
					rotation = (float)Math.Atan2((double)num2, (double)num) - 1.57f;
					int stringColor = Main.player[projectile.owner].stringColor;
					Color color = WorldGen.paintColor(stringColor);
					if (color.R < 75)
					{
						color.R = 75;
					}
					if (color.G < 75)
					{
						color.G = 75;
					}
					if (color.B < 75)
					{
						color.B = 75;
					}
					if (stringColor == 13)
					{
						color = new Color(20, 20, 20);
					}
					else if (stringColor == 14 || stringColor == 0)
					{
						color = new Color(200, 200, 200);
					}
					else if (stringColor == 28)
					{
						color = new Color(163, 116, 91);
					}
					else if (stringColor == 27)
					{
						color = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB);
					}
					color.A = (byte)((float)color.A * 0.4f);
					float num10 = 0.5f;
					color = Lighting.GetColor((int)vector.X / 16, (int)(vector.Y / 16f), color);
					color = new Color((int)((byte)((float)color.R * num10)), (int)((byte)((float)color.G * num10)), (int)((byte)((float)color.B * num10)), (int)((byte)((float)color.A * num10)));
					Main.spriteBatch.Draw(Main.fishingLineTexture, new Vector2(vector.X - Main.screenPosition.X + (float)Main.fishingLineTexture.Width * 0.5f, vector.Y - Main.screenPosition.Y + (float)Main.fishingLineTexture.Height * 0.5f) - new Vector2(6f, 0f), new Rectangle?(new Rectangle(0, 0, Main.fishingLineTexture.Width, (int)num5)), color, rotation, new Vector2((float)Main.fishingLineTexture.Width * 0.5f, 0f), 1f, SpriteEffects.None, 0f);
				}
			}
		}

		public static void DrawChain(int index, Vector2 to, string chainPath, bool electric = false, int damage = 0)
		{
			Texture2D texture = ModContent.GetTexture(chainPath);
			Projectile projectile = Main.projectile[index];
			Vector2 vector = projectile.Center;
			Rectangle? sourceRectangle = null;
			Vector2 origin = new Vector2((float)texture.Width * 0.5f, (float)texture.Height * 0.5f);
			float num = (float)texture.Height;
			Vector2 vector2 = to - vector;
			float rotation = (float)Math.Atan2((double)vector2.Y, (double)vector2.X) - 1.57f;
			bool flag = true;
			if (float.IsNaN(vector.X) && float.IsNaN(vector.Y))
			{
				flag = false;
			}
			if (float.IsNaN(vector2.X) && float.IsNaN(vector2.Y))
			{
				flag = false;
			}

			while (flag)
			{
				if ((double)vector2.Length() < (double)num + 1.0)
				{
					flag = false;
				}
				else
				{
					Vector2 value = vector2;
					value.Normalize();
					vector += value * num;
					vector2 = to - vector;
					Color color = Lighting.GetColor((int)vector.X / 16, (int)((double)vector.Y / 16.0));
					color = projectile.GetAlpha(color);
					Main.spriteBatch.Draw(texture, vector - Main.screenPosition, sourceRectangle, color, rotation, origin, 1f, SpriteEffects.None, 0f);
					if (electric)
					{
						Projectile.NewProjectile(vector.X, vector.Y, 0, 0, ModLoader.GetMod("SpiritMod").ProjectileType("ElectricChain"), damage, 0, Main.myPlayer);
					}
				}
			}
		}
		public static void DrawAroundOrigin(int index, Color lightColor)
		{
			Projectile projectile = Main.projectile[index];
			Texture2D texture2D = Main.projectileTexture[projectile.type];
			Vector2 origin = new Vector2((float)texture2D.Width * 0.5f, (float)(texture2D.Height / Main.projFrames[projectile.type]) * 0.5f);
			SpriteEffects effects = (projectile.direction == -1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			Main.spriteBatch.Draw(texture2D, projectile.Center - Main.screenPosition, new Rectangle?(Utils.Frame(texture2D, 1, Main.projFrames[projectile.type], 0, projectile.frame)), lightColor, projectile.rotation, origin, projectile.scale, effects, 0f);
		}

		public static void DrawSpear(int index, Color lightColor)
		{
			Projectile projectile = Main.projectile[index];
			Vector2 zero = Vector2.Zero;
			SpriteEffects effects = SpriteEffects.None;
			if (projectile.spriteDirection == -1)
			{
				zero.X = (float)Main.projectileTexture[projectile.type].Width;
				effects = SpriteEffects.FlipHorizontally;
			}
			Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], new Vector2(projectile.position.X - Main.screenPosition.X + (float)(projectile.width / 2), projectile.position.Y - Main.screenPosition.Y + (float)(projectile.height / 2) + projectile.gfxOffY), new Rectangle?(new Rectangle(0, 0, Main.projectileTexture[projectile.type].Width, Main.projectileTexture[projectile.type].Height)), projectile.GetAlpha(lightColor), projectile.rotation, zero, projectile.scale, effects, 0f);
		}
	}
}


