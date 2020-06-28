using Terraria.ModLoader;

namespace SpiritMod.Items.Placeable.Furniture
{
	public class Shrine1 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Shrine (Hachiman)");
		}


		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 28;
			item.value = 500;

			item.maxStack = 99;

			item.useStyle = 1;
			item.useTime = 10;
			item.useAnimation = 15;

			item.useTurn = true;
			item.autoReuse = true;
			item.consumable = true;

			item.createTile = mod.TileType("Shrine1_Tile");
		}
	}
}