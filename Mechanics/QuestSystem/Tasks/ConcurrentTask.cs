using SpiritMod.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpiritMod.Mechanics.QuestSystem
{
	public class ConcurrentTask : IQuestTask
	{
		public string ModCallName => "Concurrent";

		private IEnumerable<IQuestTask> _tasks;

		public ConcurrentTask() { }

		public ConcurrentTask(params IQuestTask[] tasks)
		{
			_tasks = tasks;
		}

		public IQuestTask Parse(object[] args)
		{
			if (!QuestUtils.TryUnbox(args[1], out object[] tasks, "Concurrent Task's Tasks"))
			{
				return null;
			}

			IQuestTask[] taskArray = new IQuestTask[tasks.Length];
			for (int i = 0; i < tasks.Length; i++)
			{
				if (!QuestUtils.TryUnbox(tasks[i], out object[] taskArgs, "Concurrent Task's Task " + (i + 1)))
				{
					return null;
				}
				IQuestTask task = QuestManager.ParseTaskFromArguments(taskArgs);
				if (task == null) return null;
				taskArray[i] = task;
			}

			return new ConcurrentTask(taskArray);
		}

		public string GetObjectives(bool showProgress)
		{
			StringBuilder builder = new StringBuilder();

			foreach (IQuestTask section in _tasks)
			{
				builder.AppendLine(section.GetObjectives(showProgress));
			}

			return builder.ToString();
		}

		public void ResetProgress()
		{
			foreach (IQuestTask section in _tasks)
			{
				section.ResetProgress();
			}
		}

		public void Activate()
		{
			foreach (IQuestTask section in _tasks)
			{
				section.Activate();
			}
		}

		public void Deactivate()
		{
			foreach (IQuestTask section in _tasks)
			{
				section.Deactivate();
			}
		}

		public bool CheckCompletion()
		{
			foreach (IQuestTask section in _tasks)
			{
				if (!section.CheckCompletion()) return false;
			}

			return true;
		}

		public void OnMPSyncTick()
		{
			foreach (IQuestTask section in _tasks)
			{
				section.OnMPSyncTick();
			}
		}
	}
}
