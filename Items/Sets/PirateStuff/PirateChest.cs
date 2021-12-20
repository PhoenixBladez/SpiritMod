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
			item.width = 32;
			item.height = 28;
			item.value = 500;
			item.maxStack = 99;
			item.useTime = 10;
			item.useAnimation = 15;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.createTile = ModContent.TileType<OceanPirateChest>();
			item.placeStyle = 1;
			item.useTurn = true;
			item.autoReuse = true;
			item.consumable = true;
		}
	}
}