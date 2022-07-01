
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Held
{
	public class FearsomeFork : ModProjectile
	{
		int timer = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fearsome Fork");
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.Trident);
			AIType = ProjectileID.Trident;
		}
		public override void AI()
		{
			timer++;
			if (timer % 7 == 1) {
				int newProj = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.position, new Vector2(0, 0), ModContent.ProjectileType<Pumpkin>(), Projectile.damage, 0, Projectile.owner);
				Main.projectile[newProj].magic = false;
				Main.projectile[newProj].melee = true;
			}
		}

	}
}
