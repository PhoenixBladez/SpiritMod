using SpiritMod.Tiles.Ambient.GraniteSpike;
using SpiritMod.Items.Material;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Placeable.Furniture.GraniteSpikes
{
	public class GraniteSpike6 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Granite Zigzag Formation");
		}


		public override void SetDefaults()
		{
			item.width = 36;
			item.height = 34;
			item.value = 150;

			item.maxStack = 99;

			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 10;
			item.useAnimation = 15;

			item.useTurn = true;
			item.autoReuse = true;
			item.consumable = true;

			item.createTile = ModContent.TileType<Spike6Tile>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.GraniteBlock, 10);
			recipe.AddIngredient(ModContent.ItemType<GraniteChunk>(), 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}