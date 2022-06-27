using SpiritMod.Items.Material;
using Terraria;
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
			Item.width = 30;
			Item.height = 20;
			Item.value = 6000;
			Item.rare = ItemRarityID.LightRed;
			Item.vanity = true;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ModContent.ItemType<Acid>(), 9);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}
