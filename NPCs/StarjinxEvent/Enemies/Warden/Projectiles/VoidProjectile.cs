using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using System;
using Terraria.ID;

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
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			projectile.QuickDraw(spriteBatch);
			return false;
		}

		public override Color? GetAlpha(Color lightColor) => Color.White * projectile.Opacity;
	}
}