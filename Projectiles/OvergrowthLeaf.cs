using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SpiritMod.Projectiles
{
	public class OvergrowthLeaf : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Overgrowth Leaf");
			Main.projFrames[Projectile.type] = 5;
		}

		public override void SetDefaults()
		{
			Projectile.width = 20;
			Projectile.height = 16;
			Projectile.aiStyle = 43;
			AIType = 227;
			Projectile.friendly = true;
			Projectile.ignoreWater = true;
			Projectile.minion = true;
			Projectile.minionSlots = 0;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 180;
		}

		public override void AI()
		{
			Projectile.frameCounter++;
			if (Projectile.frameCounter > 8) {
				Projectile.frame++;
				Projectile.frameCounter = 0;
			}
			if (Projectile.frame > 4) {
				Projectile.frame = 0;
			}
			Lighting.AddLight(Projectile.Center, ((255 - Projectile.alpha) * 0.025f) / 255f, ((255 - Projectile.alpha) * 0.25f) / 255f, ((255 - Projectile.alpha) * 0.05f) / 255f);
			Projectile.velocity.Y += Projectile.ai[0];
			if (Main.rand.NextBool(8))
				Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.GrassBlades, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.penetrate--;
			if (Projectile.penetrate <= 0)
				Projectile.Kill();
			else {
				Projectile.ai[0] += 0.1f;
				if (Projectile.velocity.X != oldVelocity.X)
					Projectile.velocity.X = -oldVelocity.X;

				if (Projectile.velocity.Y != oldVelocity.Y)
					Projectile.velocity.Y = -oldVelocity.Y;
			}
			return false;
		}

		public override void Kill(int timeLeft)
		{
			for (int k = 0; k < 3; k++) {
				Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.GrassBlades, Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f);
			}
		}
	}
}