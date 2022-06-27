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
			Projectile.width = 8;
			Projectile.height = 36;
			Projectile.aiStyle = 1;
			Projectile.friendly = false;
			Projectile.DamageType = DamageClass.Throwing;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 600;
			Projectile.extraUpdates = 16;
			Projectile.ignoreWater = true;
			AIType = ProjectileID.ThrowingKnife;
		}

		public override bool PreAI()
		{
			if (Projectile.owner != Main.myPlayer) return true;
			Dust d = Dust.NewDustPerfect(Projectile.Center - Projectile.velocity / 5, 6, Vector2.Zero, 0, default, 1.3f);
			d.noGravity = true;
			d.noLight = true;
			return true;
		}
	}
}
