using SpiritMod.Tiles.Block;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Placeable.Tiles
{
	public class CreepingIce : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Creeping Ice");
			Tooltip.SetDefault("Slows down nearby players and enemies");
		}

		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 14;
			Item.maxStack = 999;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<CreepingIceTile>();
		}
	}
}