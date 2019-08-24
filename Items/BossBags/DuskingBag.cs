using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.BossBags
{
	public class DuskingBag : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Treasure Bag");
			Tooltip.SetDefault("Right Click to open");
		}


		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 20;
			item.rare = -2;

			item.maxStack = 30;

			item.expert = true;
		}

		public override bool CanRightClick()
		{
			return true;
		}

		public override void RightClick(Player player)
		{
			player.QuickSpawnItem(ItemID.GoldCoin, Main.rand.Next(3, 7));
			player.QuickSpawnItem(mod.ItemType("DuskPendant"));
			player.QuickSpawnItem(mod.ItemType("DuskStone"), Main.rand.Next(25, 36));
			if (Main.rand.Next(73) < 10)
				player.QuickSpawnItem(mod.ItemType("DarkCrest"));

			string[] lootTable = { "CrystalShadow", "ShadowflameSword", "UmbraStaff", "ShadowSphere", "DuskCarbine" };
			int loot = Main.rand.Next(lootTable.Length);
			if (loot == 0)
				player.QuickSpawnItem(mod.ItemType("CrystalShadow"), Main.rand.Next(29, 49));
			else
				player.QuickSpawnItem(mod.ItemType(lootTable[loot]));

			if (Main.rand.NextDouble() < 1d / 7)
				player.QuickSpawnItem(Armor.Masks.DuskingMask._type);
			if (Main.rand.NextDouble() < 1d / 10)
				player.QuickSpawnItem(Boss.Trophy6._type);
		}
	}
}
