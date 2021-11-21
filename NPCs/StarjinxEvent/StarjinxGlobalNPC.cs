using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.StarjinxEvent
{
	/// <summary>Used exclusively to check if an NPC has been spawned by a Starjinx Comet. May be phased out once the waves proper are added.</summary>
	class StarjinxGlobalNPC : GlobalNPC
	{
		public bool spawnedByComet = false;

		public override bool CloneNewInstances => true;
		public override bool InstancePerEntity => true;

		public override void SetDefaults(NPC npc) => spawnedByComet = false;

		public override bool CheckDead(NPC npc)
		{
			spawnedByComet = false;
			return true;
		}

		/// <summary>Finds and targets the closest player to the given NPC that is within the Starjinx event zone.</summary>
		/// <param name="npc">NPC to find a target for.</param>
		/// <param name="sticky">If true, the NPC will not switch targets until the player is inactive, dead or outside of the Starjinx Event zone.</param>
		public static void TargetClosestSjinx(NPC npc, bool sticky = false)
		{
			int target = npc.target;

			if (sticky && Main.player[target].active && !Main.player[target].dead && Main.player[target].GetModPlayer<StarjinxPlayer>().zoneStarjinxEvent)
				return; //Do not switch targets if we have one already (and sticky is true)

			for (int i = 0; i < Main.maxPlayers; ++i) //Find target
			{
				Player p = Main.player[i];
				if (p.active && !p.dead && p.GetModPlayer<StarjinxPlayer>().zoneStarjinxEvent && p.DistanceSQ(npc.Center) < Main.player[target].DistanceSQ(npc.Center))
					target = i;
			}

			npc.target = target;

			if (!Main.player[target].active || Main.player[target].dead || !Main.player[target].GetModPlayer<StarjinxPlayer>().zoneStarjinxEvent)
				npc.target = 0; //Default to 0 if no target is found
		}

		public override void NPCLoot(NPC npc)
		{
			if (npc.modNPC is IStarjinxEnemy || npc.type == ModContent.NPCType<Enemies.Pathfinder.Pathfinder>() || npc.type == NPCID.EyeofCthulhu) //Replace eye of cthulhu with actual bosses eventually, ofc
				StarjinxEventWorld.IncrementKilledEnemies();
		}
	}
	
	class StarjinxNonInstancedGlobalNPC : GlobalNPC
	{
		public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
		{
			if (StarjinxEventWorld.StarjinxActive && player.HasBuff(ModContent.BuffType<Buffs.HighGravityBuff>()))
				maxSpawns = 0;
		}
	}
}
