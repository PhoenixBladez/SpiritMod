using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Boss
{
	public class Yikes : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spore Spire");
		}

		public override void SetDefaults()
		{
			projectile.hostile = true;
			projectile.height = 30;
			projectile.width = 34;
			projectile.friendly = false;
			projectile.aiStyle = 1;
			aiType = ProjectileID.Bullet;
			projectile.timeLeft = 500;
			projectile.tileCollide = false;
		}

		public override void AI()
		{
			projectile.velocity *= 0.97f;
			projectile.ai[1] += 1f;
			if (projectile.ai[1] >= 7200f) {
				projectile.alpha += 5;
				if (projectile.alpha > 255) {
					projectile.alpha = 255;
					projectile.Kill();
				}
			}

			projectile.localAI[0] += 1f;
			if (projectile.localAI[0] >= 10f) {
				projectile.localAI[0] = 0f;
				int num416 = 0;
				int num417 = 0;
				float num418 = 0f;
				int num419 = projectile.type;
				for (int num420 = 0; num420 < 1000; num420++) {
					if (Main.projectile[num420].active && Main.projectile[num420].owner == projectile.owner && Main.projectile[num420].type == num419 && Main.projectile[num420].ai[1] < 3600f) {
						num416++;
						if (Main.projectile[num420].ai[1] > num418) {
							num417 = num420;
							num418 = Main.projectile[num420].ai[1];
						}
					}
				}
				if (num416 > 20) {
					Main.projectile[num417].netUpdate = true;
					Main.projectile[num417].ai[1] = 36000f;
					return;
				}
			}
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(new LegacySoundStyle(3, 3).WithPitchVariance(0.4f), projectile.Center);
			Vector2 spinningpoint1 = ((float)Main.rand.NextDouble() * 6.283185f).ToRotationVector2();
			Vector2 spinningpoint2 = spinningpoint1;
			float dagada = (float)(Main.rand.Next(3, 6) * 2);
			int num2 = 5;
			float num3 = Main.rand.Next(2) == 0 ? 1f : -1f;
			bool flag = true;
			for (int index1 = 0; (double)index1 < (double)num2 * (double)dagada; ++index1)
			{
				if (index1 % num2 == 0)
				{
					spinningpoint2 = spinningpoint2.RotatedBy((double)num3 * (6.28318548202515 / (double)dagada), new Vector2());
					spinningpoint1 = spinningpoint2;
					flag = !flag;
				}
				else
				{
					float num4 = 6.283185f / ((float)num2 * dagada);
					spinningpoint1 = spinningpoint1.RotatedBy((double)num4 * (double)num3 * 3.0, new Vector2());
				}
				float adada = MathHelper.Lerp(1f, 4f, (float)(index1 % num2) / (float)num2);
				int index2 = Dust.NewDust(new Vector2(projectile.Center.X, projectile.Center.Y), 6, 6, 163, 0.0f, 0.0f, 100, new Color(), 1.4f);
				Main.dust[index2].velocity *= 0.1f;
				Main.dust[index2].velocity += spinningpoint1 * adada;
				if (flag)	
				{
					Main.dust[index2].scale = 0.9f;
				}	
					Main.dust[index2].noGravity = true;
			}		
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(92, 217, 61, 100);
		}
		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.Next(15) == 1)
				target.AddBuff(BuffID.Poisoned, 200);
		}

	}
}