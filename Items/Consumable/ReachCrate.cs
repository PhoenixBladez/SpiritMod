
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable
{
	public class ReachCrate : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thorny Crate");
			Tooltip.SetDefault("Right click to open");
		}

		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 20;
			Item.rare = ItemRarityID.Green;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.createTile = Mod.Find<ModTile>("ReachCrate_Tile").Type;
			Item.maxStack = 999;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.consumable = true;
		}

		public override bool CanRightClick() => true;

		public override void RightClick(Player player)
		{
			string[] lootTable = { "AncientBark", "EnchantedLeaf", "BismiteCrystal" };
			int loot = Main.rand.Next(lootTable.Length);
			var source = player.GetSource_OpenItem(Item.type, "RightClick");
			player.QuickSpawnItem(source, Mod.Find<ModItem>(lootTable[loot]).Type, Main.rand.Next(3, 5));
			if (NPC.downedBoss1 && Main.rand.NextBool(2))
			{
				string[] lootTable1 = { "ReachBrooch", "ReachBoomerang", "ThornHook", "ReachChestMagic" };
				int loot1 = Main.rand.Next(lootTable1.Length);
				player.QuickSpawnItem(source, Mod.Find<ModItem>(lootTable1[loot1]).Type);
			}
			int[] lootTable3 = { 2674, 2675 };
			int loot3 = Main.rand.Next(lootTable3.Length);
			int baitamt = Main.rand.Next(2, 6);

			for (int j = 0; j < baitamt; j++)
				player.QuickSpawnItem(source, lootTable3[loot3]);

			if (Main.rand.NextBool(2))
			{
				int[] lootTable2 = { 19, 20, 21, 22, 703, 704, 705, 706 };
				int loot2 = Main.rand.Next(lootTable2.Length);
				int oreamt = Main.rand.Next(5, 9);
				for (int j = 0; j < oreamt; j++)
					player.QuickSpawnItem(source, lootTable2[loot2]);
			}
			if (Main.rand.NextBool(2))
			{
				int potions;
				potions = Main.rand.Next(new int[] { 288, 290, 292, 304, 298, 2322, 2323, 291, 2329 });
				int potamt = Main.rand.Next(2, 3);
				for (int j = 0; j < potamt; j++)
					player.QuickSpawnItem(source, potions);
			}
			if (Main.rand.NextBool(20))
				player.QuickSpawnItem(source, 3200);
			if (Main.rand.NextBool(20))
				player.QuickSpawnItem(source, 3201);
			if (Main.rand.NextBool(23))
				player.QuickSpawnItem(source, 997);
			player.QuickSpawnItem(source, 72, Main.rand.Next(7, 16));
		}
	}
}