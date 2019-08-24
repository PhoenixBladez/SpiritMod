using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace SpiritMod.Projectiles
{
	public class FireChain2 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fiery Essence");
		}

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.penetrate = -1;
			projectile.timeLeft = 240;
			projectile.height = 6;
			projectile.width = 6;
			projectile.alpha = 255;
			aiType = ProjectileID.Bullet;
			projectile.extraUpdates = 1;
			projectile.ignoreWater = true;
			projectile.tileCollide = false;
			projectile.magic = true;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			projectile.localAI[1] += 1f;
			target.AddBuff(BuffID.OnFire, 280);
			projectile.velocity *= 0f;
		}

		public override bool PreAI()
		{
			projectile.localAI[1] += 1f;
			int num = 1;
			int num2 = 1;
			if ((double)projectile.localAI[1] <= 1.0)
			{
				int num3 = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)num, (float)num2, mod.ProjectileType("FireChain3"), projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
				Main.projectile[num3].localAI[0] = (float)projectile.whoAmI;
			}

			int num4 = (int)projectile.localAI[1];
			if (num4 <= 30)
			{
				if (num4 == 30 || num4 == 10)
					num2--;
			}
			else if (num4 == 50 || num4 == 70)
			{
				num2--;
			}

			if ((int)projectile.localAI[1] == 20)
			{
				int num5 = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)num, (float)num2, mod.ProjectileType("FireChain3"), projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
				Main.projectile[num5].localAI[0] = (float)projectile.whoAmI;
			}
			if ((int)projectile.localAI[1] == 30)
			{
				int num6 = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)num, (float)num2, mod.ProjectileType("t"), projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
				Main.projectile[num6].localAI[0] = (float)projectile.whoAmI;
			}
			if ((int)projectile.localAI[1] == 40)
			{
				int num7 = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)num, (float)num2, mod.ProjectileType("e"), projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
				Main.projectile[num7].localAI[0] = (float)projectile.whoAmI;
			}
			return true;
		}

		public override void AI()
		{
			int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 6, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
			int dust2 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 6, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
			Main.dust[dust].noGravity = true;
			Main.dust[dust2].noGravity = true;
			Main.dust[dust2].velocity *= 0f;
			Main.dust[dust2].velocity *= 0f;
			Main.dust[dust2].scale = 0.9f;
			Main.dust[dust].scale = 0.9f;

			int num = (int)projectile.velocity.X * 10;
			int num2 = (int)projectile.velocity.Y + 1;
			projectile.frameCounter++;
			if (projectile.frameCounter > 120)
			{
				projectile.frameCounter = 0;
				if (projectile.frame == 5)
					projectile.frame = 0;
				else
					projectile.frame++;
			}

			int num3 = 80 - num;
			int num4 = 12 - num2;
			int num5 = 16;
			projectile.localAI[1] += 0.0104719754f * (float)num4;
			projectile.localAI[1] %= 6.28318548f;

			Vector2 center = Main.projectile[(int)projectile.localAI[0]].Center;
			center.X -= (float)num5;
			projectile.rotation = (float)Math.Atan2((double)center.Y, (double)center.X) - 2f;
			projectile.Center = center + (float)num3 * new Vector2((float)Math.Cos((double)projectile.localAI[1]), (float)Math.Sin((double)projectile.localAI[1]));
		}

		private static Vector2 GetVelocity(Projectile projectile)
		{
			float num = 400f;
			Vector2 velocity = projectile.velocity;
			Vector2 vector = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
			vector.SafeNormalize(Vector2.UnitY);
			vector *= Main.rand.Next(10, 41) * 0.1f;
			if (Main.rand.Next(3) == 0)
				vector *= 2f;

			Vector2 vector3 = velocity * 0.25f + vector;
			for (int i = 0; i < 200; i++)
			{
				if (Main.npc[i].CanBeChasedBy(projectile, false))
				{
					float num2 = Main.npc[i].position.X + (Main.npc[i].width / 2);
					float num3 = Main.npc[i].position.Y + (Main.npc[i].height / 2);
					float num4 = Math.Abs(projectile.position.X + (projectile.width / 2) - num2) + Math.Abs(projectile.position.Y + (projectile.height / 2) - num3);
					if (num4 < num && Collision.CanHit(projectile.position, projectile.width, projectile.height, Main.npc[i].position, Main.npc[i].width, Main.npc[i].height))
					{
						num = num4;
						vector3.X = num2;
						vector3.Y = num3;
						Vector2 vector4 = vector3 - projectile.Center;
						vector4.Normalize();
						vector3 = vector4 * 8f;
					}
				}
			}
			return vector3 * 0.8f;
		}

		public override void PostAI()
		{
			projectile.rotation -= 10f;
			projectile.velocity *= 0.95f;
		}
	}
}
