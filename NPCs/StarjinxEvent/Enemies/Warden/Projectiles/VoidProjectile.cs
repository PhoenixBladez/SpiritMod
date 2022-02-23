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
			projectile.CloneDefaults(ProjectileID.BulletDeadeye);
			projectile.Size = new Vector2(26, 26);
			projectile.hostile = true;
			projectile.timeLeft = 5000;
			projectile.ignoreWater = true;
			projectile.aiStyle = 0;
		}

		public override void AI() => projectile.velocity = projectile.velocity;//.RotatedBy((Math.Sin(projectile.ai[0]++ * 0.1f)) * 0.05f);

		public override Color? GetAlpha(Color lightColor) => Color.White;
	}
}