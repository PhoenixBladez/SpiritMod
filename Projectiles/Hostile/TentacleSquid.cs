using Microsoft.Xna.Framework;
using SpiritMod.Dusts;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Hostile
{
	public class TentacleSquid : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("R'ylheian");
			Main.projFrames[base.projectile.type] = 4;
		}

		public override void SetDefaults()
		{
			projectile.width = 30;
			projectile.height = 30;

			projectile.damage = 3;
			projectile.friendly = false;
			projectile.hostile = true;
			projectile.tileCollide = true;
			projectile.alpha = 0;
		}
		public override void AI()
		{
			projectile.rotation = projectile.velocity.ToRotation() - 3.14f;
			projectile.frameCounter++;
			if(projectile.frameCounter >= 6) {
				projectile.frame = (projectile.frame + 1) % Main.projFrames[projectile.type];
				projectile.frameCounter = 0;
			}
		}
	}
}
