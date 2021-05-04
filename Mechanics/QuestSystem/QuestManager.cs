using Microsoft.Xna.Framework;
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
		private static int _serverSyncCounter;

        public static void Load()
        {
			_questDict = new Dictionary<Type, Quest>();
            Quests = new List<Quest>();
            ActiveQuests = new List<Quest>();
            
            // add all quests from the assembly
            IEnumerable<Type> questTypes = typeof(QuestManager).Assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(Quest)));
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
    }
}
