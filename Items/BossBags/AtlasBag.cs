using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.BossBags
{
	public class AtlasBag : ModItem
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
			player.QuickSpawnItem(ItemID.GoldCoin, Main.rand.Next(7, 12));
			player.QuickSpawnItem(mod.ItemType("AtlasEye"));
			player.QuickSpawnItem(mod.ItemType("ArcaneGeyser"), Main.rand.Next(30, 46));
			if (Main.rand.Next(8) < 1)
				player.QuickSpawnItem(mod.ItemType("UnrefinedRuneStone"));

			string[] lootTable = { "KingRock", "Mountain", "TitanboundBulwark", "CragboundStaff", "QuakeFist", "Earthshatter", };
			int loot = Main.rand.Next(lootTable.Length);
			player.QuickSpawnItem(mod.ItemType(lootTable[loot]));

			if (Main.rand.NextDouble() < 1d / 7)
				player.QuickSpawnItem(Armor.Masks.AtlasMask._type);
			if (Main.rand.NextDouble() < 1d / 10)
				player.QuickSpawnItem(Boss.Trophy8._type);
		}
	}
}
