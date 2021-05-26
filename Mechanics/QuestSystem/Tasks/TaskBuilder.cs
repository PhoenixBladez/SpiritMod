using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiritMod.Mechanics.QuestSystem.Tasks
{
	public class TaskBuilder
	{
		private QuestTask _current;
		public List<QuestTask> AllTasks { get; private set; }
		public QuestTask Start { get; private set; }

		public TaskBuilder()
		{
			AllTasks = new List<QuestTask>();
		}

        public TaskBuilder AddTask(QuestTask task)
		{
			task.TaskID = AllTasks.Count;
			AllTasks.Add(task);

			if (Start == null)
			{
				Start = task;
				_current = Start;
				return this;
			}

			_current.NextTask = task;
			_current = task;
			return this;
		}

		public TaskBuilder AddParallelTasks(params QuestTask[] tasks)
		{
			QuestTask task = new ParallelTask(tasks);

			return AddTask(task);
		}

		public TaskBuilder AddBranches(params TaskBuilder[] branches)
		{
			foreach (var builder in branches)
			{
				CombineWith(builder);
			}

			QuestTask branchTask = new BranchingTask(branches.Select(t => t.Start).ToArray());

			return AddTask(branchTask);
		}

		public void CombineWith(TaskBuilder builder)
		{
			foreach (QuestTask task in builder.AllTasks)
			{
				task.TaskID = AllTasks.Count;
				AllTasks.Add(task);
			}
		}

		public QuestTask this[int index] => AllTasks[index];
	}
}
