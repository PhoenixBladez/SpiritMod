using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.ID;

namespace SpiritMod.Items.Accessory.UnstableTeslaCoil
{
	public class Unstable_Tesla_Coil_Projectile : ModProjectile
	{
		public float x = 0f;

		public override void SetStaticDefaults() => DisplayName.SetDefault("Lightning Zap");

		public override void SetDefaults()
		{
			Projectile.width = 4;
			Projectile.height = 4;
			Projectile.hide = true;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.MaxUpdates = 15;
			Projectile.timeLeft = 66;
			Projectile.tileCollide = false;
		}

		public override void AI()
		{
			Projectile.localAI[0] += 1f;
			
			if (Projectile.localAI[0] > -1f)
				x = Projectile.Center.Y + 50;

			if (Projectile.localAI[0] > -1f)
            {
				for (int i = 0; i < 10; i++)
				{
					float PosX = Projectile.Center.X - Projectile.velocity.X / 10f * (float)i;
					float PosY = Projectile.Center.Y - Projectile.velocity.Y / 10f * (float)i;
					
					int dustIndex = Dust.NewDust(new Vector2(PosX, PosY), 0, 0, DustID.Electric, 0f, 0f, 180, default, 0.5f);
					
					Main.dust[dustIndex].position.X = PosX;
					Main.dust[dustIndex].position.Y = PosY;
					
					Main.dust[dustIndex].velocity *= 0f;
					Main.dust[dustIndex].noGravity = true;
				}
            }
			
			Vector2 destination = new Vector2(Projectile.ai[0], Projectile.ai[1]);
			Vector2 dif = destination - Projectile.Center;
			float speed = 16f / dif.Length();

			Vector2 randomSpeed = new Vector2(dif.X, dif.Y).RotatedByRandom(MathHelper.ToRadians(90));
			
			if (Projectile.localAI[0] > 1f)
				Projectile.velocity = new Vector2(randomSpeed.X * speed, randomSpeed.Y * speed);
		}

		public override void Kill(int timeLeft)
		{
			int num = 22;
			for (int index1 = 0; index1 < num; ++index1)
			{
				int index2 = Dust.NewDust(Projectile.Center, 0, 0, DustID.FireworkFountain_Yellow, 0.0f, 0.0f, 0, new Color(), 0.75f);
				Main.dust[index2].velocity *= 0.3f;
				--Main.dust[index2].velocity.Y;
				Main.dust[index2].position = Vector2.Lerp(Main.dust[index2].position, Projectile.Center, 0.75f);
			}
			SoundEngine.PlaySound(SoundID.DD2_LightningAuraZap, Projectile.Center);
		}
	}
}