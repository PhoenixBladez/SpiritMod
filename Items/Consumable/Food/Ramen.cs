using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace SpiritMod.Items.Consumable.Food
{
	public class Ramen : FoodItem
	{
		internal override Point Size => new(28, 36);
		public override void StaticDefaults() => Tooltip.SetDefault("Minor improvements to all stats\n'It'll warm you right up!'");

		public override bool CanUseItem(Player player)
		{
			player.AddBuff(BuffID.Warmth, 72000);
			return true;
		}
	}
}
