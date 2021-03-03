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
				TideWaveIncrease();
				SendPacket(mod);
			}

			if(TheTide && TideWave == 6) {
				var players = Main.player.Where(x => x.ZoneBeach && x.active && !x.dead);
				foreach(Player player in players) {
					if(!NPC.AnyNPCs(ModContent.NPCType<Rylheian>())) {
						NPC npc = Main.npc[NPC.NewNPC((int)player.Center.X, (int)player.Center.Y - 400, ModContent.NPCType<Rylheian>(), 0, 2, 1, 0, 0, player.whoAmI)];
						DustHelper.DrawDiamond(new Vector2(npc.Center.X, npc.Center.Y), 173, 8);
						DustHelper.DrawTriangle(new Vector2(npc.Center.X, npc.Center.Y), 173, 8);
						Main.PlaySound(SoundID.Zombie, npc.Center, 89);

						if (Main.netMode != NetmodeID.SinglePlayer) 
							NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, npc.whoAmI);

					}
				}
			}
		}

		public static void SendPacket(Mod mod)
		{
			if (Main.netMode == NetmodeID.SinglePlayer)
				return;
			ModPacket packet = mod.GetPacket();
			packet.Write((byte)MessageType.TideData);
			packet.Write(TidePoints);
			packet.Write(EnemyKills);
			packet.Write(TideWave);
			packet.Write(TheTide);
			packet.Send();
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
			if (TideWave > 6) {

				if (Main.netMode == NetmodeID.Server)
					NetMessage.BroadcastChatMessage(NetworkText.FromLiteral("The Tide has waned!"), new Color(61, 255, 142));
				else if(Main.netMode == NetmodeID.SinglePlayer)
					Main.NewText("The Tide has waned!", 61, 255, 142);

				TheTide = false;
				SendPacket(SpiritMod.instance);
				if (!MyWorld.downedTide) {
					Main.PlaySound(SoundLoader.customSoundType, Main.LocalPlayer.position, SpiritMod.instance.GetSoundSlot(SoundType.Custom, "Sounds/DeathSounds/TideComplete"));
					MyWorld.downedTide = true;
					if (Main.netMode == NetmodeID.Server)
						NetMessage.SendData(MessageID.WorldData);
				}
				return;
			}

			string wavetext = "Wave " + TideWave + " : ";
			IDictionary<int, float> spawnpool = TideNPC.Spawnpool.ElementAt(TideWave - 1); //find the spawn pool dictionary corresponding to the current tide wave
			wavetext += Lang.GetNPCName(spawnpool.First().Key); //write the first npc name in the spawn pool dictionary
			foreach (KeyValuePair<int, float> key in spawnpool.Skip(1)) { //then for ever npc name after it, add a comma and space before their name
				wavetext += ", " + Lang.GetNPCName(key.Key);
			}
			if (Main.netMode == NetmodeID.Server)
				NetMessage.BroadcastChatMessage(NetworkText.FromLiteral(wavetext), new Color(61, 255, 142));
			else
				Main.NewText(wavetext, 61, 255, 142);
		}
	}
}
