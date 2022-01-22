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
			item.width = 22;
			item.height = 18;
			item.value = 0;
			item.rare = ItemRarityID.White;
			item.defense = 2;
		}

		public override void AddRecipes()  //How to craft this item
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.StoneBlock, 40);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}