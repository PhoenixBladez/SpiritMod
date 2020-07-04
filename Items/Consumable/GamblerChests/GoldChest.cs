using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable.GamblerChests
{
	public class GoldChest : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gold Coffer");
			Tooltip.SetDefault("Right click to open\n'May contain a fortune'");
		}

		public override void SetDefaults()
		{
			item.width = 40;
			item.height = 40;
			item.value = Item.buyPrice(gold: 5);
			item.rare = ItemRarityID.Orange;
			item.maxStack = 30;
			item.autoReuse = true;
		}

		public override bool CanRightClick() => true;

		public override void RightClick(Player player)
		{
			int[] lootTable = { 1, 10, 50, 100, 500, 1000, 5000, 10000, 25000, 50000, 100000, 400000 };

			int loot = Main.rand.Next(lootTable.Length);
			int amount = lootTable[loot];
			player.QuickSpawnItem(ItemID.PlatinumCoin, amount / 1000000);
			amount %= 1000000;
			player.QuickSpawnItem(ItemID.GoldCoin, amount / 10000);
			amount %= 10000;
			player.QuickSpawnItem(ItemID.SilverCoin, amount / 100);
			amount %= 100;
			player.QuickSpawnItem(ItemID.CopperCoin, amount);
		}
	}
}
