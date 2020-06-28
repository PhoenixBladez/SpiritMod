using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.ReachBoss
{
	[AutoloadEquip(EquipType.Legs)]
	public class ReachBossLegs : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thornspeaker's Greaves");
			Tooltip.SetDefault("4% increased minion damage");

		}

		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 20;
			item.value = Item.buyPrice(gold: 3, silver: 2);
			item.rare = ItemRarityID.Green;
			item.defense = 3;
		}
		public override void UpdateEquip(Player player)
		{
			player.minionDamage += 0.04f;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<ReachFlowers>(), 6);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}
