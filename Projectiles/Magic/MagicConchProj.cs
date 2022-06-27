using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

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
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.aiStyle = 27;
			Projectile.width = 120;
			Projectile.height = 120;
			Projectile.penetrate = 10;
			Projectile.alpha = 255;
			Projectile.timeLeft = 150;
		}
		float swirlSize = 1.664f;
		float degrees = 0;
		public override bool PreAI()
		{
			Projectile.tileCollide = false;
			if (Projectile.timeLeft == 450) {
				for (int i = 0; i < 110; i++) {
					int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.DungeonSpirit, 0f, 0f);
					Main.dust[dust].scale = 1.5f;
					Main.dust[dust].noGravity = true;
				}
			}

			float Closeness = 50f;
			degrees += 2.5f;
			for (float swirlDegrees = degrees; swirlDegrees < 160 + degrees; swirlDegrees += 7f) {
				Closeness -= swirlSize; //It closes in
				double radians = swirlDegrees * (Math.PI / 180); //convert to radians

				Vector2 eastPosFar = Projectile.Center + new Vector2(Closeness * (float)Math.Sin(radians), Closeness * (float)Math.Cos(radians));
				Vector2 westPosFar = Projectile.Center - new Vector2(Closeness * (float)Math.Sin(radians), Closeness * (float)Math.Cos(radians));
				Vector2 northPosFar = Projectile.Center + new Vector2(Closeness * (float)Math.Sin(radians + 1.57), Closeness * (float)Math.Cos(radians + 1.57));
				Vector2 southPosFar = Projectile.Center - new Vector2(Closeness * (float)Math.Sin(radians + 1.57), Closeness * (float)Math.Cos(radians + 1.57));
				int d4 = Dust.NewDust(eastPosFar, 2, 2, DustID.DungeonSpirit, 0, 0);
				Main.dust[d4].noGravity = true;
				int d5 = Dust.NewDust(westPosFar, 2, 2, DustID.DungeonSpirit, 0, 0);
				Main.dust[d5].noGravity = true;
				int d6 = Dust.NewDust(northPosFar, 2, 2, DustID.DungeonSpirit, 0, 0);
				Main.dust[d6].noGravity = true;
				int d7 = Dust.NewDust(southPosFar, 2, 2, DustID.DungeonSpirit, 0, 0);
				Main.dust[d7].noGravity = true;


				Vector2 eastPosClose = Projectile.Center + new Vector2((Closeness - 30f) * (float)Math.Sin(radians), (Closeness - 30f) * (float)Math.Cos(radians));
				Vector2 westPosClose = Projectile.Center - new Vector2((Closeness - 30f) * (float)Math.Sin(radians), (Closeness - 30f) * (float)Math.Cos(radians));
				Vector2 northPosClose = Projectile.Center + new Vector2((Closeness - 30f) * (float)Math.Sin(radians + 1.57), (Closeness - 30f) * (float)Math.Cos(radians + 1.57));
				Vector2 southPosClose = Projectile.Center - new Vector2((Closeness - 30f) * (float)Math.Sin(radians + 1.57), (Closeness - 30f) * (float)Math.Cos(radians + 1.57));
				int d = Dust.NewDust(eastPosClose, 2, 2, DustID.DungeonSpirit, 0, 0);
				Main.dust[d].noGravity = true;
				int d1 = Dust.NewDust(westPosClose, 2, 2, DustID.DungeonSpirit, 0, 0);
				Main.dust[d1].noGravity = true;
				int d2 = Dust.NewDust(northPosClose, 2, 2, DustID.DungeonSpirit, 0, 0);
				Main.dust[d2].noGravity = true;
				int d3 = Dust.NewDust(southPosClose, 2, 2, DustID.DungeonSpirit, 0, 0);
				Main.dust[d3].noGravity = true;
			}

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

					if (num416 > 1) {
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
