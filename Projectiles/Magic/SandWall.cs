using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class SandWall : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sand Wall");
		}

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.magic = true;
			projectile.width = 24;
			projectile.height = 180;
			projectile.aiStyle = -1;
			projectile.friendly = true;
			projectile.penetrate = 6;
			projectile.alpha = 255;
			projectile.timeLeft = 360;
			projectile.tileCollide = false; //Tells the game whether or not it can collide with a tile
		}

		public override bool PreAI()
		{
			projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
			//Create particles
			int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 32, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
			int dust1 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 36, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
			int dust2 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustID.GoldCoin, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
			Main.dust[dust].noGravity = true;
			Main.dust[dust1].noGravity = true;
			Main.dust[dust2].noGravity = true;
			Main.dust[dust2].scale = 2f;
			Main.dust[dust1].scale = 1.5f;
			Main.dust[dust].scale = 1.5f;
			return false;
		}

		public override void AI()
		{
			int timer = 0;
			projectile.velocity *= 1.15f;

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

			if (projectile.localAI[1] <= 1.0f)
			{
				int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, minRadius, minSpeed, mod.ProjectileType("TrueClot2"), projectile.damage, projectile.knockBack, projectile.owner, 0.0f, 0.0f);
				Main.projectile[proj].localAI[0] = projectile.whoAmI;
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(8) == 0)
				target.AddBuff(BuffID.Midas, 300, true);
		}

	}
}
