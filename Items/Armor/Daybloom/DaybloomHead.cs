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

			ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true;
		}

		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 24;
			Item.value = Item.sellPrice(0, 0, 10, 0);
			Item.rare = ItemRarityID.White;
			Item.defense = 2;
		}

		public override void UpdateEquip(Player player) => player.statManaMax2 += 20;
		public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ModContent.ItemType<DaybloomBody>() && legs.type == ModContent.ItemType<DaybloomLegs>();

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Being outside during daytime increases defense, maximum mana,\nand life regeneration slightly";

			if (Main.dayTime)
			{
				player.statDefense += 2;
				player.statManaMax2 += 20;
				player.lifeRegen += 1;
			}
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Sunflower, 1);
			recipe.AddIngredient(ItemID.FallenStar, 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}
