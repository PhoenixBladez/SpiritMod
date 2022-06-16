using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Summon.LaserGate
{
	public class LeftHopper : RightHopper
	{
		protected override int OtherType => ModContent.ProjectileType<RightHopper>();
		public override void SetStaticDefaults() => DisplayName.SetDefault("Left Gate");
	}
}