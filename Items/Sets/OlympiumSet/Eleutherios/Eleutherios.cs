using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.OlympiumSet.Eleutherios
{
	public class Eleutherios : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Eleutherios");
			Tooltip.SetDefault("Reduces healing item effectiveness by 15%\nUsing a healing item gives a temporary damage bonus that decays to 0 over 15 seconds,\ninitial strength depends on the healing value of the consumable");
		}

		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 32;
			item.value = Item.buyPrice(0, 5, 0, 0);
			item.rare = ItemRarityID.Green;
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual) => player.GetModPlayer<OlympiumPlayer>().eleutherios = true;
	}
}
