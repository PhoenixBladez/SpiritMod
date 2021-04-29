using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem
{
    public interface IQuestSection
    {
        bool CheckCompletion();
        void Activate();
        void Deactivate();
        void ResetProgress();
        string GetObjectives(bool showProgress);
    }

    public class ConcurrentSection : IQuestSection
    {
        private IEnumerable<IQuestSection> _sections;

        public ConcurrentSection(params IQuestSection[] sections)
        {
            _sections = sections;
        }

        public string GetObjectives(bool showProgress)
        {
            StringBuilder builder = new StringBuilder();

            foreach (IQuestSection section in _sections)
            {
                builder.AppendLine(section.GetObjectives(showProgress));
            }

            return builder.ToString();
        }

        public void ResetProgress()
        {
            foreach (IQuestSection section in _sections)
            {
                section.ResetProgress();
            }
        }

        public void Activate()
        {
            foreach (IQuestSection section in _sections)
            {
                section.Activate();
            }
        }

        public void Deactivate()
        {
            foreach (IQuestSection section in _sections)
            {
                section.Deactivate();
            }
        }

        public bool CheckCompletion()
        {
            foreach (IQuestSection section in _sections)
            {
                if (!section.CheckCompletion()) return false;
            }

            return true;
        }
    }

    public class KillSection : IQuestSection
    {
        private int _monsterID;
        private int _killsRequired;
        private int _killCount;

        public KillSection(int monsterID, int amount)
        {
            _monsterID = monsterID;
            _killsRequired = amount;
        }

        public string GetObjectives(bool showProgress)
        {
            StringBuilder builder = new StringBuilder();

            // start with: - Kill x monster
            string monsterName = "";
			if (_monsterID < Terraria.ID.NPCID.Count)
			{
				monsterName = Lang.GetNPCNameValue(_monsterID);
			}
			else
			{
				monsterName = NPCLoader.GetNPC(_monsterID).DisplayName.GetTranslation(Terraria.Localization.Language.ActiveCulture);
			}
            string count = _killsRequired > 1 ? _killsRequired.ToString() : "a";
            builder.Append("- Kill ").Append(count).Append(" ").Append(monsterName);

            // if there's multiple monsters, add a character to show plurality
            if (_killsRequired > 1)
            {
                if (monsterName.Last() != 's') builder.Append('s');
                else builder.Append('\'');
            }

            // add a progress bracket at the end like: (x/y)
            if (showProgress)
            {
                builder.Append(" (").Append(_killCount).Append("/").Append(_killsRequired).Append(")");
            }

            return builder.ToString();
        }

        public void ResetProgress()
        {
            _killCount = 0;
        }

        public void Activate()
        {
            QuestGlobalNPC.OnNPCLoot += QuestGlobalNPC_OnNPCLoot;
        }

        public void Deactivate()
        {
            QuestGlobalNPC.OnNPCLoot -= QuestGlobalNPC_OnNPCLoot;
        }

        public bool CheckCompletion()
        {
            return _killCount >= _killsRequired;
        }

        private void QuestGlobalNPC_OnNPCLoot(NPC npc)
        {
            if (npc.netID == _monsterID)
            {
                _killCount++;
            }
        }
    }
}
