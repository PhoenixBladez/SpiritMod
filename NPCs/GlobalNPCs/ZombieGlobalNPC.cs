using SpiritMod.NPCs.ZombieVariants;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.GlobalNPCs
{
	public class ZombieGlobalNPC : GlobalNPC
	{
		public override bool InstancePerEntity => true;
		int timeAlive;

		public override void PostAI(NPC npc)
		{
			Player closest = Main.player[Player.FindClosest(npc.position, npc.width, npc.height)];

			if (closest.ZoneBeach)
            {
				if (npc.type == NPCID.Zombie || npc.type == NPCID.BaldZombie || npc.type == NPCID.PincushionZombie || npc.type == NPCID.SlimedZombie || npc.type == NPCID.SwampZombie || npc.type == NPCID.TwiggyZombie)
				{
					timeAlive++;
					if (timeAlive == 1)
					{
						int seed = (int)Main.GameUpdateCount; //Pseudorandom spawn rate to avoid any multiplayer syncing
						if (seed % 3 == 0)
							npc.Transform(ModContent.NPCType<SailorZombie>());
						else if (seed % 3 == 1)
							npc.Transform(ModContent.NPCType<KelpZombie>());
						else
							npc.Transform(ModContent.NPCType<DiverZombie>());
					}
				}
            }

			if (npc.type >= NPCID.ArmedZombie && npc.type <= NPCID.ArmedZombieCenx && timeAlive == 1) //All armed zombies (this felt less clunky than typing out each ID)
				npc.Transform(ModContent.NPCType<TridentZombie>());
	    }
	}
}