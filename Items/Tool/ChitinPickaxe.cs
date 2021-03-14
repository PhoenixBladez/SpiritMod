using Microsoft.Xna.Framework;
using SpiritMod.Items.Material;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.Items.Tool
{
	public class ChitinPickaxe : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chitin Pickaxe");
			Tooltip.SetDefault("Mines sand and desert related blocks faster");
		}


		public override void SetDefaults()
		{
			item.width = 36;
			item.height = 36;
			item.value = Item.sellPrice(silver: 11);
			item.rare = ItemRarityID.Blue;
			item.pick = 55;
			item.damage = 5;
			item.knockBack = 2;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 15;
			item.useAnimation = 15;
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

		public override float MeleeSpeedMultiplier(Player player)
		{
			Point mousetilecoords = Main.MouseWorld.ToTileCoordinates();
			int[] SandTiles = new int[] {
				TileID.Sand,
				TileID.Sandstone,
				TileID.SandstoneBrick,
				TileID.SandStoneSlab,
				TileID.SandFallBlock,
				TileID.CorruptHardenedSand,
				TileID.CorruptSandstone,
				TileID.CrimsonHardenedSand,
				TileID.CrimsonSandstone,
				TileID.HallowHardenedSand,
				TileID.HallowSandstone,
				TileID.HardenedSand,
				TileID.Ebonsand,
				TileID.Crimsand,
				TileID.Pearlsand,
				TileID.Glass,
				TileID.PalmWood
			};

			if (SandTiles.Contains(Main.tile[mousetilecoords.X, mousetilecoords.Y].type) && player.WithinPlacementRange(mousetilecoords.X, mousetilecoords.Y) ||
				Main.SmartCursorEnabled && Main.SmartCursorShowing && SandTiles.Contains(Main.tile[Main.SmartCursorX, Main.SmartCursorY].type)) {
				player.pickSpeed *= 0.6f;
				return 1.2f;
			}

			return 1f;
		}
	}
}
