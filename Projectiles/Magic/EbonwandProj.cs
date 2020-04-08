using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;

namespace SpiritMod.Projectiles.Magic
{
	public class EbonwandProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cursed Wisp");
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);
			projectile.width = 6;
			projectile.height = 6;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.magic = true;
			projectile.ranged = false;
            projectile.timeLeft = 60;
			projectile.penetrate = 1;
		}

		public override void AI()
		{
			projectile.rotation += .1f;
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
				if (num416 > 5)
				{
					Main.projectile[num417].netUpdate = true;
					Main.projectile[num417].ai[1] = 36000f;
					return;
				}
			}
			int num = 5;
			for (int k = 0; k < 3; k++)
				{
					int index2 = Dust.NewDust(projectile.position, 1, 1, 75, 0.0f, 0.0f, 0, new Color(), 1f);
					Main.dust[index2].position = projectile.Center - projectile.velocity / num * (float)k;
					Main.dust[index2].scale = .5f;
					Main.dust[index2].noGravity = true;
					Main.dust[index2].noLight = false;	
				}		
			for (int k = 0; k < 2; k++)
				{
					int index2 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 75, 0.0f, 0.0f, 0, new Color(), 1f);
					Main.dust[index2].scale = .8f;
                    Main.dust[index2].velocity.Y *= 4.4f;
					Main.dust[index2].noGravity = true;
					Main.dust[index2].noLight = false;	
				}		

		}
		
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Vector2 vector9 = projectile.position;
			Vector2 value19 = (projectile.rotation - 1.57079637f).ToRotationVector2();
			vector9 += value19 * 16f;
			for (int num257 = 0; num257 < 20; num257++)
			{
				int newDust = Dust.NewDust(vector9, projectile.width, projectile.height, 75, 0f, 0f, 0, default(Color), 1f);
				Main.dust[newDust].position = (Main.dust[newDust].position + projectile.Center) / 2f;
				Main.dust[newDust].velocity += value19 * 2f;
				Main.dust[newDust].velocity *= 0.5f;
				Main.dust[newDust].noGravity = true;
				vector9 -= value19 * 8f;
			}
            return true;
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(4) == 0)
				target.AddBuff(BuffID.CursedInferno, 180);
		}
	}
}
