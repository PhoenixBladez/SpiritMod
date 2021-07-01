using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.DonatorItems
{
	class DonatorDrops : GlobalNPC
	{
		public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
		{
			if (player.HasBuff(ModContent.BuffType<LoomingPresence>())) {
				spawnRate = (int)(spawnRate * 0.8);
				maxSpawns += 2;
			}
		}
	}
}
