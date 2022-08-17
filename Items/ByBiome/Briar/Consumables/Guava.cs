using Microsoft.Xna.Framework;

namespace SpiritMod.Items.ByBiome.Briar.Consumables;

public class Guava : FoodItem
{
	internal override Point Size => new(28, 26);
	public override void StaticDefaults() => Tooltip.SetDefault("Minor improvements to all stats");
}
