
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Held
{
	public class IchorImpalerProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ichor Impaler");
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.Trident);

			AIType = ProjectileID.Trident;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.NextBool(2))
				target.AddBuff(BuffID.Ichor, 420);
		}

	}
}
