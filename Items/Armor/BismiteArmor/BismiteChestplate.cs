using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.BismiteArmor
{
	[AutoloadEquip(EquipType.Body)]
	public class BismiteChestplate : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bismite Chestplate");
			Tooltip.SetDefault("4% increased damage and critical strike chance");

		}
		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 20;
			item.value = 6000;
			item.rare = ItemRarityID.Blue;
			item.defense = 3;
		}

		public override void UpdateEquip(Player player)
		{
			player.allDamage += 0.04f;
			player.magicCrit += 4;
			player.meleeCrit += 4;
			player.rangedCrit += 4;
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
