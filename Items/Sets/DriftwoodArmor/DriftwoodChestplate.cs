using SpiritMod.Items.Sets.FloatingItems.Driftwood;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.DriftwoodArmor
{
	[AutoloadEquip(EquipType.Body)]
	public class DriftwoodChestplate : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Driftwood Chestplate");
			Tooltip.SetDefault("12% increased melee damage\nGrants immunity to knockback");
		}

		public override void SetDefaults()
		{
			item.width = 38;
			item.height = 26;
			item.value = Item.sellPrice(0, 1, 0, 0);
			item.rare = ItemRarityID.Orange;
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
