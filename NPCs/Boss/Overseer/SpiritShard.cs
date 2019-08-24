using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.Overseer
{
	public class SpiritShard : ModProjectile
	{
		int target;
		// USE THIS DUST: 261

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spirit Shard");
		}

		public override void SetDefaults()
		{
			projectile.width = projectile.height = 12;

			projectile.hostile = true;
			projectile.ignoreWater = true;
			projectile.tileCollide = true;

			projectile.penetrate = 1;

			projectile.timeLeft = 175;
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 14);
			int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("UnstableWisp_Explosion"), (int)(projectile.damage), 0, Main.myPlayer);
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
			Player targetPlayer = Main.player[this.target];
			Vector2 direction = targetPlayer.Center - projectile.Center;
			direction.Normalize();
			projectile.velocity *= 0.98f;
			int dust2 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 206, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
			Main.dust[dust2].noGravity = true;
			if (Math.Sqrt((projectile.velocity.X * projectile.velocity.X) + (projectile.velocity.Y * projectile.velocity.Y)) >= 7f)
			{
				int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 206, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].scale = 2f;
				dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 206, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].scale = 2f;
			}
			if (Math.Sqrt((projectile.velocity.X * projectile.velocity.X) + (projectile.velocity.Y * projectile.velocity.Y)) < 14f)
			{
				if (Main.rand.Next(24) == 1)
				{
					direction.X = direction.X * Main.rand.Next(20, 24);
					direction.Y = direction.Y * Main.rand.Next(20, 24);
					projectile.velocity.X = direction.X;
					projectile.velocity.Y = direction.Y;
				}
			}
			return false;
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
