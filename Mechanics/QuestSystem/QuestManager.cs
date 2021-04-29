using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem
{
    public static class QuestManager
    {
		public static List<Quest> Quests { get; private set; }
		public static List<Quest> ActiveQuests { get; private set; }
		private static Dictionary<Type, Quest> _questDict;

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

				Quests.Add(q);
				_questDict[type] = q;
            }
        }

        public static void Unload()
        {
            Quests = null;
            ActiveQuests = null;
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
