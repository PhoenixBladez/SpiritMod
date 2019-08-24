using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

using Terraria.ModLoader;
using Terraria;
using Terraria.ID;

namespace SpiritMod.Projectiles.Hostile
{
	public class RuneHostile : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Flames");
			Main.projFrames[base.projectile.type] = 4;
		}

		public override void SetDefaults()
		{
			projectile.width = 16;       //projectile width
			projectile.height = 16;  //projectile height
			projectile.friendly = false;      //make that the projectile will not damage you
			projectile.hostile = true;        // 
			projectile.tileCollide = false;   //make that the projectile will be destroed if it hits the terrain
			projectile.penetrate = 1;      //how many npc will penetrate
			projectile.timeLeft = 300;   //how many time projectile projectile has before disepire
			projectile.ignoreWater = true;
			projectile.extraUpdates = 1;
			projectile.aiStyle = -1;
		}

		public override void AI()
		{
			float num = 1f - (float)projectile.alpha / 255f;
			num *= projectile.scale;
			Lighting.AddLight(projectile.Center, 0.5f * num, 0.5f * num, 0.9f * num);
			projectile.frameCounter++;
			if (projectile.frameCounter >= 8)
			{
				projectile.frame = (projectile.frame + 1) % Main.projFrames[projectile.type];
				projectile.frameCounter = 0;
			}

			float num1 = 10f;
			float num2 = 5f;
			float num3 = 40f;
			num1 = 10f;
			num2 = 7.5f;
			if (projectile.timeLeft > 30 && projectile.alpha > 0)
				projectile.alpha -= 25;
			if (projectile.timeLeft > 30 && projectile.alpha < 128 && Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
				projectile.alpha = 128;
			if (projectile.alpha < 0)
				projectile.alpha = 0;

			if (++projectile.frameCounter > 4)
			{
				projectile.frameCounter = 0;
				if (++projectile.frame >= 4)
					projectile.frame = 0;
			}
			float num4 = 0.5f;
			if (projectile.timeLeft < 120)
				num4 = 1.1f;
			if (projectile.timeLeft < 60)
				num4 = 1.6f;

			++projectile.ai[1];
			double num5 = (double)projectile.ai[1] / 180.0;
			for (float num6 = 0.0f; (double)num6 < 3.0; ++num6)
			{
				if (Main.rand.Next(3) != 0)
					return;
				Dust dust = Main.dust[Dust.NewDust(projectile.Center, 0, 0, 187, 0.0f, -2f)];
				dust.position = projectile.Center + Vector2.UnitY.RotatedBy((double)num6 * (Math.PI / 1.5) + (double)projectile.ai[1]) * 10f;
				dust.noGravity = true;
				dust.velocity = projectile.DirectionFrom(dust.position);
				dust.scale = num4;
				dust.fadeIn = 0.5f;
				dust.alpha = 200;
			}

			int index1 = (int)projectile.ai[0];
			if (index1 >= 0 && Main.player[index1].active && !Main.player[index1].dead)
			{
				if (projectile.Distance(Main.player[index1].Center) <= num3)
					return;
				Vector2 unitY = projectile.DirectionTo(Main.player[index1].Center);
				if (unitY.HasNaNs())
					unitY = Vector2.UnitY;
				projectile.velocity = (projectile.velocity * (num1 - 1f) + unitY * num2) / num1;
			}
			else
			{
				if (projectile.timeLeft > 30)
					projectile.timeLeft = 30;
				if (projectile.ai[0] == -1f)
					return;
				projectile.ai[0] = -1f;
				projectile.netUpdate = true;
			}
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 40; i++)
			{
				int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, 68, 0f, -2f, 0, default(Color), 2f);
				Main.dust[num].noGravity = true;
				Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
				Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
				if (Main.dust[num].position != projectile.Center)
				{
					Main.dust[num].velocity = projectile.DirectionTo(Main.dust[num].position) * 6f;
				}
			}
		}

	}
}
