using SpiritMod.Tiles.Furniture;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Placeable.Furniture
{
	public class SkullPile : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pile of Skulls");
		}


		public override void SetDefaults()
		{
			item.width = 48;
			item.height = 24;
			item.value = 850;

			item.maxStack = 99;

			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 10;
			item.useAnimation = 15;

			item.useTurn = true;
			item.autoReuse = true;
			item.consumable = true;

			item.createTile = ModContent.TileType<SkullPileTile>();
		}
	}
}