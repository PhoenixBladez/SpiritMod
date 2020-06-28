
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory.Leather
{
	[AutoloadEquip(EquipType.Shield)]
	public class LeatherShield : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Leather Safeguard");
			Tooltip.SetDefault("Increases max life by 10");
		}

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 28;
			item.rare = 1;
			item.value = 1900;
			item.defense = 1;
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.statLifeMax2 += 10;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<OldLeather>(), 8);
			recipe.AddRecipeGroup(RecipeGroupID.IronBar, 5);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}
