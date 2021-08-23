using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.AccessoriesMisc.DustboundRing
{
	public class Dustbound_Ring : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dustbound Ring");
			Tooltip.SetDefault("Shows the location of treasure and ore");
		}

		public override void SetDefaults()
		{
			item.width = 26;
			item.height = 26;
			item.value = Item.sellPrice(silver: 35);
			item.rare = ItemRarityID.Blue;
			item.defense = 1;
			item.accessory = true;
		}
	}
}