using SpiritMod.Items.Material;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.AcidArmor
{
	[AutoloadEquip(EquipType.Body)]
	public class AcidBody : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Acid Plate");

		}

		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 20;
			item.value = 6000;
			item.rare = ItemRarityID.LightRed;
			item.vanity = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Acid>(), 9);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}
