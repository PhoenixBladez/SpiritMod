using Microsoft.Xna.Framework;

namespace SpiritMod.Items.ByBiome.Spirit.Consumables;

public class LuminBerry : FoodItem
{
	internal override Point Size => new(26, 30);
	public override void StaticDefaults() => Tooltip.SetDefault("Minor improvements to all stats\n'They feel almost glassy...'");
}
