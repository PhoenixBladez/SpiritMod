using Terraria.ID;
using Terraria.ModLoader;
using HalloweenGrassTile = SpiritMod.Tiles.Block.HalloweenGrass;
namespace SpiritMod.Items.Placeable.Tiles
{
	public class HalloweenGrass : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spooky Grass");
		}

		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 14;
			Item.maxStack = 999;
			Item.value = 500;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<HalloweenGrassTile>();
		}
	}
}