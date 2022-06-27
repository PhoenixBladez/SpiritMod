using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.ID;

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
			Projectile.width = 4;
			Projectile.height = 4;
			Projectile.hide = true;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.aiStyle = 0;
			Projectile.localNPCHitCooldown = 240;
			Projectile.penetrate = 2;
			Projectile.minion = true;
			Projectile.MaxUpdates = 15;
			Projectile.timeLeft = 66;
			Projectile.tileCollide = false;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) => SoundEngine.PlaySound(SoundID.Trackable, (int)target.position.X, (int)target.position.Y, 20, 1f, 0.0f);

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			Projectile.localAI[0] += 1f;
			
			if (Projectile.localAI[0] > -1f)
				x = Projectile.Center.Y + 50;

			if (Projectile.localAI[0] > -1f)
            {
				for (int i = 0; i < 10; i++)
				{
					float PosX = Projectile.Center.X - Projectile.velocity.X / 10f * i;
					float PosY = Projectile.Center.Y - Projectile.velocity.Y / 10f * i;
					
					if (Vector2.Distance(player.Center, Projectile.Center) > 20f)
					{
						int dustIndex = Dust.NewDust(new Vector2(PosX, PosY), 0, 0, DustID.Frost, 0f, 0f, 180, default, 0.5f);
						
						Main.dust[dustIndex].position.X = PosX;
						Main.dust[dustIndex].position.Y = PosY;
						
						Main.dust[dustIndex].velocity *= 0f;
						Main.dust[dustIndex].noGravity = true;
					}
				}
            }
			
			if (Vector2.Distance(Projectile.Center, Main.projectile[(int)Projectile.ai[0]].Center) <= 20f)
			{
				Projectile.Kill();
			}
			
			Vector2 vector2_1 = new Vector2(Main.projectile[(int)Projectile.ai[0]].Center.X, Main.projectile[(int)Projectile.ai[0]].Center.Y);
			float speed = 16f;
			float dX = vector2_1.X - Projectile.Center.X;
			float dY = vector2_1.Y - Projectile.Center.Y;
			
			float dist = (float)Math.Sqrt((dX * dX + dY * dY));
			
			speed /= dist;
			
			Vector2 randomSpeed = new Vector2(dX, dY).RotatedByRandom(MathHelper.ToRadians(-90));
			
			if (Projectile.localAI[0] > 1f)
				Projectile.velocity = new Vector2(randomSpeed.X * speed, randomSpeed.Y * speed);
		}
	}
}