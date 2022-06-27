using Microsoft.Xna.Framework;
using SpiritMod.Items.Material;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.ScarabeusDrops
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
			Item.width = 46;
			Item.height = 46;
			Item.value = Item.sellPrice(silver: 14);
			Item.rare = ItemRarityID.Blue;
			Item.axe = 11;
			Item.damage = 12;
			Item.knockBack = 6;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 22;
			Item.useAnimation = 22;
			Item.DamageType = DamageClass.Melee;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item1;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<Chitin>(), 10);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}

		public override float UseTimeMultiplier(Player player)
		{
			Point mousetilecoords = Main.MouseWorld.ToTileCoordinates();
			int[] SandTiles = new int[] {
				TileID.PalmTree,
				TileID.Cactus
			};

			if (SandTiles.Contains(Main.tile[mousetilecoords.X, mousetilecoords.Y].TileType) && player.WithinPlacementRange(mousetilecoords.X, mousetilecoords.Y) ||
				Main.SmartCursorIsUsed && Main.SmartCursorShowing && SandTiles.Contains(Main.tile[Main.SmartCursorX, Main.SmartCursorY].TileType)) {
				return 2.5f;
			}

			return 1f;
		}
	}
}