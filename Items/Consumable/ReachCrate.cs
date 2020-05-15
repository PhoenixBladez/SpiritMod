using System;

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
            item.width = 20;
            item.height = 20;
            item.rare  = 2;
			item.useStyle = 1;
			item.createTile = mod.TileType("ReachCrate_Tile");
            item.maxStack = 999;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.consumable = true;

        }
        public override bool CanRightClick()
        {
            return true;
        }

        public override void RightClick(Player player)
        {
            string[] lootTable = { "AncientBark", "EnchantedLeaf", "BismiteCrystal"};
            int loot = Main.rand.Next(lootTable.Length);
            player.QuickSpawnItem(mod.ItemType(lootTable[loot]), Main.rand.Next(3, 5));
            if (NPC.downedBoss1 && Main.rand.Next(2) == 0)
            {
                string[] lootTable1 = { "ReachBrooch", "ReachBoomerang", "BriarRattle", "ReacgStaffChest" };
                int loot1 = Main.rand.Next(lootTable1.Length);
                player.QuickSpawnItem(mod.ItemType(lootTable1[loot1]));
            }
            {
                int[] lootTable3 = { 2674, 2675 };
                int loot3 = Main.rand.Next(lootTable3.Length);
                int baitamt = Main.rand.Next(2, 6);
                for (int j = 0; j < baitamt; j++)
                {
                    player.QuickSpawnItem(lootTable3[loot3]);
                }
            }
            if (Main.rand.Next(2) == 0)
            {
                int[] lootTable2 = { 19, 20, 21, 22, 703, 704, 705, 706};
                int loot2 = Main.rand.Next(lootTable2.Length);
                int oreamt = Main.rand.Next(5, 9);
                for (int j = 0; j < oreamt; j++)
                {
                    player.QuickSpawnItem(lootTable2[loot2]);
                }
            }
            if (Main.rand.Next(2) == 0)
            {
                int potions;
                potions = Main.rand.Next(new int[] { 288, 290, 292, 304, 298, 2322, 2323, 291, 2329 });
                int potamt = Main.rand.Next(2, 3);
                for (int j = 0; j < potamt; j++)
                {
                    player.QuickSpawnItem(potions);
                }
            }
            if (Main.rand.Next(20) == 1)
            {
                player.QuickSpawnItem(3200);
            }
            if (Main.rand.Next(20) == 1)
            {
                player.QuickSpawnItem(3201);
            }
            if (Main.rand.Next(23) == 1)
            {
                player.QuickSpawnItem(997);
            }
            player.QuickSpawnItem(72, Main.rand.Next(7, 16));
        }
    }
}
