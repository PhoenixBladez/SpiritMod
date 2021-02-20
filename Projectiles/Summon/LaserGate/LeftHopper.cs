using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Summon.LaserGate
{
	public class LeftHopper : RightHopper
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Left Gate");

		public override void SetDefaults()
		{
			base.SetDefaults();
			OtherType = ModContent.ProjectileType<RightHopper>();
		}
	}
}