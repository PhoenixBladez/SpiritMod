using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SpiritMod;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Runtime.InteropServices;
using Steamworks;

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
				SendPacket(mod);
				if (TideWave == 6) {
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
		public static void SendPacket(Mod mod)
		{
			if (Main.netMode == NetmodeID.SinglePlayer)
				return;
			ModPacket packet = mod.GetPacket();
			packet.Write(TidePoints);
			packet.Write(EnemyKills);
			packet.Write(TideWave);
			packet.Write(TheTide);
		}
		public static void HandlePacket(BinaryReader reader)
		{
			TidePoints = reader.ReadInt32();
			EnemyKills = reader.ReadInt32();
			TideWave = reader.ReadInt32();
			TheTide = reader.ReadBoolean();
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
