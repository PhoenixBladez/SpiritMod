using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.MoonWizard.Projectiles
{
	public class MoonPredictorTrail : ModProjectile
	{
		float distance = 8;
		int rotationalSpeed = 4;
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
			projectile.hostile = true;
			projectile.friendly = false;
			projectile.timeLeft = 10;
			projectile.damage = 13;
			//projectile.extraUpdates = 1;
			projectile.alpha = 255;
			projectile.width = projectile.height = 32;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 9;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;

		}
		bool initialized = false;
		Vector2 initialSpeed = Vector2.Zero;
		public override void AI()
		{
			Dust.NewDustPerfect(projectile.Center, 206);
		}
	}
}