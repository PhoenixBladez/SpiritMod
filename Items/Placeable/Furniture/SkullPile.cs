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
			Item.width = 48;
			Item.height = 24;
			Item.value = 850;

			Item.maxStack = 99;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;

			Item.createTile = ModContent.TileType<SkullPileTile>();
		}
	}
}