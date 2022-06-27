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
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}
		public override void SetDefaults()
		{
			Projectile.penetrate = 1;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.friendly = false;
			Projectile.timeLeft = 600;
			Projectile.damage = 13;
			//projectile.extraUpdates = 1;
			Projectile.width = Projectile.height = 32;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 9;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;

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
				initialPos = Projectile.Center;
				initialized = true;
				throwLine = Vector2.One.RotatedBy(Main.rand.NextFloat(6.28f));
			}
			if (Projectile.timeLeft < 585 && Projectile.timeLeft % 3 == 0 && Projectile.timeLeft > 500) 
			{
				Projectile.NewProjectile(Projectile.Center, throwLine * (dist / 20), ModContent.ProjectileType<MoonPredictorTrail>(), 0, 0);
			}
			if (Projectile.timeLeft <= 500 && Projectile.timeLeft > 464) {
				radians += 0.0872664626f;
				Projectile.velocity = (float)Math.Sin((double)radians) * (dist / 36) * throwLine;
			}
			if (Projectile.timeLeft <= 500) 
			{
				Projectile.hostile = true;
				Vector2 distance = initialPos - Projectile.Center;
				float electricDist = distance.Length();
				distance.Normalize();
				if (Projectile.timeLeft % 4 == 0) 
				{
					DustHelper.DrawElectricity(Projectile.Center, initialPos, 226, 0.6f, 30, default, 0.3f);
					int proj = Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, distance.X * 30, distance.Y * 30, ModContent.ProjectileType<MoonLightning>(), 30, 0);
					Main.projectile[proj].timeLeft = (int)(electricDist / 30);
				}
			}
		}
        public override Color? GetAlpha(Color lightColor) => Color.White;
    }
}