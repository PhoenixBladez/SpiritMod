using Microsoft.Xna.Framework;

namespace SpiritMod.Items.Consumable.Food
{
	public class Hummus : FoodItem
	{
		internal override Point Size => new(38, 28);
		public override void StaticDefaults() => Tooltip.SetDefault("Minor improvements to all stats\n'Warm and tasty!'");
	}
}
