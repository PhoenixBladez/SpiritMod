using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using System;
using Terraria.ID;
using Terraria;

namespace SpiritMod.NPCs.StarjinxEvent.Enemies.Warden.Projectiles
{
	public class VoidProjectile : ModProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Void Haze");

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.BulletDeadeye);
			projectile.Size = new Vector2(26, 26);
			projectile.hostile = true;
			projectile.timeLeft = 5000;
			projectile.ignoreWater = true;

			aiType = ProjectileID.BulletDeadeye;
		}

		public override void AI() => projectile.velocity = projectile.velocity.RotatedBy(Math.Sin(projectile.ai[0]++ * 1.2f) * 0.01f);

		public override Color? GetAlpha(Color lightColor) => Color.White;
	}
}