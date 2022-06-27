using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.GoblinGrenadier
{
	public class GoblinGrenadierGrenade : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Grenade");
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.Grenade);
			AIType = ProjectileID.Grenade;
			Projectile.hostile = true;
			Projectile.friendly = false;
		}

		public override bool PreKill(int timeLeft)
		{
			Projectile.type = ProjectileID.Grenade;
			return true;
		}
	}
}