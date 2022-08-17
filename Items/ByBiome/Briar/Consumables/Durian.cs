using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace SpiritMod.Items.ByBiome.Briar.Consumables;

public class Durian : FoodItem
{
	internal override Point Size => new(30, 30);
	public override void StaticDefaults() => Tooltip.SetDefault("Minor improvements to all stats\n'What an awful smell!'");

	public override bool? UseItem(Player player)
	{
		player.AddBuff(BuffID.Stinky, 5 * 60 * 60);
		return true;
	}
}
