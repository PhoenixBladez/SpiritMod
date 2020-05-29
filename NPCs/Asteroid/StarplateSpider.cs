using Terraria;
using Terraria.ID;
using System;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Asteroid
{
	public class StarplateSpider : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starplate Spider");
			Main.npcFrameCount[npc.type] = 4;
		}

		public override void SetDefaults()
		{
			npc.width = 54;
			npc.height = 30;
			npc.damage = 34;
			npc.defense = 9;
			npc.lifeMax = 200;
			npc.value = 860f;
			npc.knockBackResist = 0.95f;
		}

		public override void NPCLoot()
        {
            if (Main.rand.Next(2) == 0)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("StarEnergy"));
            }
            string[] lootTable = { "AstronautLegs", "AstronautHelm", "AstronautBody" };
            if (Main.rand.Next(40) == 0)
            {
                int loot = Main.rand.Next(lootTable.Length);
                {
                    npc.DropItem(mod.ItemType(lootTable[loot]));
                }
            }
        }
		
		public override void AI()
		{
			npc.spriteDirection = npc.direction;
		}
	}
}
