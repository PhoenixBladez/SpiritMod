using Microsoft.Xna.Framework;

namespace SpiritMod.Items.ByBiome.Spirit.Consumables;

public class Glowpear : FoodItem
{
	internal override Point Size => new(22, 28);
	public override void StaticDefaults() => Tooltip.SetDefault("Minor improvements to all stats\n'It looks hollow?'");
}
