using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.CryoliteSet
{
	public class CryoliteBar : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cryolite Bar");
		}


		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 24;
            Item.value = Item.sellPrice(0, 0, 25, 0);
            Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.rare = ItemRarityID.Orange;
			Item.maxStack = 999;
			Item.createTile = ModContent.TileType<CryoliteBarTile>();
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<CryoliteOre>(), 3);
			recipe.AddTile(TileID.Hellforge);
			recipe.Register();
		}
	}
}