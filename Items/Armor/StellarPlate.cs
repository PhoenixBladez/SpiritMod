using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor
{
	[AutoloadEquip(EquipType.Body)]
	public class StellarPlate : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stellar Plate");
			Tooltip.SetDefault("Increases minion damage by 10%\nIncreases your maximum number of minions by 1");

		}
		public override void SetDefaults()
		{
			item.width = 34;
			item.height = 30;
			item.value = Item.sellPrice(0, 1, 0, 0);
			item.rare = 5;
			item.defense = 12;
		}

		public override void UpdateEquip(Player player)
		{
			player.minionDamage += 0.1f;
			player.maxMinions += 1;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<StellarBar>(), 16);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}
