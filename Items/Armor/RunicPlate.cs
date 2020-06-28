using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor
{
	[AutoloadEquip(EquipType.Body)]
	public class RunicPlate : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Runic Plate");
			Tooltip.SetDefault("Increases magic critical strike chance by 8%");

		}
		public override void SetDefaults()
		{
			item.width = 34;
			item.height = 30;
			item.value = 50000;
			item.rare = ItemRarityID.Pink;
			item.defense = 14;
		}

		public override void UpdateEquip(Player player)
		{
			player.magicCrit += 8;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Rune>(), 16);
			recipe.AddIngredient(ModContent.ItemType<SoulShred>(), 4);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}