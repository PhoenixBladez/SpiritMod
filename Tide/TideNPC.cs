using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SpiritMod;
using System.Collections.Generic;
using SpiritMod.NPCs.Tides;
using static Terraria.ModLoader.ModContent;
using System.Linq;

namespace SpiritMod.Tide
{
	public class TideNPC : GlobalNPC
	{
		public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
		{
			int activePlayers = 0;
			for (int i = 0; i < 255; i++) {
				if (Main.player[i].active)
					activePlayers++;
			}

			if (TideWorld.TheTide && player.ZoneBeach) {
				maxSpawns = (int)(10 + 1.5f * activePlayers);
				spawnRate = 20;
			}
		}

		public static List<IDictionary<int, float>> Spawnpool = new List<IDictionary<int, float>> { //list containing a dictionary of spawn pool information for each wave of the tide, key int is enemy type and value float is spawn rate
			new Dictionary<int, float> { //wave 1
				{NPCType<SpearKakamora>(), 7.35f},
				{NPCType<KakamoraParachuter>(), 5.35f},
				{NPCType<SwordKakamora>(), 7.35f},
				{NPCType<KakamoraShielder>(), 1.73f},
				{NPCType<KakamoraShielderRare>(), .135f},
				{NPCType<KakamoraRunner>(), 2f},
			},
			new Dictionary<int, float> { //wave 2
				{NPCType<SpearKakamora>(), 7.35f},
				{NPCType<KakamoraParachuter>(), 5.35f},
				{NPCType<SwordKakamora>(), 7.35f},
				{NPCType<KakamoraShielder>(), 1.73f},
				{NPCType<KakamoraShielderRare>(), .135f},
				{NPCType<KakamoraRunner>(), 2f},
				{NPCType<KakamoraShaman>(), (!NPC.AnyNPCs(NPCType<KakamoraShaman>())) ? 2.35f : 0f}, //0 spawn rate when one already exists
				{NPCType<KakamoraRider>(), 2.35f}
			},
			new Dictionary<int, float> { //wave 3
				{NPCType<SpearKakamora>(), 7.35f},
				{NPCType<KakamoraParachuter>(), 5.35f},
				{NPCType<KakamoraShielder>(), 1.73f},
				{NPCType<KakamoraShielderRare>(), .135f},
				{NPCType<KakamoraShaman>(), (!NPC.AnyNPCs(NPCType<KakamoraShaman>())) ? 2.35f : 0f}, //0 spawn rate when one already exists
				{NPCType<KakamoraRider>(), 2.35f},
				{NPCType<MangoJelly>(), 3.35f}
			},
			new Dictionary<int, float> { //wave 4
				{NPCType<KakamoraShielder>(), 1.73f},
				{NPCType<KakamoraShielderRare>(), .135f},
				{NPCType<KakamoraShaman>(), (!NPC.AnyNPCs(NPCType<KakamoraShaman>())) ? 2.35f : 0f}, //0 spawn rate when one already exists
				{NPCType<KakamoraRider>(), 2.35f},
				{NPCType<MangoJelly>(), 3.35f},
				{NPCType<LargeCrustecean>(), 2.35f}
			},
			new Dictionary<int, float> { //wave 5
				{NPCType<MangoJelly>(), 3.35f},
				{NPCType<LargeCrustecean>(), 2.35f}
			},
			new Dictionary<int, float> { //wave 6
				{NPCType<Rylheian>(), 0f}, //no spawn rate because custom spawn method, just here for checking the dictionary
			}
		};

		public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
		{
			if (TideWorld.TheTide && spawnInfo.player.ZoneBeach) {
				pool.Clear();
				if (TideWorld.TidePoints < 99) {
					IDictionary<int, float> spawnpool = Spawnpool.ElementAt(TideWorld.TideWave - 1); //find the spawn pool dictionary corresponding to the current tide wave
					foreach(KeyValuePair<int, float> key in spawnpool) { //then add that dictionary info to the actual spawn pool
						pool.Add(key.Key, key.Value);
					}
				}
			}
		}

		public override void NPCLoot(NPC npc)
		{
			if(TideWorld.TheTide) { //first check if tide is even active

				IDictionary<int, float> spawnpool = Spawnpool.ElementAt(TideWorld.TideWave - 1); //find the spawn pool dictionary corresponding to the current tide wave
				foreach (KeyValuePair<int, float> key in spawnpool) { //iterate through the spawn pool, and check if the killed npc's type is in the spawn pool
					if(key.Key == npc.type) { //if it is, add progress to the wave, and complete the wave instantly if it's a rylheian
						if(npc.type == NPCType<Rylheian>()) {
							TideWorld.TidePoints += 20;
						}
						else
							TideWorld.TidePoints++;
					}
				}
			}
		}
	}
}
