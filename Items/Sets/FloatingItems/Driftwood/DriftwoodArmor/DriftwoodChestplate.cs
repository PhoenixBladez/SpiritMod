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
			item.width = 38;
			item.height = 26;
			item.value = Item.sellPrice(0, 0, 0, 0);
			item.rare = ItemRarityID.Blue;
			item.defense = 2;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<DriftwoodTileItem>(), 20);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
