using Microsoft.Xna.Framework;
using SpiritMod.Items.Material;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
namespace SpiritMod.Items.Tool
{
	public class ChitinHammer : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chitin Hammer");
			Tooltip.SetDefault("Breaks sand walls and desert related walls faster");
		}


		public override void SetDefaults()
		{
			item.width = 44;
			item.height = 44;
			item.value = Item.sellPrice(silver: 8);
			item.rare = ItemRarityID.Blue;
			item.hammer = 55;
			item.damage = 16;
			item.knockBack = 6;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 26;
			item.useAnimation = 26;
			item.melee = true;
			item.useTurn = true;
			item.autoReuse = true;
			item.UseSound = SoundID.Item1;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Chitin>(), 10);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override float UseTimeMultiplier(Player player)
		{
			Point mousetilecoords = Main.MouseWorld.ToTileCoordinates();
			ushort[] SandTiles = new ushort[] {
				WallID.SandFall,
				WallID.Sandstone,
				WallID.SandstoneBrick,
				WallID.CorruptHardenedSand,
				WallID.CorruptSandstone,
				WallID.CrimsonHardenedSand,
				WallID.CrimsonSandstone,
				WallID.HallowHardenedSand,
				WallID.HallowSandstone,
				WallID.HardenedSand,
				WallID.Glass,
				WallID.BlueStainedGlass,
				WallID.GreenStainedGlass,
				WallID.PurpleStainedGlass,
				WallID.RainbowStainedGlass,
				WallID.RedStainedGlass,
				WallID.YellowStainedGlass,
				WallID.Cactus,
				WallID.PalmWood,
				WallID.PalmWoodFence
			};

			bool flag = true; //ripped from vanilla
			if (!Main.wallHouse[Main.tile[mousetilecoords.X, mousetilecoords.Y].wall]) {
				flag = false;
				for (int i = mousetilecoords.X - 1; i < mousetilecoords.X + 2; i++) {
					for (int j = mousetilecoords.Y - 1; j < mousetilecoords.Y + 2; j++) {
						if (Main.tile[i, j].wall == 0 || Main.wallHouse[Main.tile[i, j].wall]) {
							flag = true;
							break;
						}
					}
				}
			}

			bool CheckAdjacentTile(Point point)//check to see if the selected wall is air and next to a valid wall, to make it feel more smooth
			{
				if(Main.tile[point.X, point.Y].wall == 0) {
					for(int i = point.X - 1; i <= point.X + 1; i++) {
						for(int j = point.Y - 1; j <= point.Y + 1; j++) {
							if (SandTiles.Contains(Main.tile[i, j].wall))
								return true;
						}
					}
				}
				return false;
			}

			if (SandTiles.Contains(Main.tile[mousetilecoords.X, mousetilecoords.Y].wall) && flag && player.WithinPlacementRange(mousetilecoords.X, mousetilecoords.Y) ||
				CheckAdjacentTile(mousetilecoords) && flag && player.WithinPlacementRange(mousetilecoords.X, mousetilecoords.Y) ||
				Main.SmartCursorEnabled && Main.SmartCursorShowing && SandTiles.Contains(Main.tile[Main.SmartCursorX, Main.SmartCursorY].wall) ||
				Main.SmartCursorEnabled && Main.SmartCursorShowing && CheckAdjacentTile(new Point(Main.SmartCursorX, Main.SmartCursorY))) {

				return 2.5f;
			}

			return 1f;
		}

		public override float MeleeSpeedMultiplier(Player player) => 1f / UseTimeMultiplier(player);
	}
}