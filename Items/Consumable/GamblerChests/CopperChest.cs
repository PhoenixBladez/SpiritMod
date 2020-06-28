using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable.GamblerChests
{
	public class CopperChest : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Copper Chest");
			Tooltip.SetDefault("'May contain a fortune'");
		}
		public override void SetDefaults()
		{
			item.width = 40;
			item.height = 40;
			item.value = 1000;
			item.rare = ItemRarityID.Blue;
			item.maxStack = 30;
			item.autoReuse = true;
		}

		public override bool CanRightClick()
		{
			return true;
		}
		public override void RightClick(Player player)
		{
			int[] lootTable = { 1, 1, 2, 10, 20, 100, 200, 500, 1000, 2000, 7000 };

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
		}
	}
}
