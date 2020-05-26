using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Arrow
{
	public class SleepingStar : ModProjectile
	{
		private int lastFrame = 0;
		int mode = 0; //1 = loops and changes, 2 = slower default change
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sleeping Star");
		}

		public override void SetDefaults()
		{
			projectile.penetrate = 1;
			projectile.friendly = true;
			projectile.ignoreWater = true;
			projectile.ranged = true;
			
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(4) == 0)
				target.AddBuff(mod.BuffType("StarFlame"), 200, true);
		}
		private void Trail(Vector2 from, Vector2 to, float dusttype)
		{
			float distance = Vector2.Distance(from, to);
			float step = 1 / distance;
			for (float w = 0; w < distance; w+=8)
			{
				if (dusttype == 1)
				{
					Dust.NewDustPerfect(Vector2.Lerp(from, to, w * step), mod.DustType("BlueMoonPinkDust"), Vector2.Zero);
				}
				else
				{
					Dust.NewDustPerfect(Vector2.Lerp(from, to, w * step), mod.DustType("BlueMoonBlueDust"), Vector2.Zero);
				}
			}
		}
		public override void Kill(int timeLeft)
		{
			Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 14);
			if (projectile.ai[0] == 1)
			{
			ProjectileExtras.Explode(projectile.whoAmI, 120, 120,
				delegate
			{
				for (int i = 0; i < 80; i++)
				{
					int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("BlueMoonPinkDust"), 0f, -2f, 0, default(Color), 2f);
					Main.dust[num].noGravity = true;
					Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
					Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
					if (Main.dust[num].position != projectile.Center)
						Main.dust[num].velocity = projectile.DirectionTo(Main.dust[num].position) * 6f;
				}
			});
			}
			else
			{
				ProjectileExtras.Explode(projectile.whoAmI, 120, 120,
				delegate
			{
				for (int i = 0; i < 80; i++)
				{
					int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("BlueMoonBlueDust"), 0f, -2f, 0, default(Color), 2f);
					Main.dust[num].noGravity = true;
					Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
					Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
					if (Main.dust[num].position != projectile.Center)
						Main.dust[num].velocity = projectile.DirectionTo(Main.dust[num].position) * 6f;
				}
			});
			}
		}
		bool looping = false;
		int loopCounter = 0;
		bool dustSpawn = true;
		public override void AI()
		{	
			Trail(projectile.Center, projectile.position - projectile.velocity, (float)projectile.ai[0]);
			dustSpawn = false;
			projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
			if (!looping) //change direction slightly
			{
				Vector2 currentSpeed = new Vector2(projectile.velocity.X, projectile.velocity.Y);
				projectile.velocity = currentSpeed.RotatedBy(Main.rand.Next(-1, 2) * (Math.PI / 40));
			}
			if (Main.rand.Next(90) == 1 && !looping)
			{
				loopCounter = 0;
				looping = true;
			}
			if (looping)
			{
				Vector2 currentSpeed = new Vector2(projectile.velocity.X, projectile.velocity.Y);
				projectile.velocity = currentSpeed.RotatedBy(Math.PI / 9);
				loopCounter++;
				if (loopCounter >=18)
				{
					looping = false;
				}
			}
		}
	}
}
