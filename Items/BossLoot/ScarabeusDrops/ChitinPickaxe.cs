using Microsoft.Xna.Framework;
using SpiritMod.Items.Material;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.Items.BossLoot.ScarabeusDrops
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
			Item.width = 36;
			Item.height = 36;
			Item.value = Item.sellPrice(silver: 14);
			Item.rare = ItemRarityID.Blue;
			Item.pick = 55;
			Item.damage = 5;
			Item.knockBack = 2;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 15;
			Item.useAnimation = 15;
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

		public override float UseSpeedMultiplier(Player player)
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

			if (SandTiles.Contains(Main.tile[mousetilecoords.X, mousetilecoords.Y].TileType) && player.WithinPlacementRange(mousetilecoords.X, mousetilecoords.Y) ||
				Main.SmartCursorIsUsed && Main.SmartCursorShowing && SandTiles.Contains(Main.tile[Main.SmartCursorX, Main.SmartCursorY].TileType)) {
				player.pickSpeed *= 0.6f;
				return 1.2f;
			}

			return 1f;
		}
	}
}
