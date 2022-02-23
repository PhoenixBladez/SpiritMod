using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using Terraria.ID;

namespace SpiritMod.NPCs.StarjinxEvent.Enemies.Warden.Projectiles
{
	public class VoidCastProjectile : ModProjectile
	{
		internal int connectedWhoAmI = -1;

		protected int teleported = 0;

		public override void SetStaticDefaults() => DisplayName.SetDefault("aeaa");

		public override void SetDefaults()
		{
			projectile.Size = new Vector2(48);
			projectile.hostile = true;
			projectile.timeLeft = 1000;
			projectile.ignoreWater = true;
			projectile.aiStyle = 1;

			aiType = ProjectileID.Bullet;
		}

		public override void AI()
		{

		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			projectile.QuickDraw(spriteBatch);
			return false;
		}

		public override Color? GetAlpha(Color lightColor) => Color.White * projectile.Opacity;
	}
}