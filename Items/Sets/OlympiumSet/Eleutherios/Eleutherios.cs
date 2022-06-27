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
			Tooltip.SetDefault("Reduces healing item effectiveness by 15%\nUsing a healing item gives a temporary damage bonus that decays to 0 over 15 seconds,\nInitial strength depends on the healing value of the consumable");
		}

		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 32;
			Item.rare = ItemRarityID.LightRed;
			Item.value = Item.sellPrice(gold: 2);
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual) => player.GetModPlayer<OlympiumPlayer>().eleutherios = true;
	}
}
