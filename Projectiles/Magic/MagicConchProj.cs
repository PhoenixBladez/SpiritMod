using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class MagicConchProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Whirlpool");
		}

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.magic = true;
			projectile.aiStyle = 27;
			projectile.width = 120;
			projectile.height = 120;
			projectile.penetrate = 10;
			projectile.alpha = 255;
			projectile.timeLeft = 150;
		}
		float swirlSize = 1.664f;
		float degrees = 0;
		public override bool PreAI()
		{
			projectile.tileCollide = false;
			if(projectile.timeLeft == 450) {
				for(int i = 0; i < 110; i++) {
					int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 180, 0f, 0f);
					Main.dust[dust].scale = 1.5f;
					Main.dust[dust].noGravity = true;
				}
			}


			float Closeness = 50f;
			degrees += 2.5f;
			for(float swirlDegrees = degrees; swirlDegrees < 160 + degrees; swirlDegrees += 7f) {
				Closeness -= swirlSize; //It closes in
				double radians = swirlDegrees * (Math.PI / 180); //convert to radians

				Vector2 eastPosFar = projectile.Center + new Vector2(Closeness * (float)Math.Sin(radians), Closeness * (float)Math.Cos(radians));
				Vector2 westPosFar = projectile.Center - new Vector2(Closeness * (float)Math.Sin(radians), Closeness * (float)Math.Cos(radians));
				Vector2 northPosFar = projectile.Center + new Vector2(Closeness * (float)Math.Sin(radians + 1.57), Closeness * (float)Math.Cos(radians + 1.57));
				Vector2 southPosFar = projectile.Center - new Vector2(Closeness * (float)Math.Sin(radians + 1.57), Closeness * (float)Math.Cos(radians + 1.57));
				int d4 = Dust.NewDust(eastPosFar, 2, 2, 180);
				Main.dust[d4].noGravity = true;
				int d5 = Dust.NewDust(westPosFar, 2, 2, 180);
				Main.dust[d5].noGravity = true;
				int d6 = Dust.NewDust(northPosFar, 2, 2, 180);
				Main.dust[d6].noGravity = true;
				int d7 = Dust.NewDust(southPosFar, 2, 2, 180);
				Main.dust[d7].noGravity = true;


				Vector2 eastPosClose = projectile.Center + new Vector2((Closeness - 30f) * (float)Math.Sin(radians), (Closeness - 30f) * (float)Math.Cos(radians));
				Vector2 westPosClose = projectile.Center - new Vector2((Closeness - 30f) * (float)Math.Sin(radians), (Closeness - 30f) * (float)Math.Cos(radians));
				Vector2 northPosClose = projectile.Center + new Vector2((Closeness - 30f) * (float)Math.Sin(radians + 1.57), (Closeness - 30f) * (float)Math.Cos(radians + 1.57));
				Vector2 southPosClose = projectile.Center - new Vector2((Closeness - 30f) * (float)Math.Sin(radians + 1.57), (Closeness - 30f) * (float)Math.Cos(radians + 1.57));
				int d = Dust.NewDust(eastPosClose, 2, 2, 180);
				Main.dust[d].noGravity = true;
				int d1 = Dust.NewDust(westPosClose, 2, 2, 180);
				Main.dust[d1].noGravity = true;
				int d2 = Dust.NewDust(northPosClose, 2, 2, 180);
				Main.dust[d2].noGravity = true;
				int d3 = Dust.NewDust(southPosClose, 2, 2, 180);
				Main.dust[d3].noGravity = true;
			}

			projectile.ai[1] += 1f;
			if(projectile.ai[1] >= 7200f) {
				projectile.alpha += 5;
				if(projectile.alpha > 255) {
					projectile.alpha = 255;
					projectile.Kill();
				}
			}
			projectile.localAI[0] += 1f;
			if(projectile.localAI[0] >= 10f) {
				projectile.localAI[0] = 0f;
				int num416 = 0;
				int num417 = 0;
				float num418 = 0f;
				int num419 = projectile.type;
				for(int num420 = 0; num420 < 1000; num420++) {
					if(Main.projectile[num420].active && Main.projectile[num420].owner == projectile.owner && Main.projectile[num420].type == num419 && Main.projectile[num420].ai[1] < 3600f) {
						num416++;
						if(Main.projectile[num420].ai[1] > num418) {
							num417 = num420;
							num418 = Main.projectile[num420].ai[1];
						}
					}

					if(num416 > 1) {
						Main.projectile[num417].netUpdate = true;
						Main.projectile[num417].ai[1] = 36000f;
						return false;
					}
				}
			}

			return false;
		}

	}
}
