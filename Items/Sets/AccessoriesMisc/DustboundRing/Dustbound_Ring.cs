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
			Item.width = 26;
			Item.height = 26;
			Item.value = Item.sellPrice(silver: 35);
			Item.rare = ItemRarityID.Blue;
			Item.defense = 1;
			Item.accessory = true;
		}
	}
}