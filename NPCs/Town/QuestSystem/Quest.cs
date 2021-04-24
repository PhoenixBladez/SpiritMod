using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Town.QuestSystem
{
    public abstract class Quest
    {
        protected List<IQuestSection> _questSections;
        protected int _currentSection;

        public virtual int ID => -1;
        public virtual int Difficulty => 0;
        public virtual QuestType QuestType => QuestType.Other;
        public virtual string QuestName => "";
        public virtual string QuestDescription => "";

        public bool QuestCompleted { get; private set; }

        public Quest()
        {
            _questSections = new List<IQuestSection>();
        }

        public void SetCompleted()
        {
            QuestCompleted = true;
        }

        public virtual string GetObjectives(bool showProgresss)
        {
            StringBuilder builder = new StringBuilder();

            foreach (IQuestSection section in _questSections)
            {
                builder.AppendLine(section.GetObjectives(showProgresss));
            }

            return builder.ToString();
        }

        public virtual void ResetProgress() 
        {
            QuestCompleted = false;

            foreach (IQuestSection section in _questSections)
            {
                section.ResetProgress();
            }
        }

        public virtual void OnActivate() { }

        public virtual void OnDeactivate() { }

        public virtual void OnQuestComplete() { }

        public virtual bool CanUnlock() => true;

        public virtual void Update()
        {
            if (_questSections[_currentSection].CheckCompletion())
            {
                _currentSection++;
                if (_currentSection == _questSections.Count)
                {
                    // quest completed
                }
                else
                {
                    _questSections[_currentSection].Activate();
                }
            }
        }

        public virtual void DrawQuestTexture(SpriteBatch spriteBatch, Rectangle area) { }
    }
}
