using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

using SpiritMod.Mechanics.QuestSystem.Tasks;

namespace SpiritMod.Mechanics.QuestSystem
{
	public abstract class Quest
	{
		protected TaskBuilder _tasks;
		protected QuestTask _currentTask;
		private bool _questActive;
		private bool _questUnlocked;
		private bool _questCompleted;
		private bool rewardsGiven;

		public virtual int Difficulty => 0;
		public virtual string QuestCategory => "";
		public virtual string QuestName => "";
		public virtual string QuestDescription => "";
		public virtual string QuestClient => "";
		public virtual (int, int)[] QuestRewards => null;
		public virtual bool TutorialActivateButton => false;
		public Texture2D QuestImage { get; set; }

		public int QuestCategoryIndex => QuestManager.GetCategoryInfo(QuestCategory).Index;
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
		public virtual bool IsUnlocked { get => _questUnlocked; set { _questUnlocked = value; OnQuestStateChanged?.Invoke(this); } }
		public bool IsCompleted { get => _questCompleted; set { _questCompleted = value; OnQuestStateChanged?.Invoke(this); } }
		public bool RewardsGiven { get => rewardsGiven; set { rewardsGiven = value; OnQuestStateChanged?.Invoke(this); } }
		public int WhoAmI { get; set; }

		public event Action<Quest> OnQuestStateChanged;

		public Quest()
		{
			_tasks = new TaskBuilder();
		}

		public virtual string GetObjectivesBook()
		{
			var lines = new List<string>();
			var final = new List<(string, bool)>();

			for (QuestTask task = _tasks.Start; task != null; task = task.NextTask)
			{
				task.GetBookText(lines);
				foreach (string s in lines)
				{
					final.Add((s, task.Completed));
				}
				lines.Clear();
			}

			StringBuilder builder = new StringBuilder();
			for (int i = 0; i < final.Count; i++)
			{
				if (!final[i].Item2) builder.Append("[c/2B1C11:");
				else builder.Append("[c/928269:");

				builder.Append("- ").Append(final[i].Item1).Append("]");

				if (i < final.Count - 1) builder.Append("\n");
			}

			return builder.ToString();
		}

		public virtual string GetObjectivesHUD()
		{
			// TODO: Access current task, call GetHUDText()
			return null;
		}

		public virtual void OnQuestComplete()
		{
			IsCompleted = true;

			string text = "You have completed a quest! [[sQ/" + WhoAmI + ":" + QuestName + "]]";

			if (Main.netMode == NetmodeID.SinglePlayer) Main.NewText(text, 255, 255, 255, false);
			else if (Main.netMode == NetmodeID.Server) NetMessage.BroadcastChatMessage(NetworkText.FromLiteral(text), Color.White, -1);
		}

		public virtual void OnActivate()
		{
			_currentTask = _tasks.Start;

			_currentTask.Activate();
		}

		public virtual void OnDeactivate()
		{
			if (_currentTask != null)
			{
				_currentTask.Deactivate();
			}
		}

		public void ResetAllProgress()
		{
			for (QuestTask task = _tasks.Start; task != null; task = task.NextTask)
			{
				task.ResetProgress();
			}
		}

		public virtual void Update()
		{
			if (_currentTask.CheckCompletion())
			{
				_currentTask.Completed = true;
				_currentTask.Deactivate();

				_currentTask = _currentTask.NextTask;
				if (_currentTask == null)
				{
					// quest completed
					OnQuestComplete();

					QuestManager.DeactivateQuest(this);
				}
				else
				{
					_currentTask.Activate();
				}
			}
		}

		public virtual bool IsQuestPossible() => true;

		public virtual void OnMPSync()
		{
			for (QuestTask task = _tasks.Start; task != null; task = task.NextTask)
			{
				task.OnMPSyncTick();
			}
		}

		// TODO: Write Packet and ReadPacket
		// TODO: Have each quest section have it's own WritePacket and ReadPacket
	}
}
