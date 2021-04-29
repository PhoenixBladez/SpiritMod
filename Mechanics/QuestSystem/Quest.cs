using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem
{
	public abstract class Quest
	{
		protected List<IQuestSection> _questSections;
		protected int _currentSection;
		private bool _questActive;
		private bool _questUnlocked;
		private bool _questCompleted;

		public virtual int Difficulty => 0;
		public virtual QuestType QuestType => QuestType.Other;
		public virtual string QuestName => "";
		public virtual float QuestTitleScale => 0.8f;
		public virtual string QuestDescription => "";
		public virtual string QuestClient => "";

		public bool QuestActive { get => _questActive; set { OnQuestActivityChanged?.Invoke(this, value); _questActive = value; } }
		public bool QuestUnlocked { get => _questUnlocked; set { OnQuestUnlockChanged?.Invoke(this, value); _questUnlocked = value; } }
		public bool QuestCompleted { get => _questCompleted; set { OnQuestCompletionChanged?.Invoke(this, value); _questCompleted = value; } }

		public event Action<Quest, bool> OnQuestActivityChanged;
		public event Action<Quest, bool> OnQuestUnlockChanged;
		public event Action<Quest, bool> OnQuestCompletionChanged;

		public Quest()
		{
			_questSections = new List<IQuestSection>();
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
					QuestCompleted = true;
					OnQuestComplete();
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
