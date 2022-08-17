using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace SpiritMod.Items.Consumable.Food
{
	public class GhostPepper : FoodItem
	{
		internal override Point Size => new(26, 34);
		public override void StaticDefaults() => Tooltip.SetDefault("Minor improvements to all stats\n'Will you take the risk?'");

		public override bool CanUseItem(Player player)
		{
			player.AddBuff(BuffID.Inferno, 5400);
            player.AddBuff(BuffID.OnFire, 420);
            return true;
		}
	}
}
