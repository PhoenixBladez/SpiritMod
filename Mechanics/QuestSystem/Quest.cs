using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

using SpiritMod.Mechanics.QuestSystem.Tasks;
using SpiritMod.UI.Elements;

namespace SpiritMod.Mechanics.QuestSystem
{
	public abstract class Quest
	{
		protected TaskBuilder _tasks;
		protected QuestTask _currentTask;
		private bool _questActive;
		private bool _questUnlocked;
		private bool _questCompleted;
		private bool _rewardsGiven;
		private int _completedCounter = 0;
		internal List<string> _altNames;

		public virtual int Difficulty => 1;
		public virtual string QuestCategory => "";
		public virtual string QuestName => "";
		public virtual string QuestDescription => "";
		public virtual string QuestClient => "";
		public virtual (int, int)[] QuestRewards => null;
		public virtual bool TutorialActivateButton => false;
		public virtual Texture2D QuestImage { get; set; }

		public virtual bool AppearsWhenUnlocked => true;
		public virtual bool CountsTowardsQuestTotal => true;
		public virtual bool AnnounceRelocking => false;
		public virtual bool LimitedUnlock => false;
		public virtual bool AnnounceDeactivation => false;
		public virtual bool LimitedActive => false;
		public int UnlockTime { get; set; }
		public int ActiveTime { get; set; }

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
		public bool RewardsGiven { get => _rewardsGiven; set { _rewardsGiven = value; OnQuestStateChanged?.Invoke(this); } }
		public int WhoAmI { get; set; }

		public event Action<Quest> OnQuestStateChanged;

		public Quest()
		{
			_altNames = new List<string>();
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
			var lines = new List<string>();
			_currentTask.GetHUDText(lines);

			StringBuilder builder = new StringBuilder();
			for (int i = 0; i < lines.Count; i++)
			{
				builder.Append("- ").Append(lines[i]);

				if (i < lines.Count - 1) builder.Append("\n");
			}

			return builder.ToString();
		}

		public virtual void OnQuestComplete()
		{
			IsCompleted = true;

			QuestManager.SayInChat("You have completed a quest! [[sQ/" + WhoAmI + ":" + QuestName + "]]", Color.White);
		}

		public virtual void OnUnlock() { }

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

		public virtual void ResetEverything()
		{
			ResetAllProgress();
			IsActive = false;
			IsCompleted = false;
			IsUnlocked = false;
			RewardsGiven = false;
		}

		public virtual void ResetAllProgress()
		{
			for (QuestTask task = _tasks.Start; task != null; task = task.NextTask)
			{
				task.ResetProgress();
			}
		}

		public virtual void UpdateBookOverlay(UIShaderImage image) { image.Texture = null; }

		public virtual void Update()
		{
			if (LimitedActive)
			{
				ActiveTime--;
				if (ActiveTime <= 0)
				{
					if (AnnounceDeactivation)
					{
						QuestManager.SayInChat("[[sQ/" + WhoAmI + ":" + QuestName + "]] is no longer active.", Color.White);
					}
					QuestManager.DeactivateQuest(this);
				}
			}

			if (_currentTask.CheckCompletion())
			{
				// this counter is here so the HUD works. that's all.
				_completedCounter++;
				if (_completedCounter >= 3)
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
		}

		public virtual bool IsQuestPossible() => true;

		public virtual void OnMPSync()
		{
			for (QuestTask task = _tasks.Start; task != null; task = task.NextTask)
			{
				task.OnMPSyncTick();
			}
		}

		public virtual byte[] GetTaskDataBuffer()
		{
			byte[] buffer = new byte[16];
			using (var stream = new MemoryStream(buffer))
			{
				using (var writer = new BinaryWriter(stream))
				{
					writer.Write(_currentTask.TaskID);
					_currentTask.WriteData(writer);
				}
			}
			return buffer;
		}

		public virtual void ReadFromDataBuffer(byte[] buffer)
		{
			using (var stream = new MemoryStream(buffer))
			{
				using (var reader = new BinaryReader(stream))
				{
					int taskId = reader.ReadInt32();

					_currentTask = _tasks[taskId];
					_currentTask.ReadData(reader);
				}
			}
		}

		// TODO: Write Packet and ReadPacket
		// TODO: Have each quest section have it's own WritePacket and ReadPacket
	}
}
