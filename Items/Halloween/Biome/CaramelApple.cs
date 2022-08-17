using Microsoft.Xna.Framework;

namespace SpiritMod.Items.Halloween.Biome
{
	public class CaramelApple : FoodItem
	{
		internal override Point Size => new(30, 42);
		public override void StaticDefaults() => Tooltip.SetDefault("Minor improvements to all stats");
	}
}
