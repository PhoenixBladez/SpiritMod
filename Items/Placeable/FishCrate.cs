using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Placeable
{
    public class FishCrate : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Packing Crate");
			Tooltip.SetDefault("'A logo from a popular fishing company can be seen'\nRight click to open\nContains different types of fish");
		}


        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.rare  = 3;
			item.useStyle = 1;
			item.createTile = mod.TileType("FishCrate_Tile");
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
		public override void Update(ref float gravity, ref float maxFallSpeed)
        {
			if(item.wet)
			{
			gravity *= 0f;
			maxFallSpeed *= -.09f;
			}
			else
			{
			maxFallSpeed *= 1f;
			gravity *= 1f;
			}
        }

        public override void RightClick(Player player)
        {
			if (Main.rand.Next(2) == 0)
            {
                
                player.QuickSpawnItem(mod.ItemType("RawFish"));
            }
			if (Main.rand.Next(4) == 0)
            {
				if (Main.rand.Next(2) == 0)
				{
					player.QuickSpawnItem(mod.ItemType("FloaterItem"));
				}
				else if (Main.rand.Next (2) ==0)
				{
					 player.QuickSpawnItem(mod.ItemType("CoralfishItem"));
				}
				else if (Main.rand.Next (2) ==0)
				{
					 player.QuickSpawnItem(mod.ItemType("LuvdiscItem"));
				}
				else if (Main.rand.Next (2) ==0)
				{
					 player.QuickSpawnItem(mod.ItemType("LanternfishItem"));
				}
				else
				{
				player.QuickSpawnItem(mod.ItemType("KelpfishItem"));
				}
			}
            int[] lootTable = { 2316, 2298, 2290, 2301, 2297};
								   int loot = Main.rand.Next(lootTable.Length);
		   int Fish = Main.rand.Next(3,5);
			for (int j = 0; j < Fish; j++)
			{
			player.QuickSpawnItem(lootTable[loot]);
			}
            if (Main.rand.Next(4) == 1)
            {
                int[] lootTable3 = { 2303, 2304, 2313, 2306 };
				  int loot3 = Main.rand.Next(lootTable3.Length);
				int Booty = Main.rand.Next(1,2);
			for (int j = 0; j < Booty; j++)
			{
			player.QuickSpawnItem(lootTable3[loot3]);
			}
			
            }
            if (Main.rand.Next(27) == 0)
            {
				int[] lootTable4 = { 2341, 2332, 2342 };
                								   int loot4 = Main.rand.Next(lootTable4.Length);
			player.QuickSpawnItem(lootTable4[loot4]);
            }
            
            if (Main.rand.Next(3) == 0)
            {
                int[] lootTable2 = { 3197, 3196};
                int loot2 = Main.rand.Next(lootTable2.Length);
			int Fish1 = Main.rand.Next(9,12);
			for (int j = 0; j < Fish1; j++)
			{

                player.QuickSpawnItem((lootTable2[loot2]));
			}
            }
			if(Main.hardMode && Main.rand.Next (10) == 0)
			{
				int[] lootTable51 = { 2312, 2315, 2310, 2307};
				  int loot51 = Main.rand.Next(lootTable51.Length);
				int Booty = Main.rand.Next(1,2);
			for (int j = 0; j < Booty; j++)
			{
			player.QuickSpawnItem(lootTable51[loot51]);
			}
			
			}
  
            if (Main.rand.Next(3) == 0)
            {
                int[] lootTable21 = {72};
                int loot21 = Main.rand.Next(lootTable21.Length);
			int Fish1 = Main.rand.Next(10,90);
			for (int j = 0; j < Fish1; j++)
			{

                player.QuickSpawnItem((lootTable21[loot21]));
			}
            }
			if (Main.rand.Next(7) == 0)
            {
                int[] lootTable212 = {73};
                int loot212 = Main.rand.Next(lootTable212.Length);
			int Fish1 = Main.rand.Next(1,2);
			for (int j = 0; j < Fish1; j++)
			{

                player.QuickSpawnItem((lootTable212[loot212]));
			}
            }
        }
    }
}
