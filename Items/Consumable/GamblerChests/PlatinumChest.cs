using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable.GamblerChests
{
	public class PlatinumChest : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Platinum Strongbox");
			Tooltip.SetDefault("'May contain a fortune'");
		}
		public override void SetDefaults()
		{
			item.width = 40;
			item.height = 40;
			item.value = 500000;
			item.rare = ItemRarityID.LightRed;
			item.maxStack = 30;
			item.autoReuse = true;
		}

		public override bool CanRightClick()
		{
			return true;
		}
		public override void RightClick(Player player)
		{
			int[] lootTable = { 1, 10, 50, 100, 500, 1000, 5000, 10000, 50000, 100000, 250000, 500000, 1000000, 5000000 };

			int loot = Main.rand.Next(lootTable.Length);
			int amount = (lootTable[loot]);
			while(amount >= 1000000) {
				player.QuickSpawnItem(74);
				amount -= 1000000;
			}
			while(amount >= 10000) {
				player.QuickSpawnItem(73);
				amount -= 10000;
			}
			while(amount >= 100) {
				player.QuickSpawnItem(72);
				amount -= 100;
			}
			player.QuickSpawnItem(71, amount);

			if(Main.rand.Next(20) == 1) {
				player.QuickSpawnItem(3817, Main.rand.Next(30));
			}
		}
	}
}
