using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable
{
    public class SpiritCrate : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spirit Crate");
			Tooltip.SetDefault("Right Click to open");
		}


        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.rare  = 5;
			item.useStyle = 1;
			item.createTile = mod.TileType("SpiritCrate");
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
            string[] lootTable = { "PutridPiece", "FleshClump", "SpiritOre", "Rune", "StarPiece", "Geode"};
            int loot = Main.rand.Next(lootTable.Length);
           
            player.QuickSpawnItem(mod.ItemType(lootTable[loot]), Main.rand.Next(3, 5));
            if (Main.rand.Next(4) == 1)
            {
                string[] lootTable3 = { "PutridPiece", "FleshClump", "SpiritOre", "Rune", "StarPiece", "Geode" };
                int loot3 = Main.rand.Next(lootTable3.Length);

                player.QuickSpawnItem(mod.ItemType(lootTable3[loot3]), Main.rand.Next(3, 5));
            }
            if (Main.rand.Next(7) == 0)
            {
                
                player.QuickSpawnItem(mod.ItemType("SoulStinger"));
            }
            
            if (Main.rand.Next(6) == 0)
            {
                string[] lootTable2 = { "StarCutter", "GhostJellyBomb"};
                int loot2 = Main.rand.Next(lootTable2.Length);

                player.QuickSpawnItem(mod.ItemType(lootTable2[loot2]), Main.rand.Next(30, 80));
            }
        }
    }
}
