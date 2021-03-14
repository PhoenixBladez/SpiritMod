using Microsoft.Xna.Framework;
using SpiritMod.Items.Material;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Tool
{
	public class ChitinAxe : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chitin Axe");
			Tooltip.SetDefault("Cuts down palm wood and cactus faster");
		}


		public override void SetDefaults()
		{
			item.width = 46;
			item.height = 46;
			item.value = Item.sellPrice(silver: 8);
			item.rare = ItemRarityID.Blue;
			item.axe = 11;
			item.damage = 12;
			item.knockBack = 6;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 22;
			item.useAnimation = 22;
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
			int[] SandTiles = new int[] {
				TileID.PalmTree,
				TileID.Cactus
			};

			if (SandTiles.Contains(Main.tile[mousetilecoords.X, mousetilecoords.Y].type) && player.WithinPlacementRange(mousetilecoords.X, mousetilecoords.Y) ||
				Main.SmartCursorEnabled && Main.SmartCursorShowing && SandTiles.Contains(Main.tile[Main.SmartCursorX, Main.SmartCursorY].type)) {
				return 2.5f;
			}

			return 1f;
		}
	}
}