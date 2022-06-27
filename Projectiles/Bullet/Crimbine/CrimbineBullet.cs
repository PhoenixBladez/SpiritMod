using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Bullet.Crimbine
{
	public class CrimbineBone : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Savage Bone");
		}

		public override void SetDefaults()
		{
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 300;
			Projectile.height = 12;
			Projectile.width = 3;
			AIType = ProjectileID.Bullet;

		}

		public override void AI()
		{


			Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 1.57f;
			Projectile.ai[1] += 1f;
			if (Projectile.ai[1] >= 7200f)
			{
				Projectile.alpha += 5;
				if (Projectile.alpha > 255)
				{
					Projectile.alpha = 255;
					Projectile.Kill();
				}
			}

			Projectile.localAI[0] += 1f;
			if (Projectile.localAI[0] >= 10f)
			{
				Projectile.localAI[0] = 0f;
				int num416 = 0;
				int num417 = 0;
				float num418 = 0f;
				int num419 = Projectile.type;
				for (int num420 = 0; num420 < 1000; num420++)
				{
					if (Main.projectile[num420].active && Main.projectile[num420].owner == Projectile.owner && Main.projectile[num420].type == num419 && Main.projectile[num420].ai[1] < 3600f)
					{
						num416++;
						if (Main.projectile[num420].ai[1] > num418)
						{
							num417 = num420;
							num418 = Main.projectile[num420].ai[1];
						}
					}
				}
				if (num416 > 22)
				{
					Main.projectile[num417].netUpdate = true;
					Main.projectile[num417].ai[1] = 36000f;
					return;
				}
			}
			int num = 5;
			for (int k = 0; k < 3; k++)
			{
				int index2 = Dust.NewDust(Projectile.position, 1, 1, DustID.Blood, 0.0f, 0.0f, 0, new Color(), 1f);
				Main.dust[index2].position = Projectile.Center - Projectile.velocity / num * (float)k;
				Main.dust[index2].scale = .5f;
				Main.dust[index2].velocity *= 0f;
				Main.dust[index2].noGravity = true;
				Main.dust[index2].noLight = false;
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			for (int k = 0; k < 6; k++)
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Dirt, 2.5f * 1, -2.5f, 0, Color.White, 0.7f);
			return true;
		}

		public override void Kill(int timeLeft) => SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
	}
}