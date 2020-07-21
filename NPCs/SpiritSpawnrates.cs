using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.NPCs
{
	public class SpiritSpawnRates : GlobalNPC
	{

		public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
		{
			if (player.GetSpiritPlayer().ZoneSpirit) {
				spawnRate = (int)(spawnRate * 0.6f);
				maxSpawns = (int)(maxSpawns * 1.1f);
			}
		}

	}
}