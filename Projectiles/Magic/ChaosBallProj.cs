using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class ChaosBallProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chaos Ball");
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.AmethystBolt);
			projectile.damage = 15;
			projectile.extraUpdates = 1;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[projectile.type] = 5;
			aiType = ProjectileID.AmethystBolt;
			Main.projFrames[projectile.type] = 1;
			projectile.penetrate = -1;
		}

		public override void AI()
		{
			Dust.NewDust(projectile.position, projectile.width, projectile.height, 27);
		}

	}
}
