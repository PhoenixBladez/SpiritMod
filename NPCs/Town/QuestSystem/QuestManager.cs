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
        private static List<Quest> _quests;
        private static List<Quest> _activeQuests;

        public static void Load()
        {
            _quests = new List<Quest>();
            _activeQuests = new List<Quest>();
            
            // add all quests from the assembly
            IEnumerable<Type> questTypes = typeof(QuestManager).Assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(Quest)));
            foreach (Type type in questTypes)
            {
                _quests.Add((Quest)Activator.CreateInstance(type));
            }
        }

        public static void Unload()
        {
            _quests = null;
            _activeQuests = null;
        }

        public static Quest GetQuest<T>()
        {
            foreach (Quest q in _quests)
            {
                if (q is T) return q;
            }
            return null;
        }
    }
}
