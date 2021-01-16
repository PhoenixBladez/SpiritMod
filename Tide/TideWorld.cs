using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SpiritMod;
using System.Collections.Generic;
using System.Linq;

namespace SpiritMod.Tide
{
	public class TideWorld : ModWorld
	{
		public static int TidePoints = 0;
		public static int EnemyKills = 0;
		public static int TideWave = 0;
		public static bool TheTide;

		public override void Initialize()
		{
			TheTide = false;
			TideWave = 0;
		}

		public override void PostUpdate()
		{
			//TidePoints = EnemyKills / 2;
			if (TidePoints >= 20) {
				TidePoints = 0;
				EnemyKills = 0;
				if (TideWave == 6) {
					TideWave = 1;
					Main.NewText("The Tide has waned!", 61, 255, 142);
					TheTide = false;
					if (!MyWorld.downedTide) {
						Main.PlaySound(SoundLoader.customSoundType, Main.LocalPlayer.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/DeathSounds/TideComplete"));
						MyWorld.downedTide = true;
					}
				}
				else {
					TideWaveIncrease();
				}
			}
		}

		public static void TideWaveIncrease()
		{
			TideWave++;
			string wavetext = "Wave " + TideWave + " : ";
			IDictionary<int, float> spawnpool = TideNPC.Spawnpool.ElementAt(TideWave - 1); //find the spawn pool dictionary corresponding to the current tide wave
			wavetext += Lang.GetNPCName(spawnpool.First().Key); //write the first npc name in the spawn pool dictionary
			foreach (KeyValuePair<int, float> key in spawnpool.Skip(1)) { //then for ever npc name after it, add a comma and space before their name
				wavetext += ", " + Lang.GetNPCName(key.Key);
			}
			Main.NewText(wavetext, 61, 255, 142);
		}
	}
}
