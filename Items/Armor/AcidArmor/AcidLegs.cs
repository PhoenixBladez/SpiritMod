using SpiritMod.Items.Material;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.AcidArmor
{
	[AutoloadEquip(EquipType.Legs)]
	public class AcidLegs : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Acid Leggings");
		}

		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 18;
			item.value = 16000;
			item.rare = ItemRarityID.LightRed;
			item.vanity = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Acid>(), 6);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}
