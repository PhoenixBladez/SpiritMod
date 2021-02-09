using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Arrow
{
	public class NegativeArrow : PositiveArrow
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Negative Arrow");

		public override void SetDefaults()
		{
			base.SetDefaults();
			oppositearrow = ModContent.ProjectileType<PositiveArrow>();
		}
	}
}