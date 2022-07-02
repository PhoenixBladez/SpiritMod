using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria;

namespace SpiritMod.NPCs.StarjinxEvent.Enemies.Warden.Projectiles
{
	public class VoidCastProjectile : ModProjectile
	{
		internal int connectedWhoAmI = -1;

		protected int teleported = 0;

		public override void SetStaticDefaults() => DisplayName.SetDefault("aeaa");

		public override void SetDefaults()
		{
			Projectile.Size = new Vector2(48);
			Projectile.hostile = true;
			Projectile.timeLeft = 1000;
			Projectile.ignoreWater = true;
			Projectile.aiStyle = 1;

			AIType = ProjectileID.Bullet;
		}

		public override void AI()
		{

		}

		public override bool PreDraw(ref Color lightColor)
		{
			Projectile.QuickDraw(Main.spriteBatch);
			return false;
		}

		public override Color? GetAlpha(Color lightColor) => Color.White * Projectile.Opacity;
	}
}