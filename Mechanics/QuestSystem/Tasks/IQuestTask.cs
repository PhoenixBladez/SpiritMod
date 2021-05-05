using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiritMod.Mechanics.QuestSystem
{
	public interface IQuestTask
	{
		string ModCallName { get; }
		IQuestTask Parse(object[] args);

		bool CheckCompletion();
		void Activate();
		void Deactivate();
		void ResetProgress();
		void OnMPSyncTick();
		string GetObjectives(bool showProgress);
	}
}
