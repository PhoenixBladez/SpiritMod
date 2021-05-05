using log4net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem
{
    public static class QuestManager
    {
		public const int MAX_QUESTS_ACTIVE = 5;

		public static List<Quest> Quests { get; private set; }
		public static List<Quest> ActiveQuests { get; private set; }
		public static bool QuestBookUnlocked { get; set; }

		private static Dictionary<Type, Quest> _questDict;
		private static Dictionary<string, IQuestTask> _tasksDict;
		private static int _serverSyncCounter;

        public static void Load()
        {
			_questDict = new Dictionary<Type, Quest>();
			_tasksDict = new Dictionary<string, IQuestTask>();

			Quests = new List<Quest>();
            ActiveQuests = new List<Quest>();
            
            // add all quests from the assembly
            IEnumerable<Type> questTypes = typeof(QuestManager).Assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(Quest)) && t != typeof(InstancedQuest));
            foreach (Type type in questTypes)
            {
				Quest q = (Quest)Activator.CreateInstance(type);

				// load related quest image
				string tex = "UI/QuestUI/Textures/Quests/" + type.Name;
				if (SpiritMod.Instance.TextureExists(tex))
				{
					q.QuestImage = SpiritMod.Instance.GetTexture(tex);
				}

				q.WhoAmI = Quests.Count;

				Quests.Add(q);
				_questDict[type] = q;
			}

			// store an instance of all quest tasks for cross-mod compatibility purposes.
			IEnumerable<Type> taskTypes = typeof(QuestManager).Assembly.GetTypes().Where(t => typeof(IQuestTask).IsAssignableFrom(t));
			foreach (Type type in taskTypes)
			{
				if (type.IsInterface) continue;

				var task = (IQuestTask)Activator.CreateInstance(type);
				_tasksDict[task.ModCallName] = task;
			}

			Main.OnTick += Update;
		}

        public static void Unload()
        {
			Main.OnTick -= Update;

            Quests = null;
            ActiveQuests = null;
			_questDict = null;
        }

		public static bool ActivateQuest(int index)
		{
			return ActivateQuest(Quests[index]);
		}

		public static bool ActivateQuest(Quest quest)
		{
			if (quest.IsActive) return false;
			if (ActiveQuests.Count >= MAX_QUESTS_ACTIVE)
			{
				// cannot activate quest.
				return false;
			}

			// set to active.
			quest.IsActive = true;

			ActiveQuests.Add(quest);

			return true;
		}

		public static void DeactivateQuest(int index)
		{
			DeactivateQuest(Quests[index]);
		}

		public static void DeactivateQuest(Quest quest)
		{
			if (!quest.IsActive) return;
			if (!ActiveQuests.Contains(quest)) return;

			ActiveQuests.Remove(quest);

			// set to not active.
			quest.IsActive = false;
			quest.ResetProgress();
		}

		public static void GiveRewards(Quest quest)
		{
			if (quest.RewardsGiven) return;

			quest.RewardsGiven = true;
		}

		public static void Update()
		{
			if (Main.gameMenu) return;

			// test if we need to sync
			bool syncMP = false;
			if (Main.netMode != NetmodeID.SinglePlayer)
			{
				_serverSyncCounter++;
				if (_serverSyncCounter >= 20)
				{
					_serverSyncCounter = 0;
					syncMP = true;
				}
			}

			// update quests and sync if we're a mp client or server
			var shallowCopy = new List<Quest>(ActiveQuests);
			foreach (Quest quest in shallowCopy)
			{
				quest.Update();
				if (syncMP)
				{
					if (Main.netMode == NetmodeID.MultiplayerClient) quest.OnMPSync();
					else
					{
						// TODO: Sync data with other clients.
					}
				}
			}
		}

		public static Quest GetQuest<T>()
        {
			if (_questDict.TryGetValue(typeof(T), out Quest q)) return q;
            return null;
		}

		public static void UnlockQuest<T>(bool showInChat = true)
		{
			Quest q = GetQuest<T>();
			if (q == null) return;

			UnlockQuest(q, showInChat);
		}

		public static void UnlockQuest(Quest quest, bool showInChat = true)
		{
			if (quest.IsUnlocked) return;

			quest.IsUnlocked = true;

			if (showInChat && quest.IsQuestPossible())
			{
				string text = "You have unlocked a new quest! [[sQ/" + quest.WhoAmI + ":" + quest.QuestName + "]]";

				if (Main.netMode == NetmodeID.SinglePlayer) Main.NewText(text, 255, 255, 255, false);
				else if (Main.netMode == NetmodeID.Server) NetMessage.BroadcastChatMessage(NetworkText.FromLiteral(text), Color.White, -1);
			}
		}

		public static void SetBookState(bool open)
		{
			SpiritMod.Instance.BookUserInterface.SetState(open ? SpiritMod.QuestBookUIState : null);
		}

		public static int ModCallAddQuest(object[] args)
		{
			if (args.Length < 9)
			{
				SpiritMod.Instance.Logger.Warn("Error adding custom quest! Less than 9 arguments means something must be missing, check you've added everything you need to add!");
				return -1;
			}

			int index = 1;
			// quest name
			if (!QuestUtils.TryUnbox(args[index++], out string questName, "Quest name")) return -1;

			// quest category
			if (!QuestUtils.TryUnbox(args[index++], out string questCategory, "Quest category")) return -1;
			// TODO: check if quest category exists. if it doesn't, add it, make the colour white.

			// quest client
			if (!QuestUtils.TryUnbox(args[index++], out string questClient, "Quest client")) return -1;

			// quest description
			if (!QuestUtils.TryUnbox(args[index++], out string questDesc, "Quest description")) return -1;

			// quest rewards
			if (!QuestUtils.TryUnbox(args[index++], out (int, int)[] questRewards, "Quest rewards")) return -1;

			// quest difficulty
			if (!QuestUtils.TryUnbox(args[index++], out int questDifficulty, "Quest difficulty")) return -1;

			// quest image
			if (!QuestUtils.TryUnbox(args[index++], out Texture2D questImage, "Quest image")) return -1;

			// quest objectives
			List<IQuestTask> tasks = new List<IQuestTask>();
			for (int i = index; i < args.Length; i++)
			{
				if (!QuestUtils.TryUnbox(args[i], out object[] objectiveArgs, "Quest objective " + (i - index + 1))) return -1;

				IQuestTask task = ParseTaskFromArguments(objectiveArgs);
				if (task == null)
				{
					SpiritMod.Instance.Logger.Warn("Task returned null.");
					return -1;
				}

				tasks.Add(task);
			}

			// add the quest
			Quest q = new InstancedQuest(questName, questDifficulty, questClient, questDesc, questRewards, questImage, tasks);
			q.WhoAmI = Quests.Count;
			Quests.Add(q);

			SpiritMod.Instance.Logger.Info("Added a cross-mod quest! Called: " + questName);

			return q.WhoAmI;
		}

		public static void ModCallUnlockQuest(object[] args)
		{
			if (!QuestUtils.TryUnbox(args[1], out int questIndex, "Quest ID"))
			{
				return;
			}

			if (questIndex < 0 || questIndex >= Quests.Count) return;

			UnlockQuest(Quests[questIndex]);
		}

		public static bool ModCallGetQuestValueFromContext(object[] args, int context)
		{
			if (!QuestUtils.TryUnbox(args[1], out int questIndex, "Quest ID"))
			{
				return false;
			}

			if (questIndex < 0 || questIndex >= Quests.Count) return false;

			switch (context)
			{
				default:
				case 0:
					return Quests[questIndex].IsUnlocked;
				case 1:
					return Quests[questIndex].IsActive;
				case 2:
					return Quests[questIndex].IsCompleted;
				case 3:
					return Quests[questIndex].RewardsGiven;
			}
		}

		public static IQuestTask ParseTaskFromArguments(object[] args)
		{
			if (!QuestUtils.TryUnbox(args[0], out string name, "Quest Objective Type"))
			{
				return null;
			}

			if (!_tasksDict.TryGetValue(name, out IQuestTask task))
			{
				SpiritMod.Instance.Logger.Warn("Quest task " + name + " does not exist.");
				return null;
			}

			return task.Parse(args);
		}
	}
}
