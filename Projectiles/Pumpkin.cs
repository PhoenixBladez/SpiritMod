using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class Pumpkin : ModProjectile
	{
		int timer = 0;
		bool launch = false;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pumpkin");
		}

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.magic = true;
			projectile.light = 0.5f;
			projectile.width = 24;
			projectile.height = 24;
			projectile.timeLeft = 180;
			projectile.friendly = true;
			projectile.damage = 10;
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 14);
				for (int num623 = 0; num623 < 25; num623++)
				{
					int num622 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 189, 0f, 0f, 100, default(Color), 2f);
					Main.dust[num622].noGravity = true;
					Main.dust[num622].velocity *= 1.5f;
					Main.dust[num622].scale = 0.8f;
				}
		}

		public override void AI()
		{
			timer++;
			Vector2 targetPos = projectile.Center;
			float targetDist = 1000f;
			bool targetAcquired = false;

			//loop through first 200 NPCs in Main.npc
			//this loop finds the closest valid target NPC within the range of targetDist pixels
			for (int i = 0; i < 200; i++)
			{
				if (Main.npc[i].CanBeChasedBy(projectile) && Collision.CanHit(projectile.Center, 1, 1, Main.npc[i].Center, 1, 1))
				{
					float dist = projectile.Distance(Main.npc[i].Center);
					if (dist < targetDist)
					{
						targetDist = dist;
						targetPos = Main.npc[i].Center;
						targetAcquired = true;
					}
				}
			}

			//change trajectory to home in on target
			projectile.rotation += 0.1f;
			if (timer > 60)
			{
				projectile.rotation += 0.2f;
				int num622 = Dust.NewDust(new Vector2(projectile.position.X - projectile.velocity.X, projectile.position.Y - projectile.velocity.Y), projectile.width, projectile.height, 189, 0f, 0f, 100, default(Color), 2f);
				Main.dust[num622].noGravity = true;
				Main.dust[num622].scale = 0.5f;
			}
			if (targetAcquired && timer > 90 && !launch)
			{
				float homingSpeedFactor = 15f;
				Vector2 homingVect = targetPos - projectile.Center;
				homingVect.Normalize();
				homingVect *= homingSpeedFactor;

				projectile.velocity = homingVect;
				launch = true;
			}
			
			
			Vector3 RGB = new Vector3(1f, 0.32f, 0f);
			float multiplier = 1;
			float max = 2.25f;
			float min = 1.0f;
			RGB *= multiplier;
			if (RGB.X > max)
			{
				multiplier = 0.5f;
			}
			if (RGB.X < min)
			{
				multiplier = 1.5f;
			}
			Lighting.AddLight(projectile.position, RGB.X, RGB.Y, RGB.Z);
		}

		//public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		//{
		//    Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
		//    for (int k = 0; k < projectile.oldPos.Length; k++)
		//    {
		//        Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
		//        Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
		//        spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
		//    }
		//    return true;
		//}
	}
}