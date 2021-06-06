﻿using System;

using Terraria;
using Terraria.ModLoader;

using SpiritMod.Mechanics.QuestSystem.Quests;
using Terraria.ModLoader.IO;
using System.Collections.Generic;
using System.Linq;

namespace SpiritMod.Mechanics.QuestSystem
{
	public class QuestWorld : ModWorld
	{
		public Dictionary<int, Queue<Quest>> NPCQuestQueue { get; private set; } = new Dictionary<int, Queue<Quest>>();

		public override void PostUpdate()
		{
			Player player = Main.LocalPlayer;
			MyPlayer modPlayer = player.GetSpiritPlayer();

            if (Main.hardMode)
            {
                QuestManager.UnlockQuest<ExplorerQuestBlueMoon>(true);
                QuestManager.UnlockQuest<SlayerQuestVultureMatriarch>(true);
            }
        }

		public override void Load(TagCompound tag)
		{
			try
			{
				List<string> allQuests = tag.Get<List<string>>("SpiritMod:AllQuests");

				SpiritMod.QuestHUD.Clear();
				QuestManager.QuestBookUnlocked = tag.Get<bool>("SpiritMod:QuestBookUnlocked");

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

						if (failed)
						{
							// this quest doesn't exist at all, so skip.
							continue;
						}
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

					if (allQuests.Contains(key)) allQuests.Remove(key);
				}

				// get all the unloaded quests
				QuestManager.UnloadedQuests.Clear();
				for (int i = 0; i < allQuests.Count; i++)
				{
					QuestManager.UnloadedQuests.Add(allQuests[i], ConvertBack(tag.Get<TagCompound>(allQuests[i])));
				}
			}
			catch(Exception e)
			{
				SpiritMod.Instance.Logger.Error("Error loading quests! Error:\n" + e);
			}
		}

		public override TagCompound Save()
		{
			var tag = new TagCompound();

			List<string> allQuestNames = new List<string>();

			tag.Add("SpiritMod:QuestBookUnlocked", QuestManager.QuestBookUnlocked);

			// save any quests necessary
			for (int i = 0; i < QuestManager.Quests.Count; i++)
			{
				Quest quest = QuestManager.Quests[i];

				StoredQuestData data = new StoredQuestData();
				data.IsActive = quest.IsActive;
				data.IsUnlocked = quest.IsUnlocked;
				data.IsCompleted = quest.IsCompleted;
				data.RewardsGiven = quest.RewardsGiven;
				data.TimeLeftActive = (short)quest.ActiveTime;
				data.TimeLeftUnlocked = (short)quest.UnlockTime;

				if (quest.IsActive)
				{
					byte[] buffer = quest.GetTaskDataBuffer();
					data.Buffer = buffer;
					QuestManager.DeactivateQuest(quest);
				}

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

			/*foreach (Quest quest in QuestManager.Quests)
			{
				quest.ResetEverything();
			}

			if (!Main.dedServ)
			{
				SpiritMod.QuestHUD.Clear();
			}*/

			return tag;
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
			if (!NPCQuestQueue.ContainsKey(npcID))
				NPCQuestQueue.Add(npcID, new Queue<Quest>());

			NPCQuestQueue[npcID].Enqueue(quest);
		}
	}
}