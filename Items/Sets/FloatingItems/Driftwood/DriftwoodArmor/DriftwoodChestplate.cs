using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.FloatingItems.Driftwood.DriftwoodArmor
{
	[AutoloadEquip(EquipType.Body)]
	public class DriftwoodChestplate : ModItem
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Driftwood Chestplate");

		public override void SetDefaults()
		{
			Item.width = 38;
			Item.height = 26;
			Item.value = Item.sellPrice(0, 0, 0, 0);
			Item.rare = ItemRarityID.Blue;
			Item.defense = 2;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<DriftwoodTileItem>(), 20);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
