using SpiritMod.Tiles.Ambient.Ocean;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.PirateStuff
{
	public class PirateChest : ModItem
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Pirate Chest");

		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 28;
			Item.value = 500;
			Item.maxStack = 99;
			Item.useTime = 10;
			Item.useAnimation = 15;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.createTile = ModContent.TileType<OceanPirateChest>();
			Item.placeStyle = 0;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
		}
	}
}