using SpiritMod.Tiles.Furniture;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Placeable.Furniture
{
	public class BismiteTableItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bismite Table");
		}


		public override void SetDefaults()
		{
			item.width = 64;
			item.height = 34;
			item.value = 150;

			item.maxStack = 99;

			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 10;
			item.useAnimation = 15;

			item.useTurn = true;
			item.autoReuse = true;
			item.consumable = true;

			item.createTile = ModContent.TileType<BismiteTable>();
		}
	}
}