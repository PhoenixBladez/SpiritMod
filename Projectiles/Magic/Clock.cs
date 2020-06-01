using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using SpiritMod;
using SpiritMod.Effects;
using Terraria.ModLoader;
using Terraria.Graphics.Effects;

namespace SpiritMod.Projectiles.Magic
{
	public class Clock : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Clock");
		}

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.magic = true;
			projectile.width = 10;
			projectile.height = 10;
			projectile.friendly = false;
			projectile.alpha = 255;
			projectile.penetrate = -1;
			projectile.timeLeft = 200;
			projectile.tileCollide = false;
		}
		
		private void Trail(Vector2 from, Vector2 to)
		{
			float distance = Vector2.Distance(from, to);
			float step = 1 / distance;
			for (float w = 0; w < distance; w+= 8)
			{
				int dust = Dust.NewDust(Vector2.Lerp(from, to, w * step) - new Vector2(5, 5), 10, 10, 206, 0, 0);
				Main.dust[dust].scale = 1.25f;
				Main.dust[dust].velocity = Vector2.Zero;
			}
		}
		
        int counter = -720;
		bool boom = false;
		private float distortStrength = 300f;
		
		int minuteHand = -90;
		int hourHand = -90;
		int minuteLength = 175;
		int hourLength = 125;
        public override bool PreAI()
        {
			Player player = Main.player[projectile.owner];
			if (projectile.timeLeft <= 24)
			{
				if (!boom)
				{
				   if (Main.netMode != NetmodeID.Server && !Filters.Scene["Shockwave"].IsActive())
					{
						Filters.Scene.Activate("Shockwave", projectile.Center).GetShader().UseColor(10, 5, 15).UseTargetPosition(projectile.Center);
					}
					
					boom = true;
				}
				if (Main.netMode != NetmodeID.Server && Filters.Scene["Shockwave"].IsActive())
				{
					float progress = (24f - projectile.timeLeft) / 60f; // Will range from -3 to 3, 0 being the point where the bomb explodes.
					Filters.Scene["Shockwave"].GetShader().UseProgress(progress).UseOpacity(distortStrength * (1 - progress / 3f));
				}
			}
			else
			{
				minuteHand -= 5;
				hourHand -= 1;
				Trail(projectile.Center, projectile.Center + new Vector2((float)Math.Sin(minuteHand * (Math.PI / 180)) * minuteLength, (float)Math.Cos(minuteHand * (Math.PI / 180)) * minuteLength));
				Trail(projectile.Center, projectile.Center + new Vector2((float)Math.Sin(hourHand * (Math.PI / 180)) * hourLength, (float)Math.Cos(hourHand * (Math.PI / 180)) * hourLength));
				for (int i = 0; i < 360; i+= 10)
				{
					Dust.NewDustPerfect(projectile.Center + new Vector2((float)Math.Sin(i * (Math.PI / 180)) * minuteLength, (float)Math.Cos(i * (Math.PI / 180)) * minuteLength), 206).velocity = Vector2.Zero;
					if (i % 30 == 0)
					{
						for (int j = minuteLength; j > minuteLength - 15; j-=3)
						{
							Dust.NewDustPerfect(projectile.Center + new Vector2((float)Math.Sin(i * (Math.PI / 180)) * j, (float)Math.Cos(i * (Math.PI / 180)) * j), 206).velocity = Vector2.Zero;
						}
					}
				}
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

					if (num416 > 1)
					{
						Main.projectile[num417].netUpdate = true;
						Main.projectile[num417].ai[1] = 36000f;
						return false;
					}
				}
			}
			
			return false;
		}
		public override void Kill(int timeLeft)
		{
			if(Main.netMode != NetmodeID.Server && Filters.Scene["Shockwave"].IsActive())
			{
				Filters.Scene["Shockwave"].Deactivate();
			}
		}
	}
}
