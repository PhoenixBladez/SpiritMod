using Microsoft.Xna.Framework;
using SpiritMod.YoYoOverload.Projectiles;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.YoYoOverload.Items
{
	public class CreepP : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[projectile.type] = 1;
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.CrimsonYoyo);
			projectile.damage = 18;
			projectile.extraUpdates = 1;
			aiType = ProjectileID.CrimsonYoyo;
            projectile.penetrate = 5;
		}

		public override void PostAI()
		{
			projectile.rotation -= 10f;
		}

		public override void AI()
		{
			projectile.frameCounter++;
			if (projectile.frameCounter >= 100)
			{
				projectile.frameCounter = 0;
				float num = (float)(Main.rand.Next(0, 100) * 0.052359877559829883);
				Vector2 vector = new Vector2((float)Math.Cos(num), (float)Math.Sin(num));
				int num2 = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vector.X, vector.Y, ModContent.ProjectileType<CreepT>(), projectile.damage, projectile.owner, 0, 0f, 0f);
				Main.projectile[num2].friendly = true;
				Main.projectile[num2].hostile = false;
				Main.projectile[num2].velocity *= 7f;
			}
		}
	}
}
