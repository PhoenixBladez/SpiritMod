using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory.Leather
{
	[AutoloadEquip(EquipType.HandsOn)]
	public class LeatherGlove : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Leather Fistwraps");
			Tooltip.SetDefault("Slightly increases weapon speed");
		}
		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 34;
			Item.rare = ItemRarityID.Blue;
			Item.value = 1200;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetSpiritPlayer().leatherGlove = true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<OldLeather>(), 6);
			recipe.AddRecipeGroup(RecipeGroupID.IronBar, 2);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
