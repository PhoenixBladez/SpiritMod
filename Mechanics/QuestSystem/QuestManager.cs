using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem
{
    public static class QuestManager
    {
		public static List<Quest> Quests { get; private set; }
		public static List<Quest> ActiveQuests { get; private set; }
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
        }

		public static void ActivateQuest(int index)
		{
			ActivateQuest(Quests[index]);
		}

		public static void ActivateQuest(Quest quest)
		{
			if (quest.IsActive) return;
			if (ActiveQuests.Count >= 5)
			{
				// cannot activate quest.
				// TODO: Show this in the book? warning message or something?
				return;
			}

			// set to active.
			quest.IsActive = true;

			ActiveQuests.Add(quest);
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

		public static void SetBookState(bool open)
		{
			SpiritMod.Instance.BookUserInterface.SetState(open ? SpiritMod.QuestBookUIState : null);
		}
    }
}
