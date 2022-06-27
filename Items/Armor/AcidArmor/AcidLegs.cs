using SpiritMod.Items.Material;
using Terraria;
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
			Item.width = 22;
			Item.height = 18;
			Item.value = 16000;
			Item.rare = ItemRarityID.LightRed;
			Item.vanity = true;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ModContent.ItemType<Acid>(), 6);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}
