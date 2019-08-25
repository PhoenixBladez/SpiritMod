using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.DonatorItems
{
    class DonatorDrops : GlobalNPC
    {
        public override void NPCLoot(NPC npc)
        {
            if (npc.type == NPCID.RedDevil)
            {
                if (Main.rand.Next(100) == 1)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("CombatShotgun"), 1);
                }
            }
        }

		public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
		{
			if (player.HasBuff(LoomingPresence._type))
			{
				spawnRate = (int)(spawnRate * 0.8);
				maxSpawns += 2;
			}
		}
	}
}
