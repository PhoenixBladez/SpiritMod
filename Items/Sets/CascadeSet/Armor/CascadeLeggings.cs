using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.CascadeSet.Armor
{
	[AutoloadEquip(EquipType.Legs)]
	public class CascadeLeggings : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cascade Greaves");
			Tooltip.SetDefault("15% increased movement in water");
		}

		public override void SetDefaults()
		{
			item.width = 38;
			item.height = 26;
			item.value = 4000;
			item.rare = ItemRarityID.Blue;
			item.defense = 3;
		}

		public override void UpdateEquip(Player player)
		{
			if (player.wet)
				player.moveSpeed += .15f;
		}
		public override void AddRecipes()
		{
			var recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<DeepCascadeShard>(), 10);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
