using SpiritMod.Tiles.Block;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Placeable.Tiles
{
	public class TechBlockItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Glowplate Block");
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

			Item.createTile = ModContent.TileType<TechBlock>();
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(50);
            recipe.AddIngredient(ModContent.ItemType<Items.Material.SynthMaterial>(), 1);
            recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}