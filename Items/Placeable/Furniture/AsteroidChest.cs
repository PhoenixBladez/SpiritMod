using Terraria.ID;
using Terraria.ModLoader;
using AsteroidChestTile = SpiritMod.Tiles.Furniture.SpaceJunk.AsteroidChest;

namespace SpiritMod.Items.Placeable.Furniture
{
	public class AsteroidChest : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Asteroid Chest");
		}


		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 28;
			Item.value = 500;

			Item.maxStack = 99;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;

			Item.createTile = ModContent.TileType<AsteroidChestTile>();
		}
	}
}