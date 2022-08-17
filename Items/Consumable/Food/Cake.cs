using Microsoft.Xna.Framework;

namespace SpiritMod.Items.Consumable.Food
{
	public class Cake : FoodItem
	{
		internal override Point Size => new(30, 38);
		public override void StaticDefaults()
		{
			DisplayName.SetDefault("Carrot Cake");
			Tooltip.SetDefault("Minor improvements to all stats\n'Just the perfect amount of icing!'");
		}
	}
}
