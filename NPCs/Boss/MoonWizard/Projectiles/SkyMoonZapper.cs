using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.MoonWizard.Projectiles
{
	public class SkyMoonZapper : ModProjectile
	{
		float distance = 8;
		int rotationalSpeed = 4;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Moon Zapper");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 3;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}
		public override void SetDefaults()
		{
			projectile.penetrate = 1;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.friendly = false;
			projectile.timeLeft = 100;
			projectile.damage = 13;
			//projectile.extraUpdates = 1;
			projectile.width = projectile.height = 32;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 9;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;

		}
		public override void AI()
		{
			int proj = 0;
			if (projectile.timeLeft > 20 && projectile.timeLeft % 10 == 0) {
				proj = Projectile.NewProjectile(projectile.Center, new Vector2(0, 25), mod.ProjectileType("MoonPredictorTrail"), 0, 0);
				Main.projectile[proj].timeLeft = 50;
			}
			if (projectile.timeLeft == 10)
			{
				Main.PlaySound(2, projectile.position, 122);
				DustHelper.DrawElectricity(projectile.Center, projectile.Center + new Vector2(0, 1000), 226, 1.5f, 30, default, 0.3f);
				Projectile.NewProjectile(projectile.Center + new Vector2(0, 500), Vector2.Zero, mod.ProjectileType("MoonThunder"), projectile.damage, 0);
			}
		}
	}
}