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
			Item.width = 38;
			Item.height = 26;
			Item.value = 0;
			Item.rare = ItemRarityID.Blue;
			Item.defense = 1;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<DriftwoodTileItem>(), 12);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
