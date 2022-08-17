using Microsoft.Xna.Framework;

namespace SpiritMod.Items.Consumable.Food
{
	public class TofuSatay : FoodItem
	{
		internal override Point Size => new(28, 44);
		public override void StaticDefaults() => Tooltip.SetDefault("Minor improvements to all stats\n'Fresh and fried!'");
	}
}
