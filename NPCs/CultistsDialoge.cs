using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs
{
	public class SpiritSpawnRates : GlobalNPC
	{

		public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
		{
			if (player.GetSpiritPlayer().ZoneSpirit)
			{
				spawnRate = (int)(spawnRate * 0.6f);
				maxSpawns = (int)(maxSpawns * 1.1f);
			}
		}

	}
}