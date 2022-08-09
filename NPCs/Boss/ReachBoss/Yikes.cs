using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.ReachBoss
{
	public class Yikes : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spore Spire");
		}

		public override void SetDefaults()
		{
			Projectile.hostile = true;
			Projectile.height = 30;
			Projectile.width = 34;
			Projectile.friendly = false;
			Projectile.aiStyle = 1;
			AIType = ProjectileID.Bullet;
			Projectile.timeLeft = 500;
			Projectile.tileCollide = false;
		}

		public override void AI()
		{
			Projectile.velocity *= 0.97f;
			Projectile.ai[1] += 1f;
			if (Projectile.ai[1] >= 7200f) {
				Projectile.alpha += 5;
				if (Projectile.alpha > 255) {
					Projectile.alpha = 255;
					Projectile.Kill();
				}
			}

			Projectile.localAI[0] += 1f;
			if (Projectile.localAI[0] >= 10f) {
				Projectile.localAI[0] = 0f;
				int num416 = 0;
				int num417 = 0;
				float num418 = 0f;
				int num419 = Projectile.type;
				for (int num420 = 0; num420 < 1000; num420++) {
					if (Main.projectile[num420].active && Main.projectile[num420].owner == Projectile.owner && Main.projectile[num420].type == num419 && Main.projectile[num420].ai[1] < 3600f) {
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
			SoundEngine.PlaySound(SoundID.NPCHit3 with { PitchVariance = 0.4f }, Projectile.Center);
			Vector2 spinningpoint1 = ((float)Main.rand.NextDouble() * 6.283185f).ToRotationVector2();
			Vector2 spinningpoint2 = spinningpoint1;
			float dagada = (float)(Main.rand.Next(3, 6) * 2);
			int num2 = 5;
			float num3 = Main.rand.NextBool(2) ? 1f : -1f;
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
				int index2 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y), 6, 6, DustID.PoisonStaff, 0.0f, 0.0f, 100, new Color(), 1.4f);
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