using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory.MoonlightSack
{
	public class Moonlight_Sack_Lightning : ModProjectile
	{
		public float x = 0f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Moonlight Lightning");
		}
		public override void SetDefaults()
		{
			projectile.width = 4;
			projectile.height = 4;
			projectile.hide = true;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.aiStyle = 0;
			projectile.localNPCHitCooldown = 240;
			projectile.penetrate = 2;
			projectile.minion = true;
			projectile.MaxUpdates = 15;
			projectile.timeLeft = 66;
			projectile.tileCollide = false;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Main.PlaySound(42, (int)target.position.X, (int)target.position.Y, 20, 1f, 0.0f);
		}
		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			projectile.localAI[0] += 1f;
			
			if (projectile.localAI[0] > -1f)
			{
				x = projectile.Center.Y + 50;
			}
			if (projectile.localAI[0] > -1f)
            {
				for (int i = 0; i < 10; i++)
				{
					float PosX = projectile.Center.X - projectile.velocity.X / 10f * (float)i;
					float PosY = projectile.Center.Y - projectile.velocity.Y / 10f * (float)i;
					
					if ((double)Vector2.Distance(player.Center, projectile.Center) > (double)20f)
					{
						int dustIndex = Dust.NewDust(new Vector2(PosX, PosY), 0, 0, 92, 0f, 0f, 180, default(Color), 0.5f);
						
						Main.dust[dustIndex].position.X = PosX;
						Main.dust[dustIndex].position.Y = PosY;
						
						Main.dust[dustIndex].velocity *= 0f;
						Main.dust[dustIndex].noGravity = true;
					}
				}
            }
			
			if ((double)Vector2.Distance(projectile.Center, Main.projectile[(int)projectile.ai[0]].Center) <= (double)20f)
			{
				projectile.Kill();
			}
			
			Vector2 vector2_1 = new Vector2((float) Main.projectile[(int)projectile.ai[0]].Center.X, (float) Main.projectile[(int)projectile.ai[0]].Center.Y);
			float speed = 16f;
			float dX = vector2_1.X - projectile.Center.X;
			float dY = vector2_1.Y - projectile.Center.Y;
			
			float dist = (float)Math.Sqrt((double)(dX * dX + dY * dY));
			
			speed /= dist;
			
			Vector2 randomSpeed = new Vector2(dX, dY).RotatedByRandom(MathHelper.ToRadians(-90));
			
			if (projectile.localAI[0] > 1f)
			{
				projectile.velocity = new Vector2(randomSpeed.X * speed, randomSpeed.Y * speed);
			}
		}
	}
}