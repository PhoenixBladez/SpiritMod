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
		private bool rewardsGiven;

		public virtual int Difficulty => 0;
		public virtual QuestType QuestType => QuestType.Other;
		public virtual string QuestName => "";
		public virtual float QuestTitleScale => 0.8f;
		public virtual string QuestDescription => "";
		public virtual string QuestClient => "";
		public virtual (int, int)[] QuestRewards => null;
		public virtual bool TutorialActivateButton => false;
		public Texture2D QuestImage { get; set; }

		public bool IsActive
		{
			get => _questActive; set
			{
				_questActive = value;
				OnQuestStateChanged?.Invoke(this);

				if (value) OnActivate();
				else OnDeactivate();
			}
		}
		public bool IsUnlocked { get => _questUnlocked; set { _questUnlocked = value; OnQuestStateChanged?.Invoke(this); } }
		public bool IsCompleted { get => _questCompleted; set { _questCompleted = value; OnQuestStateChanged?.Invoke(this); } }
		public bool RewardsGiven { get => rewardsGiven; set { rewardsGiven = value; OnQuestStateChanged?.Invoke(this); } }

		public event Action<Quest> OnQuestStateChanged;

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

		public virtual void ResetProgress(bool resetCompletion = false)
		{
			if (resetCompletion) IsCompleted = false;

			foreach (IQuestSection section in _questSections)
			{
				section.ResetProgress();
			}

			_currentSection = 0;
		}

		public virtual void OnQuestComplete()
		{
			IsCompleted = true;
		}

		public virtual void OnActivate()
		{
			_questSections[_currentSection].Activate();
		}

		public virtual void OnDeactivate()
		{
			if (_currentSection < _questSections.Count)
			{
				_questSections[_currentSection].Deactivate();
			}
		}

		public virtual void Update()
		{
			if (_questSections[_currentSection].CheckCompletion())
			{
				_questSections[_currentSection].Deactivate();

				_currentSection++;
				if (_currentSection == _questSections.Count)
				{
					// quest completed
					OnQuestComplete();

					QuestManager.DeactivateQuest(this);
				}
				else
				{
					_questSections[_currentSection].Activate();
				}
			}
		}

		public virtual void OnMPSync()
		{
			foreach (IQuestSection section in _questSections)
			{
				section.OnMPSyncTick();
			}
		}

		// TODO: Write Packet and ReadPacket
		// TODO: Have each quest section have it's own WritePacket and ReadPacket
	}
}
