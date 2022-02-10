using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using System;

namespace SpiritMod.NPCs.StarjinxEvent.Enemies.Warden.Projectiles
{
	public class VoidPortal : ModProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Void Port");

		public override void SetDefaults()
		{
			projectile.Size = new Vector2(50, 120);
			projectile.hostile = true;
			projectile.timeLeft = 60;
			projectile.ignoreWater = true;
		}

		public override void AI() => projectile.velocity.Y = (float)Math.Sin(projectile.ai[0]++ * 0.005f) * 0.02f;

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			projectile.QuickDraw(spriteBatch);
			return false;
		}

		public override Color? GetAlpha(Color lightColor) => Color.White * projectile.Opacity;
	}
}