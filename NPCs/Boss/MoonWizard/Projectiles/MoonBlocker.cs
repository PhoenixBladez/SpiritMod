using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.MoonWizard.Projectiles
{
	public class MoonBlocker : ModProjectile
	{
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Moon Blocker");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 3;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}
		public override void SetDefaults()
		{
			projectile.penetrate = 1;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.friendly = false;
			projectile.timeLeft = 600;
			projectile.damage = 13;
			//projectile.extraUpdates = 1;
			projectile.width = projectile.height = 32;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 9;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;

		}
		bool initialized = false;
		Vector2 initialPos = Vector2.Zero;
		Vector2 throwLine = Vector2.Zero;
		int dist = 500;
		float radians = 0f;
        public override void AI()
        {
            if (!initialized) 
			{
				initialPos = projectile.Center;
				initialized = true;
				throwLine = Vector2.One.RotatedBy(Main.rand.NextFloat(6.28f));
			}
			if (projectile.timeLeft < 585 && projectile.timeLeft % 3 == 0 && projectile.timeLeft > 500) 
			{
				Projectile.NewProjectile(projectile.Center, throwLine * (dist / 20), mod.ProjectileType("MoonPredictorTrail"), 0, 0);
			}
			if (projectile.timeLeft <= 500 && projectile.timeLeft > 464) {
				radians += 0.0872664626f;
				projectile.velocity = (float)Math.Sin((double)radians) * (dist / 36) * throwLine;
			}
			if (projectile.timeLeft <= 500) 
			{
				projectile.hostile = true;
				Vector2 distance = initialPos - projectile.Center;
				float electricDist = distance.Length();
				distance.Normalize();
				if (projectile.timeLeft % 4 == 0) 
				{
					DustHelper.DrawElectricity(projectile.Center, initialPos, 226, 0.6f, 30, default, 0.3f);
					int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, distance.X * 30, distance.Y * 30, mod.ProjectileType("MoonLightning"), 30, 0);
					Main.projectile[proj].timeLeft = (int)(electricDist / 30);
				}
			}
		}
        public override Color? GetAlpha(Color lightColor) => Color.White;
    }
}