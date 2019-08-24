using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;

namespace SpiritMod.Projectiles.Magic
{
	public class TrueClot1 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Death Clot");
		}

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.magic = true;
			projectile.width = 60;
			projectile.height = 60;
			projectile.aiStyle = -1;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.alpha = 255;
			projectile.timeLeft = 540;
		}

		public override bool PreAI()
		{
			projectile.tileCollide = false;
			int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 5, 0f, 0f);
			int dust2 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 5, 0f, 0f);
			int dust3 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 5, 0f, 0f);
			Main.dust[dust].scale = 1.5f;
			Main.dust[dust].noGravity = true;
			return true;
		}

		int timer = 20;

		public override void AI()
		{
			timer--;

			if (timer == 0)
			{
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, projectile.velocity.X, projectile.velocity.Y, ProjectileID.GoldenShowerFriendly, projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
				timer = 20;
			}

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
				if (num416 > 2)
				{
					Main.projectile[num417].netUpdate = true;
					Main.projectile[num417].ai[1] = 36000f;
					return;
				}
			}

			++projectile.localAI[1];
			int minRadius = 1;
			int minSpeed = 1;

			if (projectile.localAI[1] <= 1.0)
			{
				int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, minRadius, minSpeed, mod.ProjectileType("TrueClot2"), projectile.damage, projectile.knockBack, projectile.owner, 0.0f, 0.0f);
				Main.projectile[proj].localAI[0] = projectile.whoAmI;
			}
			else
			{
				switch ((int)projectile.localAI[1])
				{
					case 10:
						minSpeed -= 1;
						break;
					case 30:
						minSpeed -= 1;
						break;
					case 50:
						minSpeed -= 1;
						break;
					case 70:
						minSpeed -= 1;
						break;
				}

				if ((int)projectile.localAI[1] == 120)
				{
					int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, minRadius, minSpeed, mod.ProjectileType("TrueClot2"), projectile.damage, projectile.knockBack, projectile.owner, 0.0f, 0.0f);
					Main.projectile[proj].localAI[0] = projectile.whoAmI;
				}

				if ((int)projectile.localAI[1] == 180)
				{
					int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, minRadius, minSpeed, mod.ProjectileType("TrueClot2"), projectile.damage, projectile.knockBack, projectile.owner, 0.0f, 0.0f);
					Main.projectile[proj].localAI[0] = projectile.whoAmI;
				}

				if ((int)projectile.localAI[1] == 240)
				{
					int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, minRadius, minSpeed, mod.ProjectileType("TrueClot2"), projectile.damage, projectile.knockBack, projectile.owner, 0.0f, 0.0f);
					Main.projectile[proj].localAI[0] = projectile.whoAmI;
				}
				if ((int)projectile.localAI[1] == 300)
				{
					int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, minRadius, minSpeed, mod.ProjectileType("TrueClot2"), projectile.damage, projectile.knockBack, projectile.owner, 0.0f, 0.0f);
					Main.projectile[proj].localAI[0] = projectile.whoAmI;
				}
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(10) <= 1)
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0f, 0f, 305, 0, 0f, projectile.owner, projectile.owner, Main.rand.Next(1, 2));

			if (Main.rand.Next(3) == 0)
				target.AddBuff(BuffID.Ichor, 300, true);
		}

		public override void Kill(int timeLeft)
		{
			Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 30f, 0f, mod.ProjectileType("Blood3"), projectile.damage, 0f, projectile.owner, 0f, 0f);
			Projectile.NewProjectile(projectile.position.X, projectile.position.Y, -30f, 0f, mod.ProjectileType("Blood3"), projectile.damage, 0f, projectile.owner, 0f, 0f);
			Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 0f, -30f, mod.ProjectileType("Blood3"), projectile.damage, 0f, projectile.owner, 0f, 0f);
			Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 10f, 30f, mod.ProjectileType("Blood3"), projectile.damage, 0f, projectile.owner, 0f, 0f);
			Projectile.NewProjectile(projectile.position.X, projectile.position.Y, -10f, 30f, mod.ProjectileType("Blood3"), projectile.damage, 0f, projectile.owner, 0f, 0f);

			Projectile.NewProjectile(projectile.position.X - 100, projectile.position.Y - 100, 0f, 30f, mod.ProjectileType("Blood3"), projectile.damage, 0f, projectile.owner, 0f, 0f);
			Projectile.NewProjectile(projectile.position.X - -100, projectile.position.Y - 100, 0f, 30f, mod.ProjectileType("Blood3"), projectile.damage, 0f, projectile.owner, 0f, 0f);
		}

	}
}

