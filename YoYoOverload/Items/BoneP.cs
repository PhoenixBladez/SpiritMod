using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.YoYoOverload.Items
{
	public class BoneP : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[projectile.type] = 1;
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.Valor);
			projectile.damage = 24;
			projectile.extraUpdates = 1;
			aiType = ProjectileID.Valor;
		}

		public override void PostAI()
		{
			projectile.rotation -= 10f;
		}

		public override void AI()
		{
			projectile.frameCounter++;
			if(projectile.frameCounter >= 160) {
				projectile.frameCounter = 0;
				float num = (float)(Main.rand.Next(0, 361) * 0.017453292519943295);
				Vector2 vector = new Vector2((float)Math.Cos(num), (float)Math.Sin(num));
				int num2 = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vector.X, vector.Y, ProjectileID.BoneGloveProj, projectile.damage, projectile.owner, 0, 0f, 0f);
				Main.projectile[num2].friendly = true;
				Main.projectile[num2].hostile = false;
				Main.projectile[num2].velocity *= 7f;
			}
		}
	}
}
