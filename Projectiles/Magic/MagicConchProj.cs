using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
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
			projectile.penetrate = 4;
			projectile.alpha = 255;
			projectile.timeLeft = 450;
		}
		float swirlSize = 1.664f;
		float degrees = 0;
		public override bool PreAI()
		{
			projectile.tileCollide = false;
			if (projectile.timeLeft == 450)
			{
				for (int i = 0; i < 160; i++)	
				{
					int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 15, 0f, 0f);
					Main.dust[dust].scale = 1.5f;
					Main.dust[dust].noGravity = true;
				}
			}
			
			
			float Closeness = 60f;
			degrees+=2.5f;
			for (float swirlDegrees = degrees; swirlDegrees < 180 + degrees; swirlDegrees+=10f)
			{
			Closeness -= swirlSize; //It closes in
			double radians = swirlDegrees * (Math.PI / 180); //convert to radians
			
			Vector2 eastPosFar = projectile.Center + new Vector2(Closeness * (float)Math.Sin(radians), Closeness * (float)Math.Cos(radians));
			Vector2 westPosFar = projectile.Center - new Vector2(Closeness * (float)Math.Sin(radians), Closeness * (float)Math.Cos(radians));				
			Vector2 northPosFar = projectile.Center + new Vector2(Closeness * (float)Math.Sin(radians + 1.57), Closeness * (float)Math.Cos(radians + 1.57));
			Vector2 southPosFar = projectile.Center - new Vector2(Closeness * (float)Math.Sin(radians + 1.57), Closeness * (float)Math.Cos(radians + 1.57));
			Dust.NewDustPerfect(eastPosFar, mod.DustType("ConchDust"), Vector2.Zero);
			Dust.NewDustPerfect(westPosFar, mod.DustType("ConchDust"), Vector2.Zero);
			Dust.NewDustPerfect(northPosFar, mod.DustType("ConchDust"), Vector2.Zero);
			Dust.NewDustPerfect(southPosFar, mod.DustType("ConchDust"), Vector2.Zero);
	

			Vector2 eastPosClose = projectile.Center + new Vector2((Closeness - 30f) * (float)Math.Sin(radians), (Closeness - 30f) * (float)Math.Cos(radians));
			Vector2 westPosClose = projectile.Center - new Vector2((Closeness - 30f) * (float)Math.Sin(radians), (Closeness - 30f) * (float)Math.Cos(radians));
			Vector2 northPosClose = projectile.Center + new Vector2((Closeness - 30f) * (float)Math.Sin(radians + 1.57), (Closeness - 30f) * (float)Math.Cos(radians + 1.57));
			Vector2 southPosClose = projectile.Center - new Vector2((Closeness - 30f) * (float)Math.Sin(radians + 1.57), (Closeness - 30f) * (float)Math.Cos(radians + 1.57));		
			Dust.NewDustPerfect(eastPosClose, mod.DustType("ConchDust"), Vector2.Zero);
			Dust.NewDustPerfect(westPosClose, mod.DustType("ConchDust"), Vector2.Zero);
			Dust.NewDustPerfect(northPosClose, mod.DustType("ConchDust"), Vector2.Zero);
			Dust.NewDustPerfect(southPosClose, mod.DustType("ConchDust"), Vector2.Zero);
			
			
			
			
			Dust.NewDustPerfect(projectile.Center, mod.DustType("ConchDust"), Vector2.Zero);
			}
			
			projectile.ai[1] += 1f;
			if (projectile.ai[1] >= 7200f)
			{
				projectile.alpha += 5;
				if (projectile.alpha > 255)
				{
					projectile.alpha = 255;
					projectile.Kill();
				}
			}
			Lighting.AddLight((int)(projectile.position.X / 16f), (int)(projectile.position.Y / 16f), 0.196f, 0.870588235f, 0.964705882f);
			projectile.localAI[0] += 1f;
			if (projectile.localAI[0] >= 10f)
			{
				projectile.localAI[0] = 0f;
				int num416 = 0;
				int num417 = 0;
				float num418 = 0f;
				int num419 = projectile.type;
				for (int num420 = 0; num420 < 1000; num420++)
				{
					if (Main.projectile[num420].active && Main.projectile[num420].owner == projectile.owner && Main.projectile[num420].type == num419 && Main.projectile[num420].ai[1] < 3600f)
					{
						num416++;
						if (Main.projectile[num420].ai[1] > num418)
						{
							num417 = num420;
							num418 = Main.projectile[num420].ai[1];
						}
					}

					if (num416 > 2)
					{
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
