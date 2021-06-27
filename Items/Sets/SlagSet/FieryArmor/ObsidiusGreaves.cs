using SpiritMod.Items.Sets.SlagSet;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.SlagSet.FieryArmor
{
	[AutoloadEquip(EquipType.Legs)]
	public class ObsidiusGreaves : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Slag Tyrant's Greaves");
			Tooltip.SetDefault("5% increased minion damage\nIncreases your max number of minions");

		}

		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 20;
			item.value = Item.sellPrice(0, 0, 39, 0);
			item.rare = ItemRarityID.Orange;
			item.defense = 6;
		}

		public override void UpdateEquip(Player player)
		{
			player.minionDamage += 0.05f;
			player.maxMinions += 1;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<CarvedRock>(), 14);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}
