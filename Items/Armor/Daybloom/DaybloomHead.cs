using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.Daybloom
{
	[AutoloadEquip(EquipType.Head)]
	public class DaybloomHead : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sunflower Hat");
			Tooltip.SetDefault("Increases maximum mana by 20");
		}

		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 24;
			item.value = Item.sellPrice(0, 0, 10, 0);
			item.rare = ItemRarityID.White;
			item.defense = 2;
		}

		public override void UpdateEquip(Player player) => player.statManaMax2 += 20;
		public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ModContent.ItemType<DaybloomBody>() && legs.type == ModContent.ItemType<DaybloomLegs>();
		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = $"Being outside during daytime increases defense, maximum mana,\nand life regeneration slightly";
			if (Main.dayTime) {
				player.statDefense += 2;
				player.statManaMax2 += 20;
				player.lifeRegen += 1;
			}
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Sunflower, 1);
			recipe.AddIngredient(ItemID.FallenStar, 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
