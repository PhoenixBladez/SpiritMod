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
			Item.autoReuse = true;
			Item.useTurn = true;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useAnimation = 15;
			Item.rare = ItemRarityID.Blue;
			Item.useTime = 15;
			Item.maxStack = 99;
			Item.placeStyle = 0;
			Item.width = 22;
			Item.height = 20;
			Item.value = Item.buyPrice(0, 0, 5, 0);
			// TODO: find a way to make briar seeds icon show up under mouse
		}

		public override bool? UseItem(Player player)
		{
			if (Main.netMode == NetmodeID.Server)
				return false;

			Tile tile = Framing.GetTileSafely(Player.tileTargetX, Player.tileTargetY);
			if (tile.HasTile && tile.TileType == TileID.Dirt && player.WithinPlacementRange(Player.tileTargetX, Player.tileTargetY)) {
				WorldGen.PlaceTile(Player.tileTargetX, Player.tileTargetY, ModContent.TileType<BriarGrass>(), forced: true);
				player.inventory[player.selectedItem].stack--;
			}

			return null;
		}
	}
}
