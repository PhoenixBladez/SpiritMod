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
			projectile.CloneDefaults(ProjectileID.Grenade);
			aiType = ProjectileID.Grenade;
			projectile.hostile = true;
			projectile.friendly = false;
		}

		public override bool PreKill(int timeLeft)
		{
			projectile.type = ProjectileID.Grenade;
			return true;
		}
	}
}