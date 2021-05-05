using SpiritMod.Utilities;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem
{
	public class SlayTask : IQuestTask
	{
		public string ModCallName => "Slay";

		private int[] _monsterIDs;
		private int _killsRequired;
		private int _killCount;
		private string _monsterNameOverride;

		public SlayTask(int monsterID, int amount, string monsterNameOverride = null)
		{
			_monsterIDs = new int[] { monsterID };
			_killsRequired = amount;
			_monsterNameOverride = monsterNameOverride;
		}

		public SlayTask(int[] monsterIDs, int amount, string monsterNameOverride = null)
		{
			_monsterIDs = monsterIDs;
			_killsRequired = amount;
			_monsterNameOverride = monsterNameOverride;
		}

		public IQuestTask Parse(object[] args)
		{
			// get the ids
			int[] monsterIDs;
			if (!QuestUtils.TryUnbox(args[1], out monsterIDs))
			{
				if (QuestUtils.TryUnbox(args[1], out int id))
				{
					monsterIDs = new int[] { id };
				}
				else if (QuestUtils.TryUnbox(args[1], out short idShort))
				{
					monsterIDs = new int[] { idShort };
				}
				else
				{
					return null;
				}
			}
			if (monsterIDs == null || monsterIDs.Length == 0) return null;

			// get the amount of kills required
			if (!QuestUtils.TryUnbox(args[2], out int amount))
			{
				return null;
			}

			// get the name override, if there is one
			string nameOverride = null;
			if (args.Length > 3)
			{
				if (!QuestUtils.TryUnbox(args[3], out nameOverride))
				{
					return null;
				}
			}

			return new SlayTask(monsterIDs, amount, nameOverride);
		}

		public string GetObjectives(bool showProgress)
		{
			StringBuilder builder = new StringBuilder();

			string count = _killsRequired > 1 ? _killsRequired.ToString() : "a";
			builder.Append("- Kill ").Append(count).Append(" ");

			// start with: - Kill x monster, monster or monster
			if (_monsterNameOverride == null)
			{
				for (int i = 0; i < _monsterIDs.Length; i++)
				{
					string monsterName = Lang.GetNPCNameValue(_monsterIDs[i]);

					monsterName += QuestUtils.GetPluralEnding(_killsRequired, monsterName);

					if (_monsterIDs.Length == 1)
					{
						// if there's multiple monsters, add a character to show plurality
						builder.Append(monsterName);
						break;
					}
					else
					{
						builder.Append(monsterName);
						if (i < _monsterIDs.Length - 2)
						{
							builder.Append(", ");
						}
						else if (i == _monsterIDs.Length - 2)
						{
							builder.Append(" or ");
						}
					}
				}
			}
			else
			{
				builder.Append(_monsterNameOverride);
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
			if (_monsterIDs.Contains(npc.netID))
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
}
