using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.FloatingItems.Driftwood.DriftwoodArmor
{
	[AutoloadEquip(EquipType.Legs)]
	public class DriftwoodLeggings : ModItem
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Driftwood Leggings");

		public override void SetDefaults()
		{
			item.width = 38;
			item.height = 26;
			item.value = 0;
			item.rare = ItemRarityID.Blue;
			item.defense = 1;
		}

		public override void AddRecipes()
		{
			var recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<DriftwoodTileItem>(), 12);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
