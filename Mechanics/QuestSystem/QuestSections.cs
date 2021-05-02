using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem
{
	public interface IQuestSection
	{
		bool CheckCompletion();
		void Activate();
		void Deactivate();
		void ResetProgress();
		void OnMPSyncTick();
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

		public void OnMPSyncTick()
		{
			foreach (IQuestSection section in _sections)
			{
				section.OnMPSyncTick();
			}
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
			// make it so killing this type of NPC progresses the section
			if (npc.netID == _monsterID)
			{
				_killCount++;
				if (_killCount > _killsRequired) 
					_killCount = _killsRequired;
			}
		}

		public void OnMPSyncTick()
		{
		}
	}

	public class ExploreSection : IQuestSection
	{
		private Func<Player, bool> _exploreFunc;
		private float _requiredDistance;
		private float _distancedTravelled;
		private float _storedDistance;
		private string _areaName;

		/// <param name="areaName">Will be used for the objectives like so: - Explore [areaName] (x%)</param>
		public ExploreSection(Func<Player, bool> exploreFunction, float travelDistance, string areaName)
		{
			_exploreFunc = exploreFunction;
			_requiredDistance = travelDistance;
			_areaName = areaName;
		}

		public string GetObjectives(bool showProgress)
		{
			StringBuilder builder = new StringBuilder();

			builder.Append("- Explore ");
			builder.Append(_areaName);

			// add a progress bracket at the end like: (x%)
			if (showProgress)
			{
				float travelled = _distancedTravelled > _requiredDistance ? _requiredDistance : _distancedTravelled;
				float progress = travelled / _requiredDistance * 100f;
				builder.Append(" (").Append(progress.ToString("N2")).Append("%").Append(")");
			}

			return builder.ToString();
		}

		public void ResetProgress()
		{
			_distancedTravelled = 0f;
		}

		public void Activate()
		{
		}

		public void Deactivate()
		{
		}

		public bool CheckCompletion()
		{
			if (!Main.dedServ && _exploreFunc(Main.LocalPlayer))
			{
				float distanceMoved = Vector2.Distance(Main.LocalPlayer.oldPosition, Main.LocalPlayer.position);
				
				// TODO: finish MP syncing
				switch (Main.netMode)
				{
					case NetmodeID.SinglePlayer:
						_distancedTravelled += distanceMoved;
						break;
					case NetmodeID.MultiplayerClient:
						_storedDistance += distanceMoved;
						break;
				}
			}
			return _distancedTravelled >= _requiredDistance;
		}

		public void OnMPSyncTick()
		{
			// TODO: send the server our stored distance, then reset to 0
			_storedDistance = 0;
		}
	}
}
