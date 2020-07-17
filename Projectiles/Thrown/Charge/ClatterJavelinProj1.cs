using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Projectiles.Thrown.Charge
{
	public class ClatterJavelinProj1 : ModProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Clatter Javelin Tracer");

		public override void SetDefaults()
		{
			projectile.width = 8;
			projectile.height = 36;
			projectile.aiStyle = 1;
			projectile.friendly = false;
			projectile.thrown = true;
			projectile.penetrate = -1;
			projectile.timeLeft = 600;
			projectile.extraUpdates = 16;
			projectile.ignoreWater = true;
			aiType = ProjectileID.ThrowingKnife;
		}

		public override bool PreAI()
		{
			Dust d = Dust.NewDustPerfect(projectile.Center - projectile.velocity / 5, 6, Vector2.Zero, 0, default, 1.3f);
			d.noGravity = true;
			d.noLight = true;
			return true;
		}
	}
}
