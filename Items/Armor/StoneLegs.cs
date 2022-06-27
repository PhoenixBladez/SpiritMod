using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor
{
	[AutoloadEquip(EquipType.Legs)]
	public class StoneLegs : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stone Greaves");
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 18;
			Item.value = 0;
			Item.rare = ItemRarityID.White;
			Item.defense = 2;
		}

		public override void AddRecipes()  //How to craft this item
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.StoneBlock, 40);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}