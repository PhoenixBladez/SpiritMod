using Microsoft.Xna.Framework;

namespace SpiritMod.Items.Consumable.Food
{
	public class Baguette : FoodItem
	{
		internal override Point Size => new(32, 32);
		public override void StaticDefaults() => Tooltip.SetDefault("Minor improvements to all stats\n'You feel fancier already'");
	}
}
