using System.IO;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem
{
	class QuestMultiplayer
	{
		internal static void HandlePacket(BinaryReader reader, QuestMessageType messageType, bool forServer)
		{
			Main.NewText("Quest message recieved: " + messageType.ToString());

			if (forServer) //The server handles informing all clients that we need X thing done
			{
				if (messageType == QuestMessageType.Deactivate)
				{
					ModPacket packet = SpiritMod.Instance.GetPacket(MessageType.Quest, 4);
					packet.Write((byte)QuestMessageType.Deactivate);    //Packet requires a QuestMessageType and a forServer bool
					packet.Write(false);                                //That way I don't need to spam MessageTypes but there's
					packet.Write(reader.ReadString());                  //Still the functionality there
					packet.Send(-1, reader.ReadByte());                 //The rest of the params depend on the QuestMessageType
				}
				else if (messageType == QuestMessageType.Activate)
				{
					ModPacket packet = SpiritMod.Instance.GetPacket(MessageType.Quest, 4);
					packet.Write((byte)QuestMessageType.Activate);
					packet.Write(false);
					packet.Write(reader.ReadString());
					packet.Send(-1, reader.ReadByte());
				}
				else if (messageType == QuestMessageType.ProgressOrComplete)
				{
					ModPacket packet = SpiritMod.Instance.GetPacket(MessageType.Quest, 4);
					packet.Write((byte)QuestMessageType.ProgressOrComplete);
					packet.Write(false);
					packet.Write(reader.ReadString());
					packet.Send(-1, reader.ReadByte());
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
	}
}
