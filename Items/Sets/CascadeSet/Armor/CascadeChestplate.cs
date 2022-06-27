using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.CascadeSet.Armor
{
	[AutoloadEquip(EquipType.Body)]
	public class CascadeChestplate : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cascade Chestplate");
			Tooltip.SetDefault("50% knockback resistance");
		}

		public override void SetDefaults()
		{
			Item.width = 38;
			Item.height = 26;
			Item.value = 5600;
			Item.rare = ItemRarityID.Blue;
			Item.defense = 4;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<DeepCascadeShard>(), 14);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
