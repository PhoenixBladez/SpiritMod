using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.BossBags
{
	public class InfernonBag : ModItem
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
			player.QuickSpawnItem(ItemID.GoldCoin, Main.rand.Next(2, 5));
			player.QuickSpawnItem(mod.ItemType("HellsGaze")); //expert drop
			player.QuickSpawnItem(mod.ItemType("InfernalAppendage"), Main.rand.Next(25, 36));

			string[] lootTable = { "InfernalJavelin", "DiabolicHorn", "SevenSins", "InfernalSword", "InfernalStaff", "InfernalShield", "EyeOfTheInferno", };
			int loot = Main.rand.Next(lootTable.Length);
			player.QuickSpawnItem(mod.ItemType(lootTable[loot]));
			
			if (Main.rand.NextDouble() < 1d / 7)
				player.QuickSpawnItem(Armor.Masks.InfernonMask._type);
			if (Main.rand.NextDouble() < 1d / 10)
				player.QuickSpawnItem(Boss.Trophy4._type);

			if (Main.rand.NextDouble() < 10d / 73)
				player.QuickSpawnItem(mod.ItemType("SearingBand"));
		}
	}
}
