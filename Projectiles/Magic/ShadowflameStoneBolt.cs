using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class ShadowflameStoneBolt : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shadowbreak Staff");
		}

		public override void SetDefaults()
		{
			projectile.width = projectile.height = 8;

			projectile.hostile = false;
			projectile.friendly = true;
			projectile.alpha = 255;

			projectile.penetrate = 1;
		}

		public float counter = -1440;
		public override void AI()
		{
			projectile.alpha = 255;
			counter++;
			if (counter >= 1440)
			{
				counter = -1440;
			}
			for (int i = 0; i < 5; i++)
			{
				float x = projectile.Center.X - projectile.velocity.X / 10f * (float)i;
				float y = projectile.Center.Y - projectile.velocity.Y / 10f * (float)i;
				
				int num = Dust.NewDust(projectile.Center + new Vector2(0, (float)Math.Cos(counter/8.2f)*9.2f).RotatedBy(projectile.rotation), 6, 6, 27, 0f, 0f, 0, default(Color), 1f);
				Main.dust[num].velocity *= .1f;
				Main.dust[num].position -= projectile.velocity * 0.5f;
				Main.dust[num].scale *= .8f;				
				Main.dust[num].noGravity = true;
			
			}
            for (int i = 0; i < 5; i++)
			{
				float x = projectile.Center.X - projectile.velocity.X / 10f * (float)i;
				float y = projectile.Center.Y - projectile.velocity.Y / 10f * (float)i;
				
				int num = Dust.NewDust(projectile.Center + new Vector2(0, (float)Math.Cos(counter/8.2f)*9.2f).RotatedBy(projectile.rotation), 6, 6, 173, 0f, 0f, 0, default(Color), 1f);
				Main.dust[num].velocity *= .1f;
				Main.dust[num].position -= projectile.velocity * 0.5f;
				Main.dust[num].scale *= .8f;				
				Main.dust[num].noGravity = true;
			
			}
		}
		public override void Kill(int timeLeft)
		{
			Main.PlaySound(0, (int)projectile.position.X, (int)projectile.position.Y, 0);
			for (int num621 = 0; num621 < 40; num621++)
			{
				int num622 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 27, 0f, 0f, 100, default(Color), 2f);
				Main.dust[num622].velocity *= 3f;
				Main.dust[num622].noGravity = true;
				Main.dust[num622].scale = 0.85f;
			}
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(8) == 0)
				target.AddBuff(BuffID.ShadowFlame, 180);
        }
	}
}
