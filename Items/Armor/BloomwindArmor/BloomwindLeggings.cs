using SpiritMod.Items.Material;
using SpiritMod.Tiles.Furniture;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.BloomwindArmor
{
	[AutoloadEquip(EquipType.Legs)]
	public class BloomwindLeggings : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bloomwind Leggings");
			Tooltip.SetDefault("Increases your max number of minions\n7% increased minion damage");
		}
		public override void SetDefaults()
		{
			item.width = 34;
			item.height = 30;
			item.value = Item.buyPrice(gold: 4);
			item.rare = ItemRarityID.LightPurple;

			item.defense = 9;
		}

		public override void UpdateEquip(Player player)
		{
			player.maxMinions += 1;
			player.minionDamage += 0.07f;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<PrimevalEssence>(), 10);
			recipe.AddTile(ModContent.TileType<EssenceDistorter>());
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}