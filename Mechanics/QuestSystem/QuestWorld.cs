using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Mechanics.QuestSystem.Quests;
using Terraria.ModLoader.IO;
using System.Collections.Generic;
using System.Linq;

namespace SpiritMod.Mechanics.QuestSystem
{
	public class QuestWorld : ModSystem
	{
		public static bool downedWeaponsMaster = false;

		public Dictionary<int, Queue<Quest>> NPCQuestQueue { get; private set; } = new Dictionary<int, Queue<Quest>>();

		public override void PostUpdateWorld()
		{
			if (!QuestManager.QuestBookUnlocked) //Do nothing if we don't have the book
				return;

            if (Main.hardMode)
            {
                QuestManager.UnlockQuest<ExplorerQuestBlueMoon>(true);
                QuestManager.UnlockQuest<SlayerQuestVultureMatriarch>(true);

				if (Main.bloodMoon && QuestManager.GetQuest<SlayerQuestClown>().IsUnlocked)
					AddQuestQueue(NPCID.PartyGirl, QuestManager.GetQuest<SlayerQuestClown>());

				AddQuestQueue(NPCID.Dryad, QuestManager.GetQuest<OlympiumQuest>());
				AddQuestQueue(NPCID.Mechanic, QuestManager.GetQuest<GranitechQuest>());
				AddQuestQueue(ModContent.NPCType<NPCs.Town.Adventurer>(), QuestManager.GetQuest<AuroraStagQuest>());

				if (NPC.downedMechBoss1 || NPC.downedMechBoss2 || NPC.downedMechBoss3)
					AddQuestQueue(NPCID.Dryad, QuestManager.GetQuest<CritterCaptureSoulOrb>());
			}

			//if (NPC.downedBoss2)
			//	AddQuestQueue(NPCID.Stylist, QuestManager.GetQuest<StylistQuestMeteor>());
		}

		public override void LoadWorldData(TagCompound tag)
		{
			try
			{
				List<string> allQuests = tag.Get<List<string>>("SpiritMod:AllQuests");

				if(!Main.dedServ)
					SpiritMod.QuestHUD.Clear();

				QuestManager.QuestBookUnlocked = tag.Get<bool>("SpiritMod:QuestBookUnlocked");

				int ierjgo = 0;

				for (int i = 0; i < QuestManager.Quests.Count; i++)
				{
					Quest quest = QuestManager.Quests[i];
					quest.ResetEverything();

					// get the key for this quest
					string key = "SpiritMod:" + quest.QuestName;
					if (!tag.ContainsKey(key))
					{
						bool failed = true;
						foreach (string s in quest._altNames)
						{
							key = "SpiritMod:" + s;
							if (tag.ContainsKey(key))
							{
								failed = false;
								break;
							}
						}

						if (failed) // this quest doesn't exist at all, so skip.
							continue;
					}

					StoredQuestData data = ConvertBack(tag.Get<TagCompound>(key));

					quest.IsUnlocked = data.IsUnlocked;
					quest.IsCompleted = data.IsCompleted;
					quest.RewardsGiven = data.RewardsGiven;

					if (data.IsActive)
					{
						QuestManager.ActivateQuest(quest);
						quest.ReadFromDataBuffer(data.Buffer);
					}

					quest.ActiveTime = data.TimeLeftActive;
					quest.UnlockTime = data.TimeLeftUnlocked;

					if (allQuests.Contains(key))
						allQuests.Remove(key);
				}

				// get all the unloaded quests
				QuestManager.UnloadedQuests.Clear();
				for (int i = 0; i < allQuests.Count; i++)
					QuestManager.UnloadedQuests.Add(allQuests[i], ConvertBack(tag.Get<TagCompound>(allQuests[i])));

				downedWeaponsMaster = tag.GetBool("downedWeaponsMaster");

				LoadQueue(tag);
			}
			catch (Exception e)
			{
				SpiritMod.Instance.Logger.Error("Error loading quests! Error:\n" + e);
			}
		}

		public override void SaveWorldData(TagCompound tag)
		{
			tag.Add("downedWeaponsMaster", downedWeaponsMaster);

			List<string> allQuestNames = new List<string>();

			tag.Add("SpiritMod:QuestBookUnlocked", QuestManager.QuestBookUnlocked);

			// save any quests necessary
			for (int i = 0; i < QuestManager.Quests.Count; i++)
			{
				Quest quest = QuestManager.Quests[i];
				var data = ToStoredQuest(quest);

				allQuestNames.Add("SpiritMod:" + quest.QuestName);
				tag.Add(allQuestNames[i], Convert(data));
			}

			// add unloaded quests so their data is saved
			foreach (var pair in QuestManager.UnloadedQuests)
			{
				allQuestNames.Add(pair.Key);
				tag.Add(pair.Key, Convert(pair.Value));
			}

			tag.Add("SpiritMod:AllQuests", allQuestNames);

			SaveQueue(tag);
		}

		private void SaveQueue(TagCompound tag)
		{
			tag.Add("SpiritMod:QuestQueueNPCLength", NPCQuestQueue.Keys.Count);

			int npcIDRep = 0;
			int questRep = 0;
			foreach (int item in NPCQuestQueue.Keys)
			{
				tag.Add("SpiritMod:QuestQueueNPCID" + npcIDRep, item); //Writes the ID and the length of the queue
				tag.Add("SpiritMod:SingleQuestQueueLength" + npcIDRep++, NPCQuestQueue[item].Count);

				while (NPCQuestQueue[item].Count > 0) //Writes every value in the queue
				{
					Quest q = NPCQuestQueue[item].Dequeue();
					tag.Add("SpiritMod:SingleQuestQueue" + questRep++, q.QuestName);
				}
			}
		}

		private void LoadQueue(TagCompound tag)
		{
			int queuesCount = tag.GetInt("SpiritMod:QuestQueueNPCLength");

			if (queuesCount == 0)
				return; //Nothing in the dictionary, exit

			int questCount = 0;
			for (int i = 0; i < queuesCount; ++i)
			{
				int npcID = tag.GetInt("SpiritMod:QuestQueueNPCID" + i);
				int length = tag.GetInt("SpiritMod:SingleQuestQueueLength" + i);

				if (length == 0)
					continue; //Nothing in the queue, skip

				for (int j = 0; j < length; ++j)
				{
					string questName = tag.GetString("SpiritMod:SingleQuestQueue" + questCount++);
					var quest = QuestManager.Quests.FirstOrDefault(x => x.QuestName == questName);

					if (quest != null)
						AddQuestQueue(npcID, quest);
				}
			}
		} 

		public static StoredQuestData ToStoredQuest(Quest quest)
		{
			var data = new StoredQuestData
			{
				IsActive = quest.IsActive,
				IsUnlocked = quest.IsUnlocked,
				IsCompleted = quest.IsCompleted,
				RewardsGiven = quest.RewardsGiven,
				TimeLeftActive = (short)quest.ActiveTime,
				TimeLeftUnlocked = (short)quest.UnlockTime
			};

			if (quest.IsActive)
			{
				byte[] buffer = quest.GetTaskDataBuffer();
				data.Buffer = buffer;
			}
			return data;
		}

		private TagCompound Convert(StoredQuestData data)
		{
			var tag = new TagCompound
			{
				{ "a", data.IsActive },
				{ "u", data.IsUnlocked },
				{ "c", data.IsCompleted },
				{ "r", data.RewardsGiven },
				{ "tlu", data.TimeLeftUnlocked },
				{ "tla", data.TimeLeftActive },
				{ "b", data.Buffer }
			};
			return tag;
		}

		private StoredQuestData ConvertBack(TagCompound tag)
		{
			var data = new StoredQuestData
			{
				IsActive = tag.Get<bool>("a"),
				IsUnlocked = tag.Get<bool>("u"),
				IsCompleted = tag.Get<bool>("c"),
				RewardsGiven = tag.Get<bool>("r"),
				TimeLeftUnlocked = tag.Get<short>("tlu"),
				TimeLeftActive = tag.Get<short>("tla"),
				Buffer = tag.Get<byte[]>("b")
			};
			return data;
		}

		/// <summary>
		/// Adds a quest to a specific NPC's queue. Use QuestManager.GetQuest<Quest>() for the parameter.
		/// </summary>
		/// <param name="npcID">The ID of the NPC that will recieve a new quest.</param>
		/// <param name="quest">The quest to add to the queue. Use QuestManager.GetQuest<Quest>() for this.</param>
		public void AddQuestQueue(int npcID, Quest quest)
		{
			if (quest is null)
				return;

			if (!NPCQuestQueue.ContainsKey(npcID))
				NPCQuestQueue.Add(npcID, new Queue<Quest>());

			if (!NPCQuestQueue[npcID].Contains(quest) && !QuestManager.GetQuest(quest).IsUnlocked)
				NPCQuestQueue[npcID].Enqueue(quest);
		}
	}
}