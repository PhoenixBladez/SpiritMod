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
			item.width = 38;
			item.height = 26;
			item.value = 5600;
			item.rare = ItemRarityID.Blue;
			item.defense = 4;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<DeepCascadeShard>(), 14);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
