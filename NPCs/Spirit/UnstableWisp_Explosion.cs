using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Spirit
{
	public class UnstableWisp_Explosion : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Unstable Wisp");
			Main.projFrames[base.Projectile.type] = 9;
		}

		public override void SetDefaults()
		{
			Projectile.width = 124;
			Projectile.height = 106;
			Projectile.penetrate = -1;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.hostile = true;
			Projectile.friendly = false;
		}

		public override bool PreAI()
		{
			if (Projectile.ai[0] == 0f) {
				Projectile.Damage();
				Projectile.ai[0] = 1f;
			}

			Projectile.frameCounter++;
			if (Projectile.frameCounter > 3) {
				Projectile.frameCounter = 0;
				Projectile.frame++;
				if (Projectile.frame > Main.projFrames[Projectile.type]) {
					Projectile.Kill();
				}
			}
			return false;
		}

	}
}
