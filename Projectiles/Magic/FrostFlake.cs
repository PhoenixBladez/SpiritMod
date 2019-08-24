using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class FrostFlake : ModProjectile
	{
		int target;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frost Flake");

			Main.projFrames[projectile.type] = 3;
		}

		public override void SetDefaults()
		{
			projectile.width = projectile.height = 16;

			projectile.penetrate = 6;

			projectile.magic = true;
			projectile.friendly = true;
			ProjectileID.Sets.Homing[projectile.type] = true;

			projectile.timeLeft = 300;
		}

		public override bool PreAI()
		{
			if (projectile.ai[0] == 0)
			{
				projectile.frame = Main.rand.Next(Main.projFrames[projectile.type]);
				projectile.ai[0] = 1;
			}
			else
			{
				projectile.rotation += (Math.Abs(projectile.velocity.X) + Math.Abs(projectile.velocity.Y)) * 0.03f * (float)projectile.direction;

				if (projectile.ai[1] == 0 && Main.netMode != 1)
				{
					target = -1;
					float distance = 320;
					for (int k = 0; k < 200; k++)
					{
						if (Main.npc[k].active && Main.npc[k].CanBeChasedBy(projectile, false) && Collision.CanHitLine(projectile.Center, 1, 1, Main.npc[k].Center, 1, 1))
						{
							Vector2 center = Main.npc[k].Center;
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
						projectile.ai[1] = 1;
						projectile.netUpdate = true;
					}
				}
				else
				{
					NPC targetNPC = Main.npc[this.target];
					if (!targetNPC.active || !targetNPC.CanBeChasedBy(projectile, false) || !Collision.CanHitLine(projectile.Center, 1, 1, targetNPC.Center, 1, 1))
					{
						this.target = -1;
						projectile.ai[1] = 0;
						projectile.netUpdate = true;
					}
					else
					{
						float currentRot = projectile.velocity.ToRotation();
						Vector2 direction = targetNPC.Center - projectile.Center;
						float targetAngle = direction.ToRotation();
						if (direction == Vector2.Zero)
							targetAngle = currentRot;

						float desiredRot = currentRot.AngleLerp(targetAngle, 0.04f);
						projectile.velocity = new Vector2(projectile.velocity.Length(), 0f).RotatedBy(desiredRot, default(Vector2));
					}
				}
			}

			if (projectile.timeLeft <= 30)
				projectile.Opacity -= 0.032F;
			return false;
		}

		public override void AI()
		{
			Dust.NewDust(projectile.position, projectile.width, projectile.height, 67);
			Dust.NewDust(projectile.position, projectile.width, projectile.height, 67);
		}

		public override void SendExtraAI(System.IO.BinaryWriter writer)
		{
			writer.Write(this.target);
		}

		public override void ReceiveExtraAI(System.IO.BinaryReader reader)
		{
			this.target = reader.Read();
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(BuffID.Frostburn, 200, true);
		}

	}
}
