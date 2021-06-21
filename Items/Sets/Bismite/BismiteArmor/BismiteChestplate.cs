using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.Bismite.BismiteArmor
{
	[AutoloadEquip(EquipType.Body)]
	public class BismiteChestplate : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bismite Chestplate");
			Tooltip.SetDefault("3% increased critical strike chance");

		}
		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 20;
			item.value = Item.buyPrice(silver: 8);
			item.rare = ItemRarityID.Blue;
			item.defense = 3;
		}

		public override void UpdateEquip(Player player)
		{
			player.magicCrit += 3;
			player.meleeCrit += 3;
			player.rangedCrit += 3;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<BismiteCrystal>(), 10);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}
