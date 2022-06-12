using System.IO;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem
{
	class QuestMultiplayer
	{
		internal static void HandlePacket(BinaryReader reader, QuestMessageType messageType, bool forServer)
		{
			if (forServer) //The server handles informing all clients that we need X thing done
			{
				if (messageType == QuestMessageType.Deactivate)
				{
					ModPacket packet = SpiritMod.Instance.GetPacket(MessageType.Quest, 4);
					packet.Write((byte)QuestMessageType.Deactivate);    //Packet requires a QuestMessageType and a forServer bool
					packet.Write(false);                                //That way I don't need to spam MessageTypes but there's
					string name = reader.ReadString();                  //Still the functionality there
					packet.Write(name);                                 //The rest of the params depend on the QuestMessageType
					packet.Send(-1, reader.ReadByte());

					DeactivateQuest(name); //Deactivate quest on server to ensure incoming clients are synced properly. Do this for everything.
				}
				else if (messageType == QuestMessageType.Activate)
				{
					ModPacket packet = SpiritMod.Instance.GetPacket(MessageType.Quest, 4);
					packet.Write((byte)QuestMessageType.Activate);
					packet.Write(false);
					string name = reader.ReadString();
					packet.Write(name);
					packet.Send(-1, reader.ReadByte());

					ActivateQuest(name);
				}
				else if (messageType == QuestMessageType.ProgressOrComplete)
				{
					ModPacket packet = SpiritMod.Instance.GetPacket(MessageType.Quest, 4);
					packet.Write((byte)QuestMessageType.ProgressOrComplete);
					packet.Write(false);
					string name = reader.ReadString();
					packet.Write(name);
					packet.Send(-1, reader.ReadByte());

					CompleteQuestOrTask(name);
				}
				else if (messageType == QuestMessageType.ObtainQuestBook)
				{
					ModPacket packet = SpiritMod.Instance.GetPacket(MessageType.Quest, 3);
					packet.Write((byte)QuestMessageType.ObtainQuestBook);
					packet.Write(false);
					packet.Send(-1, reader.ReadByte());

					QuestManager.UnlockQuestBook(false);
				}
				else if (messageType == QuestMessageType.Unlock)
				{
					ModPacket packet = SpiritMod.Instance.GetPacket(MessageType.Quest, 4);
					packet.Write((byte)QuestMessageType.Unlock);
					packet.Write(false);
					string name = reader.ReadString();
					packet.Write(name);
					packet.Send(-1, reader.ReadByte());

					UnlockQuest(name);
				}
				else if (messageType == QuestMessageType.SyncNPCQueue)
				{
					ModPacket packet = SpiritMod.Instance.GetPacket(MessageType.Quest, 4);
					packet.Write((byte)QuestMessageType.SyncNPCQueue);
					packet.Write(false);
					ushort npcID = reader.ReadUInt16();
					packet.Write(npcID);
					packet.Send(-1, reader.ReadByte());

					RemoveFromNPCQueue(npcID);
				}
			}
			else //And then we clear it up with clients
			{
				if (messageType == QuestMessageType.Deactivate)
					DeactivateQuest(reader.ReadString());
				else if (messageType == QuestMessageType.Activate)
					ActivateQuest(reader.ReadString());
				else if (messageType == QuestMessageType.ProgressOrComplete)
					CompleteQuestOrTask(reader.ReadString());
				else if (messageType == QuestMessageType.SyncOnNPCLoot) //Client only as this is only run on the server
					ModContent.GetInstance<QuestGlobalNPC>().ClientNPCLoot(Main.npc[reader.ReadByte()]);
				else if (messageType == QuestMessageType.ObtainQuestBook)
					QuestManager.UnlockQuestBook(false);
				else if (messageType == QuestMessageType.Unlock)
					UnlockQuest(reader.ReadString());
				else if (messageType == QuestMessageType.SyncNPCQueue)
					RemoveFromNPCQueue(reader.ReadUInt16());
			}
		}

		/// <summary>Writes name, a long bitflag value containing StoredQuestData, and the Buffer if it has one.</summary>
		public static void WriteQuestToPacket(ModPacket packet, Quest quest)
		{
			packet.Write(quest.QuestName);

			var data = QuestWorld.ToStoredQuest(quest);
			long flags = 0;
			flags += (long)data.TimeLeftActive << 48;
			flags += (long)data.TimeLeftUnlocked << 32;
			flags |= (long)data.RewardsGiven.ToInt() << 16;
			flags |= (long)data.IsCompleted.ToInt() << 15;
			flags |= (long)data.IsActive.ToInt() << 14;
			flags |= (long)data.IsUnlocked.ToInt() << 13;
			packet.Write(flags);

			bool hasBuffer = data.Buffer != null && data.Buffer.Length > 0;
			packet.Write((byte)(hasBuffer ? data.Buffer.Length : 0));
			if (hasBuffer)
				for (int i = 0; i < data.Buffer.Length; ++i)
					packet.Write(data.Buffer[i]);
		}

		public static StoredQuestData ReadQuestFromLong(long flags)
		{
			bool rewarded = (flags & 0b_10000000000000000) == 0b_10000000000000000;
			bool completed = (flags & 0b_1000000000000000) == 0b_1000000000000000;
			bool active = (flags & 0b_100000000000000) == 0b_100000000000000;
			bool unlocked = (flags & 0b_10000000000000) == 0b_10000000000000;
			short timeActive = (short)((flags & 0b_111111111111111100000000000000000000000000000000000000000000000) >> 48);
			short timeUnlock = (short)((flags & 0b_111111111111111100000000000000000000000000000000) >> 32);

			return new StoredQuestData
			{
				IsUnlocked = unlocked,
				IsActive = active,
				IsCompleted = completed,
				RewardsGiven = rewarded,
				TimeLeftActive = timeActive,
				TimeLeftUnlocked = timeUnlock
			};
		}

		internal static void DeactivateQuest(string questName)
		{
			var quest = QuestManager.Quests.FirstOrDefault(x => x.QuestName == questName);

			if (quest != null)
			{
				QuestManager.Quiet = true;
				QuestManager.DeactivateQuest(quest);
				QuestManager.Quiet = false;
			}
		}

		internal static void ActivateQuest(string questName)
		{
			var quest = QuestManager.Quests.FirstOrDefault(x => x.QuestName == questName);

			if (quest != null)
			{
				QuestManager.Quiet = true;
				QuestManager.ActivateQuest(quest);
				QuestManager.Quiet = false;
			}
		}

		internal static void CompleteQuestOrTask(string questName)
		{
			var quest = QuestManager.Quests.FirstOrDefault(x => x.QuestName == questName);

			if (quest != null)
			{
				QuestManager.Quiet = true;
				quest.RunCompletion();
				QuestManager.Quiet = false;
			}
		}

		internal static void UnlockQuest(string questName)
		{
			var quest = QuestManager.Quests.FirstOrDefault(x => x.QuestName == questName);

			if (quest != null && !quest.IsUnlocked)
			{
				QuestManager.Quiet = true;
				QuestManager.UnlockQuest(quest, Main.netMode != NetmodeID.Server);
				QuestManager.Quiet = false;
			}
		}

		internal static void RemoveFromNPCQueue(ushort npcID) => ModContent.GetInstance<QuestWorld>().NPCQuestQueue[npcID].Dequeue();
	}
}
