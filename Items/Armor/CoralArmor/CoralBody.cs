using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.CoralArmor
{
	[AutoloadEquip(EquipType.Body)]
	public class CoralBody : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Coral Breastplate");
		}
		public override void SetDefaults()
		{
			item.width = 34;
			item.height = 30;
			item.value = Terraria.Item.sellPrice(0, 0, 11, 0);
			item.rare = 1;
			item.defense = 4;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Coral, 10);
			recipe.AddIngredient(ItemID.Seashell, 3);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
