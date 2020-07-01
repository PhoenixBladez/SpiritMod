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
			DisplayName.SetDefault("Daybloom Helm");
			Tooltip.SetDefault("Increases maximum mana by 20");
		}


		int timer = 0;

		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 24;
			item.value = Terraria.Item.sellPrice(0, 0, 10, 0);
			item.rare = ItemRarityID.White;
			item.defense = 2;
		}
		public override void UpdateEquip(Player player)
		{
			player.statManaMax2 += 20;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<DaybloomBody>() && legs.type == ModContent.ItemType<DaybloomLegs>();
		}
		public override void UpdateArmorSet(Player player)
		{
			var tapDir = Main.ReversedUpDownArmorSetBonuses ? "UP" : "DOWN";
			player.setBonus = $"Being outside during daytime accumulates solar energy\nOnce energy is accumulated, double tap {tapDir} to Dazzle nearby foes\n30 second warm-up";
			player.GetSpiritPlayer().daybloomSet = true;
			if(Main.dayTime && player.ZoneOverworldHeight) {
				player.GetSpiritPlayer().dazzleStacks++;
			} else {
				player.GetSpiritPlayer().dazzleStacks = 0;
			}
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Daybloom, 1);
			recipe.AddIngredient(ItemID.FallenStar, 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
