using SpiritMod.Tiles.Furniture;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Placeable.Furniture
{
	public class AdventurerMap : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Adventurer's Treasure Map");
			Tooltip.SetDefault("'South of Whiterun, east of Pelagia Farm'");
		}

		public override void SetDefaults()
		{
			Item.width = 36;
			Item.height = 28;
			Item.value = Terraria.Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.White;

			Item.maxStack = 99;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;

			Item.createTile = ModContent.TileType<AdventurerMapTile>();
		}
	}
}