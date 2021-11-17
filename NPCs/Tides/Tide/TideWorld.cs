using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SpiritMod;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Runtime.InteropServices;
using Steamworks;
using SpiritMod.NPCs.Tides;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace SpiritMod.NPCs.Tides.Tide
{
	public class TideWorld : ModWorld
	{
		public static int TidePoints = 0;
		public static int TideWave = 0;
		public static bool TheTide;

		public override void Initialize()
		{
			TheTide = false;
			TideWave = 0;
		}

		public static ModPacket CreateProgressPacket()
		{
			ModPacket packet = ModContent.GetInstance<SpiritMod>().GetPacket();
			packet.Write((byte)MessageType.TideData);
			packet.Write(TidePoints);
			packet.Write(TideWave);
			packet.Write(TheTide);

			return packet;
		}

		public static void SendInfoPacket()
		{
			if (Main.netMode == NetmodeID.SinglePlayer)
				return;

			CreateProgressPacket().Send();
		}

		public static void HandlePacket(BinaryReader reader)
		{
			TidePoints = reader.ReadInt32();
			TideWave = reader.ReadInt32();
			TheTide = reader.ReadBoolean();

			if (Main.netMode == NetmodeID.Server)
				SendInfoPacket(); // Forward packet to rest of clients
		}

		public static void TideWaveIncrease()
		{
			TideWave++;

			if (TideWave > 5)
			{
				if (Main.netMode == NetmodeID.Server)
					NetMessage.BroadcastChatMessage(NetworkText.FromLiteral("The Tide has waned!"), new Color(61, 255, 142));
				else if (Main.netMode == NetmodeID.SinglePlayer)
					Main.NewText("The Tide has waned!", 61, 255, 142);

				TheTide = false;
				TideWave = 0;
				TidePoints = 0;

				if (Main.netMode != NetmodeID.Server)
				{
					Main.musicFade[SpiritMod.Instance.GetSoundSlot(SoundType.Music, "Sounds/Music/DepthInvasion")] = 0;
					float temp = Main.soundVolume; //temporarily store main.soundvolume, since sounds dont play at all if sound volume is at 0, regardless of actual volume of the sound
					Main.soundVolume = (temp == 0) ? 1 : Main.soundVolume;
					Main.PlaySound(SoundLoader.customSoundType, Main.LocalPlayer.position, SpiritMod.Instance.GetSoundSlot(SoundType.Custom, "Sounds/DeathSounds/TideComplete"));
					Main.soundVolume = temp;
				}

				if (!MyWorld.downedTide)
				{
					MyWorld.downedTide = true;
					if (Main.netMode == NetmodeID.Server)
						NetMessage.SendData(MessageID.WorldData);
				}
			}
			else
			{
				string waveText = GetWaveChatText(TideWave);
				Color color = new Color(61, 255, 142);

				if (Main.netMode == NetmodeID.SinglePlayer)
					Main.NewText(waveText, color);
				else if (Main.netMode == NetmodeID.Server)
					NetMessage.BroadcastChatMessage(NetworkText.FromLiteral(waveText), color);
			}

			SendInfoPacket();
		}

		private static string GetWaveChatText(int wave)
		{
			string wavetext = "Wave " + wave + " : ";
			IDictionary<int, float> spawnpool = TideNPC.Spawnpool.ElementAt(wave - 1); //find the spawn pool dictionary corresponding to the current tide wave
			wavetext += Lang.GetNPCName(spawnpool.First().Key); //write the first npc name in the spawn pool dictionary
			foreach (KeyValuePair<int, float> key in spawnpool.Skip(1))
			{ //then for ever npc name after it, add a comma and space before their name
				wavetext += ", " + Lang.GetNPCName(key.Key);
			}

			return wavetext;
		}
	}
}
