using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Town.QuestSystem
{
    public static class QuestManager
    {
		public static List<Quest> Quests { get; private set; }
		public static List<Quest> ActiveQuests { get; private set; }

        public static void Load()
        {
            Quests = new List<Quest>();
            ActiveQuests = new List<Quest>();
            
            // add all quests from the assembly
            IEnumerable<Type> questTypes = typeof(QuestManager).Assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(Quest)));
            foreach (Type type in questTypes)
            {
                Quests.Add((Quest)Activator.CreateInstance(type));
            }
        }

        public static void Unload()
        {
            Quests = null;
            ActiveQuests = null;
        }

        public static Quest GetQuest<T>()
        {
            foreach (Quest q in Quests)
            {
                if (q is T) return q;
            }
            return null;
        }

		public static void SetBookState(bool open)
		{
			SpiritMod.Instance.BookUserInterface.SetState(open ? SpiritMod.QuestBookUIState : null);
		}
    }
}
