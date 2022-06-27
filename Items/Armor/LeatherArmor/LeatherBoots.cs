using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.LeatherArmor
{
	[AutoloadEquip(EquipType.Legs)]
	public class LeatherLegs : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Marksman's Boots");
		}
		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 18;
			Item.value = 3500;
			Item.rare = ItemRarityID.Blue;
			Item.defense = 1;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<OldLeather>(), 7);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}
