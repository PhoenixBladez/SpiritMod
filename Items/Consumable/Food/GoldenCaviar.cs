using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace SpiritMod.Items.Consumable.Food
{
	public class GoldenCaviar : FoodItem
	{
		internal override Point Size => new(30, 34);
		public override void StaticDefaults() => Tooltip.SetDefault("Major improvements to all stats\nEmits an aura of light\n'It has an exquisite glow'");
		public override void Defaults() => Item.value = Item.sellPrice(0, 2, 0, 0);

        public override bool CanUseItem(Player player)
        {
            player.AddBuff(BuffID.Shine, 7200);
            return true;
        }
    }
}
