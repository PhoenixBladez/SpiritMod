using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria.ID;

namespace SpiritMod.NPCs.StarjinxEvent.Enemies.Warden.Projectiles
{
	public class VoidProjectile : ModProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Void Haze");

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.BulletDeadeye);
			Projectile.Size = new Vector2(26, 26);
			Projectile.hostile = true;
			Projectile.timeLeft = 5000;
			Projectile.ignoreWater = true;
			Projectile.aiStyle = 0;
		}

		public override void AI() => Projectile.velocity = Projectile.velocity;//.RotatedBy((Math.Sin(projectile.ai[0]++ * 0.1f)) * 0.05f);

		public override Color? GetAlpha(Color lightColor) => Color.White;
	}
}