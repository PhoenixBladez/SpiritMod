using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpiritMod.Mechanics.QuestSystem
{
	public class ConcurrentTask : IQuestTask
	{
		//string ModCallName => "Concurrent";

		private IEnumerable<IQuestTask> _sections;

		public ConcurrentTask(params IQuestTask[] sections)
		{
			_sections = sections;
			QuestManager.Quests.Where(q => q.IsCompleted).Count();
		}

		public IQuestTask Parse(object[] args)
		{
			if (!(args[1] is object[] sections))
			{
				return null;
			}

			// TODO: Implement
			return null;
		}

		public string GetObjectives(bool showProgress)
		{
			StringBuilder builder = new StringBuilder();

			foreach (IQuestTask section in _sections)
			{
				builder.AppendLine(section.GetObjectives(showProgress));
			}

			return builder.ToString();
		}

		public void ResetProgress()
		{
			foreach (IQuestTask section in _sections)
			{
				section.ResetProgress();
			}
		}

		public void Activate()
		{
			foreach (IQuestTask section in _sections)
			{
				section.Activate();
			}
		}

		public void Deactivate()
		{
			foreach (IQuestTask section in _sections)
			{
				section.Deactivate();
			}
		}

		public bool CheckCompletion()
		{
			foreach (IQuestTask section in _sections)
			{
				if (!section.CheckCompletion()) return false;
			}

			return true;
		}

		public void OnMPSyncTick()
		{
			foreach (IQuestTask section in _sections)
			{
				section.OnMPSyncTick();
			}
		}
	}
}
