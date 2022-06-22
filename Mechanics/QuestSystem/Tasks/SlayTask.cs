﻿using SpiritMod.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Terraria;

namespace SpiritMod.Mechanics.QuestSystem.Tasks
{
	public class SlayTask : QuestTask
	{
		public override string ModCallName => "Slay";

		private readonly bool _Singular = false;
		private int _killsRequired;
		private int _killCount;
		private QuestPoolData? _poolData;
		private string _monsterNameOverride;

        public int[] MonsterIDs { get; private set; }

        public SlayTask() { }

		public SlayTask(int monsterID, int amount, string monsterNameOverride = null, QuestPoolData? pool = null, bool singular = false)
		{
			MonsterIDs = new int[] { monsterID };

			_killsRequired = amount;
			_monsterNameOverride = monsterNameOverride;
			_poolData = pool;

			_Singular = singular;
		}

		public SlayTask(int[] monsterIDs, int amount, string monsterNameOverride = null, QuestPoolData? pool = null, bool singular = false)
		{
			MonsterIDs = monsterIDs;
			_killsRequired = amount;
			_monsterNameOverride = monsterNameOverride;
			_poolData = pool;

			_Singular = singular;
		}

		public override QuestTask Parse(object[] args)
		{
			// get the ids
			int[] monsterIDs;
			if (!QuestUtils.TryUnbox(args[1], out monsterIDs))
			{
				if (QuestUtils.TryUnbox(args[1], out int id))
					monsterIDs = new int[] { id };

				else if (QuestUtils.TryUnbox(args[1], out short idShort))
					monsterIDs = new int[] { idShort };
				else
					return null;
			}
			if (monsterIDs == null || monsterIDs.Length == 0) return null;

			// get the amount of kills required
			if (!QuestUtils.TryUnbox(args[2], out int amount, "Kills required"))
				return null;

			// get the name override, if there is one
			string nameOverride = null;
			if (args.Length > 3)
			{
				if (!QuestUtils.TryUnbox(args[3], out nameOverride, "Slay Task name override"))
					return null;
			}

			return new SlayTask(monsterIDs, amount, nameOverride);
		}

		public override void AutogeneratedBookText(List<string> lines) => lines.Add(GetObjectives(false));
		public override void AutogeneratedHUDText(List<string> lines) => lines.Add(GetObjectives(true));

		public string GetObjectives(bool showProgress)
		{
			StringBuilder builder = new StringBuilder();
			if (_monsterNameOverride == null)
			{
				string count = _killsRequired > 1 ? _killsRequired.ToString() : (_Singular ? "" : "a");
				builder.Append("Kill ").Append(count).Append(" ");
			}
			// start with: - Kill x monster, monster or monster
			if (_monsterNameOverride == null)
			{
				for (int i = 0; i < MonsterIDs.Length; i++)
				{
					string monsterName = Lang.GetNPCNameValue(MonsterIDs[i]);

					monsterName += QuestUtils.GetPluralEnding(_killsRequired, monsterName);

					if (MonsterIDs.Length == 1)
					{
						// if there's multiple monsters, add a character to show plurality
						builder.Append(monsterName);
						break;
					}
					else
					{
						builder.Append(monsterName);
						if (i < MonsterIDs.Length - 2)
							builder.Append(", ");
						else if (i == MonsterIDs.Length - 2)
							builder.Append(" or ");
					}
				}
			}
			else
				builder.Append(_monsterNameOverride);

			// add a progress bracket at the end like: (x/y)
			if (showProgress)
				builder.Append(" [c/97E2E2:(").Append(_killCount).Append("/").Append(_killsRequired).Append(")]");

			return builder.ToString();
		}

		public override void ResetProgress()
		{
			base.ResetProgress();
			_killCount = 0;
		}

		public override void Activate(Quest fromQuest)
		{
			QuestGlobalNPC.OnNPCLoot += QuestGlobalNPC_OnNPCLoot;

			for (int i = 0; i < MonsterIDs.Length; i++)
			{
				if (_poolData != null)
					QuestGlobalNPC.AddToPool(MonsterIDs[i], _poolData.Value);
			}
		}

		public override void Deactivate()
		{
			QuestGlobalNPC.OnNPCLoot -= QuestGlobalNPC_OnNPCLoot;

			for (int i = 0; i < MonsterIDs.Length; i++)
				QuestGlobalNPC.RemoveFromPool(MonsterIDs[i]);
		}

		public override bool CheckCompletion() => _killCount >= _killsRequired;

		private void QuestGlobalNPC_OnNPCLoot(NPC npc)
		{
			// make it so killing this type of NPC progresses the section
			if (MonsterIDs.Contains(npc.netID))
			{
				_killCount++;
				if (_killCount > _killsRequired)
					_killCount = _killsRequired;
			}
		}

		public override void ReadData(BinaryReader reader) => _killCount = reader.ReadInt32();
		public override void WriteData(BinaryWriter writer) => writer.Write(_killCount);
	}
}
