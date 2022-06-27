using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Shockhopper
{
	public class HopperLaser : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Deepspace Hopper");
		}

		public override void SetDefaults()
		{
			Projectile.hostile = true;
			Projectile.width = 2;
			Projectile.height = 2;
			Projectile.aiStyle = -1;
			Projectile.friendly = false;
			Projectile.penetrate = 8;
			Projectile.alpha = 255;
			Projectile.timeLeft = 16;
			Projectile.tileCollide = true;
			Projectile.extraUpdates = 2;
		}


		private void Trail(Vector2 from, Vector2 to)
		{
			float distance = Vector2.Distance(from, to);
			float step = 1 / distance;
			for (float w = 0; w < distance; w += 4) {
				Dust.NewDustPerfect(Vector2.Lerp(from, to, w * step), 226, Vector2.Zero).noGravity = true;
			}
		}
		public override bool PreAI()
		{
			Trail(Projectile.position, Projectile.position + Projectile.velocity);
			return true;
		}

	}
}