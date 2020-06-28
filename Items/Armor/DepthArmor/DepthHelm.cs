using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.DepthArmor
{
	[AutoloadEquip(EquipType.Head)]
	public class DepthHelm : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Depth Walker's Helmet");
			Tooltip.SetDefault("10% increased melee critical strike chance\n10% increased minion damage\nIncreases your max number of minions");

		}


		int timer = 0;

		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 18;
			item.value = Item.buyPrice(gold: 4, silver: 60);
			item.rare = ItemRarityID.Pink;
			item.defense = 9;
		}
		public override void UpdateEquip(Player player)
		{
			player.meleeCrit += 10;
			player.minionDamage += 0.1f;
			player.maxMinions += 1;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<DepthChest>() && legs.type == ModContent.ItemType<DepthGreaves>();
		}
		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Press the 'Armor Bonus' to release multiple mechanical shark minions that home onto enemies\n30 second cooldown";
			player.GetSpiritPlayer().depthSet = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<DepthShard>(), 12);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}