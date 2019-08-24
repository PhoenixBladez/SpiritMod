using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Summon
{
	public class GloomgusShroom : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gloomgus Mushroom");
		}

		public override void SetDefaults()
		{
			///for reasons, I have to put a comment here.
			aiType = ProjectileID.StickyGrenade;
			projectile.friendly = true;
			projectile.minion = true;
			projectile.hostile = false;
			projectile.timeLeft = 200;
			projectile.width = 20;
			projectile.CloneDefaults(ProjectileID.StickyGrenade);
			projectile.height = 20;
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 14);
			for (int i = 0; i < 15; i++)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 187);
				Main.dust[dust].noGravity = true;
			}
		}


		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(3) == 0)
				target.AddBuff(mod.BuffType("StarFlame"), 180);
			projectile.Kill();
		}

		public override void AI()
		{
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

					if (num416 > 2)
					{
						Main.projectile[num417].netUpdate = true;
						Main.projectile[num417].ai[1] = 36000f;
						return;
					}
				}
			}
		}

	}
}