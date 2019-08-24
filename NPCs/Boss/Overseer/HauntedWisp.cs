using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.Overseer
{
	public class HauntedWisp : ModProjectile
	{
		int target;
		// USE THIS DUST: 261

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Haunted Wisp");
		}

		public override void SetDefaults()
		{
			projectile.width = projectile.height = 12;

			projectile.hostile = true;
			projectile.ignoreWater = true;
			projectile.tileCollide = false;

			projectile.penetrate = -1;

			projectile.timeLeft = 300;
		}

		public override bool PreAI()
		{
			projectile.rotation = projectile.velocity.ToRotation() + 1.57F;

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
			else
			{
				Player targetPlayer = Main.player[this.target];
				if (!targetPlayer.active || targetPlayer.dead)
				{
					this.target = -1;
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

			if (projectile.timeLeft <= 60)
			{
				projectile.alpha -= 4;
			}

			return false;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 4; i < 31; i++)
			{
				float x = projectile.oldVelocity.X * (30f / i);
				float y = projectile.oldVelocity.Y * (30f / i);
				int newDust = Dust.NewDust(new Vector2(projectile.oldPosition.X - x, projectile.oldPosition.Y - y), 8, 8, 261, projectile.oldVelocity.X, projectile.oldVelocity.Y, 100, default(Color), 1.8f);
				Main.dust[newDust].noGravity = true;
				Main.dust[newDust].velocity *= 0.5f;
				newDust = Dust.NewDust(new Vector2(projectile.oldPosition.X - x, projectile.oldPosition.Y - y), 8, 8, 261, projectile.oldVelocity.X, projectile.oldVelocity.Y, 100, default(Color), 1.4f);
				Main.dust[newDust].velocity *= 0.05f;
				Main.dust[newDust].noGravity = true;
			}
		}

		public override void SendExtraAI(System.IO.BinaryWriter writer)
		{
			writer.Write(this.target);
		}

		public override void ReceiveExtraAI(System.IO.BinaryReader reader)
		{
			this.target = reader.Read();
		}
	}
}
