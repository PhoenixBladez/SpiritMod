using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace SpiritMod.Items.Consumable.Food
{
	public class PopRocks : FoodItem
	{
		internal override Point Size => new(36, 42);
		public override void StaticDefaults() => Tooltip.SetDefault("Minor improvements to all stats\nEmits an aura of light\n'It zips around your mouth!'");

		public override bool CanUseItem(Player player)
		{
			player.AddBuff(BuffID.Shine, 7200);
			return base.CanUseItem(player);
		}
	}
}
