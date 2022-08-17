using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace SpiritMod.Items.Consumable.Food
{
	public class CaesarSalad : FoodItem
	{
		internal override Point Size => new(30, 28);
		public override void StaticDefaults() => Tooltip.SetDefault("Minor improvements to all stats\nIncreases life regeneration\n'Maybe don't use a knife to eat this one'");

        public override bool CanUseItem(Player player)
        {
            player.AddBuff(BuffID.Regeneration, 3600);
            return true;
        }
	}
}
