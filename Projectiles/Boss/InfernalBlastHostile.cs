using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Boss
{
	public class InfernalBlastHostile : ModProjectile
	{
		int target;
		// USE THIS DUST: 261

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gold Dust");
		}

		public override void SetDefaults()
		{
			projectile.width = projectile.height = 12;

			projectile.hostile = true;
			projectile.friendly = false;
			projectile.ignoreWater = true;
			projectile.tileCollide = false;
			projectile.alpha = 255;
			projectile.penetrate = 1;

			projectile.timeLeft = 120;
		}

		public override bool PreAI()
		{
			for (int i = 0; i < 16; i++)
			{
				float x = projectile.Center.X - projectile.velocity.X / 10f * (float)i;
				float y = projectile.Center.Y - projectile.velocity.Y / 10f * (float)i;
				int num = Dust.NewDust(new Vector2(x, y), 26, 26, 6, 0f, 0f, 0, default(Color), 1f);
				Main.dust[num].alpha = projectile.alpha;
				Main.dust[num].position.X = x;
				Main.dust[num].position.Y = y;
				Main.dust[num].velocity *= 0f;
				Main.dust[num].noGravity = true;
			}

			if (projectile.ai[0] == 0 && Main.netMode != 1)
			{
				target = -1;
				float distance = 2000f;
				for (int k = 0; k < 255; k++)
				{
					if (Main.player[k].active && !Main.player[k].dead)
					{
						Vector2 center = Main.player[k].Center;
						float currentDistance = Vector2.Distance(center, projectile.Center);
						if (currentDistance < distance || target == -1)
						{
							distance = currentDistance;
							target = k;
						}
					}
				}
				if (target != -1)
				{
					projectile.ai[0] = 1;
					projectile.netUpdate = true;
				}
			}
			else if (target >= 0 && target < Main.maxPlayers)
			{
				Player targetPlayer = Main.player[target];
				if (!targetPlayer.active || targetPlayer.dead)
				{
					target = -1;
					projectile.ai[0] = 0;
					projectile.netUpdate = true;
				}
				else
				{
					float currentRot = projectile.velocity.ToRotation();
					Vector2 direction = targetPlayer.Center - projectile.Center;
					float targetAngle = direction.ToRotation();
					if (direction == Vector2.Zero)
					{
						targetAngle = currentRot;
					}

					float desiredRot = currentRot.AngleLerp(targetAngle, 0.1f);
					projectile.velocity = new Vector2(projectile.velocity.Length(), 0f).RotatedBy(desiredRot, default(Vector2));
				}
			}
			return true;
		}

		public override void AI()
		{
			projectile.frameCounter++;
			if (projectile.frameCounter > 8)
			{
				projectile.frameCounter = 0;
				projectile.frame++;
				if (projectile.frame > 5)
					projectile.frame = 0;
			}

			projectile.ai[1] += 1f;
			if (projectile.ai[1] >= 7200f)
			{
				projectile.alpha += 5;
				if (projectile.alpha > 255)
				{
					projectile.alpha = 255;
					projectile.Kill();
				}
			}

			projectile.localAI[0] += 1f;
			if (projectile.localAI[0] >= 10f)
			{
				projectile.localAI[0] = 0f;
				int num416 = 0;
				int num417 = 0;
				float num418 = 0f;
				int num419 = projectile.type;
				for (int num420 = 0; num420 < 1000; num420++)
				{
					if (Main.projectile[num420].active && Main.projectile[num420].owner == projectile.owner && Main.projectile[num420].type == num419 && Main.projectile[num420].ai[1] < 3600f)
					{
						num416++;
						if (Main.projectile[num420].ai[1] > num418)
						{
							num417 = num420;
							num418 = Main.projectile[num420].ai[1];
						}
					}
				}
				if (num416 > 8)
				{
					Main.projectile[num417].netUpdate = true;
					Main.projectile[num417].ai[1] = 36000f;
					return;
				}
			}
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 4; i < 31; i++)
			{
				float x = projectile.oldVelocity.X * (30f / i);
				float y = projectile.oldVelocity.Y * (30f / i);
				int newDust = Dust.NewDust(new Vector2(projectile.oldPosition.X - x, projectile.oldPosition.Y - y), 8, 8,
					6, projectile.oldVelocity.X, projectile.oldVelocity.Y, 100, default(Color), 1.8f);
				Main.dust[newDust].noGravity = true;
				Main.dust[newDust].velocity *= 0.5f;
			}
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.Next(4) == 0)
				target.AddBuff(BuffID.OnFire, 180, true);

			projectile.Kill();
		}

		public override void SendExtraAI(System.IO.BinaryWriter writer)
		{
			writer.Write(target);
		}

		public override void ReceiveExtraAI(System.IO.BinaryReader reader)
		{
			target = reader.Read();
		}

	}
}
