
using SpiritMod.Items.Material;
using SpiritMod.Tiles.Furniture;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.BloomwindArmor
{
	[AutoloadEquip(EquipType.Body)]
	public class BloomwindChestguard : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bloomwind Chestguard");
			Tooltip.SetDefault("Increases your max number of minions\n15% increased minion damage");
		}

		public override void SetDefaults()
		{
			item.width = 34;
			item.height = 30;
			item.value = Item.buyPrice(gold: 6);
			item.rare = 6;

			item.defense = 11;
		}

		public override void UpdateEquip(Player player)
		{
			player.maxMinions += 1;
			player.minionDamage += 0.15F;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<PrimevalEssence>(), 16);
			recipe.AddTile(ModContent.TileType<EssenceDistorter>());
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}