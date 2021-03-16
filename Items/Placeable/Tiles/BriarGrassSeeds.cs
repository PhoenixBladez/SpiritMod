using SpiritMod.Tiles.Block;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Placeable.Tiles
{
	public class BriarGrassSeeds : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Briar Grass Seeds");
			Tooltip.SetDefault("Can be placed");
		}

		public override void SetDefaults()
		{
			item.autoReuse = true;
			item.useTurn = true;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useAnimation = 15;
			item.rare = ItemRarityID.Blue;
			item.useTime = 15;
			item.maxStack = 99;
			item.placeStyle = 0;
			item.width = 22;
			item.height = 20;
			item.value = Item.buyPrice(0, 0, 5, 0);
			// TODO: find a way to make briar seeds icon show up under mouse
		}

		public override bool UseItem(Player player)
		{
			if (Main.netMode == NetmodeID.Server)
				return false;

			Tile tile = Framing.GetTileSafely(Player.tileTargetX, Player.tileTargetY);
			if (tile.active() && tile.type == TileID.Dirt && player.WithinPlacementRange(Player.tileTargetX, Player.tileTargetY)) {
				WorldGen.PlaceTile(Player.tileTargetX, Player.tileTargetY, ModContent.TileType<BriarGrass>(), forced: true);
				player.inventory[player.selectedItem].stack--;
			}

			return true;
		}
	}
}
