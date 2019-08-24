using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.BossBags
{
	public class SteamRaiderBag : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Treasure Bag");
			Tooltip.SetDefault("Right-Click to open");
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
			player.QuickSpawnItem(ItemID.GoldCoin, Main.rand.Next(2, 4));
			player.QuickSpawnItem(mod.ItemType("StarMap"));
			player.QuickSpawnItem(mod.ItemType("SteamParts"), Main.rand.Next(14, 20));
			if (Main.rand.Next(80) < 11)
				player.QuickSpawnItem(mod.ItemType("StellarTech"));

			string[] lootTable = { "AstralLens", "SteamStaff", "SteamplateBow" };
			int loot = Main.rand.Next(lootTable.Length);
			player.QuickSpawnItem(mod.ItemType(lootTable[loot]));

			if (Main.rand.NextDouble() < 1d / 7)
				player.QuickSpawnItem(Armor.Masks.StarplateMask._type);
			if (Main.rand.NextDouble() < 1d / 10)
				player.QuickSpawnItem(Boss.Trophy3._type);
		}
	}
}
